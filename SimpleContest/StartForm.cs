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

namespace SimpleContest
{
    public partial class StartForm : Form
    {
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
               
                if (CheckSelectSetJavaPath(new List<string> { openFileDialog1.FileName } ))
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
            bool required = programAddresses.Any(x=>x.Substring(x.Length - 4) == ".jar");

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
            if(formState.ProgramAddressesInMatch.Count < FrameworkSettings.PlayersPerGameMin)
            {
                MessageBox.Show("Для запуска матча требуется игроков: " + FrameworkSettings.PlayersPerGameMax.ToString());
                return;
            }
            //нужно встряхнуть рандомайзер
            formState.RandomSeed = new Random().Next();
            GameCore<FormState, Turn, Round, Player>.TryRunAsSingleton((x, y) => new SimpleGame(x, y), new List<FormState> { formState }, null);
            
            formState.GameParamsList.Clear(); //todo remove
        }

        private void btnSaveRoomDescription_Click(object sender, EventArgs e)
        {

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
    }
}
