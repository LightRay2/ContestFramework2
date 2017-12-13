using Framework;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SimpleContest
{
    public partial class StartForm : Form
    {
        #region LOCAL

        #region init everything
        public StartForm()
        {
            SimpleGame.SetFrameworkSettings();
            this.KeyPreview = true;
            InitializeComponent();
        }

        FormState formState;
        bool needRefreshControls = true;
        private void StartForm_Load(object sender, EventArgs e)
        {
            LoadFormState();


            refreshTimer.Tick += (s, args) =>
            {
                if (needRefreshControls)
                {
                    needRefreshControls = false;
                    RefreshControls();
                }
            };
            refreshTimer.Start();


            if (FrameworkSettings.RunGameImmediately && formState.ProgramAddressesInMatch.Count > 0)
                btnRun_Click(null, null);

            InitServer();
        }

        private void LoadFormState()
        {
            formState = FormState.LoadOrCreate();


            formState.PropertyChanged += (s, args) => needRefreshControls = true;
        }
        #endregion

        public object CreateGameParamsFromFormState(FormState state)
        {
            var p = new GameParams();
            return p;
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;
        private void SuspendDrawing()
        {
            SendMessage(this.Handle, WM_SETREDRAW, false, 0);
        }

        private void ResumeDrawing()
        {

            SendMessage(this.Handle, WM_SETREDRAW, true, 0);
        }
        public void RefreshControls()
        {
            SuspendDrawing();
            Color checkedColor = Color.LawnGreen;
            Color uncheckedColor = Color.LightGray;
            ToolTip toolTip = new ToolTip();
            var panelPlayers_DeleteButtons = new List<Control>();
            #region panelPlayers
            panelPlayers.Controls.Clear();
            for (int i = 0; i < formState.ProgramAddressesAll.Count; i++)
            {
                string text = formState.ProgramAddressesAll[i] ?? "Человек";
                var checkBox = new CheckBox
                {
                    Tag = i,
                    Checked = formState.ProgramAddressesInMatch.Contains(i),
                    Margin = new Padding { Left = 10, Top = 10 },
                    Size = new Size(205, 30),
                    FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                    Appearance = System.Windows.Forms.Appearance.Button,
                    Text = new string(text.Reverse().Take(25).Reverse().ToArray()),
                    BackColor = formState.ProgramAddressesInMatch.Contains(i) ? checkedColor : uncheckedColor
                };
                toolTip.SetToolTip(checkBox, text);
                var deleteButton = new Button
                {
                    Tag = i,
                    Margin = new Padding { All = 10 },
                    Size = new Size(30, 30),
                    FlatStyle = FlatStyle.Flat,
                    Text = "-",
                    BackColor = uncheckedColor
                };
                checkBox.CheckedChanged += PlayerCheckedChanged;
                deleteButton.Click += deleteButton_Click;
                panelPlayers.Controls.Add(checkBox);
                panelPlayers.Controls.Add(deleteButton);
                panelPlayers_DeleteButtons.Add(deleteButton);
            }
            #endregion

            #region panel players in match
            panelPlayersInMatch.Controls.Clear();
            for (int i = 0; i < formState.ProgramAddressesInMatch.Count; i++)
            {
                string text = formState.ProgramAddressesAll[formState.ProgramAddressesInMatch[i]] ?? "Человек";
                var label = new Label
                {
                    Tag = i,
                    Text = new string(text.Reverse().Take(70).Reverse().ToArray()),
                    Padding = new Padding { All = 5 },
                    Margin = new Padding { All = 3 },
                    Size = new Size(560, 32),
                    BorderStyle = BorderStyle.FixedSingle
                };
                toolTip.SetToolTip(label, text);
                panelPlayersInMatch.Controls.Add(label);
            }
            #endregion


            btnChangeJavaPath.Visible = string.IsNullOrEmpty(formState.JavaPath) == false;

            RefreshControlsOnServerTab();

            ResumeDrawing();
            this.Refresh();
        }

        void deleteButton_Click(object sender, EventArgs e)
        {
            int index = (int)((Control)sender).Tag;
            formState.RemoveProgramAddress(index);

        }
        //todo а как сделать изначальное состояние для копирования настроек? или поменять путь?
        public void PlayerCheckedChanged(object sender, EventArgs e)
        {
            var s = (CheckBox)sender;
            int index = (int)s.Tag;
            if (s.Checked)
            {
                formState.ProgramAddressesInMatch.Add(index);
                if (FrameworkSettings.PlayersPerGameMax != 0 && formState.ProgramAddressesInMatch.Count > FrameworkSettings.PlayersPerGameMax)
                {
                    formState.ProgramAddressesInMatch.RemoveAt(0);
                    //todo check java path when run
                }
            }
            else
            {
                formState.ProgramAddressesInMatch.Remove(index);
            }
        }

        private void btnAddProgram_Click(object sender, EventArgs e)
        {
            var lastAddress = formState.ProgramAddressesAll.LastOrDefault(x => x != null);
            var initialDirectory = Path.GetDirectoryName(Application.StartupPath) + "//..//Players";
            if (!Directory.Exists(initialDirectory))
                initialDirectory = Path.GetDirectoryName(Application.StartupPath) + "//..";
            if (!Directory.Exists(initialDirectory))
                initialDirectory = Path.GetDirectoryName(Application.StartupPath);
            openFileDialog1.InitialDirectory = lastAddress == null ? initialDirectory : Path.GetDirectoryName(lastAddress);
            openFileDialog1.Filter = "Исполняемые файлы|*.exe;*.jar";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                if (CheckSelectSetJavaPath(new List<string> { openFileDialog1.FileName }))
                {
                    formState.ProgramAddressesAll.Add(openFileDialog1.FileName);
                    if (FrameworkSettings.PlayersPerGameMax != 0 && formState.ProgramAddressesInMatch.Count < FrameworkSettings.PlayersPerGameMax)
                    {
                        formState.ProgramAddressesInMatch.Add(formState.ProgramAddressesAll.Count - 1);
                    }
                }
            }
        }

        bool CheckSelectSetJavaPath(List<string> programAddresses)
        {
            if (string.IsNullOrEmpty(formState.JavaPath) == false && File.Exists(formState.JavaPath))
                return true; //уже задан
            bool required = programAddresses.Any(x => x.Substring(x.Length - 4) == ".jar");

            if (required == false)
                return true;


            var folderDialog = new FolderBrowserDialog
            {
                Description = "Укажите директорию Java (например, " + @"C:\Program Files\Java\jre1.8.0_73 )",
                ShowNewFolderButton = false

            };
            folderDialog.ShowNewFolderButton = false;
            folderDialog.Description = @"Укажите директорию Java (например, 
C:\Program Files (x86)\Java\jdk1.7.0_55 или 
C:\Program Files\Java\jre1.8.0_73 )";
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string javaPath = (folderDialog.SelectedPath + "\\bin\\java.exe");
                if (File.Exists(javaPath))
                {
                    formState.JavaPath = javaPath;
                    return true;
                }
                else
                {
                    MessageBox.Show("Выбранная директория не содержит путь /bin/java.exe");
                    return false;
                }
            }
            else
                return false;
        }

        private void btnAddHuman_Click(object sender, EventArgs e)
        {
            formState.ProgramAddressesAll.Add(null);
            if (FrameworkSettings.PlayersPerGameMax != 0 && formState.ProgramAddressesInMatch.Count < FrameworkSettings.PlayersPerGameMax)
            {
                formState.ProgramAddressesInMatch.Add(formState.ProgramAddressesAll.Count - 1);
            }
        }

        private void btnChangeOrder_Click(object sender, EventArgs e)
        {
            if (formState.ProgramAddressesInMatch.Count != 0)
            {
                int last = formState.ProgramAddressesInMatch.Last();
                formState.ProgramAddressesInMatch.RemoveAt(formState.ProgramAddressesInMatch.Count - 1);
                formState.ProgramAddressesInMatch.Insert(0, last);
            }
        }

        private void btnClearSelection_Click(object sender, EventArgs e)
        {
            formState.ProgramAddressesInMatch.Clear();
        }


        private void btnRun_Click(object sender, EventArgs e)
        {
            if (CheckSelectSetJavaPath(formState.ProgramAddressesAll.ToList()) == false)
                return;
            if (formState.ProgramAddressesInMatch.Count < FrameworkSettings.PlayersPerGameMin)
            {
                MessageBox.Show("Для запуска матча требуется игроков: " + FrameworkSettings.PlayersPerGameMax.ToString());
                return;
            }
            //нужно встряхнуть рандомайзер
            formState.RandomSeed = new Random().Next();
            GameCore<FormState, Turn, Round, Player>.TryRunAsSingleton((x, y) => new SimpleGame(x, y), new List<FormState> { formState }, null);

            formState.GameParamsList.Clear(); //todo remove
        }
        

        private void btnChangeJavaPath_Click(object sender, EventArgs e)
        {
            formState.JavaPath = null;
            CheckSelectSetJavaPath(formState.ProgramAddressesAll.ToList());
        }

        private void StartForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ExternalProgramExecuter.DeleteTempSubdir(); //todo framework

        }

        private void StartForm_KeyDown(object sender, KeyEventArgs e)
        {
            //пусть и на продуктиве будет
            // if (Debugger.IsAttached)
            {
                if (e.Control && e.Shift && e.KeyCode == Keys.C) //config
                {
                    //пересоздать конфиг
                    try
                    {
                        File.Delete(FormState.saveLoadPath);
                        LoadFormState();
                        needRefreshControls = true;
                    }
                    catch { }
                }
            }
        }

        #endregion












        #region SERVER
        ServerInfo serverInfo = new ServerInfo();
        ServerCall call;
        Timer gameRunnerTimer = new Timer { Interval = 1000 };
        void InitServer()
        {
            edtServerAddress.DataBindings.Add("Text", formState, "ServerAddress");
            edtServerLogin.DataBindings.Add("Text", formState, "ServerLogin");
            edtServerPassword.DataBindings.Add("Text", formState, "ServerPassword");
            edtServerActivateGameRunnerForServer.CheckedChanged += EdtServerActivateGameRunnerForServer_CheckedChanged;
            gameRunnerTimer.Tick += GameRunnerTimer_Tick;
            gameRunnerTimer.Start();
        }

       

        void RefreshControlsOnServerTab()
        {
            labelServerSendGamePath.Text = formState.ProgramAddressesInMatch.Count == 0 ? "" :
                 formState.ProgramAddressesAll[formState.ProgramAddressesInMatch[0]];

            #region rating
            {
                var scores = serverInfo.allGames.SelectMany(x => x.Item3).GroupBy(x => x.Item1)
                         .Select(group => Tuple.Create(group.Key, group.Count(), group.Aggregate(0, (cur, next) => cur += int.Parse(next.Item2))))
                         .OrderByDescending(x => x.Item3).ToList();
                int dy = labelServerTeams.Height+5;
                int starty = labelServerTeams.Location.Y + dy;
                for (int i = 0; i < scores.Count; i++)
                {
                    {
                        var label = new Label();
                        label.Location = new Point(labelServerTeams.Location.X, starty + dy * i);
                        label.Text = " "+ scores[i].Item1;
                        labelServerTeams.Parent.Controls.Add(label);
                    }
                    {
                        var label = new Label();
                        label.Location = new Point(labelServerGameCount.Location.X, starty + dy * i);
                        label.Text = " " + scores[i].Item2.ToString();
                        labelServerTeams.Parent.Controls.Add(label);
                    }
                    {
                        var label = new Label();
                        label.Location = new Point(labelServerPlayerScore.Location.X, starty + dy * i);
                        label.Text = " " + scores[i].Item3.ToString();
                        labelServerTeams.Parent.Controls.Add(label);
                    }
                }
            }
            #endregion

            #region games to watch
            {
                var games = serverInfo.allGames.Where(x => x.Item3.Any(playerName => formState.ServerLogin == playerName.Item1)).OrderByDescending(x=>x.Item2).ToList();
                int dy = btnServerWatchGame.Height+5;
                int starty = labelServerGameTime.Location.Y + dy;
                for (int i = 0; i < games.Count; i++)
                {
                    {
                        var label = new Label();
                        label.Location = new Point(labelServerGameTime.Location.X, starty + dy * i);
                        label.Text = games[i].Item2.ToString("HH:mm");
                        labelServerGameTime.Parent.Controls.Add(label);
                    }
                    {
                        var label = new Label();
                        label.Location = new Point(labelServerGamePlayersAndResults.Location.X, starty + dy * i);
                        label.Text = " " + string.Join(" ", games[i].Item3.Select(x=>$"{x.Item1}({x.Item2})"));
                        labelServerGameTime.Parent.Controls.Add(label);
                    }
                    {
                        var button = new Button();
                        button.Location = new Point(btnServerWatchGame.Location.X, starty + dy * i);
                        button.Size = btnServerWatchGame.Size;
                        button.Text = " " + btnServerWatchGame.Text;
                        button.Tag = games[i].Item1;
                        button.Click += btnServerWatchGame_Click;
                        labelServerGameTime.Parent.Controls.Add(button);
                    }
                }
            }
            #endregion
        }

        private void btnServerWatchGame_Click(object sender, EventArgs e)
        {
            var guid = (Guid)((Button)sender).Tag;
            var tempPath = Path.GetTempPath();
            try
            {
                var xml = call.Call<string>("GetGame", "get game start", "get game finish", null, guid);
                if(xml != null)
                {
                    var file = tempPath + Path.DirectorySeparatorChar + guid.ToString();
                    File.WriteAllText(file,xml);
                    GameCore<FormState, Turn, Round, Player>.TryRunAsSingleton((x, y) => new SimpleGame(x, y), new List<FormState> { formState }, file);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Не удалось воспроизвести");
            }
            //todo delete file
        }

        private void EdtServerActivateGameRunnerForServer_CheckedChanged(object sender, EventArgs e)
        {
            serverInfo.activateServerMode = edtServerActivateGameRunnerForServer.Checked;
            needRefreshControls = true;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //connect
            call = new ServerCall(this, formState.ServerAddress, formState.ServerLogin, formState.ServerPassword, MessageFromServerCall_Async, MessageFromServer);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //send exe

            int PART_SIZE = 32000;
            var path = labelServerSendGamePath.Text;

            if (File.Exists(path))
            {
                int currentFilePart = 0;
                var allBytes = File.ReadAllBytes(path);
                int partCount = (int)Math.Ceiling((double)allBytes.Length / PART_SIZE - 0.000000000001);
                var currentFile = new byte[partCount][];
                for (int i = 0; i < allBytes.Length; i += PART_SIZE)
                {
                    int partNumber = i / PART_SIZE;
                    int size = Math.Min(PART_SIZE, allBytes.Length - i);
                    currentFile[partNumber] = new byte[size];
                    Array.Copy(allBytes, i, currentFile[partNumber], 0, size);

                }

                Guid fileId = call.Call<Guid>("StartUploadingAndGetId", "start send exe", "file id got", Guid.Empty, Path.GetFileName(path), partCount);
                if (fileId == Guid.Empty)
                    return;
                //todo parallel for?
                for (int i = 0; i < partCount; i++)
                {
                    bool success = call.CallVoid("LoadFilePart", $"start upload {i + 1} of {partCount}", "finish upload", fileId, i, currentFile[i]);
                    if (!success)
                        break;
                }


            }
            else
                MessageBox.Show("Файл не найден");
        }

        void MessageFromServerCall_Async(string message)
        {

        }

        private void GameRunnerTimer_Tick(object sender, EventArgs e)
        {
            if (call != null)
            {
                var currentState = call.Call<Guid>("GetCurrentState", "", "", Guid.Empty);
                if (currentState != Guid.Empty && serverInfo.currentState != currentState)
                {
                    serverInfo.currentState = currentState;
                    var allGameResult = call.Call<List<Tuple<Guid, DateTime, List<Tuple<string, string>>>>>("GetAllGameResults", "start get all game results", "finish get all game results", null);
                    if (allGameResult != null)
                    {
                        serverInfo.allGames = allGameResult;
                        needRefreshControls = true;
                    }
                }
            }

            bool canRun = serverInfo.activateServerMode && GameCore<FormState, Turn, Round, Player>.IsWorking == false;
            if (canRun == false)
                return;
            Guid gameId = call.Call("GameRunnerGetGameId", "GameRunnerGetGameId start", "GameRunnerGetGameId finish", Guid.Empty);
            if (gameId != Guid.Empty)
            {
                var programCount = call.Call<int>("GameRunnerGetPlayerCount", "get player count start", "get player count finish", -1, gameId);
                if (programCount != -1)
                {

                    var directory = Path.GetTempPath() + Path.DirectorySeparatorChar + gameId.ToString();
                    Directory.CreateDirectory(directory);
                    var playerPaths = new List<string>(); //todo javapath and python??
                    for (int i = 0; i < programCount; i++)
                    {
                        var player = call.Call<Tuple<string, byte[]>>("GameRunnerGetPlayer", "start", "finish", null, gameId, i);
                        if (player == null)
                            return;
                        var path = directory + Path.DirectorySeparatorChar + player.Item1;
                        File.WriteAllBytes(path, player.Item2);
                        playerPaths.Add(path);
                    }

                    int cur = formState.ProgramAddressesAll.Count;
                    playerPaths.ForEach(path => formState.ProgramAddressesAll.Add(path));
                    formState.ProgramAddressesInMatch.Clear();
                    for (int i = cur; i < formState.ProgramAddressesAll.Count; i++)
                        formState.ProgramAddressesInMatch.Add(i);
                    formState.ReplayFolder = directory;
                    formState.ReplayFileName = "replay.txt";//now сломал его
                    formState.SaveReplays = true;
                    //нужно встряхнуть рандомайзер
                    formState.RandomSeed = new Random().Next();

                    //если игра не завершилась(прервали), вернет null
                    var gameResultOrNull = GameCore<FormState, Turn, Round, Player>.TryRunAsSingleton((x, y) => new SimpleGame(x, y), new List<FormState> { formState }, null, closeAutomaticallyAfterGameFinish: true);
                    if (gameResultOrNull == null)
                    {
                        serverInfo.activateServerMode = false;
                    }
                    else
                    {
                        var replay = formState.ReplayFolder + Path.DirectorySeparatorChar + formState.ReplayFileName;
                        var bytes = File.ReadAllText(replay);
                        call.CallVoid("GameRunnerGamePlayed", "game played start", "game played finish", gameId, gameResultOrNull, bytes);
                    }
                }
            }
        }

        void MessageFromServer(string message)
        {
            if(message == "Refresh")
            {
                
            }
            
        }
        

        public class ServerInfo
        {
            public List<Tuple<Guid, DateTime, List<Tuple<string, string>>>> allGames = new List<Tuple<Guid, DateTime, List<Tuple<string, string>>>>();
            public bool activateServerMode = false;//now handler here
            public Guid currentState = Guid.Empty;
        }


        /// <summary>
        /// Если связь потеряна, будет пытаться восстановить связь перед каждым Call
        /// </summary>
        class ServerCall
        {
            #region 

            public bool IsConnected { get; set; }
            public Exception LastException { get; set; }
            IHubProxy hubProxy;
            Form _form;
            Action<string> _logMessageAction, _MessageFromServerHandler;
            string _serverAddress, _login, _password;
            Timer _messageFromServerRefreshTimer = new Timer { Interval = 16 };
            ConcurrentBag<string> _messagesFromServer = new ConcurrentBag<string>();
            public ServerCall(Form form, string serverAddress, string login, string password, Action<string> LogMessageAction, Action<string> MessageFromServerHandler_Sync)
            {

                _form = form;
                _logMessageAction = LogMessageAction;
                _MessageFromServerHandler = MessageFromServerHandler_Sync;
                _serverAddress = serverAddress;
                _login = login;
                _password = password;

                _messageFromServerRefreshTimer.Tick += (o, e) => { while (_messagesFromServer.TryTake(out string message)) _MessageFromServerHandler(message); };
                _messageFromServerRefreshTimer.Start();

                //hubProxy.On("hello", () => Invoke(new Action(() => this.Text = "Success")));
                //hubProxy.On("authorizeResult", new Action<int, bool>((myId, isAdmin) => { this.myIdOnServer = (int)myId; ImAdmin = isAdmin; this.Invoke(new Action(() => SetAdminButtonsVisibility(isAdmin))); }));
                //hubProxy.On("fileId", (myFileId) => UploadFileToServer((int)myFileId));
                //hubProxy.On("message", (text) => AddServerMessage(text));
                //hubProxy.On("setRoomState", (x) =>
                //{
                //    gameList = JsonConvert.DeserializeObject<List<ServerGame>>(x.gameList.ToString());
                //    playerList = JsonConvert.DeserializeObject<List<ServerPlayer>>(x.playerList.ToString());
                //    this.Invoke(new Action(() => this.RefreshGameFrid()));
                //});
                //hubProxy.On("roundFinished", new Action<int, int, dynamic>((gameId, roundNumber, x) => RoundFinished(gameId, roundNumber, JsonConvert.DeserializeObject<object>(x.ToString()))));
                //hubProxy.On("runGameFromServer", (game) => RunGameFromServer(JsonConvert.DeserializeObject<ServerGame>(game.ToString())));

                //если тут вылез эксепшн, вероятно, Core.Config.serverAddress некорректен


                Connect();
            }


            public bool CallVoid(string serverMethod, string messageBeforeCall, string messageAfterSuccess, params object[] args)
            {
                //задублировано с кодом ниже
                if (!IsConnected)
                {
                    Connect();
                }
                if (IsConnected)
                {
                    try
                    {
                        Message(messageBeforeCall);
                        var task = hubProxy.Invoke<string>(serverMethod, args);
                        task.Wait();
                        if (string.IsNullOrEmpty(task.Result) == false)
                        {
                            Message(task.Result);
                        }
                        else
                        {
                            Message(messageAfterSuccess);
                            return true;
                        }
                    }
                    catch
                    {
                        Message("Ошибка при отправке сообщения серверу");
                    }
                }
                return false;
            }

            public T Call<T>(string serverMethod, string messageBeforeCall, string messageAfterSuccess, T returnIfError, params object[] args)
            {
                //задублировано с кодом выше
                if (!IsConnected)
                {
                    Connect();
                }
                if (IsConnected)
                {
                    try
                    {
                        Message(messageBeforeCall);
                        var task = hubProxy.Invoke<Tuple<T, string>>(serverMethod, args);
                        task.Wait();
                        if (string.IsNullOrEmpty(task.Result.Item2) == false)
                        {
                            Message(task.Result.Item2);
                        }
                        else
                        {
                            Message(messageAfterSuccess);
                            return task.Result.Item1;
                        }
                    }
                    catch
                    {
                        Message("Ошибка при отправке сообщения серверу");

                    }
                }

                return returnIfError;
            }

            //public void Call(ServerMethods serverMethod, string messageBeforeCall, string messageAfterSuccess, params object[] args)
            //{
            //    Call(serverMethod.ToString(), messageBeforeCall, messageAfterSuccess, args);
            //}

            //public T Call<T>(ServerMethods serverMethod, string messageBeforeCall, string messageAfterSuccess, params object[] args)
            //{
            //    return Call<T>(serverMethod.ToString(), messageBeforeCall, messageAfterSuccess, args);
            //}


            private void Connect()
            {
                try
                {
                    Message("Установка соединения...");
                    var hubConnection = new HubConnection(_serverAddress);
                    hubProxy = hubConnection.CreateHubProxy("MainHub");
                    hubProxy.On("Message", (message) => _messagesFromServer.Add(message.ToString()));
                    hubConnection.Closed += () => { Message("Соединение с сервером потеряно"); IsConnected = false; };

                    hubConnection.Start().Wait();
                    //id или сообщение об ошибке
                    var task = hubProxy.Invoke<string>("ConnectAsParticipant", _login, _password);
                    task.Wait();
                    if (string.IsNullOrEmpty(task.Result) == false)
                    {
                        Message(task.Result);
                    }
                    else
                    {
                        IsConnected = true;
                        Message("Соединение установлено");
                    }
                }
                catch (Exception ex)
                {
                    LastException = ex;
                    Message("Не удалось установить соединение. Возможно, введенный адрес сервера является некорректным");
                }
            }

            void Message(string message)
            {
                var text = DateTime.Now.ToShortTimeString() + ": " + message;
                _form.Invoke(new Action(() => _logMessageAction(message)));
            } 
            #endregion


        }



        #endregion

      
    }
}
