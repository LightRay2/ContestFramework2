using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignalRServer
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();

        }

        Timer _timer = new Timer { Interval = 1000 };
        private void MainForm_Load(object sender, EventArgs e)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8080";
            WebApp.Start(url);
            labelServerAddress.Text = $"Адрес сервера: {url}";


            //DB db = new DB();
            //db.SaveChanges();

            try
            {
                var db = State.e.CreateDb();
                if (db.Player.Count() == 0)
                {
                    db.Player.Add(new Player { Name = "one", Password = "one" });
                    db.Player.Add(new Player { Name = "two", Password = "two" });
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                //возможно , помогут две закомментированные строки сверху (создают пустую базу на диске С) или экземплр пустой базы в корне солюшна
                throw;
            }

            _timer.Tick += _timer_Tick;
            _timer.Start();
            _timer_Tick(null, null);
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            lock(MainHub.LOCKER)
            {
                var db = State.e.CreateDb();
                {
                    var sb = new StringBuilder();
                    var users =  db.Player.Where(x => x.Solution != null).OrderByDescending(x=>x.SolutionSubmitDateTime).ToList();
                    sb.AppendLine($"Пользователи, отправившие решение ({users.Count}):");

                    for (int i = 0; i < users.Count; i++)
                    {
                        var time = users[i].SolutionSubmitDateTime.ToString("HH:mm");
                        sb.AppendLine($"{time}. {users[i].Name} ({users[i].SolutionExtension})");
                    }
                    labelPlayersSentExe.Text = sb.ToString();
                }
                {
                    var sb = new StringBuilder();
                    var users =  State.e.connectedParticipants.Select(x=> db.Player.Find(x.Value)).OrderBy(x=>x.Name).ToList();
                    sb.AppendLine($"Пользователи online ({users.Count}):");
                    for (int i = 0; i < users.Count; i++)
                    {

                        sb.AppendLine($"{users[i].Name}");
                    }
                    label1.Text = sb.ToString();
                }

                labelCountOfGameRunners.Text = $"Количество серверов для запуска игр: {State.e.connectedGameRunners.Count}";
            }
        }
    }


}
