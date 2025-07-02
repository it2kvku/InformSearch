using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace InformSearch
{
    public partial class TreeVisualizationForm : Form
    {
        private TreeNodeGraph? rootNode;
        private GraphSearchResult? searchResult;
        private Panel treePanel = null!;
        private System.Windows.Forms.Timer animationTimer = null!;
        private int animationStep = 0;
        private List<TreeNodeGraph> nodesToDraw = new List<TreeNodeGraph>();
        
        // Drawing properties
        private const int NodeSize = 40;
        private const int LevelHeight = 80;
        private const int NodeSpacing = 60;
        private readonly Font nodeFont = new Font("Arial", 8, FontStyle.Bold);
        
        // Colors
        private readonly Color StartNodeColor = Color.LightGreen;
        private readonly Color GoalNodeColor = Color.LightCoral;
        private readonly Color PathNodeColor = Color.Gold;
        private readonly Color RegularNodeColor = Color.LightBlue;
        private readonly Color EdgeColor = Color.Black;
        private readonly Color PathEdgeColor = Color.Red;

        public TreeVisualizationForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "Search Tree Visualization";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            treePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                AutoScroll = true
            };
            treePanel.Paint += TreePanel_Paint;
            this.Controls.Add(treePanel);
            
            animationTimer = new System.Windows.Forms.Timer
            {
                Interval = 500 // 500ms between each node reveal
            };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        public void ShowSearchTree(GraphSearchResult result, string algorithmName)
        {
            this.Text = $"Search Tree Visualization - {algorithmName}";
            searchResult = result;
            rootNode = result.SearchTree;
            
            if (rootNode != null)
            {
                CalculateNodePositions();
                StartAnimation();
            }
        }

        private void StartAnimation()
        {
            animationStep = 0;
            nodesToDraw.Clear();
            
            if (searchResult?.ExplorationOrder != null)
            {
                animationTimer.Start();
            }
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            if (searchResult?.ExplorationOrder != null && animationStep < searchResult.ExplorationOrder.Count)
            {
                nodesToDraw.Add(searchResult.ExplorationOrder[animationStep]);
                animationStep++;
                treePanel.Invalidate();
            }
            else
            {
                animationTimer.Stop();
            }
        }

        private void CalculateNodePositions()
        {
            if (rootNode == null) return;
            
            var nodePositions = new Dictionary<TreeNodeGraph, Point>();
            var levelWidths = new Dictionary<int, int>();
            
            // Calculate level widths
            CalculateLevelWidths(rootNode, levelWidths);
            
            // Position nodes
            PositionNodesRecursively(rootNode, nodePositions, levelWidths, 0);
            
            // Update panel size if needed
            UpdatePanelSize(nodePositions);
        }

        private void CalculateLevelWidths(TreeNodeGraph node, Dictionary<int, int> levelWidths)
        {
            if (!levelWidths.ContainsKey(node.Level))
                levelWidths[node.Level] = 0;
            levelWidths[node.Level]++;
            
            foreach (var child in node.Children)
                CalculateLevelWidths(child, levelWidths);
        }

        private void PositionNodesRecursively(TreeNodeGraph node, Dictionary<TreeNodeGraph, Point> positions, 
            Dictionary<int, int> levelWidths, int indexInLevel)
        {
            int totalWidth = levelWidths[node.Level] * NodeSpacing;
            int startX = (treePanel.Width - totalWidth) / 2;
            int x = startX + indexInLevel * NodeSpacing + NodeSpacing / 2;
            int y = 50 + node.Level * LevelHeight;
            
            positions[node] = new Point(x, y);
            
            int childIndex = 0;
            foreach (var child in node.Children)
            {
                PositionNodesRecursively(child, positions, levelWidths, 
                    GetChildIndexInLevel(child, levelWidths));
                childIndex++;
            }
        }

        private int GetChildIndexInLevel(TreeNodeGraph child, Dictionary<int, int> levelWidths)
        {
            // This is a simplified version - in a real implementation, 
            // you'd want more sophisticated positioning
            return child.Id % levelWidths[child.Level];
        }

        private void UpdatePanelSize(Dictionary<TreeNodeGraph, Point> positions)
        {
            if (positions.Count == 0) return;
            
            int maxX = positions.Values.Max(p => p.X) + NodeSize + 50;
            int maxY = positions.Values.Max(p => p.Y) + NodeSize + 50;
            
            treePanel.AutoScrollMinSize = new Size(maxX, maxY);
        }

        private void TreePanel_Paint(object? sender, PaintEventArgs e)
        {
            if (rootNode == null) return;
            
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            var nodePositions = new Dictionary<TreeNodeGraph, Point>();
            var levelWidths = new Dictionary<int, int>();
            
            CalculateLevelWidths(rootNode, levelWidths);
            PositionNodesRecursively(rootNode, nodePositions, levelWidths, 0);
            
            // Draw edges first
            DrawEdges(g, nodePositions);
            
            // Draw nodes
            DrawNodes(g, nodePositions);
        }

        private void DrawEdges(Graphics g, Dictionary<TreeNodeGraph, Point> positions)
        {
            foreach (var node in nodesToDraw)
            {
                if (node.Parent != null && nodesToDraw.Contains(node.Parent))
                {
                    var parentPos = positions[node.Parent];
                    var childPos = positions[node];
                    
                    var pen = node.IsInPath && node.Parent.IsInPath ? 
                        new Pen(PathEdgeColor, 3) : new Pen(EdgeColor, 2);
                    
                    // Draw line from parent center to child center
                    g.DrawLine(pen, 
                        parentPos.X + NodeSize / 2, parentPos.Y + NodeSize / 2,
                        childPos.X + NodeSize / 2, childPos.Y + NodeSize / 2);
                    
                    pen.Dispose();
                }
            }
        }

        private void DrawNodes(Graphics g, Dictionary<TreeNodeGraph, Point> positions)
        {
            foreach (var node in nodesToDraw)
            {
                if (!positions.ContainsKey(node)) continue;
                
                var pos = positions[node];
                var rect = new Rectangle(pos.X, pos.Y, NodeSize, NodeSize);
                
                // Choose color based on node type
                Color nodeColor;
                if (node.Level == 0) // Start node
                    nodeColor = StartNodeColor;
                else if (node.IsGoal)
                    nodeColor = GoalNodeColor;
                else if (node.IsInPath)
                    nodeColor = PathNodeColor;
                else
                    nodeColor = RegularNodeColor;
                
                // Draw node circle
                using (var brush = new SolidBrush(nodeColor))
                {
                    g.FillEllipse(brush, rect);
                }
                
                g.DrawEllipse(Pens.Black, rect);
                
                // Draw node text
                string nodeText = node.NodeName;
                if (searchResult != null)
                {
                    // Add cost information based on algorithm
                    if (node.GCost > 0 || node.HCost > 0)
                    {
                        nodeText += $"\\nG:{node.GCost}";
                        if (node.HCost > 0)
                            nodeText += $" H:{node.HCost}";
                        if (node.FCost > 0)
                            nodeText += $"\\nF:{node.FCost}";
                    }
                }
                
                var textRect = new RectangleF(pos.X - 20, pos.Y - 30, NodeSize + 40, 25);
                using (var brush = new SolidBrush(Color.Black))
                {
                    var format = new StringFormat 
                    { 
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(nodeText, nodeFont, brush, textRect, format);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                animationTimer?.Dispose();
                nodeFont?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
