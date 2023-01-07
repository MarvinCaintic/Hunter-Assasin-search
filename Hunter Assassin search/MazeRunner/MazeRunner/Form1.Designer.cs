namespace MazeRunner
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.canvas = new System.Windows.Forms.PictureBox();
            this.simulator = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.algorithmsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uCSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gBFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setter = new System.Windows.Forms.Timer(this.components);
            this.walk = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.DarkGray;
            this.canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.canvas.Location = new System.Drawing.Point(-2, 26);
            this.canvas.Margin = new System.Windows.Forms.Padding(2);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(800, 800);
            this.canvas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            // 
            // simulator
            // 
            this.simulator.Interval = 1;
            this.simulator.Tick += new System.EventHandler(this.simulator_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.algorithmsToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // algorithmsToolStripMenuItem
            // 
            this.algorithmsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bFSToolStripMenuItem,
            this.dFSToolStripMenuItem,
            this.uCSToolStripMenuItem,
            this.gBFSToolStripMenuItem,
            this.aToolStripMenuItem});
            this.algorithmsToolStripMenuItem.Name = "algorithmsToolStripMenuItem";
            this.algorithmsToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.algorithmsToolStripMenuItem.Text = "Algorithms";
            // 
            // bFSToolStripMenuItem
            // 
            this.bFSToolStripMenuItem.Name = "bFSToolStripMenuItem";
            this.bFSToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.bFSToolStripMenuItem.Text = "BFS";
            this.bFSToolStripMenuItem.Click += new System.EventHandler(this.bFSToolStripMenuItem_Click);
            // 
            // dFSToolStripMenuItem
            // 
            this.dFSToolStripMenuItem.Name = "dFSToolStripMenuItem";
            this.dFSToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.dFSToolStripMenuItem.Text = "DFS";
            this.dFSToolStripMenuItem.Click += new System.EventHandler(this.dFSToolStripMenuItem_Click);
            // 
            // uCSToolStripMenuItem
            // 
            this.uCSToolStripMenuItem.Name = "uCSToolStripMenuItem";
            this.uCSToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.uCSToolStripMenuItem.Text = "UCS";
            this.uCSToolStripMenuItem.Click += new System.EventHandler(this.uCSToolStripMenuItem_Click);
            // 
            // gBFSToolStripMenuItem
            // 
            this.gBFSToolStripMenuItem.Name = "gBFSToolStripMenuItem";
            this.gBFSToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.gBFSToolStripMenuItem.Text = "GBFS";
            this.gBFSToolStripMenuItem.Click += new System.EventHandler(this.gBFSToolStripMenuItem_Click);
            // 
            // aToolStripMenuItem
            // 
            this.aToolStripMenuItem.Name = "aToolStripMenuItem";
            this.aToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.aToolStripMenuItem.Text = "A*";
            this.aToolStripMenuItem.Click += new System.EventHandler(this.aToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // setter
            // 
            this.setter.Enabled = true;
            this.setter.Tick += new System.EventHandler(this.setter_Tick);
            // 
            // walk
            // 
            this.walk.Interval = 500;
            this.walk.Tick += new System.EventHandler(this.walk_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 791);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.canvas);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Timer simulator;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem algorithmsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bFSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dFSToolStripMenuItem;
        private System.Windows.Forms.Timer setter;
        private System.Windows.Forms.Timer walk;
        private System.Windows.Forms.ToolStripMenuItem uCSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gBFSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
    }
}

