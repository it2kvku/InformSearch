using System;
using System.Collections.Generic;
using System.Linq;

namespace InformSearch
{
    /// <summary>
    /// Greedy TSP Algorithm - Always visit the nearest unvisited city
    /// </summary>
    public class GreedyTSP : ITSPAlgorithm
    {
        public TSPResult Solve(TSPGraph graph)
        {
            var result = new TSPResult();
            
            if (graph.Cities.Count == 0)
                return result;

            // Start from the first city
            int startCity = graph.Cities[0];
            var currentPath = new List<int> { startCity };
            var visited = new HashSet<int> { startCity };
            int totalCost = 0;
            int currentCity = startCity;

            // Add initial step
            result.Steps.Add(new TSPStep
            {
                CurrentPath = new List<int>(currentPath),
                CurrentCity = currentCity,
                CurrentCost = totalCost,
                Description = $"Starting at city {startCity}"
            });

            // Visit all remaining cities using greedy approach
            while (visited.Count < graph.Cities.Count)
            {
                int nearestCity = -1;
                int minCost = int.MaxValue;

                // Find the nearest unvisited city
                foreach (int city in graph.Cities)
                {
                    if (!visited.Contains(city))
                    {
                        int cost = graph.GetCost(currentCity, city);
                        if (cost < minCost)
                        {
                            minCost = cost;
                            nearestCity = city;
                        }
                    }
                }

                if (nearestCity != -1 && minCost != int.MaxValue)
                {
                    // Move to nearest city
                    currentPath.Add(nearestCity);
                    visited.Add(nearestCity);
                    totalCost += minCost;
                    currentCity = nearestCity;

                    // Add step for animation
                    result.Steps.Add(new TSPStep
                    {
                        CurrentPath = new List<int>(currentPath),
                        CurrentCity = currentCity,
                        CurrentCost = totalCost,
                        Description = $"Moved to nearest city {nearestCity} (cost: {minCost})"
                    });
                }
                else
                {
                    // No reachable city found
                    break;
                }
            }

            // Return to start city to complete the tour
            if (currentPath.Count == graph.Cities.Count)
            {
                int returnCost = graph.GetCost(currentCity, startCity);
                if (returnCost != int.MaxValue)
                {
                    currentPath.Add(startCity);
                    totalCost += returnCost;

                    result.Steps.Add(new TSPStep
                    {
                        CurrentPath = new List<int>(currentPath),
                        CurrentCity = startCity,
                        CurrentCost = totalCost,
                        Description = $"Returned to start city {startCity} (cost: {returnCost})"
                    });
                }
            }

            result.FinalPath = currentPath;
            result.TotalCost = totalCost;
            
            return result;
        }
    }

    /// <summary>
    /// UCS TSP Algorithm - Uses Uniform Cost Search to find optimal tour
    /// </summary>
    public class UCSTSP : ITSPAlgorithm
    {
        private class TSPState
        {
            public List<int> Path { get; set; }
            public HashSet<int> Visited { get; set; }
            public int Cost { get; set; }
            public int CurrentCity { get; set; }

            public TSPState()
            {
                Path = new List<int>();
                Visited = new HashSet<int>();
                Cost = 0;
            }

            public TSPState(TSPState other)
            {
                Path = new List<int>(other.Path);
                Visited = new HashSet<int>(other.Visited);
                Cost = other.Cost;
                CurrentCity = other.CurrentCity;
            }
        }

        public TSPResult Solve(TSPGraph graph)
        {
            var result = new TSPResult();
            
            if (graph.Cities.Count == 0)
                return result;

            // Priority queue for UCS (ordered by cost)
            var frontier = new SortedList<int, Queue<TSPState>>();
            var startCity = graph.Cities[0];
            
            var initialState = new TSPState();
            initialState.Path.Add(startCity);
            initialState.Visited.Add(startCity);
            initialState.CurrentCity = startCity;
            
            if (!frontier.ContainsKey(0))
                frontier[0] = new Queue<TSPState>();
            frontier[0].Enqueue(initialState);

            TSPState bestSolution = null;
            int stepCount = 0;
            const int maxSteps = 1000; // Limit steps to prevent infinite loops

            while (frontier.Count > 0 && stepCount < maxSteps)
            {
                stepCount++;
                
                // Get state with lowest cost
                var lowestCost = frontier.Keys[0];
                var currentState = frontier[lowestCost].Dequeue();
                
                if (frontier[lowestCost].Count == 0)
                    frontier.Remove(lowestCost);

                // Add step for animation
                result.Steps.Add(new TSPStep
                {
                    CurrentPath = new List<int>(currentState.Path),
                    CurrentCity = currentState.CurrentCity,
                    CurrentCost = currentState.Cost,
                    Description = $"Exploring path: {string.Join("->", currentState.Path)}"
                });

                // Check if we've visited all cities
                if (currentState.Visited.Count == graph.Cities.Count)
                {
                    // Try to return to start city
                    int returnCost = graph.GetCost(currentState.CurrentCity, startCity);
                    if (returnCost != int.MaxValue)
                    {
                        var completePath = new List<int>(currentState.Path);
                        completePath.Add(startCity);
                        int totalCost = currentState.Cost + returnCost;
                        
                        if (bestSolution == null || totalCost < bestSolution.Cost)
                        {
                            bestSolution = new TSPState
                            {
                                Path = completePath,
                                Cost = totalCost
                            };
                        }
                    }
                    continue;
                }

                // Expand to unvisited cities
                foreach (int nextCity in graph.Cities)
                {
                    if (!currentState.Visited.Contains(nextCity))
                    {
                        int moveCost = graph.GetCost(currentState.CurrentCity, nextCity);
                        if (moveCost != int.MaxValue)
                        {
                            var newState = new TSPState(currentState);
                            newState.Path.Add(nextCity);
                            newState.Visited.Add(nextCity);
                            newState.Cost += moveCost;
                            newState.CurrentCity = nextCity;

                            if (!frontier.ContainsKey(newState.Cost))
                                frontier[newState.Cost] = new Queue<TSPState>();
                            frontier[newState.Cost].Enqueue(newState);
                        }
                    }
                }
            }

            if (bestSolution != null)
            {
                result.FinalPath = bestSolution.Path;
                result.TotalCost = bestSolution.Cost;
                
                // Add final step
                result.Steps.Add(new TSPStep
                {
                    CurrentPath = new List<int>(bestSolution.Path),
                    CurrentCity = bestSolution.Path.LastOrDefault(),
                    CurrentCost = bestSolution.Cost,
                    Description = $"Found optimal tour with cost {bestSolution.Cost}"
                });
            }

            return result;
        }
    }

    /// <summary>
    /// A* TSP Algorithm - Uses heuristic to guide search
    /// </summary>
    public class AStarTSP : ITSPAlgorithm
    {
        private class TSPStateAStar
        {
            public List<int> Path { get; set; }
            public HashSet<int> Visited { get; set; }
            public int GCost { get; set; } // Actual cost so far
            public int HCost { get; set; } // Heuristic cost
            public int FCost => GCost + HCost; // Total estimated cost
            public int CurrentCity { get; set; }

            public TSPStateAStar()
            {
                Path = new List<int>();
                Visited = new HashSet<int>();
                GCost = 0;
                HCost = 0;
            }

            public TSPStateAStar(TSPStateAStar other)
            {
                Path = new List<int>(other.Path);
                Visited = new HashSet<int>(other.Visited);
                GCost = other.GCost;
                HCost = other.HCost;
                CurrentCity = other.CurrentCity;
            }
        }

        public TSPResult Solve(TSPGraph graph)
        {
            var result = new TSPResult();
            
            if (graph.Cities.Count == 0)
                return result;

            // Priority queue for A* (ordered by F-cost)
            var frontier = new SortedList<int, Queue<TSPStateAStar>>();
            var startCity = graph.Cities[0];
            
            var initialState = new TSPStateAStar();
            initialState.Path.Add(startCity);
            initialState.Visited.Add(startCity);
            initialState.CurrentCity = startCity;
            initialState.HCost = CalculateHeuristic(graph, initialState);
            
            if (!frontier.ContainsKey(initialState.FCost))
                frontier[initialState.FCost] = new Queue<TSPStateAStar>();
            frontier[initialState.FCost].Enqueue(initialState);

            TSPStateAStar bestSolution = null;
            int stepCount = 0;
            const int maxSteps = 1000; // Limit steps to prevent infinite loops

            while (frontier.Count > 0 && stepCount < maxSteps)
            {
                stepCount++;
                
                // Get state with lowest F-cost
                var lowestFCost = frontier.Keys[0];
                var currentState = frontier[lowestFCost].Dequeue();
                
                if (frontier[lowestFCost].Count == 0)
                    frontier.Remove(lowestFCost);

                // Add step for animation
                result.Steps.Add(new TSPStep
                {
                    CurrentPath = new List<int>(currentState.Path),
                    CurrentCity = currentState.CurrentCity,
                    CurrentCost = currentState.GCost,
                    Description = $"A* exploring: {string.Join("->", currentState.Path)} (F={currentState.FCost})"
                });

                // Check if we've visited all cities
                if (currentState.Visited.Count == graph.Cities.Count)
                {
                    // Try to return to start city
                    int returnCost = graph.GetCost(currentState.CurrentCity, startCity);
                    if (returnCost != int.MaxValue)
                    {
                        var completePath = new List<int>(currentState.Path);
                        completePath.Add(startCity);
                        int totalCost = currentState.GCost + returnCost;
                        
                        if (bestSolution == null || totalCost < bestSolution.GCost)
                        {
                            bestSolution = new TSPStateAStar
                            {
                                Path = completePath,
                                GCost = totalCost
                            };
                        }
                    }
                    continue;
                }

                // Expand to unvisited cities
                foreach (int nextCity in graph.Cities)
                {
                    if (!currentState.Visited.Contains(nextCity))
                    {
                        int moveCost = graph.GetCost(currentState.CurrentCity, nextCity);
                        if (moveCost != int.MaxValue)
                        {
                            var newState = new TSPStateAStar(currentState);
                            newState.Path.Add(nextCity);
                            newState.Visited.Add(nextCity);
                            newState.GCost += moveCost;
                            newState.CurrentCity = nextCity;
                            newState.HCost = CalculateHeuristic(graph, newState);

                            if (!frontier.ContainsKey(newState.FCost))
                                frontier[newState.FCost] = new Queue<TSPStateAStar>();
                            frontier[newState.FCost].Enqueue(newState);
                        }
                    }
                }
            }

            if (bestSolution != null)
            {
                result.FinalPath = bestSolution.Path;
                result.TotalCost = bestSolution.GCost;
                
                // Add final step
                result.Steps.Add(new TSPStep
                {
                    CurrentPath = new List<int>(bestSolution.Path),
                    CurrentCity = bestSolution.Path.LastOrDefault(),
                    CurrentCost = bestSolution.GCost,
                    Description = $"A* found optimal tour with cost {bestSolution.GCost}"
                });
            }

            return result;
        }

        /// <summary>
        /// Calculate heuristic: minimum cost to visit all remaining cities
        /// Simple heuristic: (remaining cities count) * minimum edge cost
        /// </summary>
        private int CalculateHeuristic(TSPGraph graph, TSPStateAStar state)
        {
            int remainingCities = graph.Cities.Count - state.Visited.Count;
            if (remainingCities == 0)
            {
                // Need to return to start
                return graph.GetCost(state.CurrentCity, graph.Cities[0]);
            }

            // Estimate: remaining cities * minimum edge cost
            int minEdgeCost = graph.GetMinimumEdgeCost();
            return remainingCities * minEdgeCost;
        }
    }
}
