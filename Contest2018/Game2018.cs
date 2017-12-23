using Framework;
using OpenTK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;



//todo сделать что то с первым ходом!
namespace Contest2018
{
    #region small classes
    public class Round : IRound<Turn, Player>
    {
        public List<Turn> turns { get; set; }
        public Random random { get; set; }
        public double totalStage { get; set; }
        public string nameForTimeLine { get; set; }

        //здесь переменные, описывающие конкретный раунд
        //public char[,] ch = new char[3, 3];
    }

    public class Turn : ITurn<Player>, ITimelineCell
    {
        public string input { get; set; }
        public string output { get; set; }
        public Player player { get; set; }
        public string shortStatus { get; set; }

        public Color colorOnTimeLine { get; set; }
        public Color colorStatusOnTimeLine { get; set; }
        public Enum fontOnTimeLine { get; set; }
        public string nameOnTimeLine { get; set; }

        public int action;//действие игрока
        public int pos;//его предыдущая позиция
        public int delta;
        public string memory = null;
        public Turn()
        {
            fontOnTimeLine = Game2018.EFont.timelineNormal;
        }
    }
    public class GameParams
    {

    }
    public class Player : IPlayer
    {
        public string programAddress { get; set; }
        public bool controlledByHuman { get; set; }
        public string name { get; set; }
        public int team;

        public int hptower = 20000;
        public int gold = 1000;
        public string memory = null;
        public int pos = 0;
    }
    public enum TypeofObject
    {
        farm=1,
        observationtower = 2,
        catapult =3,
        cannon=4,
        ballista=5,
    }

    public class ObjectGame
    {
        public int hp = 0;
        public TypeofObject obj;
        public int distance;
        public int pos;
        public int damage;
        public bool isnew;//для отрисовки
        public ObjectGame( TypeofObject obj, int hp, int distance,int damage,int pos)
        {
            this.hp = hp;
            this.obj = obj;
            this.distance = distance;
            this.pos = pos;
            this.damage = damage;
            isnew = true;
        }
    }
    #endregion

    /// <summary>
    /// Везде point.X соответствует номеру строки
    /// </summary>
    public class Game2018 : IGame<FormState, Turn, Round, Player>
    {
        public int roundNumber { get; set; }
        public int frameNumber { get; set; }
        public List<Player> players { get; set; }
        public List<Round> rounds { get; set; }
        public bool GameFinished { get; set; }
        public int clickedRound { get; set; }

        public int FollowingPlayer = 1;
        public List<ObjectGame> gameobjects;
        public List<ObjectGame> deadGameObjects = new List<ObjectGame>();
        public string st;
        public enum EFont
        {
            timelineNormal,
            timelineError,
            main,
            player0,
            player1
        }
        enum ESprite {
            tower,
            cannonl,
            cannonr,
            ballistal,
            ballistar,
            catapultl,
            catapultr,
            farm,
            observationtower,
            playerr,
            playerl,
            explosion,
        }



        Random _rand;


        GamePurpose _gameInstancePurpose;
        public Game2018(FormState settings, GamePurpose purpose)
        {
            #region constructor
            _rand = new Random(settings.RandomSeed);
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            _gameInstancePurpose = purpose;
            if (_gameInstancePurpose == GamePurpose.LoadSpritesAndFonts)
                return;
            //----
            gameobjects = new List<ObjectGame>();
            //----


            #region create players
            players = settings.ProgramAddressesInMatch
                    .Select(index => settings.ProgramAddressesAll[index])
                    .Select(address => new Player
                    {
                        name = address == null ? "Человек" : Path.GetFileNameWithoutExtension(address),
                        controlledByHuman = address == null,
                        programAddress = address
                    }).ToList();

            for (int i = 0; i < 2; i++)
            {
                players[i].team = i;
            }
            players[0].pos = 0;
            players[1].pos = 17;

            #endregion


            #endregion
        }


        public static void SetFrameworkSettings()
        {
            FrameworkSettings.GameNameEnglish = "SimpleGame";
            FrameworkSettings.RunGameImmediately = false;
            FrameworkSettings.AllowFastGameInBackgroundThread = true;
            FrameworkSettings.FramesPerTurn = 20;

            FrameworkSettings.PlayersPerGameMin = 2;
            FrameworkSettings.PlayersPerGameMax = 2;
            FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create("..//Players//Easy.exe", true));
            FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create("..//Players//Normal.exe", true));
            //  FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create(Path.GetDirectoryName(Application.StartupPath) + "//..//Players//Hard.exe", false));

            FrameworkSettings.Timeline.Enabled = true;
            FrameworkSettings.Timeline.Position = TimelinePositions.right;
            FrameworkSettings.Timeline.TileLength = 50;
            FrameworkSettings.Timeline.TileWidth = 50;
            FrameworkSettings.Timeline.TurnScrollSpeedByMouseOrArrow = 8;
            FrameworkSettings.Timeline.TurnScrollSpeedByPageUpDown = 100;
            FrameworkSettings.Timeline.FollowAnimationTimeMs = 600;
        }

        public void LoadSpritesAndFonts()
        {
            if (FontList.All.Count == 0 && SpriteList.All.Count == 0)
            {
                FontList.Load(EFont.timelineNormal, "Times New Roman", 20.0, Color.FromArgb(193, 180, 255), FontStyle.Bold);
                FontList.Load(EFont.timelineError, "Times New Roman", 20.0, Color.Red, FontStyle.Bold);

                FontList.Load(EFont.main, "Times New Roman", 30.0, Color.Black, FontStyle.Bold);

                FontList.Load(EFont.player0, "Times New Roman", 15.0, Color.FromArgb(230, 70, 70), FontStyle.Bold);
                FontList.Load(EFont.player1, "Times New Roman", 3.0, Color.FromArgb(70, 230, 70), FontStyle.Bold);

                SpriteList.Load(ESprite.tower, -90);
                SpriteList.Load(ESprite.ballistal, -90);
                SpriteList.Load(ESprite.ballistar, -90);
                SpriteList.Load(ESprite.cannonl, -90);
                SpriteList.Load(ESprite.cannonr, -90);
                SpriteList.Load(ESprite.catapultl, -90);
                SpriteList.Load(ESprite.catapultr, -90);
                SpriteList.Load(ESprite.observationtower, -90);
                SpriteList.Load(ESprite.farm, -90);
                SpriteList.Load(ESprite.explosion, defaultSizeExact: new Vector2d(50), defaultDepth: 10000, frameCountHorizontal: 6, frameCountVertical: 5);
            }
        }


        public List<string> GetCurrentSituation()
        {
            return null;

        }


        Rect2d _arena = new Rect2d(0, 0, 100, 100);
        int _turnLimit = 120;
        public string GetInputFile(Player player)
        {
            var st = new StringBuilder();
            st.AppendLine(roundNumber.ToString());
            if (player.team == 0)
            {
                st.AppendLine(player.hptower.ToString() + " " + player.gold.ToString() + " " + player.pos.ToString());
                st.AppendLine(players[(player.team + 1) % 2].hptower.ToString() + " " + players[(player.team + 1) % 2].gold.ToString() + " " + players[(player.team + 1) % 2].pos.ToString());
            }
            else
            {
                st.AppendLine(player.hptower.ToString() + " " + player.gold.ToString() + " " + (17 - player.pos).ToString());
                st.AppendLine(players[(player.team + 1) % 2].hptower.ToString() + " " + players[(player.team + 1) % 2].gold.ToString() + " " + (17 - players[(player.team + 1) % 2].pos).ToString());
            }
            //st.AppendLine(gameobjects.Count.ToString());
            int[] arrt = new int[18];
            int[] arrhp = new int[18];
            for (int i = 0; i < gameobjects.Count; i++)
            {
                arrt[gameobjects[i].pos] = (int)gameobjects[i].obj;
                arrhp[gameobjects[i].pos] = (int)gameobjects[i].hp;
                /*if(player.team==0)
                {
                    st.AppendLine(((int)gameobjects[i].obj).ToString() + " " + gameobjects[i].hp.ToString() + " " + gameobjects[i].pos.ToString());
                }
                else
                {
                    if(gameobjects[i].pos<9)
                    st.AppendLine(((int)gameobjects[i].obj).ToString() + " " + gameobjects[i].hp.ToString() + " " + (17-gameobjects[i].pos).ToString() );
                    else
                    st.AppendLine(((int)gameobjects[i].obj).ToString() + " " + gameobjects[i].hp.ToString() + " " + (17-gameobjects[i].pos).ToString());
                }*/
            }
            if (player.team == 0)
            {
                for (int i = 0; i < 18; i++)
                {
                    st.Append(arrt[i].ToString() + " ");
                }
                st.AppendLine("");
                for (int i = 0; i < 18; i++)
                {
                    st.Append(arrhp[i].ToString() + " ");
                }
                st.AppendLine("");
            }
            else
            {
                for (int i = 17; i >= 0; i--)
                {
                    st.Append(arrt[i].ToString() + " ");
                }
                st.AppendLine("");
                for (int i = 17; i >= 0; i--)
                {
                    st.Append(arrhp[i].ToString() + " ");
                }
                st.AppendLine("");
            }
            st.AppendLine(player.memory??"-1");
            this.st = st.ToString();
            return st.ToString(); ;
        }

        public Turn GetProgramTurn(Player player, string output, ExecuteResult executionResult, string executionResultRussianComment)
        {
            var turn = new Turn
            {
                shortStatus = executionResultRussianComment,
                output = output,
                colorOnTimeLine = player.team == 0 ? Color.DarkRed : Color.DarkGreen,
                nameOnTimeLine = roundNumber.ToString(),
                colorStatusOnTimeLine = Color.Gold,
                input = roundNumber.ToString(),
            }; //todo now in interface just edit turn, no return


            turn.player = new Player(); //todo нужно в будущем победить этот страх (видимо, во втором потоке и в первом Player разный, поэтому можно только номер команды использовать, а не прямую ссылку)
            //turn.player = player;
            turn.player.name = player.name;
            turn.player.pos = player.pos;
            turn.player.hptower = player.hptower;
            turn.player.memory = player.memory;
            turn.player.team = player.team;
            turn.player.controlledByHuman = player.controlledByHuman;
            turn.player.gold = player.gold;
            turn.player.programAddress = player.programAddress;
            if (executionResult == ExecuteResult.Ok)
            {
                var reader = new StringReader(output);
                var s = reader.ReadLine().Split(' ');
                try
                {

                    char ch = s[0][0];
                    int action = Convert.ToInt32(s[1]);
                    turn.pos = player.pos;
                    if ((ch != 'R' && ch != 'L' && ch != 'S') || (action < 0 || action > 6))
                    {
                        throw new Exception();
                    }
                    else
                    {

                        if (player.pos == 0 && ch == 'L')
                        {
                            throw new Exception();
                        }
                        if (player.pos == 8 && ch == 'R')
                        {
                            throw new Exception();
                        }
                        if (player.pos == 17 && ch == 'L')
                        {
                            throw new Exception();
                        }
                        if (player.pos == 9 && ch == 'R')
                        {
                            throw new Exception();
                        }
                        turn.delta = 0;
                        if (ch == 'R')
                        {
                            if (player.team == 1)
                            {
                                turn.delta = -1;
                            }
                            else
                                turn.delta = 1;
                        }
                        if (ch == 'L')
                        {
                            if (player.team == 1)
                            {
                                turn.delta = 1;
                            }
                            else
                                turn.delta = -1;
                        }
                        turn.player.pos = turn.player.pos + turn.delta;
                        turn.action = action;
                    }
                }
                catch
                {
                    turn.fontOnTimeLine = EFont.timelineError;
                    turn.shortStatus = "Неправильный формат вывода";
                    return turn;
                }

                try
                {
                    var nextString = reader.ReadLine()??"";
                    if (nextString.StartsWith("memory "))
                    {
                        turn.memory = nextString.Substring(7);
                        turn.shortStatus += ". Использовано запоминание";
                    }
                    return turn;
                }
                catch
                {
                    return turn;
                }
            }
            else
            {
                turn.fontOnTimeLine = EFont.timelineError;
            }
            return turn;
        }

        void CheckDoubleWithException(double x)
        {
            if (double.IsInfinity(x) || double.IsNaN(x))
                throw new Exception();
        }

        public List<Player> GetTurnOrderForNextRound()
        {

            /*List<Player> l = new List<Player>();
            l.Add(players[FollowingPlayer]);
            return l;*/
            return players;
        }


        public void PreparationsBeforeRound()
        {
            //FollowingPlayer = (FollowingPlayer + 1) % 2;
        }

        Animator<int> animobject;
        List<Animator<Vector2d>> animshells;
        Animator<int> animplayer0;
        Animator<int> animplayer1;
        ObjectGame obj;
        public static int Linear(int start, int finish, double stage)
        {
            var dif = finish - start;
            if (dif == 0)
            {
                return start;
            }
            return start + (int)(dif * stage);
        }
        public static Vector2d Parabol(Vector2d vecstart, Vector2d vecfinish, double stage)//ax^2+bx+c=y
        {
            double start = vecstart.X;
            double finish = vecfinish.X;
            double y = 332 - Math.Abs(finish - start) * 20 / 65;
            double x = (finish + start) / 2;
            double a = -(332 - y) / ((finish - x) * (start - x));
            double b = (332 - y) / (finish - x) - (finish + x) * a;
            double c = 332 - a * start * start - b * start;
            double newx = start + (finish - start) * stage;
            double newy = a * newx * newx + b * newx + c;
            return new Vector2d(newx, newy);
        }
        public void ProcessRoundAndSetTotalStage(Round round)
        {
            round.turns.ForEach(x => players[ x.player.team].memory = x.memory);

            // frame.Circle(Color.Green, 200 + 100 * round.turns[0].x + 50, 200 + 100 * round.turns[0].y + 50, 45);
            // anim = new Animator<Vector2d>(Animator.Linear,new Vector2d( 200 + 100 * round.turns[0].x + 50, 200 + 100 * round.turns[0].y + 50), new Vector2d(200 + 100 * round.turns[0].x + 50, 200 + 100 * round.turns[0].y + 50), 1);
            // animdb = new Animator<int>(Linear, 0, 255, 1);
            round.totalStage = 0;
            if (round.turns[0].delta != 0 || round.turns[1].delta != 0)
               round.totalStage = 1;
            if ((round.turns[0].action != 0 && round.turns[0].action != 6) || (round.turns[1].action != 0 && round.turns[1].action != 6))
                round.totalStage += 0.5;
            if ((round.turns[0].action == 6 || round.turns[1].action == 6))
                round.totalStage += 1.3;
            animshells = new List<Animator<Vector2d>>();
            for(int i=0;i<gameobjects.Count;i++)
            {
                gameobjects[i].isnew = false;
            }
            for (int k = 0; k < 2; k++)
            {
                int currentplayer = round.turns[k].player.team;
                players[currentplayer].pos = round.turns[k].player.pos;
                if (currentplayer == 0)
                {
                    animplayer0 = new Animator<int>(Linear, 65 * round.turns[k].pos, 65 * players[0].pos, 1);
                    // animplayer1 = new Animator<int>(Linear, 65*players[1].pos, 65 * players[1].pos, 0);
                }
                else
                {
                    //  animplayer0 = new Animator<int>(Linear, 65 * players[0].pos, 65 * players[0].pos, 0);
                    animplayer1 = new Animator<int>(Linear, 65 * round.turns[k].pos, 65 * players[1].pos, 1);
                }

                if (round.turns[k].action != 0 && round.turns[k].action != 6)
                {
                    gameobjects.RemoveAll((x) => x.pos == round.turns[k].player.pos);
                    animobject = new Animator<int>(Linear, 0, 255, 0.5);
                }
                switch (round.turns[k].action)
                {
                    case 1:
                        {
                            if (players[currentplayer].gold >= 600 + roundNumber)
                            {
                                players[currentplayer].gold -= 600 + roundNumber;
                                gameobjects.Add(new ObjectGame(TypeofObject.farm, 150, 0, 0, round.turns[k].player.pos));
                            }
                            break;
                        }
                    case 2:
                        {
                            if (players[currentplayer].gold >= 200)
                            {
                                players[currentplayer].gold -= 200;
                                gameobjects.Add(new ObjectGame(TypeofObject.observationtower, 200, 0, 0, round.turns[k].player.pos));
                            }
                            break;
                        }
                    case 3:
                        {
                            if (players[currentplayer].gold >= 1000 - 3 * roundNumber)
                            {
                                players[currentplayer].gold -= 1000 - 3 * roundNumber;
                                gameobjects.Add(new ObjectGame(TypeofObject.catapult, 100, 8, 50, round.turns[k].player.pos));
                            }
                            break;
                        }
                    case 4:
                        {
                            if (players[currentplayer].gold >= 1800 - 5 * roundNumber)
                            {
                                players[currentplayer].gold -= 1800 - 5 * roundNumber;
                                gameobjects.Add(new ObjectGame(TypeofObject.cannon, 175, 10, 75, round.turns[k].player.pos));
                            }
                            break;
                        }
                    case 5:
                        {
                            if (players[currentplayer].gold >= 1600 - 7 * roundNumber)
                            {
                                players[currentplayer].gold -= 1600 - 7 * roundNumber;
                                gameobjects.Add(new ObjectGame(TypeofObject.ballista, 100, 12, 60, round.turns[k].player.pos));
                            }
                            break;
                        }
                    case 6:
                        {
                            if (players[currentplayer].gold >= 200 && gameobjects.Find(x => x.obj == TypeofObject.observationtower && players[currentplayer].pos == x.pos) != null)
                            {
                                players[currentplayer].gold -= 200;
                                for (int i = 0; i < gameobjects.Count; i++)
                                {
                                    double Purpose = -100;
                                    int r = _rand.Next(-1, 3);
                                    if (round.turns[k].player.team == 0 && gameobjects[i].pos < 9 && (int)gameobjects[i].obj > 2)
                                    {
                                        Purpose = Math.Min(18, (gameobjects[i].pos + gameobjects[i].distance + r % 2));
                                        double start = /*gameobjects[i].pos + */140 + 65 * gameobjects[i].pos;
                                        var finish = 140 + 65 * Purpose;
                                        animshells.Add(new Animator<Vector2d>(Parabol, new Vector2d(start, 332), new Vector2d(finish, 332), 1));
                                        //animshells.Add(new Animator<Vector2d>(Parabol, new Vector2d(start, 332), new Vector2d((start + 65 * gameobjects[i].distance < 1270 ? start + 65 * gameobjects[i].distance : 1270) + r % 2 * 65, 332), 1));
                                    }
                                    else
                                    {
                                        if (round.turns[k].player.team == 1 && gameobjects[i].pos > 8 && (int)gameobjects[i].obj > 2)
                                        {
                                            Purpose = Math.Max(-1, (gameobjects[i].pos - gameobjects[i].distance - r % 2));
                                            double start = /*gameobjects[i].pos + */140 + 65 * gameobjects[i].pos;
                                            var finish = 140 + 65 * Purpose;
                                            animshells.Add(new Animator<Vector2d>(Parabol, new Vector2d(start, 332), new Vector2d(finish, 332), 1));

                                            //animshells.Add(new Animator<Vector2d>(Parabol, new Vector2d(start, 332), new Vector2d((start - 65 * gameobjects[i].distance > 100 ? start - 65 * gameobjects[i].distance : 100) + r % 2 * 65, 332), 1));
                                        }
                                    }
                                    if (Purpose != -100)
                                    {
                                        if (Purpose < 0)
                                        {
                                            players[0].hptower -= gameobjects[i].damage;
                                        }
                                        else
                                        {
                                            if (Purpose > 17)
                                            {
                                                players[1].hptower -= gameobjects[i].damage;
                                            }
                                            else
                                            {
                                                for (int j = 0; j < gameobjects.Count; j++)
                                                {
                                                    if (Purpose == gameobjects[j].pos)
                                                    {
                                                        gameobjects[j].hp -= gameobjects[i].damage;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                round.turns[k].action = 0;
                            }
                            break;
                        }
                }
                if (round.turns[k].action != 6)
                {
                    for (int i = 0; i < gameobjects.Count; i++)
                    {
                        if (gameobjects[i].obj == TypeofObject.farm && Math.Abs(17 * currentplayer - gameobjects[i].pos) < 9)
                        {
                            players[currentplayer].gold += 50;
                        }
                    }
                    players[currentplayer].gold += 50;
                }
            }
            deadGameObjects= gameobjects.Where(x => x.hp <= 0).ToList();
            gameobjects.RemoveAll(x => x.hp <= 0);
            if (roundNumber == _turnLimit || players[0].hptower * players[1].hptower <= 0)
            {
                GameFinished = true;
            }

        }

        public void DrawAll(Frame frame, double stage, double totalStage, bool humanMove, GlInput input) //todo human move??
        {
            //!!! будьте внимательны (ранний drawall перед любыми методами)
            // int frameWidth = 160, frameHeight = 120;
            frame.CameraViewport(1400, 1050);

            frame.PolygonWithDepth(Color.White, -100, new Rect2d(0, 0, 1400, 1050)); //todo line around polygon
                                                                                     //frame.SpriteCorner(ESprite.brownGrunge, 0, -100, sizeOnlyHeight: frameHeight + 100);

            //todo nikita если щелкаем на первый квадратик, никакого process еще не произошло, а отрисовывать что то нужно. я пока написал, чтобы не вылетало, но это не выход:
            if (animshells == null)
                return;

            //  frame.SpriteCorner(ESprite.back2, 0, 0, sizeOnlyWidth: frameWidth);

            //var fieldCorner = new Vector2d(10, 10);
            //var lineWidth = 0.4;

            //frame.Path(Color.Black, lineWidth, _arena + fieldCorner);

            //frame.SpriteCorner(ESprite.field, fieldCorner, sizeExact: _arena.size, opacity: 0.4);

            //frame.Polygon(Color.FromArgb(150, 0, 0, 0), new Rect2d(110, 0, 1000, 1000));
            //frame.Polygon(Color.FromArgb(150, 0, 0, 0), new Rect2d(0, 0, 110, 10));
            //frame.Polygon(Color.FromArgb(150, 0, 0, 0), new Rect2d(0, 10, 10, 100));
            // frame.Polygon(Color.FromArgb(150, 0, 0, 0), new Rect2d(0, 110, 110, 1000));

            // frame.Path(Color.Black, 2, _arena + fieldCorner);
            //  frame.Path(Color.Black, lineWidth, fieldCorner + new Vector2d(_arena.size.X / 2, 0), fieldCorner + new Vector2d(_arena.size.X / 2, _arena.size.Y));

            // if (_manAnimators.Count != 0) //т е еще не было process turn
            //  {
            frame.Polygon(Color.Black, new Rect2d(100, 300, 1170, 5));
            frame.Polygon(Color.Black, new Rect2d(100, 365, 1170, 5));
            //frame.Circle(Color.Black, 60, 332, 40);
            //frame.Circle(Color.Black, 1310, 332, 40);
            frame.SpriteCenter(ESprite.tower, 60, 324, angleDeg: 90);
            frame.SpriteCenter(ESprite.tower, 1310, 324, angleDeg: 90);
            for (int i = 0; i < 19; i++)
            {
                frame.Polygon(Color.Black, new Rect2d(100 + 65 * i, 300, 5, 65));
            }
            if (animplayer0 != null)
            {
                frame.SpriteCenter(ESprite.playerr, 132 + animplayer0.Get(stage > 1 ? 1 : stage), 430);
                frame.SpriteCenter(ESprite.playerl, 132 + animplayer1.Get(stage > 1 ? 1 : stage), 430);
            }
            var aliveAndDeadObjects = gameobjects.Union(deadGameObjects).ToList();
            for (int i = 0; i < aliveAndDeadObjects.Count; i++)
            {
                if (!aliveAndDeadObjects[i].isnew)
                    switch ((int)aliveAndDeadObjects[i].obj)
                    {
                        case 1:
                            {
                                frame.SpriteCenter(ESprite.farm, 132 + 65 * aliveAndDeadObjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                break;
                            }
                        case 2:
                            {
                                frame.SpriteCenter(ESprite.observationtower, 132 + 65 * aliveAndDeadObjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                break;
                            }
                        case 3:
                            {
                                if (aliveAndDeadObjects[i].pos < 9)
                                {
                                    frame.SpriteCenter(ESprite.catapultr, 132 + 65 * aliveAndDeadObjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                }
                                else
                                {
                                    frame.SpriteCenter(ESprite.catapultl, 132 + 65 * aliveAndDeadObjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                }
                                break;
                            }
                        case 4:
                            {
                                if (aliveAndDeadObjects[i].pos < 9)
                                {
                                    frame.SpriteCenter(ESprite.cannonr, 132 + 65 * aliveAndDeadObjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                }
                                else
                                {
                                    frame.SpriteCenter(ESprite.cannonl, 132 + 65 * aliveAndDeadObjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                }
                                break;
                            }
                        case 5:
                            {
                                if (aliveAndDeadObjects[i].pos < 9)
                                {
                                    frame.SpriteCenter(ESprite.ballistar, 132 + 65 * aliveAndDeadObjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                }
                                else
                                {
                                    frame.SpriteCenter(ESprite.ballistal, 132 + 65 * aliveAndDeadObjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                }
                                break;
                            }
                    }
            }
            if (stage>1 || totalStage<1)
                for (int i = 0; i < gameobjects.Count; i++)
                    if (gameobjects[i].isnew)
                    {
                        switch ((int)gameobjects[i].obj)
                        {
                            case 1:
                                {
                                    frame.SpriteCenter(ESprite.farm, 132 + 65 * gameobjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                    break;
                                }
                            case 2:
                                {
                                    frame.SpriteCenter(ESprite.observationtower, 132 + 65 * gameobjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                    break;
                                }
                            case 3:
                                {
                                    if (gameobjects[i].pos < 9)
                                    {
                                        frame.SpriteCenter(ESprite.catapultr, 132 + 65 * gameobjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                    }
                                    else
                                    {
                                        frame.SpriteCenter(ESprite.catapultl, 132 + 65 * gameobjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    if (gameobjects[i].pos < 9)
                                    {
                                        frame.SpriteCenter(ESprite.cannonr, 132 + 65 * gameobjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                    }
                                    else
                                    {
                                        frame.SpriteCenter(ESprite.cannonl, 132 + 65 * gameobjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                    }
                                    break;
                                }
                            case 5:
                                {
                                    if (gameobjects[i].pos < 9)
                                    {
                                        frame.SpriteCenter(ESprite.ballistar, 132 + 65 * gameobjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                    }
                                    else
                                    {
                                        frame.SpriteCenter(ESprite.ballistal, 132 + 65 * gameobjects[i].pos, 332, angleDeg: 90, sizeOnlyHeight: 65, sizeOnlyWidth: 65);
                                    }
                                    break;
                                }
                        }
                    }
            for (int i = 0; i < animshells.Count; i++)
            {
                if (totalStage - stage < 1.3)
                {
                    if (totalStage - stage > 0.3)
                        frame.Circle(Color.Black, animshells[i].Get(1.3 - (totalStage - stage)), 10);
                    else {
                        int frameNumber = (int)(((0.3 - (totalStage - stage)) * 10) * 10).ToRange(0, 29);
                       // if (frameNumber < 29) {
                            frame.SpriteCenter(ESprite.explosion, animshells[i].Get(stage), sizeExact: new Vector2d(100), frameNumber: frameNumber   );
                      //  }
                    }
                }
            }



            frame.TextCustomAnchor(EFont.main, players[0].gold.ToString(), 0, 0, Vector2d.Zero + new Vector2d(100));
            frame.TextCustomAnchor(EFont.main, players[1].gold.ToString(), 1, 0, Vector2d.UnitX * 1400 + new Vector2d(-100,100));

            var offset = Vector2d.UnitY * 70;
            frame.TextCustomAnchor(EFont.main, players[0].hptower.ToString(), 0, 0, Vector2d.Zero + new Vector2d(100)+ offset);
            frame.TextCustomAnchor(EFont.main, players[1].hptower.ToString(), 1, 0, Vector2d.UnitX * 1400 + new Vector2d(-100, 100)+ offset);
            // frame.TextCenter(EFont.player0, st, 300, 550);//вывод всего кода


            // }

            // frame.TextTopLeft(EFont.player0, players[0].name + ": " + players[0].score.ToString(), 10, 3);
            //frame.TextCustomAnchor(EFont.player1, players[1].name + ": " + players[1].score.ToString(), 1, 0, 110, 3);

            //frame.TextCustomAnchor(EFont.main, roundNumber.ToString(), 0.5, 0, 60, 60);


            #region old
            //  //!!! будьте внимательны (ранний drawall перед любыми методами)
            //  int frameWidth = 160, frameHeight = 120;
            //  frame.CameraViewport(frameWidth, frameHeight);

            //  // frame.Polygon(Color.Wheat, new Rect2d(0, 0, frameWidth, frameHeight)); //todo line around polygon
            //  frame.SpriteCorner(ESprite.brownGrunge, 0, -100, sizeOnlyHeight: frameHeight+100);


            ////  frame.SpriteCorner(ESprite.back2, 0, 0, sizeOnlyWidth: frameWidth);

            //  var fieldCorner = new Vector2d(10, 10);
            //  var lineWidth = 0.4;

            //  frame.Path(Color.Black, lineWidth, _arena + fieldCorner);

            //  frame.SpriteCorner(ESprite.field, fieldCorner, sizeExact: _arena.size, opacity:0.6);

            //  frame.Polygon(Color.FromArgb(180, 0, 0, 0), new Rect2d(110, 0, 1000, 1000));
            //  frame.Polygon(Color.FromArgb(180, 0, 0, 0), new Rect2d(0, 0, 110, 10));
            //  frame.Polygon(Color.FromArgb(180, 0, 0, 0), new Rect2d(0, 10, 10, 100));
            //  frame.Polygon(Color.FromArgb(180, 0, 0, 0), new Rect2d(0, 110, 110, 1000));

            //  frame.Path(Color.Black, 2, _arena + fieldCorner);
            //  //  frame.Path(Color.Black, lineWidth, fieldCorner + new Vector2d(_arena.size.X / 2, 0), fieldCorner + new Vector2d(_arena.size.X / 2, _arena.size.Y));

            //  // if (_manAnimators.Count != 0) //т е еще не было process turn
            //  //  {



            //  for (int i = 0; i < _manList.Count; i++)
            //  {
            //      var man = _manList[i];
            //      var pos = _manAnimators[i].Get(stage);
            //      var direction = _manAnimators[i].Get(stage + 0.001) - _manAnimators[i].Get(stage - 0.001);
            //      var lookAt = pos + direction;
            //      if (i == 0)
            //          frame.SpriteCenter(ESprite.man0, pos + fieldCorner,angleLookToPoint: lookAt, sizeExact: new Vector2d(_manRadius * 2));
            //      else
            //          frame.SpriteCenter(ESprite.man1, pos + fieldCorner, angleLookToPoint: lookAt, sizeExact: new Vector2d(_manRadius*2));
            //     // frame.Circle(man.Color, _manAnimators[i].Get(stage) + fieldCorner, _manRadius);
            //  }

            //  _ballList.ForEach(ball => frame.SpriteCenter(ESprite.mushroom, ball + fieldCorner, sizeExact: new Vector2d(_ballRadius * 2))
            //              /*frame.Circle(Color.Gray, ball + fieldCorner, _ballRadius)*/);

            //  _ballFantom.ForEach(ball =>
            //  {
            //      if (ball.Item2 >= stage)
            //          frame.SpriteCenter(ESprite.mushroom, ball.Item1 + fieldCorner, sizeExact: new Vector2d(_ballRadius * 2));
            //        //  frame.Circle(Color.Gray, ball.Item1 + fieldCorner, _ballRadius);
            //  });


            //  // }

            //  frame.TextTopLeft(EFont.timelineNormal, players[0].name+": "+ players[0].score.ToString(), 3, 3);
            //  frame.TextCustomAnchor(EFont.timelineNormal, players[1].name + ": " + players[1].score.ToString(), 1, 0, 107, 3); 
            #endregion
            /* if (GameFinished==false)
             {
                 if (anim != null && roundNumber < rounds.Count && rounds[roundNumber].turns[0].player.team == 0)
                 {
                     frame.Circle(Color.FromArgb(animdb.Get(stage), Color.Green), anim.Get(stage), 45 * stage);
                 }
                 else
                 {
                     if (roundNumber < rounds.Count && anim != null)
                     {
                         frame.Polygon(Color.FromArgb(animdb.Get(stage), Color.Red), 200 + 50 * (1 - stage) + 100 * rounds[roundNumber].turns[0].x, 200 + 50 * (1 - stage) + 100 * rounds[roundNumber].turns[0].y, 200 + 50 * (1 - stage) + 100 * rounds[roundNumber].turns[0].x + 4, 200 + 50 * (1 - stage) + 100 * rounds[roundNumber].turns[0].y - 4, 300 - 50 * (1 - stage) + 100 * rounds[roundNumber].turns[0].x + 4, 300 - 50 * (1 - stage) + 100 * rounds[roundNumber].turns[0].y - 4, 300 - 50 * (1 - stage) + 100 * rounds[roundNumber].turns[0].x, 300 - 50 * (1 - stage) + 100 * rounds[roundNumber].turns[0].y);
                         frame.Polygon(Color.FromArgb(animdb.Get(stage), Color.Red), 200 + 50 * (1 - stage) + 100 * rounds[roundNumber].turns[0].x, 300 + 100 * rounds[roundNumber].turns[0].y, 200 + 100 * rounds[roundNumber].turns[0].x + 4, 300 + 100 * rounds[roundNumber].turns[0].y + 4, 300 + 100 * rounds[roundNumber].turns[0].x + 4, 200 + 100 * rounds[roundNumber].turns[0].y + 4, 300 + 100 * rounds[roundNumber].turns[0].x, 200 + 100 * rounds[roundNumber].turns[0].y);
                     }
                 }
             }*/

        }

        public Turn TryGetHumanTurn(Player player, GlInput input)
        {
            return new Turn();
        }

    }
}
