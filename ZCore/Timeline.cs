using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Framework
{
    public enum EDirections { down, right }
    public class Timeline
    {
        int _timelineTick = 0;
        bool _thereWasADraw = false;
        public Timeline()
        {
        }


        public double _firstTurnOffset = 0;
        public int _indexOfLastViewedTile = -1;
        public double _timelineSpeed = 0;
        

        public void Draw(Frame frame, GlInput input, List<ITimelineCell> turns, List<int> currentGameTurn)
        {
            var tileWidth = FrameworkSettings.Timeline.TileWidth;
            var tileLength = FrameworkSettings.Timeline.TileLength;
            var statusNearTileWidth = tileWidth / 3;
            var maxLength = ((IFramePainterInfo)frame).cameraViewport.size.Y;
            Vector2d positionOfFirstTile = new Vector2d(((IFramePainterInfo)frame).cameraViewport.right - tileWidth, 0);
           // var font = FrameworkSettings.Timeline.FontNormalTurn;

            if(currentGameTurn.Count>0)
                _indexOfLastViewedTile = Math.Max(currentGameTurn.Max(), _indexOfLastViewedTile);

            Rect2d fullTimelineRect = GetTimelineRect(frame);
            bool timelineUnderMouse = GeomHelper.PointInSimpleRect(input.Mouse, fullTimelineRect);
            var turnUnderMouseString =input.AllButtonsUnderMouse().FirstOrDefault(x => x.StartsWith("__timeline"));
            int turnUnderMouseIndex = turnUnderMouseString == null ? -1 : int.Parse(turnUnderMouseString.Replace("__timeline", ""));
            var highlightTurns = new List<int>(currentGameTurn);
            highlightTurns.Add(turnUnderMouseIndex);
            #region implementation
            //todo all variables named for down direction and only down is implemented

            //draw last two
            var timeLineX = positionOfFirstTile.X;
            {

                var lastPos = positionOfFirstTile.Y + maxLength - tileLength;
                //todo add rect instead

                var rect = new Rect2d(timeLineX, lastPos, tileWidth, tileLength);
                var statusRect = new Rect2d(timeLineX - statusNearTileWidth, lastPos, statusNearTileWidth, tileLength);

                if(turns.Count >0)
                {
                    var lastTurn = turns.Last();
                    string buttonName = string.Format("__timeline{0}", turns.Count - 1);
                    frame.Polygon( ShadedColor(rect,frame,lastTurn.colorOnTimeLine, !timelineUnderMouse,   highlightTurns.Contains(turns.Count - 1))  , rect);
                    frame.TextCenter(lastTurn.fontOnTimeLine, lastTurn.nameOnTimeLine, rect.center);
                    input.Button(rect, buttonName);
                }
                if(_indexOfLastViewedTile >=0)
                {
                    var lastViewedTurn = turns[_indexOfLastViewedTile];
                    string buttonName = string.Format("__timeline{0}", _indexOfLastViewedTile);
                    rect = rect - Vector2d.UnitY * tileLength * 1.5;
                    frame.Polygon(ShadedColor(rect, frame, lastViewedTurn.colorOnTimeLine, !timelineUnderMouse,  highlightTurns.Contains(_indexOfLastViewedTile)), rect);
                    frame.TextCenter(lastViewedTurn.fontOnTimeLine,  lastViewedTurn.nameOnTimeLine, rect.center);
                    input.Button(rect, buttonName);
                }

                maxLength -= 3 * tileLength; //todo закрыть неровности кнопками вверх вниз
                //todo брать ходы из другого места
            }
            //draw first 
            {
                for (int i = 0; i < turns.Count; i++)
                {
                    var turn = turns[i];
                    var rect = new Rect2d(timeLineX, _firstTurnOffset + i * tileLength, tileWidth, tileLength);

                    if (rect.bottom <= positionOfFirstTile.Y || rect.bottom > positionOfFirstTile.Y + maxLength)
                        continue;

                    var statusRect = new Rect2d(rect.lefttop - Vector2d.UnitX * statusNearTileWidth, statusNearTileWidth, tileLength);
                    string buttonName = string.Format("__timeline{0}", i);
                    frame.Polygon(ShadedColor(rect,frame, turn.colorOnTimeLine, !timelineUnderMouse, highlightTurns.Contains(i)), rect);
                    frame.TextCenter(turn.fontOnTimeLine, turn.nameOnTimeLine, rect.center);
                    input.Button(rect, buttonName);
                }
            }
            #endregion

        }


        Color ShadedColor(Rect2d tile, Frame frame, Color color, bool shade, bool underMouse)
        {
            if (underMouse) {
                var position = tile.center;
                frame.Polygon(color, 
                    Rect2d.FromCenterAndSize(  position,   new Vector2d(FrameworkSettings.Timeline.TileLength / 6)) 
                    - new Vector2d( FrameworkSettings.Timeline.TileWidth*0.8,0));
            }

          //  if(underMouse)
          //      return Color.FromArgb(255, color.R, color.G, color.B);

            if (!shade)
                return Color.FromArgb(150, color.R, color.G, color.B);
            return Color.FromArgb(80, color.R, color.G, color.B);
        }

        Animator<double> _animator = new Animator<double>(Animator.EaseOutQuad, 0, 0, 1);
        
        enum AnimatorReasons {none, wheel, followLastViewed};
        AnimatorReasons _lastAnimatorReason = AnimatorReasons.none;
        public int ManageTimelineByInputAndGetClickedTurn(out int turnUnderMouse, Frame frame, GlInput input, int turnCount,  List<int> currentGameTurns, double speedMultiplier)
        {
            turnUnderMouse = -1;

            Vector2d positionOfFirstTile = new Vector2d(((IFramePainterInfo)frame).cameraViewport.right - FrameworkSettings.Timeline.TileWidth, 0);
            Rect2d fullTimelineRect = GetTimelineRect(frame);
            //  var TIMELINE_SPEED_DECREASE_PER_TICK = FrameworkSettings.Timeline.TileLength / 30;

            double scrollTime = FrameworkSettings.Timeline.ScrollAnimationTimeMs;
            double followTime = speedMultiplier* FrameworkSettings.Timeline.FollowAnimationTimeMs * speedMultiplier;
            double allowedRangeDown = (-turnCount * FrameworkSettings.Timeline.TileWidth + fullTimelineRect.size.Y * 0.7).ToRange(0, double.MinValue),
                allowedRangeUp = 0;

            if (GeomHelper.PointInSimpleRect(input.Mouse, fullTimelineRect))
            {
                if (_lastAnimatorReason == AnimatorReasons.followLastViewed)
                {
                    _animator = new Animator<double>(Animator.Linear, _firstTurnOffset, _firstTurnOffset, 1);
                    _lastAnimatorReason = AnimatorReasons.none;
                }
                int d = input.Wheel * FrameworkSettings.Timeline.TurnScrollSpeedByMouseOrArrow;
                if (input.KeyTime(System.Windows.Input.Key.Up) == 1) d = FrameworkSettings.Timeline.TurnScrollSpeedByMouseOrArrow;
                if (input.KeyTime(System.Windows.Input.Key.Down) == 1) d = -FrameworkSettings.Timeline.TurnScrollSpeedByMouseOrArrow;
                if (input.KeyTime(System.Windows.Input.Key.PageUp) == 1) d = FrameworkSettings.Timeline.TurnScrollSpeedByPageUpDown;
                if (input.KeyTime(System.Windows.Input.Key.PageDown) == 1) d = -FrameworkSettings.Timeline.TurnScrollSpeedByPageUpDown;
                if (d != 0)
                {
                    double go = d * FrameworkSettings.Timeline.TileLength;
                    _animator = new Animator<double>(Animator.EaseOutQuad, _firstTurnOffset,
                       ((_lastAnimatorReason == AnimatorReasons.wheel ? _animator.finish : _firstTurnOffset) + go).ToRange(allowedRangeDown, allowedRangeUp),
                        scrollTime / FrameworkSettings.ForInnerUse.TimerInterval, _timelineTick - 1);
                    _lastAnimatorReason = AnimatorReasons.wheel;
                    _timelineSpeed = 0;
                    _firstTurnOffset += input.Wheel * FrameworkSettings.Timeline.TileLength;
                }
            }
            else
            {
                if(_indexOfLastViewedTile >=0)
                    _lastAnimatorReason = AnimatorReasons.followLastViewed;
                if (_lastAnimatorReason == AnimatorReasons.followLastViewed)
                {
                    if (currentGameTurns.Count != 0)
                    {
                        _animator = new Animator<double>(Animator.EaseOutQuad,
                            _firstTurnOffset,
                            (fullTimelineRect.size.Y * 0.4 - currentGameTurns.First() * FrameworkSettings.Timeline.TileLength).ToRange(allowedRangeDown, allowedRangeUp),
                            followTime / FrameworkSettings.ForInnerUse.TimerInterval, _timelineTick - 1);
                        _lastAnimatorReason = AnimatorReasons.followLastViewed;
                    }
                }
            }

            //if (_indexOfLastViewedTile >= 0)
            //{
            //    var halfYPosition = fullTimelineRect.size.Y / 2;
            //    var viewedTurnPosition = _firstTurnOffset + _indexOfLastViewedTile * FrameworkSettings.Timeline.TileLength;
            //    if (_followLastTurn && viewedTurnPosition < halfYPosition)
            //    {
            //        _animator = new Animator<double>(Animator.EaseOutQuad, 
            //            _firstTurnOffset, halfYPosition - _indexOfLastViewedTile * FrameworkSettings.Timeline.TileLength, 
            //            _followDuration, currentFrameTick-1);
            //    }

            //}

            _firstTurnOffset = _animator.Get(_timelineTick);


            //if (input.RightMouseTime >= 1 && GeomHelper.PointInSimpleRect(input.Mouse, fullTimelineRect))
            //{
            //    _timelineSpeed = 0;
            //    if (input.RightMouseTime > 1)
            //    {
            //        _firstTurnOffset += input.MouseDelta.Y;
            //    }
            //}
            //else if (input.RightMouseUp)
            //{
            //    _timelineSpeed = input.MouseDelta.Y;
            //}


            //if (input.Wheel != 0 && GeomHelper.PointInSimpleRect(input.Mouse, fullTimelineRect))
            //{
            //    _timelineSpeed = 0;
            //    _firstTurnOffset += input.Wheel * FrameworkSettings.Timeline.TileLength;
            //}

            //_firstTurnOffset += _timelineSpeed;
            //if (_timelineSpeed > 0)
            //{
            //    _timelineSpeed = Math.Max(0, _timelineSpeed - TIMELINE_SPEED_DECREASE_PER_TICK);
            //}
            //else
            //{
            //    _timelineSpeed = Math.Min(0, _timelineSpeed + TIMELINE_SPEED_DECREASE_PER_TICK);

            //}

            //correction  - must be <=0 and at least one tile should be visible
            //  if (_thereWasADraw == false)
            //  {
            //     _firstTurnOffset = -10000000;
            //      _thereWasADraw = true;
            //  }
            //   _firstTurnOffset = _firstTurnOffset.ToRange(-turnCount * FrameworkSettings.Timeline.TileWidth + FrameworkSettings.Timeline.TileWidth * 10, 0); //todo now only ten are visible



            _timelineTick++;

            var underMouse = input.AllButtonsUnderMouse().FirstOrDefault(x => x.StartsWith("__timeline"));
            if (underMouse != null)
            {

                int turnIndex = int.Parse(underMouse.Replace("__timeline", ""));


                turnUnderMouse = turnIndex;
            }



            var clickedTurn = input.AllClickedButtons().FirstOrDefault(x => x.StartsWith("__timeline"));
            if (clickedTurn != null)
            {

                int turnIndex = int.Parse(clickedTurn.Replace("__timeline", ""));
                
                    
                return turnIndex;
            }


            return -1;
        }


        private static Rect2d GetTimelineRect(Frame frame)
        {
            Vector2d positionOfFirstTile = new Vector2d(((IFramePainterInfo)frame).cameraViewport.right - FrameworkSettings.Timeline.TileWidth, 0);
            return new OpenTK.Rect2d(positionOfFirstTile, FrameworkSettings.Timeline.TileWidth, ((IFramePainterInfo)frame).cameraViewport.size.Y);
        }


        Color InvertedColor(Color color)
        {
            //not perfect , from stackoverflow, just changes the color
            Color invertedColor = Color.FromArgb(color.ToArgb() ^ 0xffffff);

            if (invertedColor.R > 110 && invertedColor.R < 150 &&
                invertedColor.G > 110 && invertedColor.G < 150 &&
                invertedColor.B > 110 && invertedColor.B < 150)
            {
                int avg = (invertedColor.R + invertedColor.G + invertedColor.B) / 3;
                avg = avg > 128 ? 200 : 60;
                invertedColor = Color.FromArgb(avg, avg, avg);
            }
            return invertedColor;
        }
    }
}
