using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InformSearch;

public partial class Form1 : Form
{
    // Graph properties
    private SearchGraph graph = null!;
    private Panel graphPanel = null!;
    private Dictionary<string, Button> nodeButtons = new Dictionary<string, Button>();
    
    // Store last search result for tree visualization
    private GraphSearchResult? lastSearchResult = null;
    private string lastAlgorithmName = "";
    
    // Colors for better visualization
    private readonly Color StartColor = Color.LimeGreen;
    private readonly Color GoalColor = Color.Crimson;
    private readonly Color ExploredColor = Color.Gold;
    private readonly Color PathColor = Color.Orange;
    private readonly Color DefaultColor = Color.LightSteelBlue;

    public Form1()
    {
        InitializeComponent();
        InitializeGraph();
    }
    
    private void InitializeGraph()
    {
        graph = new SearchGraph();
        
        // Clear and setup the graph panel
        panelGrid.Controls.Clear();
        nodeButtons.Clear();
        
        // Create buttons for each node
        foreach (var kvp in graph.Nodes)
        {
            var nodeName = kvp.Key;
            var node = kvp.Value;
            
            Button btn = new Button
            {
                Size = new Size(100, 100), // Even larger buttons for maximum visibility
                Location = node.Position,
                BackColor = nodeName == graph.StartNode ? StartColor : 
                           nodeName == graph.GoalNode ? GoalColor : DefaultColor,
                Text = nodeName,
                Font = new Font("Arial", 20, FontStyle.Bold), // Even larger font
                FlatStyle = FlatStyle.Flat,
                Tag = nodeName,
                ForeColor = Color.Black
            };
            
            // Add thick black border for maximum visibility
            btn.FlatAppearance.BorderColor = Color.Black;
            btn.FlatAppearance.BorderSize = 4;
            
            nodeButtons[nodeName] = btn;
            panelGrid.Controls.Add(btn);
        }
        
        // Draw edges
        graphPanel = new Panel
        {
            Size = new Size(900, 600), // Much larger panel for better spacing
            Location = new Point(0, 0),
            BackColor = Color.White // White background for better contrast
        };
        graphPanel.Paint += GraphPanel_Paint;
        panelGrid.Controls.Add(graphPanel);
        graphPanel.SendToBack();
        
        panelGrid.Size = new Size(900, 600);
    }
    
    
    private void GraphPanel_Paint(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        
        // Draw edges between nodes
        foreach (var kvp in graph.Nodes)
        {
            var fromNode = kvp.Value;
            var fromCenter = new Point(fromNode.Position.X + 50, fromNode.Position.Y + 50); // Center of larger buttons
            
            foreach (var edge in fromNode.Edges)
            {
                var toNode = graph.Nodes[edge.Key];
                var toCenter = new Point(toNode.Position.X + 50, toNode.Position.Y + 50); // Center of larger buttons
                
                // Calculate edge endpoints to not overlap with buttons
                var direction = new PointF(toCenter.X - fromCenter.X, toCenter.Y - fromCenter.Y);
                var length = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
                var unitDir = new PointF(direction.X / length, direction.Y / length);
                
                var startPoint = new Point(
                    (int)(fromCenter.X + unitDir.X * 50), // Adjusted for larger buttons
                    (int)(fromCenter.Y + unitDir.Y * 50)
                );
                var endPoint = new Point(
                    (int)(toCenter.X - unitDir.X * 50),
                    (int)(toCenter.Y - unitDir.Y * 50)
                );
                
                // Draw arrow with very thick line for maximum visibility
                using (var pen = new Pen(Color.DarkBlue, 5))
                {
                    g.DrawLine(pen, startPoint, endPoint);
                    
                    // Draw arrow head
                    DrawArrowHead(g, pen, startPoint, endPoint);
                }
                
                // Draw cost label with larger font and better background
                var midPoint = new Point((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
                using (var brush = new SolidBrush(Color.White))
                using (var textBrush = new SolidBrush(Color.DarkRed))
                using (var font = new Font("Arial", 16, FontStyle.Bold)) // Larger font for cost
                {
                    string costText = edge.Value.ToString();
                    var textSize = g.MeasureString(costText, font);
                    var textRect = new RectangleF(
                        midPoint.X - textSize.Width / 2 - 4,
                        midPoint.Y - textSize.Height / 2 - 4,
                        textSize.Width + 8,
                        textSize.Height + 8
                    );
                    
                    // Draw white background with blue border for cost label
                    g.FillEllipse(brush, textRect);
                    g.DrawEllipse(new Pen(Color.DarkBlue, 2), textRect);
                    
                    // Draw cost text
                    g.DrawString(costText, font, textBrush, midPoint.X - textSize.Width / 2, midPoint.Y - textSize.Height / 2);
                }
            }
        }
    }
    
    private void DrawArrowHead(Graphics g, Pen pen, Point from, Point to)
    {
        double angle = Math.Atan2(to.Y - from.Y, to.X - from.X);
        int arrowLength = 20; // Even larger arrow head for better visibility
        double arrowAngle = Math.PI / 3; // Wider arrow angle for better visibility
        
        Point arrowPoint1 = new Point(
            (int)(to.X - arrowLength * Math.Cos(angle - arrowAngle)),
            (int)(to.Y - arrowLength * Math.Sin(angle - arrowAngle))
        );
        
        Point arrowPoint2 = new Point(
            (int)(to.X - arrowLength * Math.Cos(angle + arrowAngle)),
            (int)(to.Y - arrowLength * Math.Sin(angle + arrowAngle))
        );
        
        // Draw arrow head with filled triangle
        using (var brush = new SolidBrush(pen.Color))
        {
            Point[] arrowHead = { to, arrowPoint1, arrowPoint2 };
            g.FillPolygon(brush, arrowHead);
            // Add outline for even better visibility
            g.DrawPolygon(new Pen(Color.Black, 1), arrowHead);
        }
    }
    
    private void GridButton_Click(object? sender, EventArgs e)
    {
        // For the graph visualization, we don't need interactive editing
        // The graph structure is predefined
    }
    
    private void UpdateButtonStates()
    {
        // No longer needed for graph visualization
    }
    
    private void btnSetStart_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Start node is fixed at 'A' for this graph.", "Info");
    }
    
    private void btnSetGoal_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Goal node is fixed at 'G' for this graph.", "Info");
    }
    
    private void btnSetObstacle_Click(object sender, EventArgs e)
    {
        MessageBox.Show("This graph has a predefined structure. No obstacles can be added.", "Info");
    }
    
    private void btnReset_Click(object sender, EventArgs e)
    {
        lastSearchResult = null;
        lastAlgorithmName = "";
        
        // Reset node colors
        foreach (var kvp in nodeButtons)
        {
            var nodeName = kvp.Key;
            var button = kvp.Value;
            
            if (nodeName == graph.StartNode)
                button.BackColor = StartColor;
            else if (nodeName == graph.GoalNode)
                button.BackColor = GoalColor;
            else
                button.BackColor = DefaultColor;
            
            button.Text = nodeName;
        }
        
        lblAlgorithm.Text = "Algorithm: None";
        lblCost.Text = "Path Cost: 0";
        lblSteps.Text = "Steps Explored: 0";
        btnShowTree.Enabled = false;
        graphPanel.Invalidate();
    }
    
    private async void btnGreedy_Click(object sender, EventArgs e)
    {
        if (!ValidateInput()) return;
        await RunSearchAlgorithm("Greedy", GreedySearch);
    }
    
    private async void btnUCS_Click(object sender, EventArgs e)
    {
        if (!ValidateInput()) return;
        await RunSearchAlgorithm("UCS", UniformCostSearch);
    }
    
    private async void btnAStar_Click(object sender, EventArgs e)
    {
        if (!ValidateInput()) return;
        await RunSearchAlgorithm("A*", AStarSearch);
    }
    
    private bool ValidateInput()
    {
        // Graph structure is predefined, so always valid
        return true;
    }
    
    private async Task RunSearchAlgorithm(string algorithmName, Func<GraphSearchResult> algorithm)
    {
        // Clear previous results
        ClearVisualization();
        
        lblAlgorithm.Text = $"Algorithm: {algorithmName}";
        lblCost.Text = "Path Cost: Searching...";
        lblSteps.Text = "Steps Explored: 0";
        
        // Disable buttons during search
        SetButtonsEnabled(false);
        
        try
        {
            var result = algorithm();
            lastSearchResult = result;
            lastAlgorithmName = algorithmName;
            
            if (result.Success)
            {
                await VisualizeSearch(result);
                lblCost.Text = $"Path Cost: {result.PathCost}";
                lblSteps.Text = $"Steps Explored: {result.ExploredNodes.Count}";
                btnShowTree.Enabled = true;
            }
            else
            {
                MessageBox.Show("No path found!", "Search Result");
                lblCost.Text = "Path Cost: No path found";
                lblSteps.Text = $"Steps Explored: {result.ExploredNodes.Count}";
                btnShowTree.Enabled = result.SearchTree != null;
            }
        }
        finally
        {
            SetButtonsEnabled(true);
        }
    }
    
    private void SetButtonsEnabled(bool enabled)
    {
        btnGreedy.Enabled = enabled;
        btnUCS.Enabled = enabled;
        btnAStar.Enabled = enabled;
        btnReset.Enabled = enabled;
        btnSetStart.Enabled = enabled;
        btnSetGoal.Enabled = enabled;
        btnSetObstacle.Enabled = enabled;
        if (!enabled)
            btnShowTree.Enabled = false;
    }
    
    private void btnShowTree_Click(object sender, EventArgs e)
    {
        if (lastSearchResult?.SearchTree != null)
        {
            var treeForm = new TreeVisualizationForm();
            treeForm.ShowSearchTree(lastSearchResult, lastAlgorithmName);
            treeForm.Show();
        }
        else
        {
            MessageBox.Show("No search tree available. Please run a search algorithm first.", "No Tree Data");
        }
    }
    
    private void ClearVisualization()
    {
        // Reset node colors
        foreach (var kvp in nodeButtons)
        {
            var nodeName = kvp.Key;
            var button = kvp.Value;
            
            if (nodeName == graph.StartNode)
                button.BackColor = StartColor;
            else if (nodeName == graph.GoalNode)
                button.BackColor = GoalColor;
            else
                button.BackColor = DefaultColor;
            
            button.Text = nodeName;
        }
        graphPanel.Invalidate();
    }
    
    private async Task VisualizeSearch(GraphSearchResult result)
    {
        // First, show explored nodes
        foreach (var nodeName in result.ExploredNodes)
        {
            if (nodeName != graph.StartNode && nodeName != graph.GoalNode)
            {
                nodeButtons[nodeName].BackColor = ExploredColor;
                await Task.Delay(500); // Animation delay
                Application.DoEvents();
            }
        }
        
        // Then, highlight the final path
        if (result.Path != null && result.Path.Count > 0)
        {
            await Task.Delay(500);
            foreach (var nodeName in result.Path)
            {
                if (nodeName != graph.StartNode && nodeName != graph.GoalNode)
                {
                    nodeButtons[nodeName].BackColor = PathColor;
                    await Task.Delay(300);
                    Application.DoEvents();
                }
            }
        }
        
        // Show costs for explored nodes
        foreach (var kvp in result.NodeCosts)
        {
            string nodeName = kvp.Key;
            int cost = kvp.Value;
            if (nodeName != graph.StartNode && nodeName != graph.GoalNode && nodeButtons.ContainsKey(nodeName))
            {
                nodeButtons[nodeName].Text = $"{nodeName}\n{cost}"; // Correct line break
            }
        }
    }
}
