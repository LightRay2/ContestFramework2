namespace SimpleContest
{
    partial class StartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabLocalGames = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelPlayers = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddHuman = new System.Windows.Forms.Button();
            this.btnAddProgram = new System.Windows.Forms.Button();
            this.btnChangeJavaPath = new System.Windows.Forms.Button();
            this.btnChangeOrder = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRun = new System.Windows.Forms.Button();
            this.panelPlayersInMatch = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClearSelection = new System.Windows.Forms.Button();
            this.tabServer = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabServerSend = new System.Windows.Forms.TabPage();
            this.panel7 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.edtServerPassword = new System.Windows.Forms.TextBox();
            this.edtServerLogin = new System.Windows.Forms.TextBox();
            this.edtServerAddress = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.labelServerSendGamePath = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tabServerRating = new System.Windows.Forms.TabPage();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.labelServerGameCount = new System.Windows.Forms.Label();
            this.labelServerPlayerScore = new System.Windows.Forms.Label();
            this.labelServerTeams = new System.Windows.Forms.Label();
            this.tabServerReplays = new System.Windows.Forms.TabPage();
            this.panel9 = new System.Windows.Forms.Panel();
            this.btnServerWatchGame = new System.Windows.Forms.Button();
            this.labelServerGamePlayersAndResults = new System.Windows.Forms.Label();
            this.labelServerGameTime = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.tabServerGameRunner = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.edtServerActivateGameRunnerForServer = new System.Windows.Forms.CheckBox();
            this.tabHelp = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabLocalGames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabServer.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabServerSend.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tabServerRating.SuspendLayout();
            this.panel8.SuspendLayout();
            this.tabServerReplays.SuspendLayout();
            this.panel9.SuspendLayout();
            this.tabServerGameRunner.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabHelp.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabLocalGames);
            this.tabControl1.Controls.Add(this.tabServer);
            this.tabControl1.Controls.Add(this.tabHelp);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(906, 549);
            this.tabControl1.TabIndex = 0;
            // 
            // tabLocalGames
            // 
            this.tabLocalGames.AutoScroll = true;
            this.tabLocalGames.BackColor = System.Drawing.Color.Transparent;
            this.tabLocalGames.Controls.Add(this.splitContainer1);
            this.tabLocalGames.Location = new System.Drawing.Point(4, 27);
            this.tabLocalGames.Margin = new System.Windows.Forms.Padding(4);
            this.tabLocalGames.Name = "tabLocalGames";
            this.tabLocalGames.Padding = new System.Windows.Forms.Padding(4);
            this.tabLocalGames.Size = new System.Drawing.Size(898, 518);
            this.tabLocalGames.TabIndex = 0;
            this.tabLocalGames.Text = "Запустить игру";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddHuman);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddProgram);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.btnChangeJavaPath);
            this.splitContainer1.Panel2.Controls.Add(this.btnChangeOrder);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.panelPlayersInMatch);
            this.splitContainer1.Panel2.Controls.Add(this.btnClearSelection);
            this.splitContainer1.Size = new System.Drawing.Size(890, 510);
            this.splitContainer1.SplitterDistance = 280;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelPlayers);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(276, 444);
            this.panel1.TabIndex = 2;
            // 
            // panelPlayers
            // 
            this.panelPlayers.AutoScroll = true;
            this.panelPlayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlayers.Location = new System.Drawing.Point(0, 18);
            this.panelPlayers.Name = "panelPlayers";
            this.panelPlayers.Size = new System.Drawing.Size(276, 426);
            this.panelPlayers.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "             ";
            // 
            // btnAddHuman
            // 
            this.btnAddHuman.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddHuman.Location = new System.Drawing.Point(0, 31);
            this.btnAddHuman.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.btnAddHuman.Name = "btnAddHuman";
            this.btnAddHuman.Size = new System.Drawing.Size(276, 31);
            this.btnAddHuman.TabIndex = 1;
            this.btnAddHuman.Text = "Добавить человека";
            this.btnAddHuman.UseVisualStyleBackColor = true;
            this.btnAddHuman.Visible = false;
            this.btnAddHuman.Click += new System.EventHandler(this.btnAddHuman_Click);
            // 
            // btnAddProgram
            // 
            this.btnAddProgram.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddProgram.Location = new System.Drawing.Point(0, 0);
            this.btnAddProgram.Name = "btnAddProgram";
            this.btnAddProgram.Size = new System.Drawing.Size(276, 31);
            this.btnAddProgram.TabIndex = 0;
            this.btnAddProgram.Text = "Добавить программу...";
            this.btnAddProgram.UseVisualStyleBackColor = true;
            this.btnAddProgram.Click += new System.EventHandler(this.btnAddProgram_Click);
            // 
            // btnChangeJavaPath
            // 
            this.btnChangeJavaPath.Location = new System.Drawing.Point(3, 166);
            this.btnChangeJavaPath.Name = "btnChangeJavaPath";
            this.btnChangeJavaPath.Size = new System.Drawing.Size(186, 35);
            this.btnChangeJavaPath.TabIndex = 2;
            this.btnChangeJavaPath.Text = "Изменить путь до Java";
            this.btnChangeJavaPath.UseVisualStyleBackColor = true;
            this.btnChangeJavaPath.Visible = false;
            this.btnChangeJavaPath.Click += new System.EventHandler(this.btnChangeJavaPath_Click);
            // 
            // btnChangeOrder
            // 
            this.btnChangeOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeOrder.Location = new System.Drawing.Point(195, 166);
            this.btnChangeOrder.Name = "btnChangeOrder";
            this.btnChangeOrder.Size = new System.Drawing.Size(192, 35);
            this.btnChangeOrder.TabIndex = 4;
            this.btnChangeOrder.Text = "Изменить порядок";
            this.btnChangeOrder.UseVisualStyleBackColor = true;
            this.btnChangeOrder.Click += new System.EventHandler(this.btnChangeOrder_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.btnRun);
            this.panel2.Location = new System.Drawing.Point(310, 419);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(248, 69);
            this.panel2.TabIndex = 3;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(3, 3);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(242, 62);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "НАЧАТЬ МАТЧ";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // panelPlayersInMatch
            // 
            this.panelPlayersInMatch.AutoScroll = true;
            this.panelPlayersInMatch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPlayersInMatch.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelPlayersInMatch.Location = new System.Drawing.Point(0, 0);
            this.panelPlayersInMatch.Name = "panelPlayersInMatch";
            this.panelPlayersInMatch.Size = new System.Drawing.Size(598, 160);
            this.panelPlayersInMatch.TabIndex = 0;
            this.panelPlayersInMatch.WrapContents = false;
            // 
            // btnClearSelection
            // 
            this.btnClearSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSelection.Location = new System.Drawing.Point(393, 166);
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Size = new System.Drawing.Size(189, 35);
            this.btnClearSelection.TabIndex = 5;
            this.btnClearSelection.Text = "Очистить выбор";
            this.btnClearSelection.UseVisualStyleBackColor = true;
            this.btnClearSelection.Click += new System.EventHandler(this.btnClearSelection_Click);
            // 
            // tabServer
            // 
            this.tabServer.Controls.Add(this.panel6);
            this.tabServer.Location = new System.Drawing.Point(4, 27);
            this.tabServer.Name = "tabServer";
            this.tabServer.Padding = new System.Windows.Forms.Padding(3);
            this.tabServer.Size = new System.Drawing.Size(898, 518);
            this.tabServer.TabIndex = 7;
            this.tabServer.Text = "Отправить решение на сервер";
            this.tabServer.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.Control;
            this.panel6.Controls.Add(this.tabControl2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(892, 512);
            this.panel6.TabIndex = 2;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabServerSend);
            this.tabControl2.Controls.Add(this.tabServerRating);
            this.tabControl2.Controls.Add(this.tabServerReplays);
            this.tabControl2.Controls.Add(this.tabServerGameRunner);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(892, 512);
            this.tabControl2.TabIndex = 0;
            // 
            // tabServerSend
            // 
            this.tabServerSend.Controls.Add(this.panel7);
            this.tabServerSend.Location = new System.Drawing.Point(4, 27);
            this.tabServerSend.Name = "tabServerSend";
            this.tabServerSend.Padding = new System.Windows.Forms.Padding(3);
            this.tabServerSend.Size = new System.Drawing.Size(884, 481);
            this.tabServerSend.TabIndex = 0;
            this.tabServerSend.Text = "Отправить решение";
            this.tabServerSend.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.SystemColors.Control;
            this.panel7.Controls.Add(this.button2);
            this.panel7.Controls.Add(this.edtServerPassword);
            this.panel7.Controls.Add(this.edtServerLogin);
            this.panel7.Controls.Add(this.edtServerAddress);
            this.panel7.Controls.Add(this.label15);
            this.panel7.Controls.Add(this.label14);
            this.panel7.Controls.Add(this.label13);
            this.panel7.Controls.Add(this.label12);
            this.panel7.Controls.Add(this.button1);
            this.panel7.Controls.Add(this.labelServerSendGamePath);
            this.panel7.Controls.Add(this.label10);
            this.panel7.Controls.Add(this.label11);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(878, 475);
            this.panel7.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(677, 87);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(191, 40);
            this.button2.TabIndex = 20;
            this.button2.Text = "Установить соединение";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // edtServerPassword
            // 
            this.edtServerPassword.Location = new System.Drawing.Point(652, 40);
            this.edtServerPassword.Name = "edtServerPassword";
            this.edtServerPassword.Size = new System.Drawing.Size(216, 24);
            this.edtServerPassword.TabIndex = 19;
            this.edtServerPassword.UseSystemPasswordChar = true;
            // 
            // edtServerLogin
            // 
            this.edtServerLogin.Location = new System.Drawing.Point(356, 40);
            this.edtServerLogin.Name = "edtServerLogin";
            this.edtServerLogin.Size = new System.Drawing.Size(203, 24);
            this.edtServerLogin.TabIndex = 18;
            // 
            // edtServerAddress
            // 
            this.edtServerAddress.Location = new System.Drawing.Point(83, 40);
            this.edtServerAddress.Name = "edtServerAddress";
            this.edtServerAddress.Size = new System.Drawing.Size(190, 24);
            this.edtServerAddress.TabIndex = 17;
            this.edtServerAddress.Text = "http://localhost:8080";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(581, 43);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 18);
            this.label15.TabIndex = 16;
            this.label15.Text = "Пароль:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(296, 43);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(54, 18);
            this.label14.TabIndex = 15;
            this.label14.Text = "Логин:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 43);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 18);
            this.label13.TabIndex = 14;
            this.label13.Text = "Сервер:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(374, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(108, 18);
            this.label12.TabIndex = 13;
            this.label12.Text = "Авторизация";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(677, 317);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(191, 39);
            this.button1.TabIndex = 12;
            this.button1.Text = "Отправить решение";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelServerSendGamePath
            // 
            this.labelServerSendGamePath.AutoSize = true;
            this.labelServerSendGamePath.Location = new System.Drawing.Point(14, 263);
            this.labelServerSendGamePath.Name = "labelServerSendGamePath";
            this.labelServerSendGamePath.Size = new System.Drawing.Size(112, 18);
            this.labelServerSendGamePath.TabIndex = 11;
            this.labelServerSendGamePath.Text = "<здесь адрес>";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(374, 148);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(162, 18);
            this.label10.TabIndex = 10;
            this.label10.Text = "Отправить решение";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 180);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(800, 72);
            this.label11.TabIndex = 9;
            this.label11.Text = resources.GetString("label11.Text");
            // 
            // tabServerRating
            // 
            this.tabServerRating.Controls.Add(this.panel8);
            this.tabServerRating.Location = new System.Drawing.Point(4, 27);
            this.tabServerRating.Name = "tabServerRating";
            this.tabServerRating.Padding = new System.Windows.Forms.Padding(3);
            this.tabServerRating.Size = new System.Drawing.Size(884, 481);
            this.tabServerRating.TabIndex = 1;
            this.tabServerRating.Text = "Предварительный рейтинг";
            this.tabServerRating.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.AutoScroll = true;
            this.panel8.BackColor = System.Drawing.SystemColors.Control;
            this.panel8.Controls.Add(this.label16);
            this.panel8.Controls.Add(this.labelServerGameCount);
            this.panel8.Controls.Add(this.labelServerPlayerScore);
            this.panel8.Controls.Add(this.labelServerTeams);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(3, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(878, 475);
            this.panel8.TabIndex = 2;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(19, 21);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(755, 36);
            this.label16.TabIndex = 12;
            this.label16.Text = "Предварительный рейтинг не оказывает влияние на итоговый результат, но может быть" +
    " использован для\r\nформирования турнирной сетки.";
            // 
            // labelServerGameCount
            // 
            this.labelServerGameCount.AutoSize = true;
            this.labelServerGameCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelServerGameCount.Location = new System.Drawing.Point(504, 68);
            this.labelServerGameCount.Name = "labelServerGameCount";
            this.labelServerGameCount.Size = new System.Drawing.Size(137, 18);
            this.labelServerGameCount.TabIndex = 2;
            this.labelServerGameCount.Text = "Сыграно матчей";
            // 
            // labelServerPlayerScore
            // 
            this.labelServerPlayerScore.AutoSize = true;
            this.labelServerPlayerScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelServerPlayerScore.Location = new System.Drawing.Point(698, 68);
            this.labelServerPlayerScore.Name = "labelServerPlayerScore";
            this.labelServerPlayerScore.Size = new System.Drawing.Size(89, 18);
            this.labelServerPlayerScore.TabIndex = 1;
            this.labelServerPlayerScore.Text = "Результат";
            // 
            // labelServerTeams
            // 
            this.labelServerTeams.AutoSize = true;
            this.labelServerTeams.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelServerTeams.Location = new System.Drawing.Point(19, 68);
            this.labelServerTeams.Name = "labelServerTeams";
            this.labelServerTeams.Size = new System.Drawing.Size(78, 18);
            this.labelServerTeams.TabIndex = 0;
            this.labelServerTeams.Text = "Команда";
            // 
            // tabServerReplays
            // 
            this.tabServerReplays.Controls.Add(this.panel9);
            this.tabServerReplays.Location = new System.Drawing.Point(4, 27);
            this.tabServerReplays.Name = "tabServerReplays";
            this.tabServerReplays.Padding = new System.Windows.Forms.Padding(3);
            this.tabServerReplays.Size = new System.Drawing.Size(884, 481);
            this.tabServerReplays.TabIndex = 2;
            this.tabServerReplays.Text = "Просмотр игр, проведенных на сервере";
            this.tabServerReplays.UseVisualStyleBackColor = true;
            // 
            // panel9
            // 
            this.panel9.AutoScroll = true;
            this.panel9.BackColor = System.Drawing.SystemColors.Control;
            this.panel9.Controls.Add(this.btnServerWatchGame);
            this.panel9.Controls.Add(this.labelServerGamePlayersAndResults);
            this.panel9.Controls.Add(this.labelServerGameTime);
            this.panel9.Controls.Add(this.label17);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(878, 475);
            this.panel9.TabIndex = 2;
            // 
            // btnServerWatchGame
            // 
            this.btnServerWatchGame.Location = new System.Drawing.Point(736, 44);
            this.btnServerWatchGame.Name = "btnServerWatchGame";
            this.btnServerWatchGame.Size = new System.Drawing.Size(127, 32);
            this.btnServerWatchGame.TabIndex = 16;
            this.btnServerWatchGame.Text = "Смотреть!";
            this.btnServerWatchGame.UseVisualStyleBackColor = true;
            this.btnServerWatchGame.Visible = false;
            // 
            // labelServerGamePlayersAndResults
            // 
            this.labelServerGamePlayersAndResults.AutoSize = true;
            this.labelServerGamePlayersAndResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelServerGamePlayersAndResults.Location = new System.Drawing.Point(109, 44);
            this.labelServerGamePlayersAndResults.Name = "labelServerGamePlayersAndResults";
            this.labelServerGamePlayersAndResults.Size = new System.Drawing.Size(188, 18);
            this.labelServerGamePlayersAndResults.TabIndex = 15;
            this.labelServerGamePlayersAndResults.Text = "Участники и результат";
            // 
            // labelServerGameTime
            // 
            this.labelServerGameTime.AutoSize = true;
            this.labelServerGameTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelServerGameTime.Location = new System.Drawing.Point(17, 44);
            this.labelServerGameTime.Name = "labelServerGameTime";
            this.labelServerGameTime.Size = new System.Drawing.Size(58, 18);
            this.labelServerGameTime.TabIndex = 14;
            this.labelServerGameTime.Text = "Время";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(17, 13);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(504, 18);
            this.label17.TabIndex = 13;
            this.label17.Text = "Для запуска кликните на один из доступных для просмотра повторов.";
            // 
            // tabServerGameRunner
            // 
            this.tabServerGameRunner.Controls.Add(this.panel4);
            this.tabServerGameRunner.Location = new System.Drawing.Point(4, 22);
            this.tabServerGameRunner.Name = "tabServerGameRunner";
            this.tabServerGameRunner.Padding = new System.Windows.Forms.Padding(3);
            this.tabServerGameRunner.Size = new System.Drawing.Size(884, 486);
            this.tabServerGameRunner.TabIndex = 3;
            this.tabServerGameRunner.Text = "Режим сервера";
            this.tabServerGameRunner.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Control;
            this.panel4.Controls.Add(this.edtServerActivateGameRunnerForServer);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(878, 480);
            this.panel4.TabIndex = 1;
            // 
            // edtServerActivateGameRunnerForServer
            // 
            this.edtServerActivateGameRunnerForServer.AutoSize = true;
            this.edtServerActivateGameRunnerForServer.Location = new System.Drawing.Point(16, 22);
            this.edtServerActivateGameRunnerForServer.Name = "edtServerActivateGameRunnerForServer";
            this.edtServerActivateGameRunnerForServer.Size = new System.Drawing.Size(123, 22);
            this.edtServerActivateGameRunnerForServer.TabIndex = 0;
            this.edtServerActivateGameRunnerForServer.Text = "Активировать";
            this.edtServerActivateGameRunnerForServer.UseVisualStyleBackColor = true;
            // 
            // tabHelp
            // 
            this.tabHelp.Controls.Add(this.panel3);
            this.tabHelp.Location = new System.Drawing.Point(4, 22);
            this.tabHelp.Name = "tabHelp";
            this.tabHelp.Size = new System.Drawing.Size(898, 523);
            this.tabHelp.TabIndex = 4;
            this.tabHelp.Text = "Помощь";
            this.tabHelp.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(898, 523);
            this.panel3.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 236);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(798, 18);
            this.label9.TabIndex = 7;
            this.label9.Text = "Кликните меню \"Помощь\" в игровом окне, чтобы ознакомиться с вспомогательными возм" +
    "ожностями системы.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(416, 18);
            this.label8.TabIndex = 6;
            this.label8.Text = "Нажмите кнопку НАЧАТЬ МАТЧ. Откроется игровое окно.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(385, 202);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 18);
            this.label7.TabIndex = 5;
            this.label7.Text = "Игровое окно";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(385, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(173, 18);
            this.label6.TabIndex = 4;
            this.label6.Text = "Добавление игроков";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(385, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 18);
            this.label5.TabIndex = 3;
            this.label5.Text = "Запуск матча";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(825, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "Кликните на добавленных игроков, чтобы выбрать их для участия в матче. Кликните п" +
    "овторно для отмены выбора.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(481, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Используйте левую панель, чтобы добавить игроков в программу. ";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 549);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(906, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 16;
            // 
            // StartForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(906, 571);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "StartForm";
            this.Text = "ContestAI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StartForm_FormClosed);
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StartForm_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabLocalGames.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabServer.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabServerSend.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.tabServerRating.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.tabServerReplays.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.tabServerGameRunner.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabHelp.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLocalGames;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnAddProgram;
        private System.Windows.Forms.Button btnAddHuman;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel panelPlayers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel panelPlayersInMatch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnChangeOrder;
        private System.Windows.Forms.Button btnClearSelection;
        private System.Windows.Forms.TabPage tabHelp;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChangeJavaPath;
        private System.Windows.Forms.TabPage tabServer;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabServerSend;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.TabPage tabServerRating;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TabPage tabServerReplays;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelServerSendGamePath;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox edtServerPassword;
        private System.Windows.Forms.TextBox edtServerLogin;
        private System.Windows.Forms.TextBox edtServerAddress;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label labelServerGameCount;
        private System.Windows.Forms.Label labelServerPlayerScore;
        private System.Windows.Forms.Label labelServerTeams;
        private System.Windows.Forms.Label labelServerGamePlayersAndResults;
        private System.Windows.Forms.Label labelServerGameTime;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TabPage tabServerGameRunner;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox edtServerActivateGameRunnerForServer;
        private System.Windows.Forms.Button btnServerWatchGame;
    }
}