namespace InformSearch
{
    partial class TSPForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnParse;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RadioButton rbGreedy;
        private System.Windows.Forms.RadioButton rbUCS;
        private System.Windows.Forms.RadioButton rbAStar;
        private System.Windows.Forms.GroupBox gbAlgorithm;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblInput;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnParse = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.rbGreedy = new System.Windows.Forms.RadioButton();
            this.rbUCS = new System.Windows.Forms.RadioButton();
            this.rbAStar = new System.Windows.Forms.RadioButton();
            this.gbAlgorithm = new System.Windows.Forms.GroupBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblInput = new System.Windows.Forms.Label();
            this.gbAlgorithm.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblInput.Location = new System.Drawing.Point(630, 20);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(150, 17);
            this.lblInput.TabIndex = 0;
            this.lblInput.Text = "Graph Input (n m, then edges):";
            
            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtInput.Location = new System.Drawing.Point(630, 50);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInput.Size = new System.Drawing.Size(250, 200);
            this.txtInput.TabIndex = 1;
            this.txtInput.Text = "4 6\r\n1 2 10\r\n1 3 15\r\n1 4 20\r\n2 3 35\r\n2 4 25\r\n3 4 30";
            
            // 
            // btnParse
            // 
            this.btnParse.Location = new System.Drawing.Point(630, 260);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(120, 30);
            this.btnParse.TabIndex = 2;
            this.btnParse.Text = "Parse Input";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            
            // 
            // gbAlgorithm
            // 
            this.gbAlgorithm.Controls.Add(this.rbGreedy);
            this.gbAlgorithm.Controls.Add(this.rbUCS);
            this.gbAlgorithm.Controls.Add(this.rbAStar);
            this.gbAlgorithm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.gbAlgorithm.Location = new System.Drawing.Point(630, 300);
            this.gbAlgorithm.Name = "gbAlgorithm";
            this.gbAlgorithm.Size = new System.Drawing.Size(250, 120);
            this.gbAlgorithm.TabIndex = 3;
            this.gbAlgorithm.TabStop = false;
            this.gbAlgorithm.Text = "Select Algorithm";
            
            // 
            // rbGreedy
            // 
            this.rbGreedy.AutoSize = true;
            this.rbGreedy.Checked = true;
            this.rbGreedy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.rbGreedy.Location = new System.Drawing.Point(15, 25);
            this.rbGreedy.Name = "rbGreedy";
            this.rbGreedy.Size = new System.Drawing.Size(180, 19);
            this.rbGreedy.TabIndex = 0;
            this.rbGreedy.TabStop = true;
            this.rbGreedy.Text = "Greedy (Nearest Neighbor)";
            this.rbGreedy.UseVisualStyleBackColor = true;
            
            // 
            // rbUCS
            // 
            this.rbUCS.AutoSize = true;
            this.rbUCS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.rbUCS.Location = new System.Drawing.Point(15, 50);
            this.rbUCS.Name = "rbUCS";
            this.rbUCS.Size = new System.Drawing.Size(170, 19);
            this.rbUCS.TabIndex = 1;
            this.rbUCS.Text = "UCS (Uniform Cost Search)";
            this.rbUCS.UseVisualStyleBackColor = true;
            
            // 
            // rbAStar
            // 
            this.rbAStar.AutoSize = true;
            this.rbAStar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.rbAStar.Location = new System.Drawing.Point(15, 75);
            this.rbAStar.Name = "rbAStar";
            this.rbAStar.Size = new System.Drawing.Size(130, 19);
            this.rbAStar.TabIndex = 2;
            this.rbAStar.Text = "A* (with Heuristic)";
            this.rbAStar.UseVisualStyleBackColor = true;
            
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.LightGreen;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnStart.Location = new System.Drawing.Point(630, 430);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 40);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start TSP";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.LightCoral;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnClear.Location = new System.Drawing.Point(760, 430);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 40);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            
            // 
            // lblResult
            // 
            this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblResult.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblResult.Location = new System.Drawing.Point(630, 490);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(250, 40);
            this.lblResult.TabIndex = 6;
            this.lblResult.Text = "Result: -";
            
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblStatus.Location = new System.Drawing.Point(630, 540);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(250, 20);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Ready";
            
            // 
            // TSPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 580);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.btnParse);
            this.Controls.Add(this.gbAlgorithm);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblStatus);
            this.Name = "TSPForm";
            this.Text = "Traveling Salesman Problem (TSP) Visualizer";
            this.gbAlgorithm.ResumeLayout(false);
            this.gbAlgorithm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
