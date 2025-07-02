using System;
using System.Drawing;
using System.Windows.Forms;

namespace InformSearch
{
    public partial class MainSelector : Form
    {
        public MainSelector()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Algorithm Visualizer Selector";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Title label
            Label titleLabel = new Label
            {
                Text = "Choose Algorithm Visualizer",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(50, 30),
                Size = new Size(300, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Graph Search button
            Button btnGraphSearch = new Button
            {
                Text = "Graph Search Algorithms\n(Greedy, UCS, A*)",
                Location = new Point(50, 80),
                Size = new Size(280, 60),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightBlue,
                UseVisualStyleBackColor = false
            };
            btnGraphSearch.Click += (s, e) =>
            {
                this.Hide();
                var form1 = new Form1();
                form1.FormClosed += (sender, args) => this.Show();
                form1.Show();
            };

            // TSP button
            Button btnTSP = new Button
            {
                Text = "Traveling Salesman Problem\n(TSP Visualizer)",
                Location = new Point(50, 160),
                Size = new Size(280, 60),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightGreen,
                UseVisualStyleBackColor = false
            };
            btnTSP.Click += (s, e) =>
            {
                this.Hide();
                var tspForm = new TSPForm();
                tspForm.FormClosed += (sender, args) => this.Show();
                tspForm.Show();
            };

            // Exit button
            Button btnExit = new Button
            {
                Text = "Exit",
                Location = new Point(150, 240),
                Size = new Size(80, 30),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = Color.LightCoral
            };
            btnExit.Click += (s, e) => this.Close();

            // Add controls
            this.Controls.AddRange(new Control[] {
                titleLabel,
                btnGraphSearch,
                btnTSP,
                btnExit
            });
        }
    }
}
