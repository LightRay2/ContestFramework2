using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignalRServer
{
    public class MainHub : Hub
    {
        DB db = State.e.CreateDb();
        const string GAME_RUNNER_PASSWORD = "qazASD890";
        public static object LOCKER = new object();

        public string ConnectAsParticipant(string name, string password)
        {
            lock (LOCKER)
            {
                if( password == GAME_RUNNER_PASSWORD)
                {
                    State.e.connectedGameRunners.Add(Context.ConnectionId);
                    return null;
                }
                var player = db.Player.FirstOrDefault(x => x.Name == name && x.Password == password);
                if (player == null)
                    return "wrong name or password";
                State.e.connectedParticipants[Context.ConnectionId] = player.Id;
                return null;
            }
        }

        public Tuple< List<Tuple<Guid,DateTime, List<Tuple<string, string>>>>,string> GetAllGameResults()
        {
            lock(LOCKER)
            {
                return Tuple.Create( db.Game.ToList().Select(x => Tuple.Create(x.Id, x.DateTime, x.GamePlayer.Select(y => Tuple.Create(y.Player.Name, y.Result)).ToList())).ToList(), "");

            }
        }

        public Tuple<Guid,string> GetCurrentState()
        {
            lock (LOCKER)
            {
                return Tuple.Create(State.e.CurrentState, "");
            }
        }
        public Tuple<string,string> GetGame(Guid guid)
        {
            lock (LOCKER)
            {
                var game = db.Game.Find(guid);
                if (game == null)
                    return Tuple.Create("", "game not found");
                else
                    return Tuple.Create(game.Content, "");
            }
        }


        public Tuple<Guid, string> GameRunnerGetGameId()
        {
            lock (LOCKER)
            {
                if (State.e.connectedGameRunners.Contains(Context.ConnectionId) && db.Player.Count(x => x.Solution != null) >= 2) //todo 2?
                {
                    var guid = Guid.NewGuid();
                    var players = db.Player.Where(p=>p.Solution != null).ToList();
                    var one = players[ new Random().Next(players.Count)];
                    players.Remove(one);
                    var two = players[new Random().Next(players.Count)];
                    State.e.RunningGames.TryAdd(guid, new List<Guid> { one.Id, two.Id });
                    return Tuple.Create(guid, "");
                }
                return Tuple.Create(Guid.Empty,"game runner not found");
            }
        }

        public Tuple<int, string> GameRunnerGetPlayerCount(Guid gameId)
        {
            lock (LOCKER)
            {
                return Tuple.Create(State.e.RunningGames[gameId].Count, "");
            }
        }

        public Tuple<Tuple<string, byte[]>, string> GameRunnerGetPlayer(Guid gameId, int playerNumber)
        {
            lock (LOCKER)
            {
                var list = State.e.RunningGames[gameId];
                var player = db.Player.Find(list[playerNumber]);
                return Tuple.Create(Tuple.Create( player.Name + player.SolutionExtension, player.Solution),"");  
            }
        }

        public string GameRunnerGamePlayed(Guid gameId , List<string> gameResult, string xmlData)
        {
            lock (LOCKER)
            {
                var players =  State.e.RunningGames[gameId].Select(id=>db.Player.Find(id)).ToList();
                var game = new Game { DateTime = DateTime.Now, Content = xmlData };
                for(int i = 0; i < players.Count; i++)
                {
                    players[i].GamePlayer.Add(new GamePlayer { Game = game, Result = gameResult[i] });
                }
                State.e.CurrentState = Guid.NewGuid();
                db.SaveChanges();
                return "";
            }
        }

        public string SubmitSolution(string solutionExtension, byte[] solution)
        {
            lock (LOCKER)
            {
                var player = db.Player.Find(State.e.connectedParticipants[Context.ConnectionId]);
                if (player == null)
                    return "not authorized";
                player.Solution = solution;
                player.SolutionExtension = solutionExtension;
                player.SolutionSubmitDateTime = DateTime.Now;
                db.SaveChanges();
                return "";
            }
        }
        
    }
}
