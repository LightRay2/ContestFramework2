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
        public static char[,] ch = new char[3, 3];
        public enum EFont
        {
            timelineNormal,
            timelineError,
            main,
            player0,
            player1
        }
        enum ESprite {
            back2,
            field,
            man0,
            man1,
            brownGrunge,
            mushroom
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
            for(int i=0;i<3;i++)
            {
                for(int j=0;j<3;j++)
                {
                    ch[i, j] = '-';
                }
            }//----


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
            FrameworkSettings.Timeline.TileLength = 7;
            FrameworkSettings.Timeline.TileWidth = 7;
            FrameworkSettings.Timeline.TurnScrollSpeedByMouseOrArrow = 8;
            FrameworkSettings.Timeline.TurnScrollSpeedByPageUpDown = 100;
            FrameworkSettings.Timeline.FollowAnimationTimeMs = 600;
        }

        public void LoadSpritesAndFonts()
        {
            if (FontList.All.Count == 0 && SpriteList.All.Count == 0)
            {
                FontList.Load(EFont.timelineNormal, "Times New Roman", 2.0, Color.FromArgb(193, 180, 255), FontStyle.Bold);
                FontList.Load(EFont.timelineError, "Times New Roman", 2.0, Color.Red, FontStyle.Bold);

                FontList.Load(EFont.main, "Times New Roman", 3.0, Color.FromArgb(193, 209, 255), FontStyle.Bold);

                FontList.Load(EFont.player0, "Times New Roman", 3.0, Color.FromArgb(230,70,70), FontStyle.Bold);
                FontList.Load(EFont.player1, "Times New Roman", 3.0, Color.FromArgb(70, 230, 70), FontStyle.Bold);

                SpriteList.Load(ESprite.man0, -90);
                SpriteList.Load(ESprite.man1, -90);
            }
        }
        public string GetCurrentSituation()
        {
            return null;

        }


        Rect2d _arena = new Rect2d(0, 0, 100, 100);
        int _turnLimit = 100;
        public string GetInputFile(Player player)
        {
            var st = new StringBuilder();
            for(int i=0;i<3;i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    st.AppendLine(ch[i,j]+" ");
                }
                st.AppendLine("\n");
            }
            return st.ToString();
        }

        public Turn GetProgramTurn(Player player, string output, ExecuteResult executionResult, string executionResultRussianComment)
        {
            var turn = new Turn
            {
                shortStatus = executionResultRussianComment,
                output = output,
                colorOnTimeLine = player.team == 0 ? Color.DarkRed : Color.DarkGreen,            //Color.FromArgb(148,36,26) : Color.FromArgb(85,110,84),//
                nameOnTimeLine = roundNumber.ToString(),
                colorStatusOnTimeLine = Color.Gold
            }; //todo now in interface just edit turn, no return

            /*turn.manAims = new List<Vector2d>();
            for (int i = 0; i < 1; i++)
                turn.manAims.Add(player.manList[i].position);*/

            int k = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (ch[i, j] != '-')
                    {
                        k++;
                    }
                }
            }
            if (k == 9)
            {
                GameFinished = true;
                return turn;
            }

            if (executionResult == ExecuteResult.Ok)
            {
                var reader = new StringReader(output);

                try
                {
                    var s = reader.ReadLine().Split(' ');
                    int i = Convert.ToInt32(s[0]);
                    int j = Convert.ToInt32(s[1]);
                    if(ch[i,j]!='-' )
                    {
                        throw new Exception();
                    }
                    else
                    {
                        if(player.team==0)
                        {
                            ch[i, j] = 'o';
                        }
                        else
                        {
                            ch[i, j] = 'x';
                        }
                    }
                }
                catch
                {
                    turn.fontOnTimeLine = EFont.timelineError;
                    turn.shortStatus = "Неправильный формат вывода";
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
            return players;
        }


        public void PreparationsBeforeRound()
        {

        }


        public void ProcessRoundAndSetTotalStage(Round round)
        {
            //Array.Copy(ch, round.ch, 9);
        }


        public void DrawAll(Frame frame, double stage, double totalStage, bool humanMove, GlInput input) //todo human move??
        {

            //!!! будьте внимательны (ранний drawall перед любыми методами)
           // int frameWidth = 160, frameHeight = 120;
            //frame.CameraViewport(frameWidth, frameHeight);

            frame.PolygonWithDepth(Color.White, -100, new Rect2d(0, 0,1400, 700)); //todo line around polygon
                                                                                   //frame.SpriteCorner(ESprite.brownGrunge, 0, -100, sizeOnlyHeight: frameHeight + 100);


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
            frame.Polygon(Color.Black, new Rect2d(200, 200, 300, 2));
            frame.Polygon(Color.Black, new Rect2d(200, 300, 300, 2));
            frame.Polygon(Color.Black, new Rect2d(200, 400, 300, 2));
            frame.Polygon(Color.Black, new Rect2d(200, 500, 300, 2));

            frame.Polygon(Color.Black, new Rect2d(200, 200, 2, 300));
            frame.Polygon(Color.Black, new Rect2d(300, 200, 2, 300));
            frame.Polygon(Color.Black, new Rect2d(400, 200, 2, 300));
            frame.Polygon(Color.Black, new Rect2d(500, 200, 2, 300));
            for(int i=0;i<3;i++)
            {
                for(int j=0;j<3;j++)
                {
                    if(ch[i,j]=='x')
                    {
                        frame.Polygon(Color.Red, 200 + 100 * j, 200 + 100 * i, 200 + 100 * j+4, 200 + 100 * i-4, 300 + 100 * j+4, 300 + 100 * i-4, 300 + 100 * j, 300 + 100 * i);
                        frame.Polygon(Color.Red, 200 + 100 * j, 300 + 100 * i, 200 + 100 * j+4, 300 + 100 * i+4, 300 + 100 * j+4, 200 + 100 * i+4, 300 + 100 * j, 200 + 100 * i);
                    }
                    if (ch[i, j] == 'o')
                    {
                        frame.Circle(Color.Green, 200 + 100 * j + 50, 200 + 100 * i + 50, 45);
                    }
                }
            }





            // }

            // frame.TextTopLeft(EFont.player0, players[0].name + ": " + players[0].score.ToString(), 10, 3);
            //frame.TextCustomAnchor(EFont.player1, players[1].name + ": " + players[1].score.ToString(), 1, 0, 110, 3);

            //frame.TextCustomAnchor(EFont.main, roundNumber.ToString(), 0.5, 0, 60, 3);


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


        }

        public Turn TryGetHumanTurn(Player player, GlInput input)
        {
            return new Turn();
        }

    }
}
