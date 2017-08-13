namespace Framework
{
    partial class GameForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.режимыВоспроизведенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPause = new System.Windows.Forms.ToolStripMenuItem();
            this.паузаПослеТекущегоХодаTABToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.скоростьПросмотраToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSpeed10 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSpeed20 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSpeed40 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSpeed60 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSpeed80 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSpeed100 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSpeed150 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSpeed200 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSpeed250 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSpeed300 = new System.Windows.Forms.ToolStripMenuItem();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.infoActions = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelEmpty = new System.Windows.Forms.ToolStripStatusLabel();
            this.infoUnderMouse = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelForOpenglControl = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.режимыВоспроизведенияToolStripMenuItem,
            this.скоростьПросмотраToolStripMenuItem,
            this.помощьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(928, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // режимыВоспроизведенияToolStripMenuItem
            // 
            this.режимыВоспроизведенияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuPause,
            this.паузаПослеТекущегоХодаTABToolStripMenuItem});
            this.режимыВоспроизведенияToolStripMenuItem.Name = "режимыВоспроизведенияToolStripMenuItem";
            this.режимыВоспроизведенияToolStripMenuItem.Size = new System.Drawing.Size(130, 19);
            this.режимыВоспроизведенияToolStripMenuItem.Text = "Режимы просмотра";
            // 
            // menuPause
            // 
            this.menuPause.Name = "menuPause";
            this.menuPause.Size = new System.Drawing.Size(245, 22);
            this.menuPause.Text = "Пауза (P)";
            this.menuPause.Click += new System.EventHandler(this.menuPauseClicked);
            // 
            // паузаПослеТекущегоХодаTABToolStripMenuItem
            // 
            this.паузаПослеТекущегоХодаTABToolStripMenuItem.Name = "паузаПослеТекущегоХодаTABToolStripMenuItem";
            this.паузаПослеТекущегоХодаTABToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.паузаПослеТекущегоХодаTABToolStripMenuItem.Text = "Пауза после хода (клавиша \'[\' )";
            this.паузаПослеТекущегоХодаTABToolStripMenuItem.Click += new System.EventHandler(this.паузаПослеТекущегоХодаTABToolStripMenuItem_Click);
            // 
            // скоростьПросмотраToolStripMenuItem
            // 
            this.скоростьПросмотраToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSpeed10,
            this.menuSpeed20,
            this.menuSpeed40,
            this.menuSpeed60,
            this.menuSpeed80,
            this.menuSpeed100,
            this.menuSpeed150,
            this.menuSpeed200,
            this.menuSpeed250,
            this.menuSpeed300});
            this.скоростьПросмотраToolStripMenuItem.Name = "скоростьПросмотраToolStripMenuItem";
            this.скоростьПросмотраToolStripMenuItem.Size = new System.Drawing.Size(135, 19);
            this.скоростьПросмотраToolStripMenuItem.Text = "Скорость просмотра";
            // 
            // menuSpeed10
            // 
            this.menuSpeed10.Name = "menuSpeed10";
            this.menuSpeed10.Size = new System.Drawing.Size(102, 22);
            this.menuSpeed10.Text = "10%";
            this.menuSpeed10.Click += new System.EventHandler(this.menuSpeed10_Click);
            // 
            // menuSpeed20
            // 
            this.menuSpeed20.Name = "menuSpeed20";
            this.menuSpeed20.Size = new System.Drawing.Size(102, 22);
            this.menuSpeed20.Text = "20%";
            this.menuSpeed20.Click += new System.EventHandler(this.menuSpeed20_Click);
            // 
            // menuSpeed40
            // 
            this.menuSpeed40.Name = "menuSpeed40";
            this.menuSpeed40.Size = new System.Drawing.Size(102, 22);
            this.menuSpeed40.Text = "40%";
            this.menuSpeed40.Click += new System.EventHandler(this.menuSpeed40_Click);
            // 
            // menuSpeed60
            // 
            this.menuSpeed60.Name = "menuSpeed60";
            this.menuSpeed60.Size = new System.Drawing.Size(102, 22);
            this.menuSpeed60.Text = "60%";
            this.menuSpeed60.Click += new System.EventHandler(this.menuSpeed60_Click);
            // 
            // menuSpeed80
            // 
            this.menuSpeed80.Name = "menuSpeed80";
            this.menuSpeed80.Size = new System.Drawing.Size(102, 22);
            this.menuSpeed80.Text = "80%";
            this.menuSpeed80.Click += new System.EventHandler(this.menuSpeed80_Click);
            // 
            // menuSpeed100
            // 
            this.menuSpeed100.Name = "menuSpeed100";
            this.menuSpeed100.Size = new System.Drawing.Size(102, 22);
            this.menuSpeed100.Text = "100%";
            this.menuSpeed100.Click += new System.EventHandler(this.menuSpeed100_Click);
            // 
            // menuSpeed150
            // 
            this.menuSpeed150.Name = "menuSpeed150";
            this.menuSpeed150.Size = new System.Drawing.Size(102, 22);
            this.menuSpeed150.Text = "150%";
            this.menuSpeed150.Click += new System.EventHandler(this.menuSpeed150_Click);
            // 
            // menuSpeed200
            // 
            this.menuSpeed200.Name = "menuSpeed200";
            this.menuSpeed200.Size = new System.Drawing.Size(102, 22);
            this.menuSpeed200.Text = "200%";
            this.menuSpeed200.Click += new System.EventHandler(this.menuSpeed200_Click);
            // 
            // menuSpeed250
            // 
            this.menuSpeed250.Name = "menuSpeed250";
            this.menuSpeed250.Size = new System.Drawing.Size(102, 22);
            this.menuSpeed250.Text = "250%";
            this.menuSpeed250.Click += new System.EventHandler(this.menuSpeed250_Click);
            // 
            // menuSpeed300
            // 
            this.menuSpeed300.Name = "menuSpeed300";
            this.menuSpeed300.Size = new System.Drawing.Size(102, 22);
            this.menuSpeed300.Text = "300%";
            this.menuSpeed300.Click += new System.EventHandler(this.menuSpeed300_Click);
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(77, 19);
            this.помощьToolStripMenuItem.Text = "Помощь...";
            this.помощьToolStripMenuItem.Click += new System.EventHandler(this.помощьToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoActions,
            this.toolStripStatusLabelEmpty,
            this.infoUnderMouse});
            this.statusStrip1.Location = new System.Drawing.Point(0, 678);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(928, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // infoActions
            // 
            this.infoActions.Name = "infoActions";
            this.infoActions.Size = new System.Drawing.Size(68, 17);
            this.infoActions.Text = "infoActions";
            // 
            // toolStripStatusLabelEmpty
            // 
            this.toolStripStatusLabelEmpty.Name = "toolStripStatusLabelEmpty";
            this.toolStripStatusLabelEmpty.Size = new System.Drawing.Size(749, 17);
            this.toolStripStatusLabelEmpty.Spring = true;
            // 
            // infoUnderMouse
            // 
            this.infoUnderMouse.Name = "infoUnderMouse";
            this.infoUnderMouse.Size = new System.Drawing.Size(96, 17);
            this.infoUnderMouse.Text = "infoUnderMouse";
            // 
            // panelForOpenglControl
            // 
            this.panelForOpenglControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForOpenglControl.Location = new System.Drawing.Point(0, 25);
            this.panelForOpenglControl.Name = "panelForOpenglControl";
            this.panelForOpenglControl.Size = new System.Drawing.Size(928, 653);
            this.panelForOpenglControl.TabIndex = 3;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 700);
            this.Controls.Add(this.panelForOpenglControl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "GameForm";
            this.Text = "ContestAI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameForm_FormClosed);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel infoActions;
        private System.Windows.Forms.ToolStripStatusLabel infoUnderMouse;
        private System.Windows.Forms.Panel panelForOpenglControl;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelEmpty;
        private System.Windows.Forms.ToolStripMenuItem скоростьПросмотраToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuSpeed10;
        private System.Windows.Forms.ToolStripMenuItem menuSpeed20;
        private System.Windows.Forms.ToolStripMenuItem menuSpeed40;
        private System.Windows.Forms.ToolStripMenuItem menuSpeed60;
        private System.Windows.Forms.ToolStripMenuItem menuSpeed80;
        private System.Windows.Forms.ToolStripMenuItem menuSpeed100;
        private System.Windows.Forms.ToolStripMenuItem menuSpeed150;
        private System.Windows.Forms.ToolStripMenuItem menuSpeed200;
        private System.Windows.Forms.ToolStripMenuItem menuSpeed250;
        private System.Windows.Forms.ToolStripMenuItem menuSpeed300;
        private System.Windows.Forms.ToolStripMenuItem режимыВоспроизведенияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuPause;
        private System.Windows.Forms.ToolStripMenuItem паузаПослеТекущегоХодаTABToolStripMenuItem;
    }
}