namespace CrozzleDesktopApp
{
    partial class MainForm
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
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.MenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadConfigMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadCrozzleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WebBrowser = new System.Windows.Forms.WebBrowser();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuList});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(554, 24);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "menuStrip1";
            // 
            // MenuList
            // 
            this.MenuList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadConfigMenuItem,
            this.LoadCrozzleMenuItem});
            this.MenuList.Name = "MenuList";
            this.MenuList.Size = new System.Drawing.Size(50, 20);
            this.MenuList.Text = "Menu";
            // 
            // LoadConfigMenuItem
            // 
            this.LoadConfigMenuItem.Name = "LoadConfigMenuItem";
            this.LoadConfigMenuItem.Size = new System.Drawing.Size(175, 22);
            this.LoadConfigMenuItem.Text = "Load configuration";
            this.LoadConfigMenuItem.Click += new System.EventHandler(this.LoadConfigMenuItemClick);
            // 
            // LoadCrozzleMenuItem
            // 
            this.LoadCrozzleMenuItem.Name = "LoadCrozzleMenuItem";
            this.LoadCrozzleMenuItem.Size = new System.Drawing.Size(175, 22);
            this.LoadCrozzleMenuItem.Text = "Load Crozzle";
            this.LoadCrozzleMenuItem.Click += new System.EventHandler(this.LoadCrozzleMenuItemClick);
            // 
            // WebBrowser
            // 
            this.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebBrowser.Location = new System.Drawing.Point(0, 24);
            this.WebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.WebBrowser.Name = "WebBrowser";
            this.WebBrowser.Size = new System.Drawing.Size(554, 402);
            this.WebBrowser.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 426);
            this.Controls.Add(this.WebBrowser);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainForm";
            this.Text = "Crozzle";
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuList;
        private System.Windows.Forms.ToolStripMenuItem LoadConfigMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadCrozzleMenuItem;
        private System.Windows.Forms.WebBrowser WebBrowser;
    }
}