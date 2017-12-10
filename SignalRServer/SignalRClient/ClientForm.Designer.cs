namespace SignalRClient
{
    partial class ClientForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.edtSendREsultOfCurrentGame = new System.Windows.Forms.TextBox();
            this.labelPlayerRank = new System.Windows.Forms.Label();
            this.labelPlayLastGame = new System.Windows.Forms.Label();
            this.edtNameAndPassword = new System.Windows.Forms.TextBox();
            this.labelGetListOfGames = new System.Windows.Forms.TextBox();
            this.edtGameGuid = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(82, 288);
            this.label1.MaximumSize = new System.Drawing.Size(500, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(215, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "send exe";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(215, 71);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "get player rank";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(215, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(138, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "authorize as player";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(215, 213);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(138, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "authorize as help server";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(215, 100);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(138, 23);
            this.button5.TabIndex = 5;
            this.button5.Text = "get list of games";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(215, 129);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(138, 23);
            this.button6.TabIndex = 6;
            this.button6.Text = "play last game";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(215, 242);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(138, 23);
            this.button7.TabIndex = 7;
            this.button7.Text = "send results of current game";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // edtSendREsultOfCurrentGame
            // 
            this.edtSendREsultOfCurrentGame.Location = new System.Drawing.Point(418, 246);
            this.edtSendREsultOfCurrentGame.Name = "edtSendREsultOfCurrentGame";
            this.edtSendREsultOfCurrentGame.Size = new System.Drawing.Size(292, 20);
            this.edtSendREsultOfCurrentGame.TabIndex = 8;
            this.edtSendREsultOfCurrentGame.TextChanged += new System.EventHandler(this.edtSendREsultOfCurrentGame_TextChanged);
            // 
            // labelPlayerRank
            // 
            this.labelPlayerRank.AutoSize = true;
            this.labelPlayerRank.Location = new System.Drawing.Point(381, 76);
            this.labelPlayerRank.MaximumSize = new System.Drawing.Size(500, 0);
            this.labelPlayerRank.Name = "labelPlayerRank";
            this.labelPlayerRank.Size = new System.Drawing.Size(35, 13);
            this.labelPlayerRank.TabIndex = 9;
            this.labelPlayerRank.Text = "label2";
            this.labelPlayerRank.Click += new System.EventHandler(this.label2_Click);
            // 
            // labelPlayLastGame
            // 
            this.labelPlayLastGame.AutoSize = true;
            this.labelPlayLastGame.Location = new System.Drawing.Point(365, 159);
            this.labelPlayLastGame.MaximumSize = new System.Drawing.Size(500, 0);
            this.labelPlayLastGame.Name = "labelPlayLastGame";
            this.labelPlayLastGame.Size = new System.Drawing.Size(35, 13);
            this.labelPlayLastGame.TabIndex = 11;
            this.labelPlayLastGame.Text = "label2";
            // 
            // edtNameAndPassword
            // 
            this.edtNameAndPassword.Location = new System.Drawing.Point(368, 2);
            this.edtNameAndPassword.Name = "edtNameAndPassword";
            this.edtNameAndPassword.Size = new System.Drawing.Size(292, 20);
            this.edtNameAndPassword.TabIndex = 12;
            this.edtNameAndPassword.Text = "one one";
            // 
            // labelGetListOfGames
            // 
            this.labelGetListOfGames.Location = new System.Drawing.Point(368, 100);
            this.labelGetListOfGames.Name = "labelGetListOfGames";
            this.labelGetListOfGames.Size = new System.Drawing.Size(292, 20);
            this.labelGetListOfGames.TabIndex = 13;
            // 
            // edtGameGuid
            // 
            this.edtGameGuid.Location = new System.Drawing.Point(368, 132);
            this.edtGameGuid.Name = "edtGameGuid";
            this.edtGameGuid.Size = new System.Drawing.Size(292, 20);
            this.edtGameGuid.TabIndex = 14;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 499);
            this.Controls.Add(this.edtGameGuid);
            this.Controls.Add(this.labelGetListOfGames);
            this.Controls.Add(this.edtNameAndPassword);
            this.Controls.Add(this.labelPlayLastGame);
            this.Controls.Add(this.labelPlayerRank);
            this.Controls.Add(this.edtSendREsultOfCurrentGame);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "ClientForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.Click += new System.EventHandler(this.ClientForm_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox edtSendREsultOfCurrentGame;
        private System.Windows.Forms.Label labelPlayerRank;
        private System.Windows.Forms.Label labelPlayLastGame;
        private System.Windows.Forms.TextBox edtNameAndPassword;
        private System.Windows.Forms.TextBox labelGetListOfGames;
        private System.Windows.Forms.TextBox edtGameGuid;
    }
}

