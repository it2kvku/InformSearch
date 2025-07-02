using System;
using System.Collections.Generic;
using System.Drawing;

namespace InformSearch
{
    public class GraphNode
    {
        public string Name { get; set; }
        public Point Position { get; set; }
        public Dictionary<string, int> Edges { get; set; } = new Dictionary<string, int>();
        public int HeuristicToGoal { get; set; }

        public GraphNode(string name, Point position, int heuristic = 0)
        {
            Name = name;
            Position = position;
            HeuristicToGoal = heuristic;
        }

        public void AddEdge(string target, int cost)
        {
            Edges[target] = cost;
        }
    }

    public class SearchGraph
    {
        public Dictionary<string, GraphNode> Nodes { get; private set; }
        public string StartNode { get; private set; }
        public string GoalNode { get; private set; }

        public SearchGraph()
        {
            Nodes = new Dictionary<string, GraphNode>();
            InitializeGraph();
        }

        private void InitializeGraph()
        {
            // Create nodes with well-spaced layout for better visibility
            // Heuristic values are estimated distances to goal G
            Nodes["A"] = new GraphNode("A", new Point(80, 300), 60);   // Start node - left side
            Nodes["B"] = new GraphNode("B", new Point(280, 150), 50);  // Upper middle
            Nodes["C"] = new GraphNode("C", new Point(280, 450), 45);  // Lower middle
            Nodes["D"] = new GraphNode("D", new Point(280, 300), 40);  // Center middle
            Nodes["E"] = new GraphNode("E", new Point(480, 200), 30);  // Upper right
            Nodes["F"] = new GraphNode("F", new Point(480, 400), 20);  // Lower right
            Nodes["G"] = new GraphNode("G", new Point(680, 300), 0);   // Goal node - right side
            Nodes["H"] = new GraphNode("H", new Point(680, 150), 15);  // Upper far right

            // Add edges with costs as described
            // From A
            Nodes["A"].AddEdge("B", 10); // A → B (assuming cost 10)
            Nodes["A"].AddEdge("D", 15); // A → D (assuming cost 15)
            Nodes["A"].AddEdge("C", 25); // A → C (given cost 25)

            // From B
            Nodes["B"].AddEdge("E", 32); // B → E (given cost 32)

            // From C
            Nodes["C"].AddEdge("E", 20); // C → E (assuming cost 20)
            Nodes["C"].AddEdge("F", 30); // C → F (assuming cost 30)

            // From D
            Nodes["D"].AddEdge("F", 35); // D → F (given cost 35)

            // From E
            Nodes["E"].AddEdge("H", 19); // E → H (given cost 19)

            // From F
            Nodes["F"].AddEdge("G", 17); // F → G (given cost 17)

            // From H
            Nodes["H"].AddEdge("G", 10); // H → G (given cost 10)

            StartNode = "A";
            GoalNode = "G";
        }

        public List<string> GetNeighbors(string nodeName)
        {
            if (Nodes.ContainsKey(nodeName))
            {
                return new List<string>(Nodes[nodeName].Edges.Keys);
            }
            return new List<string>();
        }

        public int GetEdgeCost(string from, string to)
        {
            if (Nodes.ContainsKey(from) && Nodes[from].Edges.ContainsKey(to))
            {
                return Nodes[from].Edges[to];
            }
            return int.MaxValue;
        }

        public int GetHeuristic(string nodeName)
        {
            if (Nodes.ContainsKey(nodeName))
            {
                return Nodes[nodeName].HeuristicToGoal;
            }
            return 0;
        }
    }

    // Update TreeNode to work with string node names
    public class TreeNodeGraph
    {
        public string NodeName { get; set; }
        public TreeNodeGraph? Parent { get; set; }
        public List<TreeNodeGraph> Children { get; set; } = new List<TreeNodeGraph>();
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => GCost + HCost;
        public bool IsGoal { get; set; }
        public bool IsInPath { get; set; }
        public int Level { get; set; }
        public int Id { get; set; }

        public TreeNodeGraph(string nodeName, int id)
        {
            NodeName = nodeName;
            Id = id;
        }
    }

    // Update SearchResult to work with the graph
    public class GraphSearchResult
    {
        public bool Success { get; set; }
        public List<string> Path { get; set; } = new List<string>();
        public HashSet<string> ExploredNodes { get; set; } = new HashSet<string>();
        public int PathCost { get; set; }
        public Dictionary<string, int> NodeCosts { get; set; } = new Dictionary<string, int>();
        public TreeNodeGraph? SearchTree { get; set; }
        public List<TreeNodeGraph> ExplorationOrder { get; set; } = new List<TreeNodeGraph>();
    }
}
