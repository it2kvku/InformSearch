using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InformSearch
{
    public partial class TSPForm : Form
    {
        private TSPGraph graph;
        private Dictionary<int, Point> cityPositions;
        private Panel graphPanel;
        private TSPResult lastResult;
        private string currentAlgorithm = "";
        private bool isAnimating = false;

        // Colors for visualization
        private readonly Color CityColor = Color.LightBlue;
        private readonly Color EdgeColor = Color.LightGray;
        private readonly Color CurrentPathColor = Color.Red;
        private readonly Color FinalPathColor = Color.Green;
        private readonly Color HighlightCityColor = Color.Yellow;

        // Animation speed control for TSP
        private int tspAnimationSpeed = 200; // Default speed in milliseconds
        
        // Speed constants for TSP
        private const int TSP_SPEED_SLOW = 800;
        private const int TSP_SPEED_NORMAL = 200;
        private const int TSP_SPEED_FAST = 50;
        private const int TSP_SPEED_INSTANT = 1;

        public TSPForm()
        {
            InitializeComponent();
            InitializeGraph();
        }

        private void InitializeGraph()
        {
            graph = new TSPGraph();
            cityPositions = new Dictionary<int, Point>();
            
            // Setup graph panel
            graphPanel = new Panel
            {
                Size = new Size(600, 600),
                Location = new Point(10, 10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            graphPanel.Paint += GraphPanel_Paint;
            this.Controls.Add(graphPanel);
        }

        private void GenerateCityPositions()
        {
            cityPositions.Clear();
            if (graph.Cities.Count == 0) return;

            // Arrange cities in a circle
            Point center = new Point(300, 300);
            int radius = 200;
            double angleStep = 2 * Math.PI / graph.Cities.Count;

            for (int i = 0; i < graph.Cities.Count; i++)
            {
                int cityId = graph.Cities[i];
                double angle = i * angleStep;
                int x = (int)(center.X + radius * Math.Cos(angle));
                int y = (int)(center.Y + radius * Math.Sin(angle));
                cityPositions[cityId] = new Point(x, y);
            }
        }

        private void GraphPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (graph.Cities.Count == 0) return;

            // Draw edges
            DrawEdges(g);

            // Draw cities
            DrawCities(g);

            // Draw current path if animating
            if (isAnimating && lastResult?.CurrentPath != null)
            {
                DrawCurrentPath(g, lastResult.CurrentPath);
            }

            // Draw final path
            if (lastResult?.FinalPath != null && !isAnimating)
            {
                DrawFinalPath(g, lastResult.FinalPath);
            }
        }

        private void DrawEdges(Graphics g)
        {
            using (var pen = new Pen(EdgeColor, 1))
            using (var font = new Font("Arial", 10))
            using (var textBrush = new SolidBrush(Color.Black))
            {
                foreach (var edge in graph.Edges)
                {
                    if (cityPositions.ContainsKey(edge.From) && cityPositions.ContainsKey(edge.To))
                    {
                        Point from = cityPositions[edge.From];
                        Point to = cityPositions[edge.To];

                        g.DrawLine(pen, from, to);

                        // Draw cost label
                        Point midPoint = new Point((from.X + to.X) / 2, (from.Y + to.Y) / 2);
                        string costText = edge.Cost.ToString();
                        var textSize = g.MeasureString(costText, font);
                        
                        // Draw white background for text
                        using (var bgBrush = new SolidBrush(Color.White))
                        {
                            g.FillRectangle(bgBrush, 
                                midPoint.X - textSize.Width / 2 - 2,
                                midPoint.Y - textSize.Height / 2 - 2,
                                textSize.Width + 4,
                                textSize.Height + 4);
                        }
                        
                        g.DrawString(costText, font, textBrush, 
                            midPoint.X - textSize.Width / 2, 
                            midPoint.Y - textSize.Height / 2);
                    }
                }
            }
        }

        private void DrawCities(Graphics g)
        {
            using (var cityBrush = new SolidBrush(CityColor))
            using (var pen = new Pen(Color.Black, 2))
            using (var font = new Font("Arial", 12, FontStyle.Bold))
            using (var textBrush = new SolidBrush(Color.Black))
            {
                foreach (var city in graph.Cities)
                {
                    if (cityPositions.ContainsKey(city))
                    {
                        Point pos = cityPositions[city];
                        Rectangle cityRect = new Rectangle(pos.X - 15, pos.Y - 15, 30, 30);

                        // Highlight current city during animation
                        if (isAnimating && lastResult?.CurrentCity == city)
                        {
                            using (var highlightBrush = new SolidBrush(HighlightCityColor))
                            {
                                g.FillEllipse(highlightBrush, cityRect);
                            }
                        }
                        else
                        {
                            g.FillEllipse(cityBrush, cityRect);
                        }

                        g.DrawEllipse(pen, cityRect);

                        // Draw city ID
                        string cityText = city.ToString();
                        var textSize = g.MeasureString(cityText, font);
                        g.DrawString(cityText, font, textBrush,
                            pos.X - textSize.Width / 2,
                            pos.Y - textSize.Height / 2);
                    }
                }
            }
        }

        private void DrawCurrentPath(Graphics g, List<int> path)
        {
            if (path.Count < 2) return;

            using (var pen = new Pen(CurrentPathColor, 4))
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    if (cityPositions.ContainsKey(path[i]) && cityPositions.ContainsKey(path[i + 1]))
                    {
                        Point from = cityPositions[path[i]];
                        Point to = cityPositions[path[i + 1]];
                        g.DrawLine(pen, from, to);
                        
                        // Draw arrow
                        DrawArrow(g, pen, from, to);
                    }
                }
            }
        }

        private void DrawFinalPath(Graphics g, List<int> path)
        {
            if (path.Count < 2) return;

            using (var pen = new Pen(FinalPathColor, 3))
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    if (cityPositions.ContainsKey(path[i]) && cityPositions.ContainsKey(path[i + 1]))
                    {
                        Point from = cityPositions[path[i]];
                        Point to = cityPositions[path[i + 1]];
                        g.DrawLine(pen, from, to);
                        
                        // Draw arrow
                        DrawArrow(g, pen, from, to);
                    }
                }
            }
        }

        private void DrawArrow(Graphics g, Pen pen, Point from, Point to)
        {
            double angle = Math.Atan2(to.Y - from.Y, to.X - from.X);
            int arrowLength = 10;
            double arrowAngle = Math.PI / 6;

            Point arrowPoint1 = new Point(
                (int)(to.X - arrowLength * Math.Cos(angle - arrowAngle)),
                (int)(to.Y - arrowLength * Math.Sin(angle - arrowAngle))
            );

            Point arrowPoint2 = new Point(
                (int)(to.X - arrowLength * Math.Cos(angle + arrowAngle)),
                (int)(to.Y - arrowLength * Math.Sin(angle + arrowAngle))
            );

            g.DrawLine(pen, to, arrowPoint1);
            g.DrawLine(pen, to, arrowPoint2);
        }

        private void ParseInput()
        {
            try
            {
                string[] lines = txtInput.Lines;
                if (lines.Length < 1) return;

                // Parse first line: n (cities) m (edges)
                string[] firstLine = lines[0].Split(' ');
                int n = int.Parse(firstLine[0]);
                int m = int.Parse(firstLine[1]);

                graph = new TSPGraph();

                // Add cities 1 to n
                for (int i = 1; i <= n; i++)
                {
                    graph.AddCity(i);
                }

                // Parse edges
                for (int i = 1; i <= m && i < lines.Length; i++)
                {
                    string[] edgeParts = lines[i].Split(' ');
                    if (edgeParts.Length >= 3)
                    {
                        int from = int.Parse(edgeParts[0]);
                        int to = int.Parse(edgeParts[1]);
                        int cost = int.Parse(edgeParts[2]);
                        graph.AddEdge(from, to, cost);
                    }
                }

                GenerateCityPositions();
                graphPanel.Invalidate();
                
                lblStatus.Text = $"Loaded {n} cities and {m} edges.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing input: {ex.Message}", "Input Error");
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (graph.Cities.Count == 0)
            {
                MessageBox.Show("Please enter graph data first.", "No Data");
                return;
            }

            btnStart.Enabled = false;
            btnParse.Enabled = false;
            isAnimating = true;

            try
            {
                ITSPAlgorithm algorithm = null;
                
                if (rbGreedy.Checked)
                {
                    algorithm = new GreedyTSP();
                    currentAlgorithm = "Greedy";
                }
                else if (rbUCS.Checked)
                {
                    algorithm = new UCSTSP();
                    currentAlgorithm = "UCS";
                }
                else if (rbAStar.Checked)
                {
                    algorithm = new AStarTSP();
                    currentAlgorithm = "A*";
                }

                if (algorithm != null)
                {
                    lblStatus.Text = $"Running {currentAlgorithm} algorithm...";
                    lastResult = await RunAlgorithmWithAnimation(algorithm);
                    
                    lblResult.Text = $"Path: {string.Join(" -> ", lastResult.FinalPath)} | Cost: {lastResult.TotalCost}";
                    lblStatus.Text = $"{currentAlgorithm} completed.";
                }
            }
            finally
            {
                isAnimating = false;
                btnStart.Enabled = true;
                btnParse.Enabled = true;
                graphPanel.Invalidate();
            }
        }

        private async Task<TSPResult> RunAlgorithmWithAnimation(ITSPAlgorithm algorithm)
        {
            var result = algorithm.Solve(graph);
            
            // Animate the solution process
            foreach (var step in result.Steps)
            {
                lastResult = new TSPResult
                {
                    CurrentPath = step.CurrentPath,
                    CurrentCity = step.CurrentCity,
                    TotalCost = step.CurrentCost
                };
                
                graphPanel.Invalidate();
                await Task.Delay(tspAnimationSpeed); // Use animation speed variable
            }

            // Show final result
            lastResult = result;
            return result;
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            ParseInput();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            graph = new TSPGraph();
            cityPositions.Clear();
            lastResult = null;
            isAnimating = false;
            graphPanel.Invalidate();
            lblResult.Text = "Result: -";
            lblStatus.Text = "Ready";
        }
    }
}
