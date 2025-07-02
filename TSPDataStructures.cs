using System;
using System.Collections.Generic;
using System.Linq;

namespace InformSearch
{
    // Data structures for TSP
    public class TSPEdge
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Cost { get; set; }

        public TSPEdge(int from, int to, int cost)
        {
            From = from;
            To = to;
            Cost = cost;
        }
    }

    public class TSPGraph
    {
        public List<int> Cities { get; private set; }
        public List<TSPEdge> Edges { get; private set; }
        private Dictionary<int, Dictionary<int, int>> adjacencyMatrix;

        public TSPGraph()
        {
            Cities = new List<int>();
            Edges = new List<TSPEdge>();
            adjacencyMatrix = new Dictionary<int, Dictionary<int, int>>();
        }

        public void AddCity(int cityId)
        {
            if (!Cities.Contains(cityId))
            {
                Cities.Add(cityId);
                adjacencyMatrix[cityId] = new Dictionary<int, int>();
            }
        }

        public void AddEdge(int from, int to, int cost)
        {
            AddCity(from);
            AddCity(to);
            
            Edges.Add(new TSPEdge(from, to, cost));
            adjacencyMatrix[from][to] = cost;
            
            // TSP is typically undirected, so add reverse edge
            if (!adjacencyMatrix[to].ContainsKey(from))
            {
                Edges.Add(new TSPEdge(to, from, cost));
                adjacencyMatrix[to][from] = cost;
            }
        }

        public int GetCost(int from, int to)
        {
            if (adjacencyMatrix.ContainsKey(from) && adjacencyMatrix[from].ContainsKey(to))
            {
                return adjacencyMatrix[from][to];
            }
            return int.MaxValue; // No direct connection
        }

        public List<int> GetNeighbors(int city)
        {
            if (adjacencyMatrix.ContainsKey(city))
            {
                return adjacencyMatrix[city].Keys.ToList();
            }
            return new List<int>();
        }

        public int GetMinimumEdgeCost()
        {
            if (Edges.Count == 0) return 0;
            return Edges.Min(e => e.Cost);
        }
    }

    // Step-by-step tracking for animation
    public class TSPStep
    {
        public List<int> CurrentPath { get; set; }
        public int CurrentCity { get; set; }
        public int CurrentCost { get; set; }
        public string Description { get; set; }

        public TSPStep()
        {
            CurrentPath = new List<int>();
        }
    }

    public class TSPResult
    {
        public List<int> FinalPath { get; set; }
        public int TotalCost { get; set; }
        public List<TSPStep> Steps { get; set; }
        public List<int> CurrentPath { get; set; } // For animation
        public int CurrentCity { get; set; } // For highlighting current city

        public TSPResult()
        {
            FinalPath = new List<int>();
            Steps = new List<TSPStep>();
            CurrentPath = new List<int>();
        }
    }

    // Algorithm interface
    public interface ITSPAlgorithm
    {
        TSPResult Solve(TSPGraph graph);
    }
}
