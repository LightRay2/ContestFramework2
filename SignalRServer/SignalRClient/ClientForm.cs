using Microsoft.AspNet.SignalR.Client;
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

namespace SignalRClient
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {

        }
        ServerCall call;
        

        private void button3_Click(object sender, EventArgs e)
        {
            //authorize as player
            call = new ServerCall(this, "http://localhost:8080", edtNameAndPassword.Text.Split()[0], edtNameAndPassword.Text.Split()[1], (x) => label1.Text = x + "\n" + label1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //authorize as help server
            call.CallVoid("ConnectAsGameRunner", "start connect", "finish connect", "qazASD890");
        }


        #region useless

        //misc
        private void ClientForm_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void edtSendREsultOfCurrentGame_TextChanged(object sender, EventArgs e)
        {

        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            int PART_SIZE = 32000;
            //send exe
            var path = @"C:\Users\L\Documents\_Projects\SignalRServer\SignalRClient\bin\Debug\SignalRClient.exe";

            if (File.Exists(path))
            {
                int currentFilePart = 0;
                var allBytes = File.ReadAllBytes(path);
                int partCount = (int)Math.Ceiling((double)allBytes.Length / PART_SIZE- 0.000000000001);
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
                //now parallel for?
                for (int i = 0; i < partCount;i++)
                {
                    bool success = call.CallVoid("LoadFilePart", $"start upload {i+1} of {partCount}", "finish upload", fileId, i, currentFile[i]);
                    if (!success)
                        break;
                }

            
            }
            else
                MessageBox.Show("Файл не найден");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //get player rank
            var allGameResult = call.Call< List<Tuple<Guid, List<Tuple<string, string>>>>>("GetAllGameResults", "start get all game results", "finish get all game results", null);
            if (allGameResult != null)
            {
                var scores = allGameResult.SelectMany(x => x.Item2).GroupBy(x => x.Item1)
                  .Select(group => Tuple.Create(group.Key, group.Count(), group.Aggregate(0, (cur, next) => cur += int.Parse(next.Item2))))
                  .OrderByDescending(x => x.Item3).ToList();
                labelPlayerRank.Text = string.Join(Environment.NewLine, scores);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //all games
            var allGameResult = call.Call< List<Tuple<Guid, List<Tuple<string, string>>>>>("GetAllGameResults", "start get all game results", "finish get all game results", null);
            if (allGameResult != null)
            {
                labelGetListOfGames.Text = string.Join(Environment.NewLine, allGameResult
                    .Select(game => $"{game.Item1}: {string.Join(" ", game.Item2.Select(player => player.Item1 + ":" + player.Item2))}"));
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //play game
            var guid = Guid.Parse(edtGameGuid.Text);
            var game = call.Call<string>("GetGame", "start get game", "finish get game", null, guid);
            if(game != null)
            {
                labelPlayLastGame.Text = game;
            }
        }

        //authorize as player:
        //send exe
        //get rank of all players on server
        //get list of acailable games for watch
        //watch particular game


        //authorize as help server
        //get players and start settings
        //send game results

    }
}
