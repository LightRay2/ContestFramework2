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
            _timer.Tick += _timer_Tick;
            _timer.Start();
            _timer_Tick(null,null);
            //using (WebApp.Start(url))
            //{
            //   this.Text = $"Server running on {url}";
            //}
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            lock(MainHub.LOCKER)
            {
                {
                    var sb = new StringBuilder();
                    var users = ServerState.e.Players.Where(x => x.lastProgramRelativePathOrNull != null).OrderByDescending(x=>x.programSent).ToList();
                    sb.AppendLine($"Пользователи, отправившие решение ({users.Count}):");

                    for (int i = 0; i < users.Count; i++)
                    {
                        var time = users[i].programSent.ToString("HH:mm");
                        sb.AppendLine($"{time}. {users[i].uniqueName} ({Path.GetExtension(users[i].lastProgramRelativePathOrNull)})");
                    }
                    labelPlayersSentExe.Text = sb.ToString();
                }
                {
                    var sb = new StringBuilder();
                    var users = Manager.e.connectedParticipants.Select(x=>x.Value).ToList();
                    sb.AppendLine($"Пользователи online ({users.Count}):");
                    for (int i = 0; i < users.Count; i++)
                    {

                        sb.AppendLine($"{i}. {users[i].uniqueName}");
                    }
                    label1.Text = sb.ToString();
                }

                labelCountOfGameRunners.Text = $"Количество серверов для запуска игр: {Manager.e.connectedGameRunners.Count}";
            }
        }
    }


}
