namespace CSharpTester
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
            this.input = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allocConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toDLLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toEXEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // input
            // 
            this.input.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.input.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.input.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.input.ForeColor = System.Drawing.Color.White;
            this.input.Location = new System.Drawing.Point(0, 24);
            this.input.Name = "input";
            this.input.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.input.Size = new System.Drawing.Size(1269, 620);
            this.input.TabIndex = 0;
            this.input.Text = "";
            this.input.TextChanged += new System.EventHandler(this.OnNeedsSyntax);
            this.input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.input_KeyDown);
            this.input.KeyUp += new System.Windows.Forms.KeyEventHandler(this.input_KeyUp);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.buildToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1269, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.runToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.runToolStripMenuItem.Text = "Run (F5)";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.RunToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allocConsoleToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // allocConsoleToolStripMenuItem
            // 
            this.allocConsoleToolStripMenuItem.AutoToolTip = true;
            this.allocConsoleToolStripMenuItem.Checked = true;
            this.allocConsoleToolStripMenuItem.CheckOnClick = true;
            this.allocConsoleToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allocConsoleToolStripMenuItem.Name = "allocConsoleToolStripMenuItem";
            this.allocConsoleToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.allocConsoleToolStripMenuItem.Text = "Alloc Console";
            this.allocConsoleToolStripMenuItem.ToolTipText = "Automatically allocate a console window for the assembly when run.";
            // 
            // buildToolStripMenuItem
            // 
            this.buildToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toDLLToolStripMenuItem,
            this.toEXEToolStripMenuItem});
            this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
            this.buildToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.buildToolStripMenuItem.Text = "Build";
            // 
            // toDLLToolStripMenuItem
            // 
            this.toDLLToolStripMenuItem.Name = "toDLLToolStripMenuItem";
            this.toDLLToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.toDLLToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.toDLLToolStripMenuItem.Text = "To DLL...";
            this.toDLLToolStripMenuItem.Click += new System.EventHandler(this.toDLLToolStripMenuItem_Click);
            // 
            // toEXEToolStripMenuItem
            // 
            this.toEXEToolStripMenuItem.Name = "toEXEToolStripMenuItem";
            this.toEXEToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.toEXEToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.toEXEToolStripMenuItem.Text = "To EXE...";
            this.toEXEToolStripMenuItem.Click += new System.EventHandler(this.toEXEToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1269, 642);
            this.Controls.Add(this.input);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "CSharp Tester";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox input;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allocConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toDLLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toEXEToolStripMenuItem;
    }
}

