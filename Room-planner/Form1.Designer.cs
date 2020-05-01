namespace Room_planner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mainWindow = new System.Windows.Forms.SplitContainer();
            this.canvas = new System.Windows.Forms.PictureBox();
            this.operationsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonsBox = new System.Windows.Forms.GroupBox();
            this.buttonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.bedButton = new System.Windows.Forms.Button();
            this.sofaButton = new System.Windows.Forms.Button();
            this.coffeeButton = new System.Windows.Forms.Button();
            this.tableButton = new System.Windows.Forms.Button();
            this.wallButton = new System.Windows.Forms.Button();
            this.displayBox = new System.Windows.Forms.GroupBox();
            this.furnitureList = new System.Windows.Forms.ListBox();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renewCanvas = new System.Windows.Forms.ToolStripMenuItem();
            this.saveBlueprintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBlueprintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.russianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.mainWindow)).BeginInit();
            this.mainWindow.Panel1.SuspendLayout();
            this.mainWindow.Panel2.SuspendLayout();
            this.mainWindow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.operationsPanel.SuspendLayout();
            this.buttonsBox.SuspendLayout();
            this.buttonsPanel.SuspendLayout();
            this.displayBox.SuspendLayout();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainWindow
            // 
            resources.ApplyResources(this.mainWindow, "mainWindow");
            this.mainWindow.Name = "mainWindow";
            // 
            // mainWindow.Panel1
            // 
            resources.ApplyResources(this.mainWindow.Panel1, "mainWindow.Panel1");
            this.mainWindow.Panel1.Controls.Add(this.canvas);
            this.mainWindow.Panel1.SizeChanged += new System.EventHandler(this.mainWindow_Panel1_SizeChanged);
            // 
            // mainWindow.Panel2
            // 
            this.mainWindow.Panel2.Controls.Add(this.operationsPanel);
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.canvas, "canvas");
            this.canvas.Name = "canvas";
            this.canvas.TabStop = false;
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseDown);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
            this.canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseUp);
            // 
            // operationsPanel
            // 
            resources.ApplyResources(this.operationsPanel, "operationsPanel");
            this.operationsPanel.Controls.Add(this.buttonsBox, 0, 0);
            this.operationsPanel.Controls.Add(this.displayBox, 0, 1);
            this.operationsPanel.Name = "operationsPanel";
            // 
            // buttonsBox
            // 
            this.buttonsBox.Controls.Add(this.buttonsPanel);
            resources.ApplyResources(this.buttonsBox, "buttonsBox");
            this.buttonsBox.Name = "buttonsBox";
            this.buttonsBox.TabStop = false;
            // 
            // buttonsPanel
            // 
            resources.ApplyResources(this.buttonsPanel, "buttonsPanel");
            this.buttonsPanel.Controls.Add(this.bedButton);
            this.buttonsPanel.Controls.Add(this.sofaButton);
            this.buttonsPanel.Controls.Add(this.coffeeButton);
            this.buttonsPanel.Controls.Add(this.tableButton);
            this.buttonsPanel.Controls.Add(this.wallButton);
            this.buttonsPanel.Name = "buttonsPanel";
            // 
            // bedButton
            // 
            this.bedButton.BackColor = System.Drawing.Color.White;
            this.bedButton.BackgroundImage = global::Room_planner.Properties.Resources.double_bed;
            resources.ApplyResources(this.bedButton, "bedButton");
            this.bedButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bedButton.Name = "bedButton";
            this.bedButton.UseVisualStyleBackColor = false;
            this.bedButton.Click += new System.EventHandler(this.selectItem_Click);
            // 
            // sofaButton
            // 
            this.sofaButton.BackColor = System.Drawing.Color.White;
            this.sofaButton.BackgroundImage = global::Room_planner.Properties.Resources.sofa;
            resources.ApplyResources(this.sofaButton, "sofaButton");
            this.sofaButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sofaButton.Name = "sofaButton";
            this.sofaButton.UseVisualStyleBackColor = false;
            this.sofaButton.Click += new System.EventHandler(this.selectItem_Click);
            // 
            // coffeeButton
            // 
            this.coffeeButton.BackColor = System.Drawing.Color.White;
            this.coffeeButton.BackgroundImage = global::Room_planner.Properties.Resources.coffee_table;
            resources.ApplyResources(this.coffeeButton, "coffeeButton");
            this.coffeeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.coffeeButton.Name = "coffeeButton";
            this.coffeeButton.UseVisualStyleBackColor = false;
            this.coffeeButton.Click += new System.EventHandler(this.selectItem_Click);
            // 
            // tableButton
            // 
            this.tableButton.BackColor = System.Drawing.Color.White;
            this.tableButton.BackgroundImage = global::Room_planner.Properties.Resources.table;
            resources.ApplyResources(this.tableButton, "tableButton");
            this.tableButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tableButton.Name = "tableButton";
            this.tableButton.UseVisualStyleBackColor = false;
            this.tableButton.Click += new System.EventHandler(this.selectItem_Click);
            // 
            // wallButton
            // 
            this.wallButton.BackColor = System.Drawing.Color.White;
            this.wallButton.BackgroundImage = global::Room_planner.Properties.Resources.wall;
            resources.ApplyResources(this.wallButton, "wallButton");
            this.wallButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.wallButton.Name = "wallButton";
            this.wallButton.UseVisualStyleBackColor = false;
            this.wallButton.Click += new System.EventHandler(this.selectItem_Click);
            // 
            // displayBox
            // 
            this.displayBox.Controls.Add(this.furnitureList);
            resources.ApplyResources(this.displayBox, "displayBox");
            this.displayBox.Name = "displayBox";
            this.displayBox.TabStop = false;
            // 
            // furnitureList
            // 
            resources.ApplyResources(this.furnitureList, "furnitureList");
            this.furnitureList.FormattingEnabled = true;
            this.furnitureList.Name = "furnitureList";
            this.furnitureList.SelectedIndexChanged += new System.EventHandler(this.furnitureList_SelectedIndexChanged);
            // 
            // menu
            // 
            this.menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            resources.ApplyResources(this.menu, "menu");
            this.menu.Name = "menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renewCanvas,
            this.saveBlueprintToolStripMenuItem,
            this.openBlueprintToolStripMenuItem,
            this.languageToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // renewCanvas
            // 
            this.renewCanvas.Name = "renewCanvas";
            resources.ApplyResources(this.renewCanvas, "renewCanvas");
            this.renewCanvas.Click += new System.EventHandler(this.renewCanvas_Click);
            // 
            // saveBlueprintToolStripMenuItem
            // 
            this.saveBlueprintToolStripMenuItem.Name = "saveBlueprintToolStripMenuItem";
            resources.ApplyResources(this.saveBlueprintToolStripMenuItem, "saveBlueprintToolStripMenuItem");
            this.saveBlueprintToolStripMenuItem.Click += new System.EventHandler(this.saveBlueprintToolStripMenuItem_Click);
            // 
            // openBlueprintToolStripMenuItem
            // 
            this.openBlueprintToolStripMenuItem.Name = "openBlueprintToolStripMenuItem";
            resources.ApplyResources(this.openBlueprintToolStripMenuItem, "openBlueprintToolStripMenuItem");
            this.openBlueprintToolStripMenuItem.Click += new System.EventHandler(this.openBlueprintToolStripMenuItem_Click);
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.russianToolStripMenuItem,
            this.englishToolStripMenuItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            resources.ApplyResources(this.languageToolStripMenuItem, "languageToolStripMenuItem");
            // 
            // russianToolStripMenuItem
            // 
            this.russianToolStripMenuItem.Name = "russianToolStripMenuItem";
            resources.ApplyResources(this.russianToolStripMenuItem, "russianToolStripMenuItem");
            this.russianToolStripMenuItem.Click += new System.EventHandler(this.russianToolStripMenuItem_Click);
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            resources.ApplyResources(this.englishToolStripMenuItem, "englishToolStripMenuItem");
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainWindow);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Name = "Form1";
            this.mainWindow.Panel1.ResumeLayout(false);
            this.mainWindow.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainWindow)).EndInit();
            this.mainWindow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.operationsPanel.ResumeLayout(false);
            this.buttonsBox.ResumeLayout(false);
            this.buttonsPanel.ResumeLayout(false);
            this.displayBox.ResumeLayout(false);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainWindow;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.TableLayoutPanel operationsPanel;
        private System.Windows.Forms.GroupBox buttonsBox;
        private System.Windows.Forms.FlowLayoutPanel buttonsPanel;
        private System.Windows.Forms.GroupBox displayBox;
        private System.Windows.Forms.ListBox furnitureList;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renewCanvas;
        private System.Windows.Forms.Button bedButton;
        private System.Windows.Forms.Button sofaButton;
        private System.Windows.Forms.Button coffeeButton;
        private System.Windows.Forms.Button tableButton;
        private System.Windows.Forms.Button wallButton;
        private System.Windows.Forms.ToolStripMenuItem saveBlueprintToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openBlueprintToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem russianToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
    }
}

