using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServer
{
    public class MainHub : Hub
    {
        ServerPlayer Player { get { return Manager.e.connectedParticipants.TryGetValue(Context.ConnectionId, out ServerPlayer res) ? res : (ServerPlayer)null; } }

        static object LOCKER = new object();
        //data
        //player unique name,password, player exe or java or py (or null)
        //results of matches -time, names, scores as string(default parsing to int), json data for replay if bool
        //on server:
        public string ConnectAsParticipant(string name, string password)
        {
            lock (LOCKER)
            {
                var player = ServerState.e.Players.FirstOrDefault(x => x.uniqueName == name && x.password == password);
                if (player == null)
                    return "wrong name or password";
                Manager.e.connectedParticipants[Context.ConnectionId] = player;
                return null;
            }
        }
        public string ConnectAsGameRunner(string password)
        {
            //now should be united with participant
            lock (LOCKER)
            {
                if(password != "qazASD890")
                {
                    return "wrong password";
                }
                Manager.e.connectedGameRunners.Add(Context.ConnectionId);
                return "";
                    
            }
        }
        public void GamePlayed(string jsonInfo)
        {
            lock (LOCKER)
            {

            }
        }

        public Tuple< List<Tuple<Guid,List<Tuple<string, string>>>>,string> GetAllGameResults()
        {
            lock(LOCKER)
            {
                return Tuple.Create( ServerState.e.Games.Select(x => Tuple.Create(x.id, x.playersAndScores.Select(y => Tuple.Create(y.Item1.uniqueName, y.Item2)).ToList())).ToList(), "");

            }
        }

        public Tuple<string,string> GetGame(Guid guid)
        {
            lock (LOCKER)
            {
                var game = ServerState.e.Games.FirstOrDefault(g => g.id == guid);
                if (game == null)
                    return Tuple.Create("", "game not found");
                else
                    return Tuple.Create(game.JsonData, "");
            }
        }


        public Tuple<Guid, string> GameRunnerGetGameId()
        {
            lock (LOCKER)
            {
                if (ServerState.e.Players.Count(x => x.lastProgramRelativePathOrNull != null) >= 2) //todo 2?
                {

                }
            }
            return null;
        }

        public Tuple<int, string> GameRunnerGetPlayerCount(Guid gameId)
        {
            return null;
        }

        public Tuple<string, byte[], string> GameRunnerGetPlayer(Guid gameId, int playerNumber)
        {
            return null;
        }

        public string GameRunnerGamePlayed(Guid gameId , List<string> gameResult, byte[] xmlData)
        {
            return null;
            //todo all cliens  - Refresh 
            //AND in connect as participant
        }

        #region file upload
        public Tuple<Guid, string> StartUploadingAndGetId(string fileName, int partCount)
        {
            if (Player == null)
                return Tuple.Create(default(Guid), "not authorized");

            var uploadingFileInfo = new UploadingFileInfo(partCount) { fileName = fileName };
            var guid = Guid.NewGuid();
            Manager.e.uploadingFiles.TryAdd(guid, uploadingFileInfo);
            return Tuple.Create(guid, "");
        }
        static object _locker = new object();
        public string LoadFilePart(Guid id, int partNumber, byte[] filePart)
        {
            if (Player == null)
                return "not authorized";

            lock (_locker)
            {
                if (Manager.e.uploadingFiles.ContainsKey(id))
                {
                    var file = Manager.e.uploadingFiles[id];

                    bool finished = file.AddPartAndCheckFinish(filePart, partNumber);

                    if (finished)
                    {
                        string relative = $"{id}{file.fileName}";
                        string filePhysicalName = ServerState.FolderExecutablesWithSlash + relative;
                        File.WriteAllBytes(filePhysicalName, file.bytes.SelectMany(x => x).ToArray());
                        Player.programSent = DateTime.Now;
                        Player.lastProgramRelativePathOrNull = relative;
                        ServerState.e.SaveChanges();//now cross thread bad
                    }
                    return null;
                }
                else
                {
                    return "error uploading file. try again";
                }
            }

        }

        #endregion
    }
}
