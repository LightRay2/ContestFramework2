using Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Framework
{
    public partial class GameForm : Form
    {
        Func<GlInput, Frame> _processMethod;
        public static bool UserWantsToClose = false;
        public static bool GameInBackgroundRunning = false;
        double _watchSpeedMultiplier;
        List<Tuple<double, ToolStripMenuItem>> speedItems;
        public  double watchSpeedMultiplier { get { return _watchSpeedMultiplier; }
        set
            {
                menuSpeed10.Checked = menuSpeed20.Checked = menuSpeed40.Checked = menuSpeed60.Checked = menuSpeed80.Checked = menuSpeed100.Checked
                = menuSpeed150.Checked = menuSpeed200.Checked = menuSpeed250.Checked = menuSpeed300.Checked = false;
                speedItems.OrderBy(x => Math.Abs(value - x.Item1)).First().Item2.Checked = true;
                _watchSpeedMultiplier = value;
            }
        }

        bool _gamePaused = false;
        public bool GamePaused { get { return _gamePaused; } set { _gamePaused = value; menuPause.Checked = value; } }
        bool _userWantsPauseAfterTurn = false;
        public bool UserWantsPauseAfterTurn { get { return _userWantsPauseAfterTurn; }
        set
            {
                _userWantsPauseAfterTurn = value;
                паузаПослеТекущегоХодаTABToolStripMenuItem.Checked = value;
                if(value)
                    GamePaused = false;    
            }
        }

        public string InfoUnderMouse { get { return infoUnderMouse.Text; } set { infoUnderMouse.Text = value; } }

        public string InfoAction { get { return  infoActions.Text; } set { infoActions.Text = value; } }







        public GameForm(Func<GlInput, Frame> processMethod)
        {
            UserWantsToClose = false;
        _processMethod = processMethod;
            
            InitializeComponent();
            if (!DesignMode)
            {
                this.glControl1 = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 8, 4), 1, 0, OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible);
                this.glControl1.BackColor = System.Drawing.Color.Black;
                this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
               // this.glControl1.Location = new System.Drawing.Point(0, 25);
               // this.glControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
                this.glControl1.Name = "glControl1";
              //  this.glControl1.Size = new System.Drawing.Size(928, 675);
                this.glControl1.TabIndex = 0;
                this.glControl1.VSync = false;
                this.panelForOpenglControl.Controls.Add(this.glControl1);
            }


            speedItems = new List<Tuple<double, ToolStripMenuItem>> {
                    Tuple.Create(0.1, menuSpeed10)  ,
                    Tuple.Create(0.2, menuSpeed20), Tuple.Create(0.4, menuSpeed40), Tuple.Create(0.60, menuSpeed60),
                    Tuple.Create(0.80, menuSpeed80), Tuple.Create(1.00, menuSpeed100), Tuple.Create(1.50, menuSpeed150),
                    Tuple.Create(3.00, menuSpeed200), Tuple.Create(2.50, menuSpeed250), Tuple.Create(3.00, menuSpeed300) };
        }
        
        private void GameForm_Load(object sender, EventArgs e)
        {
            var screen = Screen.AllScreens;
            Rectangle monitorSize = screen[0].WorkingArea;
            int h = monitorSize.Bottom ;
            int w = Math.Min(h * 4 / 3,
                monitorSize.Right );

            this.Location = new Point();
            this.Size = new Size(w, h);

            GameController _mainController = new GameController(glControl1,
               _processMethod, this);
            Thread.Sleep(50);//опенгл почему то не всегда до конца прогружается
        }

        public DialogResult ShowDialog(bool uselesss)
        {
         //   while (loaded == false)
        //        Thread.Sleep(10);
            
            return this.ShowDialog();
        }
        public DialogResult ThreadSafeMessageBox(string title, string message, MessageBoxButtons buttons)
        {
            return ((DialogResult)this.Invoke(
                new Func<string, string, MessageBoxButtons, DialogResult>(MessageBox.Show),
               message, title,buttons) ) ;
        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.glControl1.Dispose();
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
             
            if (GameInBackgroundRunning)
            {
                UserWantsToClose = true;
                e.Cancel = true;
            }
    }

        private void menuSpeed10_Click(object sender, EventArgs e)
        {
            watchSpeedMultiplier = speedItems.First(x=>((ToolStripMenuItem)sender) ==x.Item2).Item1;
        }

        private void menuSpeed20_Click(object sender, EventArgs e)
        {
            watchSpeedMultiplier = speedItems.First(x=>((ToolStripMenuItem)sender) ==x.Item2).Item1;
           
        }

        private void menuSpeed40_Click(object sender, EventArgs e)
        {
            watchSpeedMultiplier = speedItems.First(x => ((ToolStripMenuItem)sender) == x.Item2).Item1;

        }

        private void menuSpeed60_Click(object sender, EventArgs e)
        {
            watchSpeedMultiplier = speedItems.First(x => ((ToolStripMenuItem)sender) == x.Item2).Item1;

        }

        private void menuSpeed80_Click(object sender, EventArgs e)
        {

            watchSpeedMultiplier = speedItems.First(x=>((ToolStripMenuItem)sender) ==x.Item2).Item1;
        }

        private void menuSpeed100_Click(object sender, EventArgs e)
        {
            watchSpeedMultiplier = speedItems.First(x => ((ToolStripMenuItem)sender) == x.Item2).Item1;

        }

        private void menuSpeed150_Click(object sender, EventArgs e)
        {
            watchSpeedMultiplier = speedItems.First(x => ((ToolStripMenuItem)sender) == x.Item2).Item1;

        }

        private void menuSpeed200_Click(object sender, EventArgs e)
        {

            watchSpeedMultiplier = speedItems.First(x=>((ToolStripMenuItem)sender) ==x.Item2).Item1;
        }

        private void menuSpeed250_Click(object sender, EventArgs e)
        {

            watchSpeedMultiplier = speedItems.First(x=>((ToolStripMenuItem)sender) ==x.Item2).Item1;
        }

        private void menuSpeed300_Click(object sender, EventArgs e)
        {
            watchSpeedMultiplier = speedItems.First(x => ((ToolStripMenuItem)sender) == x.Item2).Item1;

        }

        private void menuPauseClicked(object sender, EventArgs e)
        {
            GamePaused = !GamePaused;
        }

        private void паузаПослеТекущегоХодаTABToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserWantsPauseAfterTurn = true;
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("ПОШАГОВОЕ ВОСПРОИЗВЕДЕНИЕ: нажимайте клавишу '[' для просмотра одиночных ходов. Для возобновления нажмите P (отключение паузы).");
            sb.AppendLine();
            sb.AppendLine("ПОМОЩЬ В ОТЛАДКЕ: наведите мышкой на список ходов справа и используйте колесико или клавиши ВВЕРХ, ВНИЗ, PAGE UP, PAGE DOWN для перемотки. Кликните на ход для перехода (дополнительно в буфер обмена скопируется input.txt для данного хода). Наведите на ход и нажмите I или O для получения input.txt или output.txt для данного хода. Для вставки текста из буфера обмена используйте сочетание CTRL+V в любом текстовом редакторе.");
            sb.AppendLine();
            sb.AppendLine("СТАТУС ХОДА: при наведении на ход в правом нижнем углу выводится его статус. Если ход некорректен, он будет выделен в списке справа.");
            sb.AppendLine();
            sb.AppendLine(FrameworkSettings.AdditionalHelpOnGameForm);
            MessageBox.Show(sb.ToString());
        }
    }
}
