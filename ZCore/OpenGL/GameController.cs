using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;
using System.Diagnostics;
using OpenTK;

namespace Framework
{
    class GameController
    {
        

        System.Windows.Forms.Timer _loopTimer = new System.Windows.Forms.Timer {Interval= FrameworkSettings.ForInnerUse.TimerInterval };

        public GameForm _parentForm;
        GLControl control;
        GlInput _keyboardState;
        Func<GlInput, Frame> _processMethod;
        public GameController(GLControl control, Func<GlInput, Frame> processMethod, GameForm gameForm)
        {
            _parentForm = gameForm;
            this.control = control;
            _keyboardState = new GlInput();
            _keyboardState.Init(gameForm, control);
            _processMethod = processMethod;

            //инициализация openGL
            Initializer.SetupViewport(control);
            control.Resize += new EventHandler((o, e) => Initializer.SetupViewport(o as GLControl));

            Initializer.LoadTextures(SpriteList.All);
            FramePainter._textManager = new TextManager(); //todo framework code

            //_textureCodes = Initializer.LoadTexturesOld();



            //игровой круг
            _loopTimer.Start();
            _loopTimer.Tick += MainLoop;

            _parentForm.FormClosed += new FormClosedEventHandler((o, e) => _loopTimer.Stop());
        }



        bool previousStateDrawed = true;
        void MainLoop(object sender, EventArgs e)
        {
            //вроде эти проверки излишни

            if (!previousStateDrawed) return; //если вдруг не успели отрисоваться за время кадра, подождем следующего тика
             previousStateDrawed = false;

             _keyboardState.EveryFrameStartRefresh();
            Frame frame = _processMethod(_keyboardState);
            if (frame == null)
                _parentForm.Close();
            else
            {
                //todo check if all sprites exist
                _keyboardState.CameraViewport = (frame as IFramePainterInfo).cameraViewport;
                if (Debugger.IsAttached)
                {
                    _parentForm.Text = _keyboardState.Mouse.ToString() + " ( будет скрыто при запуске не из под студии ) ";
                }
                FramePainter.DrawFrame(control, frame);
                control.SwapBuffers();
            }
            previousStateDrawed = true; //справились с рисованием
        }





        void CloseWindow()
        {
            _parentForm.Close();
        }











    }
}

