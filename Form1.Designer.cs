namespace InformSearch;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    
    private Panel panelGrid;
    private Panel panelControls;
    private Button btnGreedy;
    private Button btnUCS;
    private Button btnAStar;
    private Button btnReset;
    private Button btnSetStart;
    private Button btnSetGoal;
    private Button btnSetObstacle;
    private Button btnShowTree;
    private Label lblAlgorithm;
    private Label lblCost;
    private Label lblSteps;
    private ComboBox cmbSpeed;
    private Label lblSpeed;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.panelGrid = new Panel();
        this.btnGreedy = new Button();
        this.btnUCS = new Button();
        this.btnAStar = new Button();
        this.btnReset = new Button();
        this.btnSetStart = new Button();
        this.btnSetGoal = new Button();
        this.btnSetObstacle = new Button();
        this.btnShowTree = new Button();
        this.lblAlgorithm = new Label();
        this.lblCost = new Label();
        this.lblSteps = new Label();
        this.panelControls = new Panel();
        this.cmbSpeed = new ComboBox();
        this.lblSpeed = new Label();
        this.cmbSpeed = new ComboBox();
        this.lblSpeed = new Label();
        this.SuspendLayout();
        
        // 
        // panelGrid
        // 
        this.panelGrid.Location = new Point(12, 12);
        this.panelGrid.Name = "panelGrid";
        this.panelGrid.Size = new Size(900, 600); // Larger graph panel
        this.panelGrid.TabIndex = 0;
        this.panelGrid.BorderStyle = BorderStyle.FixedSingle;
        this.panelGrid.BackColor = Color.White;
        
        // 
        // panelControls
        // 
        this.panelControls.Location = new Point(930, 12); // Moved right to accommodate larger graph
        this.panelControls.Name = "panelControls";
        this.panelControls.Size = new Size(200, 600);
        this.panelControls.TabIndex = 1;
        this.panelControls.BorderStyle = BorderStyle.FixedSingle;
        
        // 
        // btnGreedy
        // 
        this.btnGreedy.Location = new Point(10, 10);
        this.btnGreedy.Name = "btnGreedy";
        this.btnGreedy.Size = new Size(180, 40);
        this.btnGreedy.TabIndex = 0;
        this.btnGreedy.Text = "Greedy Search";
        this.btnGreedy.UseVisualStyleBackColor = true;
        this.btnGreedy.Click += new EventHandler(this.btnGreedy_Click);
        
        // 
        // btnUCS
        // 
        this.btnUCS.Location = new Point(10, 60);
        this.btnUCS.Name = "btnUCS";
        this.btnUCS.Size = new Size(180, 40);
        this.btnUCS.TabIndex = 1;
        this.btnUCS.Text = "UCS Search";
        this.btnUCS.UseVisualStyleBackColor = true;
        this.btnUCS.Click += new EventHandler(this.btnUCS_Click);
        
        // 
        // btnAStar
        // 
        this.btnAStar.Location = new Point(10, 110);
        this.btnAStar.Name = "btnAStar";
        this.btnAStar.Size = new Size(180, 40);
        this.btnAStar.TabIndex = 2;
        this.btnAStar.Text = "A* Search";
        this.btnAStar.UseVisualStyleBackColor = true;
        this.btnAStar.Click += new EventHandler(this.btnAStar_Click);
        
        // 
        // btnReset
        // 
        this.btnReset.Location = new Point(10, 160);
        this.btnReset.Name = "btnReset";
        this.btnReset.Size = new Size(180, 40);
        this.btnReset.TabIndex = 3;
        this.btnReset.Text = "Reset Grid";
        this.btnReset.UseVisualStyleBackColor = true;
        this.btnReset.Click += new EventHandler(this.btnReset_Click);
        
        // 
        // btnSetStart
        // 
        this.btnSetStart.Location = new Point(10, 220);
        this.btnSetStart.Name = "btnSetStart";
        this.btnSetStart.Size = new Size(180, 30);
        this.btnSetStart.TabIndex = 4;
        this.btnSetStart.Text = "Set Start Point";
        this.btnSetStart.UseVisualStyleBackColor = true;
        this.btnSetStart.Click += new EventHandler(this.btnSetStart_Click);
        
        // 
        // btnSetGoal
        // 
        this.btnSetGoal.Location = new Point(10, 260);
        this.btnSetGoal.Name = "btnSetGoal";
        this.btnSetGoal.Size = new Size(180, 30);
        this.btnSetGoal.TabIndex = 5;
        this.btnSetGoal.Text = "Set Goal Point";
        this.btnSetGoal.UseVisualStyleBackColor = true;
        this.btnSetGoal.Click += new EventHandler(this.btnSetGoal_Click);
        
        // 
        // btnSetObstacle
        // 
        this.btnSetObstacle.Location = new Point(10, 300);
        this.btnSetObstacle.Name = "btnSetObstacle";
        this.btnSetObstacle.Size = new Size(180, 30);
        this.btnSetObstacle.TabIndex = 6;
        this.btnSetObstacle.Text = "Set Obstacles";
        this.btnSetObstacle.UseVisualStyleBackColor = true;
        this.btnSetObstacle.Click += new EventHandler(this.btnSetObstacle_Click);
        
        // 
        // btnShowTree
        // 
        this.btnShowTree.Location = new Point(10, 340);
        this.btnShowTree.Name = "btnShowTree";
        this.btnShowTree.Size = new Size(180, 30);
        this.btnShowTree.TabIndex = 7;
        this.btnShowTree.Text = "Show Search Tree";
        this.btnShowTree.UseVisualStyleBackColor = true;
        this.btnShowTree.Enabled = false;
        this.btnShowTree.Click += new EventHandler(this.btnShowTree_Click);
        
        // 
        // lblAlgorithm
        // 
        this.lblAlgorithm.Location = new Point(10, 380);
        this.lblAlgorithm.Name = "lblAlgorithm";
        this.lblAlgorithm.Size = new Size(180, 20);
        this.lblAlgorithm.TabIndex = 8;
        this.lblAlgorithm.Text = "Algorithm: None";
        
        // 
        // lblCost
        // 
        this.lblCost.Location = new Point(10, 410);
        this.lblCost.Name = "lblCost";
        this.lblCost.Size = new Size(180, 20);
        this.lblCost.TabIndex = 9;
        this.lblCost.Text = "Path Cost: 0";
        
        // 
        // lblSteps
        // 
        this.lblSteps.Location = new Point(10, 440);
        this.lblSteps.Name = "lblSteps";
        this.lblSteps.Size = new Size(180, 20);
        this.lblSteps.TabIndex = 10;
        this.lblSteps.Text = "Steps Explored: 0";
        
        // 
        // lblSpeed
        // 
        this.lblSpeed.Location = new Point(10, 470);
        this.lblSpeed.Name = "lblSpeed";
        this.lblSpeed.Size = new Size(180, 20);
        this.lblSpeed.TabIndex = 11;
        this.lblSpeed.Text = "Animation Speed:";
        
        // 
        // cmbSpeed
        // 
        this.cmbSpeed.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbSpeed.FormattingEnabled = true;
        this.cmbSpeed.Items.AddRange(new object[] {
            "Instant",
            "Fast", 
            "Normal",
            "Slow"});
        this.cmbSpeed.Location = new Point(10, 495);
        this.cmbSpeed.Name = "cmbSpeed";
        this.cmbSpeed.Size = new Size(180, 28);
        this.cmbSpeed.TabIndex = 12;
        this.cmbSpeed.SelectedIndex = 2; // Default to "Normal"
        
        // 
        // cmbSpeed
        // 
        this.cmbSpeed.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbSpeed.FormattingEnabled = true;
        this.cmbSpeed.Items.AddRange(new object[] {
        "Slow",
        "Normal",
        "Fast"});
        this.cmbSpeed.Location = new Point(10, 470);
        this.cmbSpeed.Name = "cmbSpeed";
        this.cmbSpeed.Size = new Size(180, 28);
        this.cmbSpeed.TabIndex = 11;
        this.cmbSpeed.SelectedIndex = 1; // Default to Normal speed
        
        // 
        // lblSpeed
        // 
        this.lblSpeed.Location = new Point(10, 510);
        this.lblSpeed.Name = "lblSpeed";
        this.lblSpeed.Size = new Size(180, 20);
        this.lblSpeed.TabIndex = 12;
        this.lblSpeed.Text = "Speed: Normal";
        
        // Add controls to panels
        this.panelControls.Controls.Add(this.btnGreedy);
        this.panelControls.Controls.Add(this.btnUCS);
        this.panelControls.Controls.Add(this.btnAStar);
        this.panelControls.Controls.Add(this.btnReset);
        this.panelControls.Controls.Add(this.btnSetStart);
        this.panelControls.Controls.Add(this.btnSetGoal);
        this.panelControls.Controls.Add(this.btnSetObstacle);
        this.panelControls.Controls.Add(this.btnShowTree);
        this.panelControls.Controls.Add(this.lblAlgorithm);
        this.panelControls.Controls.Add(this.lblCost);
        this.panelControls.Controls.Add(this.lblSteps);
        this.panelControls.Controls.Add(this.cmbSpeed);
        this.panelControls.Controls.Add(this.lblSpeed);
        
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new SizeF(8F, 20F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(1160, 640); // Much larger form to accommodate bigger graph
        this.Controls.Add(this.panelGrid);
        this.Controls.Add(this.panelControls);
        this.Name = "Form1";
        this.Text = "Search Algorithm Visualizer - Enhanced Graph View";
        this.ResumeLayout(false);
    }

    #endregion
}
