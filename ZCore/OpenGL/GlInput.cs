using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Framework
{
    //todo инпут при долгом рисовании, по 1 секунде кадр
    public class GlInput
    {
          Dictionary<Key, int> keyTime;
        public   int KeyTime(Key key)
        {
            if (!keyTime.Keys.Contains(key))
                return 0;
            return keyTime[key];
        }
        /// <summary>
        /// если мышь снаружи, то последняя позиция
        /// </summary>
        public   Vector2d Mouse;
        public   Vector2d MouseRelative;
        /// <summary>
        /// на сколько переместились за последний кадр
        /// </summary>
        public Vector2d MouseDelta;
        public   int _mousePhysicalX, _mousePhysicalY;

          int _leftMouseTime = -1, _rightMouseTime = -1, _middleMouseTime = -1;
        public   int LeftMouseTime { get { return _leftMouseTime >= 0 ? _leftMouseTime : 0; } }
        public   int RightMouseTime { get { return _rightMouseTime >= 0 ? _rightMouseTime : 0; } }
        public   int MiddleMouseTime { get { return _middleMouseTime >= 0 ? _middleMouseTime : 0; } }
        public   bool MouseInside;
          bool _setMouseLeftUp, _setMouseRightUp, _setMouseMiddleUp;
        public   bool LeftMouseUp, RightMouseUp, MiddleMouseUp;
        /// <summary>
        /// -1 0 1
        /// </summary>
        public   int Wheel;//todo сильно зависит от фокуса
          Control _graphicControl;
        private   int _setWheel;
        public   Rect2d CameraViewport = new Rect2d(0, 0, 800, 600);

        public   void EveryFrameStartRefresh()
        {
            Vector2d mouseRelativePrevious = MouseRelative;
            MouseRelative = new Vector2d((double)_mousePhysicalX / _graphicControl.Width,
               (double)_mousePhysicalY / _graphicControl.Height);
            Mouse = GetAbsoluteCoordByRelativeOnScreen(MouseRelative);
            var prev = GetAbsoluteCoordByRelativeOnScreen(mouseRelativePrevious);
            MouseDelta = Mouse - prev;


            var allKeys = GetDownKeys().ToList();
            var newDict = new Dictionary<Key, int>();
            allKeys.ForEach(key =>
            {
                if (keyTime.ContainsKey(key))
                    newDict[key] = keyTime[key] + 1;
                else
                    newDict[key] = 1;

            });
            keyTime = newDict;

            if (_leftMouseTime != -1)
                _leftMouseTime++;
            if (_rightMouseTime != -1)
                _rightMouseTime++;
            if (_middleMouseTime != -1)
                _middleMouseTime++;

            LeftMouseUp = _setMouseLeftUp;
            RightMouseUp = _setMouseRightUp;
            MiddleMouseUp = _setMouseMiddleUp;
            _setMouseLeftUp = _setMouseMiddleUp = _setMouseRightUp = false;

            Wheel = _setWheel;
            _setWheel = 0;

            RefreshButtons();
        }

        public   Vector2d GetAbsoluteCoordByRelativeOnScreen(Vector2d relative)
        {
            //todo не учитывается угол поворота
            // return new Vector2d(relative.X * GlCore.WIDTH / Frame.cameraScale.X + Frame.cameraOrigin.X,
            //    relative.Y * GlCore.HEIGHT / Frame.cameraScale.Y + Frame.cameraOrigin.Y);
            return new Vector2d(relative.X * CameraViewport.size.X + CameraViewport.left,
                 relative.Y * CameraViewport.size.Y + CameraViewport.top);

        }

        public   void Init(Form form, Control graphicControl)
        {
            form.KeyPreview = true;
          //  form.KeyUp += form_KeyUp;
           // form.KeyDown += form_KeyDown;
            form.MouseWheel += form_MouseWheel;

            _graphicControl = graphicControl;
            _graphicControl.LostFocus += _graphicControl_LostFocus;
            _graphicControl.MouseMove += _graphicControl_MouseMove;
            _graphicControl.MouseLeave += _graphicControl_MouseLeave;
            _graphicControl.MouseDown += _graphicControl_MouseDown;
            _graphicControl.MouseUp += _graphicControl_MouseUp;
            keyTime = new Dictionary<Key, int>();
        }

        private   readonly byte[] DistinctVirtualKeys = Enumerable
    .Range(0, 256)
    .Select(KeyInterop.KeyFromVirtualKey)
    .Where(item => item != Key.None)
    .Distinct()
    .Select(item => (byte)KeyInterop.VirtualKeyFromKey(item))
    .ToArray();

        /// <summary>
        /// Gets all keys that are currently in the down state.
        /// </summary>
        /// <returns>
        /// A collection of all keys that are currently in the down state.
        /// </returns>
        public   IEnumerable<Key> GetDownKeys()
        {
            var keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            var downKeys = new List<Key>();
            for (var index = 0; index < DistinctVirtualKeys.Length; index++)
            {
                var virtualKey = DistinctVirtualKeys[index];
                if ((keyboardState[virtualKey] & 0x80) != 0)
                {
                    downKeys.Add(KeyInterop.KeyFromVirtualKey(virtualKey));
                }
            }

            return downKeys;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static  extern bool GetKeyboardState(byte[] keyState);


          void form_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (MouseInside)
            {
                if (e.Delta > 0)
                    _setWheel = 1;
                else if (e.Delta < 0)
                    _setWheel = -1;
            }
        }


          void _graphicControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _leftMouseTime = -1;
                _setMouseLeftUp = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                _rightMouseTime = -1;
                _setMouseRightUp = true;
            }
            if (e.Button == MouseButtons.Middle)
            {
                _middleMouseTime = -1;
                _setMouseMiddleUp = true;
            }
        }

          void _graphicControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _leftMouseTime = 0;
            if (e.Button == MouseButtons.Right)
                _rightMouseTime = 0;
            if (e.Button == MouseButtons.Middle)
                _middleMouseTime = 0;
        }




          void _graphicControl_MouseLeave(object sender, EventArgs e)
        {
            MouseInside = false;
        }

          void _graphicControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //todo recalc coord when wheel
            MouseInside = true;
            _mousePhysicalX = e.X;
            _mousePhysicalY = e.Y;
        }

          void _graphicControl_LostFocus(object sender, EventArgs e)
        {
            keyTime.Clear();
        }

        //todo modifier keys
        //  void form_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (_graphicControl.Focused)
        //        keyTime.Add(e.KeyCode, 0);
        //}

        //  void form_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (_graphicControl.Focused)
        //        keyTime.Remove(e.KeyData);
        //}

        public bool ButtonUnderMouse(string buttonName)
        {
            return _processedButtonDict.ContainsKey(buttonName) &&  _processedButtonDict[buttonName].Item1;
        }
        public bool ButtonClicked (string buttonName)
        {
            return _processedButtonDict.ContainsKey(buttonName) && _processedButtonDict[buttonName].Item2;
        }
        public List<string> AllButtonsUnderMouse()
        {
            return _processedButtonDict.Where(x => x.Value.Item1).Select(x => x.Key).ToList();
        }
        public List<string> AllClickedButtons()
        {
            return _processedButtonDict.Where(x => x.Value.Item2).Select(x => x.Key).ToList();
        }

        /// <summary>
        /// Можно сделать одно название с несколькими прямоугольнками, если вызвать несколько раз 
        /// </summary>
        /// <param name="p"></param>
        public void Button(Rect2d rect, string name)
        {
            _justCreatedButtonList.Add(Tuple.Create(rect, name));
        }

        void RefreshButtons()
        {
            _processedButtonDict = new Dictionary<string, Tuple<bool, bool>>();
            for(int i = 0; i < _justCreatedButtonList.Count; i++)
            {
                bool underMouse = false, clicked = false;
                var button = _justCreatedButtonList[i];
                if (GeomHelper.PointInSimpleRect(Mouse, button.Item1))
                {
                     underMouse =  true;
                    if (LeftMouseTime == 1)
                    {
                         clicked = true;
                    }
                }
                if(_processedButtonDict.ContainsKey(button.Item2))
                {
                    underMouse |= _processedButtonDict[button.Item2].Item1;
                    clicked |= _processedButtonDict[button.Item2].Item2;
                    _processedButtonDict.Remove(button.Item2);
                }
                _processedButtonDict.Add(button.Item2, Tuple.Create(underMouse, clicked));
            }
            _justCreatedButtonList = new List<Tuple<Rect2d, string>>();
        }

        Dictionary<string, Tuple<bool, bool>> _processedButtonDict = new Dictionary<string, Tuple<bool, bool>>();
        List<Tuple<Rect2d, string>> _justCreatedButtonList  = new List<Tuple<Rect2d, string>>();
    }
}
