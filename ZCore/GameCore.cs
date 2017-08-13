using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
//using Newtonsoft.Json;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Framework
{
    public enum GamePurpose { visualizationGame, fastGameInBackgroundWithoutVisualization, LoadSpritesAndFonts }
    /// <summary>
    /// Все, что должен делать главный класс - изменить состояние и вернуть новый кадр назад контроллера
    /// </summary>
    public class GameCore<TParamsFromStartForm, TTurn, TRound, TPlayer>
        where TParamsFromStartForm : IParamsFromStartForm
        where TTurn : ITurn<TPlayer>
        where TPlayer : IPlayer
        where TRound : IRound<TTurn, TPlayer>, new()
    {
        public static Random Rand = new Random();
        public static bool IsWorking = false;
        static ConcurrentDictionary<int, object> _roundsFromServer;

        public class Replay
        {
            public TParamsFromStartForm ParamsFromStartForm { get; set; }
            public List<TRound> AllRounds { get; set; }
        }
        public static bool TryRunAsSingleton(Func<TParamsFromStartForm, GamePurpose, IGame<TParamsFromStartForm, TTurn, TRound, TPlayer>> GameCreationDelegate, List<TParamsFromStartForm> settings, string replayFile = null, ConcurrentDictionary<int, object> roundsFromServer = null)
        {
           // try
            {
                if (IsWorking)
                    return false;
                IsWorking = true;
                _roundsFromServer = roundsFromServer;



                // if (FrameworkSettings.Timeline.Enabled && (FrameworkSettings.Timeline.FontNormalTurn == null ||
                //     FrameworkSettings.Timeline.FontErrorTurn == null))
                //     throw new Exception("Tileline включен, но не задан FrameworkSettings.Timeline.Font");

                _instance = new GameCore<TParamsFromStartForm, TTurn, TRound, TPlayer>();
                _instance._GameCreationDelegate = GameCreationDelegate;
                // _instance._game = GameCreationDelegate( game;
                if (replayFile == null)
                    _instance._settings = settings;
                else
                {
                    var replay = Serialize.TryReadFromXmlFile<Replay>(replayFile);
                    if (replay == null)
                        return false;

                    try
                    {

                        _instance._settings = new List<TParamsFromStartForm> { replay.ParamsFromStartForm };
                        _instance._allRounds = new ConcurrentDictionary<int, TRound>(Enumerable.Range(0, replay.AllRounds.Count).Select(x => new KeyValuePair<int, TRound>(x, replay.AllRounds[x])));
                    }
                    catch (Exception ex)
                    {
                        if (Debugger.IsAttached)
                            throw;
                        return false;
                    }
                }
                GameCreationDelegate(_instance._settings[0], GamePurpose.LoadSpritesAndFonts).LoadSpritesAndFonts();

                _instance._gameForm = new GameForm(_instance.Process);
                _instance._gameForm.watchSpeedMultiplier = _instance._settings.First().FramesPerTurnMultiplier;
                _instance._gameForm.InfoAction = "Матч запущен";
                _instance._gameForm.InfoUnderMouse = "Кликните меню ПОМОЩЬ для получения информации";
                _instance._gameForm.FormClosed += new FormClosedEventHandler((o, e) => IsWorking = false);
                _instance._gameForm.Load += (s, e) => _instance.TryInitNextGame();
                _instance._gameForm.ShowDialog(true);


                //todo keyboard
            }
           // catch { /*if (Debugger.IsAttached) throw;*/ }
            return true;
        }

        static GameCore<TParamsFromStartForm, TTurn, TRound, TPlayer> _instance;
        static GameCore()
        {
        }

        public static void SetWatchSpeedMultiplier(double mult)
        {
            _instance._settings[_instance._currentGameNumber].FramesPerTurnMultiplier = mult;
        }


        Func<TParamsFromStartForm, GamePurpose, IGame<TParamsFromStartForm, TTurn, TRound, TPlayer>> _GameCreationDelegate;
        GameForm _gameForm;
        IGame<TParamsFromStartForm, TTurn, TRound, TPlayer> _game;
        Timeline _timeline = new Timeline();
        int _currentGameNumber = -1;
        bool PauseButtonPressed = false;
        List<TParamsFromStartForm> _settings;
        TRound _currentRound;

        ConcurrentDictionary<int, TRound> _allRounds;

        enum EGameMode { localWithHuman, localWithoutHuman, fromServer, replayFile };
        EGameMode _gameMode;

        bool TryInitNextGame()
        {
            _currentGameNumber++;
            if (_currentGameNumber >= _settings.Count)
                return false;
            var settings = _settings[_currentGameNumber];

            _game = _GameCreationDelegate(settings, GamePurpose.visualizationGame);
            _game.roundNumber = -1;
            _game.rounds = new List<TRound>();

            if (_game.players.Count == 0)
                throw new Exception("В State.Init() должны быть созданы players");

            if (_allRounds != null)
            {
                _gameMode = EGameMode.replayFile;
            }
            else
            {
                _gameMode = _game.players.Any(x => x.controlledByHuman) ?
                    EGameMode.localWithHuman :
                    EGameMode.localWithoutHuman;
                _allRounds = new ConcurrentDictionary<int, TRound>();
            }

            if (_gameMode == EGameMode.localWithoutHuman)
            {
                if (FrameworkSettings.AllowFastGameInBackgroundThread)
                    RunGameWithoutHumanInBackground();
                else
                    _gameMode = EGameMode.localWithHuman;
            }

            //_turnReceiver = new TurnReceiver(_currentState, _engine, settings.javaPath, settings.ThisIsReplayGame);
            //if (settings.ThisIsReplayGame)
            //{
            //    bool success = _turnReceiver.TryLoadReplay(settings.replayPath);
            //    if (!success)
            //    {
            //        _currentState.Message = "Не удалось загрузить повтор. Нажмите Enter";
            //        _currentState.IsFinished = true;
            //    }
            //}


            //  AnimationGoNextTurn();
            //  DrawAll();

            GoToPhase(EProcessPhase.beforeRound);
            return true;
        }

        enum EProcessPhase { beforeRound, getTurnsOfNextRound, processRound, waitUntilAnimationFinished, gameFinished }
        EProcessPhase _processPhase;
        double animationStage = 0;
        double animationFinishStage = 1;
        private List<TPlayer> _currentPlayerOrder;

        bool animationFinished { get { return animationStage.Equal(animationFinishStage); } }

        void GoToPhase(EProcessPhase phase)
        {
            switch (phase)
            {
                case EProcessPhase.beforeRound:
                    _game.roundNumber++;
                    _game.PreparationsBeforeRound();
                    if (_gameForm.UserWantsPauseAfterTurn)
                    {
                        _gameForm.UserWantsPauseAfterTurn = false;
                        PauseButtonPressed = true;
                        _gameForm.GamePaused = true;
                    }
                    break;
                case EProcessPhase.getTurnsOfNextRound:
                    //сюда приходим только без бекграундной игры
                    bool nextRoundExists = false;
                    //   if (_roundsFromServer == null)
                    //    {
                    nextRoundExists = _allRounds.TryGetValue(_game.roundNumber, out _currentRound);
                    //   }
                    // else
                    //  {
                    //object tmp;
                    //if (_roundsFromServer.TryGetValue(_game.roundNumber, out tmp))
                    //{
                    //    nextRoundExists = true;
                    //}
                    //_currentRound = JsonConvert.DeserializeObject<TRound>(tmp.ToString());
                    //   }


                    if (!nextRoundExists)
                    {
                        _currentRound = new TRound();
                        _currentRound.random = new System.Random(Rand.Next());
                        _currentRound.turns = new List<TTurn>();
                        _currentPlayerOrder = _game.GetTurnOrderForNextRound();
                    }
                    else
                    {
                        GoToPhase(EProcessPhase.processRound);
                        return;
                    }
                    break;
                case EProcessPhase.processRound:
                    _game.rounds.Add(_currentRound);
                    _allRounds.TryAdd(_game.roundNumber, _currentRound);

                    var stopwatch = Stopwatch.StartNew();
                    _game.ProcessRoundAndSetTotalStage(_currentRound);
                    var ms = stopwatch.ElapsedMilliseconds;

                    animationStage = 0;
                    animationFinishStage = _currentRound.totalStage;
                    if (_settings[_currentGameNumber].FramesPerTurnMultiplier != _gameForm.watchSpeedMultiplier)
                        _settings[_currentGameNumber].FramesPerTurnMultiplier = _gameForm.watchSpeedMultiplier;
                    GoToPhase(EProcessPhase.waitUntilAnimationFinished);
                    return;
                    break;
                case EProcessPhase.gameFinished:
                    _gameForm.InfoAction = "Игра завершена";
                    break;
            }

            _processPhase = phase;
        }

        public Frame Process(GlInput input)
        {
            if (input.KeyTime(System.Windows.Input.Key.Escape) == 1)
                return null;
            if (GameForm.UserWantsToClose && !GameForm.GameInBackgroundRunning)
                return null;
            if (input.KeyTime(System.Windows.Input.Key.P) == 1)
            {
                PauseButtonPressed = !PauseButtonPressed;
                _gameForm.GamePaused = PauseButtonPressed;
            }
            else
                PauseButtonPressed = _gameForm.GamePaused;

            _gameForm.UserWantsPauseAfterTurn |= input.KeyTime(System.Windows.Input.Key.Oem4) == 1;//кнопка открытая квадратная скобка, или русская буква х

            bool goToBeforeRoundAfterDrawing = false;

            if (!PauseButtonPressed)
            {
                //фазы лесенкой, чтобы можно было хоть по всем пройтись

                if (_processPhase == EProcessPhase.beforeRound)
                {
                    if (_game.GameFinished)
                        GoToPhase(EProcessPhase.gameFinished);
                    else
                    {
                        if (_gameMode == EGameMode.localWithHuman)
                        {
                            GoToPhase(EProcessPhase.getTurnsOfNextRound);
                        }
                        else if (_gameMode == EGameMode.localWithoutHuman || _gameMode == EGameMode.replayFile)
                        {
                            if (_allRounds.ContainsKey(_game.roundNumber))
                            {
                                _allRounds.TryGetValue(_game.roundNumber, out _currentRound);
                                GoToPhase(EProcessPhase.processRound);
                            }
                            else
                            {
                                if (_gameMode == EGameMode.replayFile)
                                {
                                    _game.GameFinished = true;
                                    GoToPhase(EProcessPhase.gameFinished);
                                }
                            }
                            //  else
                            //  {
                            //      if (_game.roundNumber > 5)
                            //      {
                            //           _gameForm.InfoAction = "Рекомендуем снизить скорость просмотра";
                            //      }
                            // }
                        }
                        //иначе остаемся крутиться в before round, пока во втором потоке не сформируется ход

                    }
                }

                if (_processPhase == EProcessPhase.getTurnsOfNextRound)
                {
                    bool humanTurnWasAdded = false; 
                    while (_currentRound.turns.Count < _currentPlayerOrder.Count)
                    {
                        
                        var player = _currentPlayerOrder[_currentRound.turns.Count];
                        if (player.controlledByHuman)
                        {
                            if (humanTurnWasAdded && FrameworkSettings.ForInnerUse.GetAllUserTurnsImmediately == false)
                                break;

                            var turn = _game.TryGetHumanTurn(player, input);
                            if (turn != null)
                            {
                                turn.input = _game.GetInputFile(player);
                                _currentRound.turns.Add(turn);
                                humanTurnWasAdded = true;
                            }
                            else
                                break;
                        }
                        else
                        {
                            ExternalProgramExecuter epe =
                                new ExternalProgramExecuter(player.programAddress, "input.txt", "output.txt", _settings[_currentGameNumber].JavaPath, _settings[_currentGameNumber].PythonPath);

                            string inputFile = _game.GetInputFile(player);
                            string output;
                            string comment; //exitCode, нигде не используется

                            GameForm.GameInBackgroundRunning = true;
                            ExecuteResult res = epe.Execute(inputFile, FrameworkSettings.ExecutionTimeLimitSeconds, out output, out comment);
                            GameForm.GameInBackgroundRunning = false;

                            var turn = _game.GetProgramTurn(player, output, res, ExternalProgramExecuter.ExecuteResultToString(res));
                            turn.input = inputFile;
                            _currentRound.turns.Add(turn);
                        }
                    }

                    if (_currentRound.turns.Count == _currentPlayerOrder.Count)
                        GoToPhase(EProcessPhase.processRound);
                }

                if (_processPhase == EProcessPhase.waitUntilAnimationFinished)
                {
                    UpdateAnimationStage();
                    if (animationFinished)
                        goToBeforeRoundAfterDrawing = true;
                }

                if (_processPhase == EProcessPhase.gameFinished)
                {
                    if (input.KeyTime(System.Windows.Input.Key.Enter) == 1)
                    {
                        bool success = TryInitNextGame();
                        if (!success)
                        {
                            return null;
                        }
                    }
                }
            }

            Frame frame = new Frame();
            _game.DrawAll(frame, animationStage, animationFinishStage, _processPhase == EProcessPhase.getTurnsOfNextRound, input);
            if (_currentRound != null)
                DrawAndProcessTimeline(frame, input);

            if (goToBeforeRoundAfterDrawing)
                GoToPhase(EProcessPhase.beforeRound);

            _game.frameNumber++;




            return frame;
        }

        private void DrawAndProcessTimeline(Frame frame, GlInput input)
        {
            #region timeline
            {
                var rounds = _allRounds.Values.ToList(); //thread safe snapshot
                var turns = rounds.SelectMany(x => x.turns).ToList();
                if (turns.Count > 0)
                {
                    int currentTurn = rounds.Take(_game.roundNumber).Select(x => x.turns.Count).DefaultIfEmpty(0).Sum();
                    var currentTurns = new List<int>();
                    for (int i = 0; i < _currentRound.turns.Count; i++)
                    {
                        currentTurns.Add(currentTurn + i);
                    }
                    while (currentTurns.Count > 0 && currentTurns.Last() >= turns.Count)
                    {
                        currentTurns = currentTurns.Select(x => x - 1).ToList();
                    }
                    int turnUnderMouse;
                    var clickedTurn = _timeline.ManageTimelineByInputAndGetClickedTurn(out turnUnderMouse, frame, input, turns.Count, currentTurns, _settings[_currentGameNumber].FramesPerTurnMultiplier);
                    if (clickedTurn >= 0 && clickedTurn < turns.Count)
                    {
                        //double code
                        string turnName = (turns[clickedTurn] as ITimelineCell).nameOnTimeLine;
                        var inputTxt = turns[clickedTurn].input;
                        if (string.IsNullOrEmpty(inputTxt))
                        {
                            _gameForm.InfoAction = string.Format("input.txt для хода {0} отсутствует", turnName);
                        }
                        else
                        {
                            Clipboard.SetText(inputTxt);
                            _gameForm.InfoAction = string.Format("input.txt для хода {0} скопирован в буфер обмена", turnName);
                        }

                        var clickedRound = rounds.IndexOf(rounds.First(r => r.turns.Contains(turns[clickedTurn])));
                        SetStateBeforeGivenRound(clickedRound);
                    }
                    if (turnUnderMouse >= 0 && turnUnderMouse < turns.Count)
                    {
                        string turnName = (turns[turnUnderMouse] as ITimelineCell).nameOnTimeLine;
                        _gameForm.InfoUnderMouse = string.Format("Ход {0}. Статус: {1}", turnName, (turns[turnUnderMouse] as ITimelineCell).shortStatus);
                        if (input.KeyTime(System.Windows.Input.Key.I) == 1)
                        {
                            //double code
                            var inputTxt = turns[turnUnderMouse].input;
                            if (string.IsNullOrEmpty(inputTxt))
                            {
                                _gameForm.InfoAction = string.Format("input.txt для хода {0} отсутствует", turnName);
                            }
                            else
                            {
                                Clipboard.SetText(inputTxt);
                                _gameForm.InfoAction = string.Format("input.txt для хода {0} скопирован в буфер обмена", turnName);
                            }
                        }
                        if (input.KeyTime(System.Windows.Input.Key.O) == 1)
                        {
                            var outputTxt = turns[turnUnderMouse].output;
                            if (string.IsNullOrEmpty(outputTxt))
                            {

                                _gameForm.InfoAction = string.Format("output.txt для хода {0} отсутствует", turnName);
                            }
                            else
                            {
                                Clipboard.SetText(outputTxt);
                                _gameForm.InfoAction = string.Format("output.txt для хода {0} скопирован в буфер обмена", turnName);

                            }
                        }
                    }
                    _timeline.Draw(frame, input, turns.Cast<ITimelineCell>().ToList(), currentTurns);
                }

            }
            #endregion
        }


        private void UpdateAnimationStage()
        {
            int framesPerTurn = (int)Math.Ceiling(FrameworkSettings.FramesPerTurn * _settings[_currentGameNumber].FramesPerTurnMultiplier);
            animationStage += 1.0 / framesPerTurn;
            if (animationStage > animationFinishStage)
                animationStage = animationFinishStage;
        }

        //код для сервера уже не очень актуален
        ///// <summary>
        ///// метод можно чуть переделать как в бекграундном просчете
        ///// </summary>
        ///// <param name="game"></param>
        ///// <param name="javaPath"></param>
        ///// <param name="RoundPlayed"></param>
        ///// <returns></returns>
        //public static string RunOnServerOrGetError(IGame<TParamsFromStartForm, TTurn, TRound, TPlayer> game, string javaPath, Action<object, string> RoundPlayed)
        //{

        //    game.roundNumber = -1;

        //    if (game.players.Count == 0)
        //        return "Игра без игроков невозможна";

        //    while (!game.GameFinished)
        //    {
        //        game.roundNumber++;
        //        var round = new TRound();
        //        round.random = new System.Random(Rand.Next());
        //        round.turns = new List<TTurn>();

        //        game.PreparationsBeforeRound();
        //        List<TPlayer> order = game.GetTurnOrderForNextRound();
        //        while (round.turns.Count < order.Count)
        //        {
        //            TPlayer player = order[round.turns.Count];
        //            ExternalProgramExecuter epe =
        //                        new ExternalProgramExecuter(player.programAddress, "input.txt", "output.txt", javaPath);

        //            string input = game.GetInputFile(player);
        //            string output;
        //            string comment;

        //            GameForm.GameInBackgroundRunning = true;
        //            ExecuteResult res = epe.Execute(input, 2, out output, out comment);
        //            GameForm.GameInBackgroundRunning = false;

        //            var turn = game.GetProgramTurn(player, output, res, ExternalProgramExecuter.ExecuteResultToString(res));
        //            turn.output = output;
        //            round.turns.Add(turn);
        //        }
        //        game.rounds.Add(round);
        //        game.ProcessRoundAndSetTotalStage(round);

        //        RoundPlayed(round, game.GetCurrentSituation());
        //    }

        //    return "Ok";
        //}

        public void RunGameWithoutHumanInBackground()
        {
            GameForm.GameInBackgroundRunning = true;
            new Thread(new ThreadStart(RunGameWithoutHuman)) { IsBackground = true, Priority = ThreadPriority.Normal }.Start();
        }

        public void RunGameWithoutHuman()
        {
            var game = _GameCreationDelegate(_settings[_currentGameNumber], GamePurpose.fastGameInBackgroundWithoutVisualization);
            game.roundNumber = -1;
            game.rounds = new List<TRound>();

            int runtimeErrorOrTimeLimitCounter = 0;
            while (!game.GameFinished && GameForm.UserWantsToClose == false)
            {
                game.roundNumber++;
                var round = new TRound();
                round.random = new System.Random(Rand.Next());
                round.turns = new List<TTurn>();

                game.PreparationsBeforeRound();
                List<TPlayer> order = game.GetTurnOrderForNextRound();
                bool addRound = true;
                while (round.turns.Count < order.Count)
                {
                    TPlayer player = order[round.turns.Count];
                    ExternalProgramExecuter epe =
                                new ExternalProgramExecuter(player.programAddress, "input.txt", "output.txt", _settings[_currentGameNumber].JavaPath, _settings[_currentGameNumber].PythonPath);


                    string input = game.GetInputFile(player);
                    string output;
                    string comment;

                    ExecuteResult res = epe.Execute(input, FrameworkSettings.ExecutionTimeLimitSeconds, out output, out comment);

                    var turn = game.GetProgramTurn(player, output, res, ExternalProgramExecuter.ExecuteResultToString(res));
                    turn.output = output; //todo this is bad
                    turn.input = input;
                    round.turns.Add(turn);

                    if (res == ExecuteResult.InternalError || res == ExecuteResult.NotStarted || res == ExecuteResult.TimeOut)
                        runtimeErrorOrTimeLimitCounter++;
                    if (runtimeErrorOrTimeLimitCounter == 2)
                    {
                        //todo test game stipping
                        var dialogResult = _gameForm.ThreadSafeMessageBox("Остановка игры", "Вызов программы дважды завершился с ошибкой. YES = остановить игру, NO = продолжить до двух новых ошибочных запусков, CANCEL = продолжить до двадцати новых ошибочных запусков", MessageBoxButtons.YesNoCancel);
                        if (dialogResult == DialogResult.Yes)
                        {
                            game.GameFinished = true;
                            addRound = false;
                            break;
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            runtimeErrorOrTimeLimitCounter = 0;
                        }
                        else
                        {
                            runtimeErrorOrTimeLimitCounter = -18;
                        }
                    }
                }
                if (addRound)
                {
                    game.rounds.Add(round);
                    _allRounds.TryAdd(game.roundNumber, round);
                    game.ProcessRoundAndSetTotalStage(round);
                }
            }

            if (_settings[_currentGameNumber].SaveReplays)
            {
                var path = _settings[_currentGameNumber].ReplayFolder + "\\" + _settings[_currentGameNumber].ReplayFileName;
                try
                {
                    var replay = new Replay
                    {
                        AllRounds = _allRounds.Values.ToList(),
                        ParamsFromStartForm = _settings[_currentGameNumber]
                    };
                    Serialize.TryWriteToXmlFile(path, replay);
                }
                catch
                {
                    if (Debugger.IsAttached)
                        throw;
                }
            }

            GameForm.GameInBackgroundRunning = false;

        }

        public void SetStateBeforeGivenRound(int goBeforeRound)
        {
            try { 
                var tmp = _gameForm.InfoAction;
                _gameForm.InfoAction = string.Format("Осуществляем переход...");
                var game = _GameCreationDelegate(_settings[_currentGameNumber], GamePurpose.visualizationGame);
                game.roundNumber = -1;
                game.rounds = new List<TRound>();

                while (!game.GameFinished && game.roundNumber + 1 < goBeforeRound)
                {
                    game.roundNumber++;
                    game.PreparationsBeforeRound();

                    TRound round;
                    _allRounds.TryGetValue(game.roundNumber, out round);
                    game.rounds.Add(round);

                    game.ProcessRoundAndSetTotalStage(round);

                    animationFinishStage = round.totalStage;

                }
                animationStage = goBeforeRound == 0 ? 0 : 1;

                _game = game;

                TRound currentRound;
                _allRounds.TryGetValue(goBeforeRound, out currentRound);
                _gameForm.InfoAction = string.Format("Просмотр с момента перед ходом {0}. {1}",
                    currentRound.turns.Count == 0 ? "" : (currentRound.turns.First() as ITimelineCell).nameOnTimeLine, tmp);

                GoToPhase(EProcessPhase.beforeRound);
            }
            catch { if (Debugger.IsAttached) throw; }
        }

    }
}
