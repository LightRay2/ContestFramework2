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
using System.Xml.Serialization;

namespace CoreCommented
{
    //#region small classes
    //public class Round : IRound<Turn, Player>
    //{
    //    public List<Turn> turns { get; set; }
    //    public Random random { get; set; }
    //    public double totalStage { get; set; }
    //    public string nameForTimeLine { get; set; }

    //    //здесь переменные, описывающие конкретный раунд
    //}

    //public class Turn : ITurn<Player>, ITimelineCell
    //{
    //    public Vector2d? ballAim;
    //    public List<Vector2d> manAims;

    //    public string input { get; set; }
    //    public string output { get; set; }
    //    public Player player { get; set; }
    //    public string shortStatus { get; set; }


    //    [XmlIgnore]
    //    public Color colorOnTimeLine { get; set; }
    //    public int ColorArgb { get { return colorOnTimeLine.ToArgb(); } set { colorOnTimeLine = Color.FromArgb(value); } }
    //    [XmlIgnore]
    //    public Enum fontOnTimeLine { get; set; }
    //    public string FontOnTimeLineString { get { return fontOnTimeLine.ToString(); } set { fontOnTimeLine = (EFont)Enum.Parse(typeof(EFont), value); } }
    //    public string nameOnTimeLine { get; set; }
    //    public Turn()
    //    {
    //        fontOnTimeLine = EFont.timelineNormal;
    //    }
    //}

    //public class Player : IPlayer
    //{
    //    public string programAddress { get; set; }
    //    public bool controlledByHuman { get; set; }
    //    public string name { get; set; }

    //    public int team;

    //    [XmlIgnore]
    //    Vector2d position;
    //    internal int score;
    //    public string memoryFromPreviousTurn = null;

    //    public int possession;

    //}
    //public class GameParams
    //{

    //}

    //public class Man
    //{
    //    public Vector2d position;
    //    public int team;

    //    public Color Color { get; internal set; }
    //}
    //#endregion
    //public enum EFont
    //{
    //    timelineNormal,
    //    timelineError,
    //    playerNumbers,
    //    TeamOne,
    //    TeamTwo,
    //    Time,
    //    CoordsOnField,
    //    Goal,
    //    ScoreOne,
    //    ScoreTwo,
    //    Possession
    //}
    ///// <summary>
    ///// Везде point.X соответствует номеру строки
    ///// </summary>
    //public class Game : IGame<FormState, Turn, Round, Player>
    //{
    //    public int roundNumber { get; set; }
    //    public int frameNumber { get; set; }
    //    public List<Player> players { get; set; }
    //    public List<Round> rounds { get; set; }
    //    public bool GameFinished { get; set; }
    //    public int clickedRound { get; set; }

    //    Rect2d _arena = new Rect2d(0, 0, 1000, 750);

    //    enum ESprite
    //    {
            
    //    }


        

    //    GamePurpose _gameInstancePurpose;
    //    FormState _formState;
    //    public Game(FormState settings, GamePurpose purpose)
    //    {
    //        _formState = settings;
    //        #region constructor
    //        clickedRound = -1;
    //        _rand = new Random(settings.RandomSeed);
    //        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
    //        _gameInstancePurpose = purpose;
    //        if (_gameInstancePurpose == GamePurpose.LoadSpritesAndFonts)
    //            return;



    //        #region create players
    //        players = settings.ProgramAddressesInMatch
    //                .Select(index => settings.ProgramAddressesAll[index])
    //                .Select(address => new Player
    //                {
    //                    name = address == null ? "Человек" : Path.GetFileNameWithoutExtension(address),
    //                    controlledByHuman = address == null,
    //                    programAddress = address
    //                }).ToList();

    //        for (int i = 0; i < 2; i++)
    //        {
    //            players[i].team = i;

    //        }

    //        #endregion

    //        var positions = new List<Vector2d>
    //        {
    //             new Vector2d (20, 10),
    //             new Vector2d(20, 30),
    //             new Vector2d(20,50),
    //             new Vector2d (40, 20),
    //             new Vector2d(40,40),


    //             new Vector2d (80, 10),
    //             new Vector2d(80, 30),
    //             new Vector2d(80,50),
    //             new Vector2d (60, 20),
    //             new Vector2d(60,40)
    //        };

    //        _manAnimators = new List<Animator<Vector2d>>();
    //        for (int i = 0; i < 10; i++)
    //        {
    //            var man = new Man
    //            {
    //                team = i / 5,
    //                Color = i / 5 == 0 ? Color.Blue : Color.Green,
    //                position = positions[i]
    //            };
    //            _manList.Add(man);

    //            if (i < 5)
    //                players[0].manList.Add(man);
    //            else
    //                players[1].manList.Add(man);

    //            _manAnimators.Add(new Animator<Vector2d>(Animator.Linear, man.position, man.position, 1));
    //        }

    //        AddBallToGame();

    //        #endregion
    //    }
        

    //    public static void SetFrameworkSettings()
    //    {
    //        FrameworkSettings.GameNameEnglish = "Samara2017";
    //        FrameworkSettings.RunGameImmediately = false;
    //        FrameworkSettings.AllowFastGameInBackgroundThread = true;
    //        FrameworkSettings.FramesPerTurn = 15;
    //        FrameworkSettings.ExecutionTimeLimitSeconds = 1;

    //        FrameworkSettings.PlayersPerGameMin = 2;
    //        FrameworkSettings.PlayersPerGameMax = 2;
    //        FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create("..//Players//Easy.exe", true));
    //        FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create("..//Players//Easy.exe", true));
    //        // FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create( "..//Players//Normal.exe", false));
    //        //  FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create( "..//Players//Hard.exe", false));
    //        //  FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create("..//Players//VeryHard.exe", true));
    //        //  FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create( "..//Players//Extreme.exe", false));

    //        FrameworkSettings.Timeline.Enabled = true;
    //        FrameworkSettings.Timeline.Position = TimelinePositions.right;
    //        FrameworkSettings.Timeline.TileLength = 4;
    //        FrameworkSettings.Timeline.TileWidth = 4;
    //        //  FrameworkSettings.Timeline.FontNormalTurn = EFont.timelineNormal;
    //        //  FrameworkSettings.Timeline.FontErrorTurn = EFont.timelineError;
    //        FrameworkSettings.Timeline.TurnScrollSpeedByMouseOrArrow = 4;
    //        FrameworkSettings.Timeline.TurnScrollSpeedByPageUpDown = 100;
    //        FrameworkSettings.Timeline.FollowAnimationTimeMs = 600;
    //    }

    //    public void LoadSpritesAndFonts()
    //    {
    //        if (FontList.All.Count == 0 && SpriteList.All.Count == 0)
    //        {
    //            FontList.Load(EFont.timelineNormal, "Times New Roman", 1.6, Color.FromArgb(200, 200, 200));
    //            FontList.Load(EFont.timelineError, "Times New Roman", 1.9, Color.Red, FontStyle.Bold);
    //            FontList.Load(EFont.playerNumbers, "Times New Roman", 1.5, Color.White);
    //            FontList.Load(EFont.Time, "Times New Roman", 2.0, Color.FromArgb(200, 200, 200), FontStyle.Bold);
    //            FontList.Load(EFont.TeamOne, "Times New Roman", 2.0, Color.FromArgb(200, 0, 180, 230), FontStyle.Bold);
    //            FontList.Load(EFont.TeamTwo, "Times New Roman", 2.0, Color.FromArgb(200, 180, 140, 0), FontStyle.Bold);
    //            FontList.Load(EFont.ScoreOne, "Times New Roman", 4.0, Color.FromArgb(255, 0, 180, 230), FontStyle.Bold);
    //            FontList.Load(EFont.ScoreTwo, "Times New Roman", 4.0, Color.FromArgb(255, 180, 140, 0), FontStyle.Bold);
    //            //  SpriteList.Load(ESprite.green);
    //            FontList.Load(EFont.CoordsOnField, "Times New Roman", 1.5, Color.FromArgb(200, 200, 200), FontStyle.Bold);
    //            FontList.Load(EFont.Possession, "Times New Roman", 1.5, Color.FromArgb(150, 150, 150), FontStyle.Bold);
    //            FontList.Load(EFont.Goal, "Lucida Console", 20, Color.FromArgb(150, 220, 0, 0), FontStyle.Bold);

    //        }
    //    }

    //    public void PreparationsBeforeRound()
    //    {

    //    }


    //    public List<Player> GetTurnOrderForNextRound()
    //    {
    //        return players;
    //    }


    //    public string GetCurrentSituation()
    //    {
    //        return null;
    //    }
        
    //    public string GetInputFile(Player player)
    //    {
    //        //player count 
    //        //player positions, hp, time to go, base position, time to end defence, 0-1 if has bomb
    //        //wall count
    //        //wall center positions
    //        //shell count
    //        //shell positions, finishPositions
    //        //bomb count
    //        //bomb positions
    //        //bonus bomb count
    //        //bonus bomb positions
    //        //bonus defence count
    //        //bonus defence positions
           
    //    }

    //    public Turn GetProgramTurn(Player player, string output, ExecuteResult executionResult, string executionResultRussianComment)
    //    {
          
    //    }

    //    void CheckDoubleWithException(double x)
    //    {
    //        if (double.IsInfinity(x) || double.IsNaN(x))
    //            throw new Exception();
    //    }

       

       
        
    //    public void ProcessRoundAndSetTotalStage(Round round)
    //    {
           
    //    }


      
    //    public void DrawAll(Frame frame, double stage, double totalStage, bool humanMove, GlInput input) //todo human move??
    //    {
            
    //    }
        
    //    public Turn TryGetHumanTurn(Player player, GlInput input)
    //    {
    //        return new Turn();
    //    }

    //}
}
