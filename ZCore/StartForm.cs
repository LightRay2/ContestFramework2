using Framework;
using System;
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

namespace Framework
{
    public partial class StartForm : Form
    {
        //#region init everything
        public StartForm()
        {
            //Board.SetFrameworkSettings();
            InitializeComponent();
        }

        //FormState formState;
        //bool needRefreshControls = true;
        private void StartForm_Load(object sender, EventArgs e)
        {
           

        //    FrameworkSettings.PlayersPerGame = 4;
        //    formState = FormState.LoadOrCreate();

        //    #region  data bindings to editors
        //    edtMatchDate.DataBindings.Add("Value", formState, "MatchDate");
        //    edtMinTimePerMatch.DataBindings.Add("Text", formState, "MinTimePerMatch");
        //    #endregion

        //    formState.PropertyChanged += (s, args) => needRefreshControls = true;
        //    refreshTimer.Tick += (s, args) =>
        //    {
        //        if (needRefreshControls)
        //        {
        //            needRefreshControls = false;
        //            RefreshControls();
        //        }
        //    };
        //    refreshTimer.Start();

        //    gvMatches.Rows.Add(DateTime.Now.AddMinutes(10), "Lightray - Inspiration", "24-12");
        //    gvMatches.Rows.Add(DateTime.Now.AddMinutes(-1), "Lightray - abc04", "Идет 97 ход");
        //    gvMatches.Rows.Add(DateTime.Now.AddMinutes(-10), "Lightray - Inspiration", "9-17"); 
        //    gvMatches.Rows.Add(DateTime.Now.AddMinutes(-20), "Lightray - abc04", "3-5");

        //    gvMatches.Rows[0].DefaultCellStyle.BackColor = Color.Bisque;
        //    gvMatches.Rows[1].DefaultCellStyle.BackColor = Color.Khaki;
        //    gvMatches.Rows[2].DefaultCellStyle.BackColor = Color.LightGreen;
        //    gvMatches.Rows[3].DefaultCellStyle.BackColor = Color.LightGreen;

        //    if (FrameworkSettings.RunGameImmediately && formState.ProgramAddressesInMatch.Count > 0)
        //        btnRun_Click(null, null);
        }
        //#endregion

        //public object CreateGameParamsFromFormState(FormState state)
        //{
        //    var p = new GameParams();
        //    return p;
        //}

        //[DllImport("user32.dll")]
        //public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        //private const int WM_SETREDRAW = 11; 
        //private void SuspendDrawing()
        //{
        //        SendMessage(this.Handle, WM_SETREDRAW, false, 0);
        //}

        //private void ResumeDrawing()
        //{
            
        //        SendMessage(this.Handle, WM_SETREDRAW, true, 0);
        //}
        //public void RefreshControls()
        //{
        //    SuspendDrawing();
        //    Color checkedColor = Color.LawnGreen;
        //    Color uncheckedColor = Color.LightGray;
        //    ToolTip toolTip = new ToolTip();
        //    var panelPlayers_DeleteButtons = new List<Control>();
        //    #region panelPlayers
        //    panelPlayers.Controls.Clear();
        //    for (int i = 0; i < formState.ProgramAddressesAll.Count; i++)
        //    {
        //        string text = formState.ProgramAddressesAll[i] ?? "Человек";
        //        var checkBox = new CheckBox
        //        {
        //            Tag = i,
        //            Checked = formState.ProgramAddressesInMatch.Contains(i),
        //            Margin = new Padding { Left = 10, Top = 10 },
        //            Size = new Size(205, 30),
        //            FlatStyle = System.Windows.Forms.FlatStyle.Flat,
        //            Appearance = System.Windows.Forms.Appearance.Button,
        //            Text = new string(text.Reverse().Take(25).Reverse().ToArray()),
        //            BackColor = formState.ProgramAddressesInMatch.Contains(i) ? checkedColor : uncheckedColor
        //        };
        //        toolTip.SetToolTip(checkBox, text);
        //        var deleteButton = new Button
        //        {
        //            Tag = i,
        //            Margin = new Padding { All = 10 },
        //            Size = new Size(30, 30),
        //            FlatStyle = FlatStyle.Flat,
        //            Text = "-",
        //            BackColor = uncheckedColor
        //        };
        //        checkBox.CheckedChanged += PlayerCheckedChanged;
        //        deleteButton.Click += deleteButton_Click;
        //        panelPlayers.Controls.Add(checkBox);
        //        panelPlayers.Controls.Add(deleteButton);
        //        panelPlayers_DeleteButtons.Add(deleteButton);
        //    }
        //    #endregion

        //    #region panel players in match
        //    panelPlayersInMatch.Controls.Clear();
        //    for(int i =0; i<formState.ProgramAddressesInMatch.Count;i++){
        //        string text = formState.ProgramAddressesAll[formState.ProgramAddressesInMatch[i]] ?? "Человек";
        //        var label = new Label
        //        {
        //            Tag = i,
        //            Text = new string( text.Reverse().Take(70).Reverse().ToArray()),
        //            Padding = new Padding { All=5 },
        //            Margin = new Padding{All=3},
        //            Size  = new Size(560, 32),
        //            BorderStyle = BorderStyle.FixedSingle
        //        };
        //        toolTip.SetToolTip(label, text);
        //        panelPlayersInMatch.Controls.Add(label);
        //    }
        //    #endregion

        //    btnAddToGameList.Text = 
        //        string.Format("В список игр ({0})", formState.GameParamsList.Count);

        //    #region запуск матчей - локальный или серверный мод
        //    btnAddHuman.Enabled = btnAddProgram.Enabled = !formState.RunMatchesServerMode;
        //    panelPlayers_DeleteButtons.ForEach(b => b.Enabled = !formState.RunMatchesServerMode);
        //    panelMatchTimeOnServer.Visible = formState.RunMatchesServerMode;
        //    #endregion


        //    ResumeDrawing();
        //    this.Refresh();
        //}

        void deleteButton_Click(object sender, EventArgs e)
        {
        //    int index = (int)((Control)sender).Tag;
        //    formState.ProgramAddressesInMatch.Remove(index);
        //    for (int i = 0; i < formState.ProgramAddressesInMatch.Count; i++)
        //    {
        //        if (formState.ProgramAddressesInMatch[i] > index)
        //            formState.ProgramAddressesInMatch[i]--;
        //    }
        //    formState.ProgramAddressesAll.RemoveAt(index);
        }
        ////todo а как сделать изначальное состояние для копирования настроек? или поменять путь?
        //public void PlayerCheckedChanged(object sender, EventArgs e)
        //{
        //    var s = (CheckBox)sender;
        //    int index = (int)s.Tag;
        //    if(s.Checked){
        //        formState.ProgramAddressesInMatch.Add(index);
        //        if (FrameworkSettings.PlayersPerGame != 0 && formState.ProgramAddressesInMatch.Count > FrameworkSettings.PlayersPerGame)
        //        {
        //            formState.ProgramAddressesInMatch.RemoveAt(0);
        //            //todo check java path when run
        //        }
        //    }
        //    else {
        //        formState.ProgramAddressesInMatch.Remove(index);
        //    }
        //}

        private void btnAddProgram_Click(object sender, EventArgs e)
        {
        //    var lastAddress = formState.ProgramAddressesAll.LastOrDefault(x=>x!=null);
        //    var initialDirectory =Path.GetDirectoryName( Application.StartupPath ) + "//..//Players";
        //    if(!Directory.Exists(initialDirectory))
        //        initialDirectory = Path.GetDirectoryName( Application.StartupPath ) + "//..";
        //    if(!Directory.Exists(initialDirectory))
        //        initialDirectory =Path.GetDirectoryName( Application.StartupPath );
        //    openFileDialog1.InitialDirectory = lastAddress == null? initialDirectory: Path.GetDirectoryName(lastAddress);
        //    openFileDialog1.Filter = "Исполняемые файлы|*.exe;*.java";
        //    if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        formState.ProgramAddressesAll.Add(openFileDialog1.FileName);
        //        if (FrameworkSettings.PlayersPerGame != 0 && formState.ProgramAddressesInMatch.Count < FrameworkSettings.PlayersPerGame)
        //        {
        //            formState.ProgramAddressesInMatch.Add(formState.ProgramAddressesAll.Count - 1);
        //        }
        //    }
        }

        private void btnAddHuman_Click(object sender, EventArgs e)
        {
        //    formState.ProgramAddressesAll.Add(null);
        //    if (FrameworkSettings.PlayersPerGame != 0 && formState.ProgramAddressesInMatch.Count < FrameworkSettings.PlayersPerGame)
        //    {
        //        formState.ProgramAddressesInMatch.Add(formState.ProgramAddressesAll.Count - 1);
        //    }
        }

        private void btnChangeOrder_Click(object sender, EventArgs e)
        {
        //    if(formState.ProgramAddressesInMatch.Count != 0){
        //        int last= formState.ProgramAddressesInMatch.Last();
        //        formState.ProgramAddressesInMatch.RemoveAt(formState.ProgramAddressesInMatch.Count-1);
        //        formState.ProgramAddressesInMatch.Insert(0,last);
        //    }
        }

        private void btnClearSelection_Click(object sender, EventArgs e)
        {
        //    formState.ProgramAddressesInMatch.Clear();
        }

        ////todo local replays
        private void btnAddToGameList_Click(object sender, EventArgs e)
        {
        //    var newGameParams = CreateGameParamsFromFormState(formState); //todo вынести это в гейм?
        //    formState.GameParamsList.Add(newGameParams);
        }

        private void btnRemoveFromGameList_Click(object sender, EventArgs e)
        {
        //    if(formState.GameParamsList.Count != 0)
        //        formState.GameParamsList.RemoveAt(formState.GameParamsList.Count - 1);
            
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
        //    //todo run game and check java and count of programs
        //    GameCore<FormState, Turn, Round, Player>.TryRunAsSingleton( (x,y)=> new Board(x, y), new List<FormState> { formState }, null);
        //    //after all
        //    formState.GameParamsList.Clear();
        }

        private void btnSaveRoomDescription_Click(object sender, EventArgs e)
        {

        }

        private void StartForm_FormClosed(object sender, FormClosedEventArgs e)
        { 
        //    ExternalProgramExecuter.DeleteTempSubdir(); //todo framework

        }
    }
}
