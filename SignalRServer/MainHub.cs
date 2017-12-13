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

        public static object LOCKER = new object();
        //data
        //player unique name,password, player exe or java or py (or null)
        //results of matches -time, names, scores as string(default parsing to int), json data for replay if bool
        //on server:
        public string ConnectAsParticipant(string name, string password)
        {
            lock (LOCKER)
            {
                if( password == ServerState.GameRunnerPassword)
                {
                    Manager.e.connectedGameRunners.Add(Context.ConnectionId);
                    return null;
                }
                var player = ServerState.e.Players.FirstOrDefault(x => x.uniqueName == name && x.password == password);
                if (player == null)
                    return "wrong name or password";
                Manager.e.connectedParticipants[Context.ConnectionId] = player;
                return null;
            }
        }
        public void GamePlayed(string jsonInfo)
        {
            lock (LOCKER)
            {

            }
        }

        public Tuple< List<Tuple<Guid,DateTime, List<Tuple<string, string>>>>,string> GetAllGameResults()
        {
            lock(LOCKER)
            {
                return Tuple.Create( ServerState.e.Games.Select(x => Tuple.Create(x.id, x.dateTime, x.playersAndScores.Select(y => Tuple.Create(y.Item1.uniqueName, y.Item2)).ToList())).ToList(), "");

            }
        }

        public Tuple<Guid,string> GetCurrentState()
        {
            lock (LOCKER)
            {
                return Tuple.Create(ServerState.e.CurrentState, "");
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
                if (Manager.e.connectedGameRunners.Contains(Context.ConnectionId) && ServerState.e.Players.Count(x => x.lastProgramRelativePathOrNull != null) >= 2) //todo 2?
                {
                    var guid = Guid.NewGuid();
                    var players = ServerState.e.Players.Where(p=>p.lastProgramRelativePathOrNull!=null).ToList();
                    var one = players[ new Random().Next(players.Count)];
                    players.Remove(one);
                    var two = players[new Random().Next(players.Count)];
                    Manager.e.RunningGames.TryAdd(guid, new List<ServerPlayer> { one, two });
                    return Tuple.Create(guid, "");
                }
                return Tuple.Create(Guid.Empty,"game runner not found");
            }
        }

        public Tuple<int, string> GameRunnerGetPlayerCount(Guid gameId)
        {
            lock (LOCKER)
            {
                return Tuple.Create(Manager.e.RunningGames[gameId].Count, "");
            }
        }

        public Tuple<Tuple<string, byte[]>, string> GameRunnerGetPlayer(Guid gameId, int playerNumber)
        {
            lock (LOCKER)
            {
                var list = Manager.e.RunningGames[gameId];
                return Tuple.Create(Tuple.Create( list[playerNumber].uniqueName + Path.GetExtension( list[playerNumber].lastProgramRelativePathOrNull), File.ReadAllBytes(list[playerNumber].AbsolutePath)),"");  
            }
        }

        public string GameRunnerGamePlayed(Guid gameId , List<string> gameResult, string xmlData)
        {
            lock (LOCKER)
            {
                var players = Manager.e.RunningGames[gameId];
                var playersAndScores = new List<Tuple<ServerPlayer, string>>();
                for(int i = 0; i < players.Count; i++)
                {
                    playersAndScores.Add(Tuple.Create(players[i], gameResult[i]));
                }
                var game = new ServerGame(playersAndScores, xmlData);
                ServerState.e.Games.Add(game);
                ServerState.e.CurrentState = Guid.NewGuid();
                return "";
            }
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
