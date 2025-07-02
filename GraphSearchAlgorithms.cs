using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace InformSearch
{
    public class Node : IComparable<Node>
    {
        public string NodeName { get; set; }
        public int GCost { get; set; }  // Cost from start
        public int HCost { get; set; }  // Heuristic cost to goal
        public int FCost => GCost + HCost;  // Total cost for A*
        public Node? Parent { get; set; }

        public Node(string nodeName)
        {
            NodeName = nodeName;
            GCost = 0;
            HCost = 0;
            Parent = null;
        }

        public int CompareTo(Node? other)
        {
            if (other == null) return 1;
            return FCost.CompareTo(other.FCost);
        }
    }

    public partial class Form1
    {
        private int nodeIdCounter = 0;

        private GraphSearchResult GreedySearch()
        {
            nodeIdCounter = 0;
            var result = new GraphSearchResult();
            var openSet = new PriorityQueue<Node, int>();
            var closedSet = new HashSet<string>();
            var allNodes = new Dictionary<string, Node>();
            var treeNodes = new Dictionary<string, TreeNodeGraph>();

            var startNode = new Node(graph.StartNode);
            startNode.HCost = graph.GetHeuristic(graph.StartNode);
            
            var startTreeNode = new TreeNodeGraph(graph.StartNode, nodeIdCounter++);
            startTreeNode.HCost = startNode.HCost;
            startTreeNode.Level = 0;
            result.SearchTree = startTreeNode;
            treeNodes[graph.StartNode] = startTreeNode;
            result.ExplorationOrder.Add(startTreeNode);

            openSet.Enqueue(startNode, startNode.HCost);
            allNodes[graph.StartNode] = startNode;

            while (openSet.Count > 0)
            {
                var currentNode = openSet.Dequeue();
                var currentNodeName = currentNode.NodeName;

                if (closedSet.Contains(currentNodeName))
                    continue;

                closedSet.Add(currentNodeName);
                result.ExploredNodes.Add(currentNodeName);

                if (currentNodeName == graph.GoalNode)
                {
                    result.Success = true;
                    result.Path = ReconstructPath(currentNode);
                    result.PathCost = currentNode.GCost;
                    result.NodeCosts = GetNodeCosts(allNodes);
                    treeNodes[currentNodeName].IsGoal = true;
                    MarkPathInTree(treeNodes, result.Path);
                    return result;
                }

                foreach (var neighbor in graph.GetNeighbors(currentNodeName))
                {
                    if (closedSet.Contains(neighbor))
                        continue;

                    var neighborNode = new Node(neighbor)
                    {
                        Parent = currentNode,
                        GCost = currentNode.GCost + graph.GetEdgeCost(currentNodeName, neighbor),
                        HCost = graph.GetHeuristic(neighbor)
                    };

                    if (!allNodes.ContainsKey(neighbor))
                    {
                        allNodes[neighbor] = neighborNode;
                        openSet.Enqueue(neighborNode, neighborNode.HCost);
                        
                        // Add to tree
                        var neighborTreeNode = new TreeNodeGraph(neighbor, nodeIdCounter++);
                        neighborTreeNode.HCost = neighborNode.HCost;
                        neighborTreeNode.GCost = neighborNode.GCost;
                        neighborTreeNode.Parent = treeNodes[currentNodeName];
                        neighborTreeNode.Level = treeNodes[currentNodeName].Level + 1;
                        treeNodes[currentNodeName].Children.Add(neighborTreeNode);
                        treeNodes[neighbor] = neighborTreeNode;
                        result.ExplorationOrder.Add(neighborTreeNode);
                    }
                }
            }

            result.NodeCosts = GetNodeCosts(allNodes);
            return result;
        }

        private GraphSearchResult UniformCostSearch()
        {
            nodeIdCounter = 0;
            var result = new GraphSearchResult();
            var openSet = new PriorityQueue<Node, int>();
            var closedSet = new HashSet<string>();
            var allNodes = new Dictionary<string, Node>();
            var treeNodes = new Dictionary<string, TreeNodeGraph>();

            var startNode = new Node(graph.StartNode);
            var startTreeNode = new TreeNodeGraph(graph.StartNode, nodeIdCounter++);
            startTreeNode.GCost = 0;
            startTreeNode.Level = 0;
            result.SearchTree = startTreeNode;
            treeNodes[graph.StartNode] = startTreeNode;
            result.ExplorationOrder.Add(startTreeNode);

            openSet.Enqueue(startNode, 0);
            allNodes[graph.StartNode] = startNode;

            while (openSet.Count > 0)
            {
                var currentNode = openSet.Dequeue();
                var currentNodeName = currentNode.NodeName;

                if (closedSet.Contains(currentNodeName))
                    continue;

                closedSet.Add(currentNodeName);
                result.ExploredNodes.Add(currentNodeName);

                if (currentNodeName == graph.GoalNode)
                {
                    result.Success = true;
                    result.Path = ReconstructPath(currentNode);
                    result.PathCost = currentNode.GCost;
                    result.NodeCosts = GetNodeCosts(allNodes);
                    treeNodes[currentNodeName].IsGoal = true;
                    MarkPathInTree(treeNodes, result.Path);
                    return result;
                }

                foreach (var neighbor in graph.GetNeighbors(currentNodeName))
                {
                    if (closedSet.Contains(neighbor))
                        continue;

                    int newGCost = currentNode.GCost + graph.GetEdgeCost(currentNodeName, neighbor);

                    if (!allNodes.ContainsKey(neighbor) || newGCost < allNodes[neighbor].GCost)
                    {
                        var neighborNode = new Node(neighbor)
                        {
                            GCost = newGCost,
                            Parent = currentNode
                        };

                        allNodes[neighbor] = neighborNode;
                        openSet.Enqueue(neighborNode, newGCost);
                        
                        // Add to tree (only if new or better path)
                        if (!treeNodes.ContainsKey(neighbor))
                        {
                            var neighborTreeNode = new TreeNodeGraph(neighbor, nodeIdCounter++);
                            neighborTreeNode.GCost = newGCost;
                            neighborTreeNode.Parent = treeNodes[currentNodeName];
                            neighborTreeNode.Level = treeNodes[currentNodeName].Level + 1;
                            treeNodes[currentNodeName].Children.Add(neighborTreeNode);
                            treeNodes[neighbor] = neighborTreeNode;
                            result.ExplorationOrder.Add(neighborTreeNode);
                        }
                        else if (newGCost < treeNodes[neighbor].GCost)
                        {
                            // Update parent if better path found
                            var oldParent = treeNodes[neighbor].Parent;
                            if (oldParent != null)
                                oldParent.Children.Remove(treeNodes[neighbor]);
                            
                            treeNodes[neighbor].Parent = treeNodes[currentNodeName];
                            treeNodes[neighbor].GCost = newGCost;
                            treeNodes[neighbor].Level = treeNodes[currentNodeName].Level + 1;
                            treeNodes[currentNodeName].Children.Add(treeNodes[neighbor]);
                        }
                    }
                }
            }

            result.NodeCosts = GetNodeCosts(allNodes);
            return result;
        }

        private GraphSearchResult AStarSearch()
        {
            nodeIdCounter = 0;
            var result = new GraphSearchResult();
            var openSet = new PriorityQueue<Node, int>();
            var closedSet = new HashSet<string>();
            var allNodes = new Dictionary<string, Node>();
            var treeNodes = new Dictionary<string, TreeNodeGraph>();

            var startNode = new Node(graph.StartNode);
            startNode.HCost = graph.GetHeuristic(graph.StartNode);
            
            var startTreeNode = new TreeNodeGraph(graph.StartNode, nodeIdCounter++);
            startTreeNode.GCost = 0;
            startTreeNode.HCost = startNode.HCost;
            startTreeNode.Level = 0;
            result.SearchTree = startTreeNode;
            treeNodes[graph.StartNode] = startTreeNode;
            result.ExplorationOrder.Add(startTreeNode);

            openSet.Enqueue(startNode, startNode.FCost);
            allNodes[graph.StartNode] = startNode;

            while (openSet.Count > 0)
            {
                var currentNode = openSet.Dequeue();
                var currentNodeName = currentNode.NodeName;

                if (closedSet.Contains(currentNodeName))
                    continue;

                closedSet.Add(currentNodeName);
                result.ExploredNodes.Add(currentNodeName);

                if (currentNodeName == graph.GoalNode)
                {
                    result.Success = true;
                    result.Path = ReconstructPath(currentNode);
                    result.PathCost = currentNode.GCost;
                    result.NodeCosts = GetNodeCosts(allNodes);
                    treeNodes[currentNodeName].IsGoal = true;
                    MarkPathInTree(treeNodes, result.Path);
                    return result;
                }

                foreach (var neighbor in graph.GetNeighbors(currentNodeName))
                {
                    if (closedSet.Contains(neighbor))
                        continue;

                    int newGCost = currentNode.GCost + graph.GetEdgeCost(currentNodeName, neighbor);
                    int hCost = graph.GetHeuristic(neighbor);
                    int newFCost = newGCost + hCost;

                    if (!allNodes.ContainsKey(neighbor) || newGCost < allNodes[neighbor].GCost)
                    {
                        var neighborNode = new Node(neighbor)
                        {
                            GCost = newGCost,
                            HCost = hCost,
                            Parent = currentNode
                        };

                        allNodes[neighbor] = neighborNode;
                        openSet.Enqueue(neighborNode, newFCost);
                        
                        // Add to tree (only if new or better path)
                        if (!treeNodes.ContainsKey(neighbor))
                        {
                            var neighborTreeNode = new TreeNodeGraph(neighbor, nodeIdCounter++);
                            neighborTreeNode.GCost = newGCost;
                            neighborTreeNode.HCost = hCost;
                            neighborTreeNode.Parent = treeNodes[currentNodeName];
                            neighborTreeNode.Level = treeNodes[currentNodeName].Level + 1;
                            treeNodes[currentNodeName].Children.Add(neighborTreeNode);
                            treeNodes[neighbor] = neighborTreeNode;
                            result.ExplorationOrder.Add(neighborTreeNode);
                        }
                        else if (newGCost < treeNodes[neighbor].GCost)
                        {
                            // Update parent if better path found
                            var oldParent = treeNodes[neighbor].Parent;
                            if (oldParent != null)
                                oldParent.Children.Remove(treeNodes[neighbor]);
                            
                            treeNodes[neighbor].Parent = treeNodes[currentNodeName];
                            treeNodes[neighbor].GCost = newGCost;
                            treeNodes[neighbor].HCost = hCost;
                            treeNodes[neighbor].Level = treeNodes[currentNodeName].Level + 1;
                            treeNodes[currentNodeName].Children.Add(treeNodes[neighbor]);
                        }
                    }
                }
            }

            result.NodeCosts = GetNodeCosts(allNodes);
            return result;
        }

        private void MarkPathInTree(Dictionary<string, TreeNodeGraph> treeNodes, List<string> path)
        {
            foreach (var nodeName in path)
            {
                if (treeNodes.ContainsKey(nodeName))
                {
                    treeNodes[nodeName].IsInPath = true;
                }
            }
        }

        private List<string> ReconstructPath(Node goalNode)
        {
            var path = new List<string>();
            var current = goalNode;

            while (current != null)
            {
                path.Add(current.NodeName);
                current = current.Parent;
            }

            path.Reverse();
            return path;
        }

        private Dictionary<string, int> GetNodeCosts(Dictionary<string, Node> allNodes)
        {
            var costs = new Dictionary<string, int>();
            foreach (var kvp in allNodes)
            {
                costs[kvp.Key] = kvp.Value.GCost;
            }
            return costs;
        }
    }
}
