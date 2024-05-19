namespace ZH_forms1.View
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            menuStrip1 = new MenuStrip();
            startGameToolStripMenuItem = new ToolStripMenuItem();
            sizeSettingToolStripMenuItem = new ToolStripMenuItem();
            x10ToolStripMenuItem = new ToolStripMenuItem();
            x20ToolStripMenuItem = new ToolStripMenuItem();
            x8ToolStripMenuItem = new ToolStripMenuItem();
            quitToolStripMenuItem = new ToolStripMenuItem();
            gameTable = new Panel();
            label1 = new Label();
            figureNum = new Label();
            label2 = new Label();
            Player = new Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { startGameToolStripMenuItem, sizeSettingToolStripMenuItem, quitToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(502, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // startGameToolStripMenuItem
            // 
            startGameToolStripMenuItem.Name = "startGameToolStripMenuItem";
            startGameToolStripMenuItem.Size = new Size(97, 24);
            startGameToolStripMenuItem.Text = "Start Game";
            startGameToolStripMenuItem.Click += newGame_Click;
            // 
            // sizeSettingToolStripMenuItem
            // 
            sizeSettingToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { x10ToolStripMenuItem, x20ToolStripMenuItem, x8ToolStripMenuItem });
            sizeSettingToolStripMenuItem.Name = "sizeSettingToolStripMenuItem";
            sizeSettingToolStripMenuItem.Size = new Size(99, 24);
            sizeSettingToolStripMenuItem.Text = "Size setting";
            // 
            // x10ToolStripMenuItem
            // 
            x10ToolStripMenuItem.Name = "x10ToolStripMenuItem";
            x10ToolStripMenuItem.Size = new Size(115, 26);
            x10ToolStripMenuItem.Text = "4x4";
            x10ToolStripMenuItem.Click += setSize4x4_Click;
            // 
            // x20ToolStripMenuItem
            // 
            x20ToolStripMenuItem.Name = "x20ToolStripMenuItem";
            x20ToolStripMenuItem.Size = new Size(115, 26);
            x20ToolStripMenuItem.Text = "6x6";
            x20ToolStripMenuItem.Click += setSize6x6_Click;
            // 
            // x8ToolStripMenuItem
            // 
            x8ToolStripMenuItem.Name = "x8ToolStripMenuItem";
            x8ToolStripMenuItem.Size = new Size(115, 26);
            x8ToolStripMenuItem.Text = "8x8";
            x8ToolStripMenuItem.Click += setSize8x8_Click;
            // 
            // quitToolStripMenuItem
            // 
            quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            quitToolStripMenuItem.Size = new Size(51, 24);
            quitToolStripMenuItem.Text = "Quit";
            quitToolStripMenuItem.Click += exit_Click;
            // 
            // gameTable
            // 
            gameTable.Location = new Point(0, 31);
            gameTable.Name = "gameTable";
            gameTable.Size = new Size(500, 500);
            gameTable.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(327, 534);
            label1.Name = "label1";
            label1.Size = new Size(107, 20);
            label1.TabIndex = 2;
            label1.Text = "FigureNumber:";
            // 
            // figureNum
            // 
            figureNum.AutoSize = true;
            figureNum.Location = new Point(440, 534);
            figureNum.Name = "figureNum";
            figureNum.Size = new Size(15, 20);
            figureNum.TabIndex = 3;
            figureNum.Text = "-";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(203, 534);
            label2.Name = "label2";
            label2.Size = new Size(52, 20);
            label2.TabIndex = 4;
            label2.Text = "Player:";
            // 
            // Player
            // 
            Player.AutoSize = true;
            Player.Location = new Point(261, 534);
            Player.Name = "Player";
            Player.Size = new Size(15, 20);
            Player.TabIndex = 5;
            Player.Text = "-";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(502, 563);
            Controls.Add(Player);
            Controls.Add(label2);
            Controls.Add(figureNum);
            Controls.Add(label1);
            Controls.Add(gameTable);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem startGameToolStripMenuItem;
        private ToolStripMenuItem sizeSettingToolStripMenuItem;
        private ToolStripMenuItem x10ToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem;
        private Panel gameTable;
        private ToolStripMenuItem x20ToolStripMenuItem;
        private Label label1;
        private Label figureNum;
        private ToolStripMenuItem x8ToolStripMenuItem;
        private Label label2;
        private Label Player;
    }
}
