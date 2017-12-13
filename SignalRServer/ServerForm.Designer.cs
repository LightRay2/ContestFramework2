namespace SignalRServer
{
    partial class ServerForm
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
            this.labelServerAddress = new System.Windows.Forms.Label();
            this.labelPlayersSentExe = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCountOfGameRunners = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabInfo = new System.Windows.Forms.TabPage();
            this.tabMode = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelServerAddress
            // 
            this.labelServerAddress.AutoSize = true;
            this.labelServerAddress.Location = new System.Drawing.Point(7, 12);
            this.labelServerAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelServerAddress.Name = "labelServerAddress";
            this.labelServerAddress.Size = new System.Drawing.Size(39, 20);
            this.labelServerAddress.TabIndex = 0;
            this.labelServerAddress.Text = "label";
            // 
            // labelPlayersSentExe
            // 
            this.labelPlayersSentExe.AutoSize = true;
            this.labelPlayersSentExe.Location = new System.Drawing.Point(7, 45);
            this.labelPlayersSentExe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPlayersSentExe.MaximumSize = new System.Drawing.Size(300, 0);
            this.labelPlayersSentExe.Name = "labelPlayersSentExe";
            this.labelPlayersSentExe.Size = new System.Drawing.Size(300, 60);
            this.labelPlayersSentExe.TabIndex = 1;
            this.labelPlayersSentExe.Text = "labelPlayersSentExe.............................===============================.." +
    "...................";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(339, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.MaximumSize = new System.Drawing.Size(300, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(298, 60);
            this.label1.TabIndex = 2;
            this.label1.Text = "labelPlayersConnected.............................===============================" +
    ".....................";
            // 
            // labelCountOfGameRunners
            // 
            this.labelCountOfGameRunners.AutoSize = true;
            this.labelCountOfGameRunners.Location = new System.Drawing.Point(682, 45);
            this.labelCountOfGameRunners.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCountOfGameRunners.MaximumSize = new System.Drawing.Size(300, 0);
            this.labelCountOfGameRunners.Name = "labelCountOfGameRunners";
            this.labelCountOfGameRunners.Size = new System.Drawing.Size(191, 20);
            this.labelCountOfGameRunners.TabIndex = 3;
            this.labelCountOfGameRunners.Text = "labelCountOfGameRunners";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabInfo);
            this.tabControl1.Controls.Add(this.tabMode);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1044, 643);
            this.tabControl1.TabIndex = 4;
            // 
            // tabInfo
            // 
            this.tabInfo.AutoScroll = true;
            this.tabInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tabInfo.Controls.Add(this.labelServerAddress);
            this.tabInfo.Controls.Add(this.labelCountOfGameRunners);
            this.tabInfo.Controls.Add(this.labelPlayersSentExe);
            this.tabInfo.Controls.Add(this.label1);
            this.tabInfo.Location = new System.Drawing.Point(4, 29);
            this.tabInfo.Name = "tabInfo";
            this.tabInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabInfo.Size = new System.Drawing.Size(1036, 610);
            this.tabInfo.TabIndex = 0;
            this.tabInfo.Text = "Информация";
            // 
            // tabMode
            // 
            this.tabMode.AutoScroll = true;
            this.tabMode.BackColor = System.Drawing.SystemColors.Control;
            this.tabMode.Location = new System.Drawing.Point(4, 29);
            this.tabMode.Name = "tabMode";
            this.tabMode.Padding = new System.Windows.Forms.Padding(3);
            this.tabMode.Size = new System.Drawing.Size(1036, 610);
            this.tabMode.TabIndex = 1;
            this.tabMode.Text = "Режим работы";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 643);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Arial Unicode MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ServerForm";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabInfo.ResumeLayout(false);
            this.tabInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelServerAddress;
        private System.Windows.Forms.Label labelPlayersSentExe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCountOfGameRunners;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInfo;
        private System.Windows.Forms.TabPage tabMode;
    }
}

