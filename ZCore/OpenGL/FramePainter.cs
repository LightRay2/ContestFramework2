using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using QuickFont;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Framework
{
    class FramePainter
    {
        public static TextManager _textManager = new TextManager();
        private static double brightenFactor = 1;
        /// <summary>
        /// 1.0 - 4.0
        /// </summary>
        public static double BrightenFactor
        {
            get { return brightenFactor; }
            set { brightenFactor = Math.Min(4, Math.Max(1, value)); }
        }
        public static void DrawFrame(Control control, IFramePainterInfo frame)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //camera
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var rect = frame.cameraViewport;
            GL.Ortho(rect.left, rect.right, rect.bottom, rect.top, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Rotate(frame.cameraRotationDeg, 0, 0, 1);
            //--

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //тут отвечаем только за глубину

            //глубина, тип, индекс
            var everything = new List<Tuple<double, int, int>>();
            for (int i = 0; i < frame.spriteList.Count; i++)
            {
                var x = frame.spriteList[i];
                if (SpriteList.All.ContainsKey(x.SpriteEnum) == false)
                {
                    SpriteList.Load(x.SpriteEnum);
                    Initializer.LoadTextures(new Dictionary<Enum, SpriteList> { { x.SpriteEnum, SpriteList.All[x.SpriteEnum] } });

                    //решил разрешить подгружать спрайты на лету
                    //throw new Exception("Спрайт с именем " + x.SpriteEnum.ToString() + " не добавлен при инициализации");

                }
                everything.Add(Tuple.Create(x.depth??SpriteList.All[x.SpriteEnum].DefaultDrawSettings.depth ?? 0.0,
                 0, i));
            }
            for (int i = 0; i < frame.textList.Count; i++)
            {
                var x = frame.textList[i];
                everything.Add(
                Tuple.Create(x.Item7 ?? FontList.All[x.Item1].depth,
                 1, i));
            }
            for (int i = 0; i < frame.polygonList.Count; i++)
            {
                var x = frame.polygonList[i];
                everything.Add(
                Tuple.Create(x.Item3,
                 2, i));
            }
            for (int i = 0; i < frame.pathList.Count; i++)
            {
                var x = frame.pathList[i];
                everything.Add(
                Tuple.Create(x.Item3,
                 3, i));
            }

            everything.Sort((a, b) =>
            {
                if (a.Item1 == b.Item1)
                {
                    if (a.Item2 == b.Item2)
                        return a.Item3.CompareTo(b.Item3);
                    else
                        return a.Item2.CompareTo(b.Item2);
                }
                else
                    return a.Item1.CompareTo(b.Item1);
            });

            foreach (var item in everything)
            {
                if (item.Item2 == 0)
                    DrawSprite(frame.spriteList[item.Item3]);
                else if (item.Item2 == 1)
                    DrawText(frame, item.Item3);
                else if (item.Item2 == 2)
                    DrawPolygon(frame.polygonList[item.Item3].Item1, frame.polygonList[item.Item3].Item2);
                else if (item.Item2 == 3)
                    DrawLineStrip(frame.pathList[item.Item3].Item1, frame.pathList[item.Item3].Item2, frame.pathList[item.Item3].Item4);
            }

            GL.Finish();

            double ms = stopwatch.ElapsedMilliseconds;
            double ticks = stopwatch.ElapsedTicks;
        }

        static void DrawSprite(SpriteDrawSettings drawSettings)
        {
            //рисуется в immediate mode , так что ускорение не сильное (сильное только для больших картинок)

            //prepare
            var sprite = SpriteList.All[drawSettings.SpriteEnum];
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, sprite.OpenglTexture);
            
            //opacity
            {
                var opacity = drawSettings.opacity ?? sprite.DefaultDrawSettings.opacity ?? 1;
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                GL.Color4(opacity, opacity, opacity, opacity);
            }

            //size
            Vector2d size;
            {
                if (drawSettings.sizeExact != null)
                    size = drawSettings.sizeExact.Value;
                else if (drawSettings.sizeOnlyWidth != null)
                    size = sprite.InitialSize * (drawSettings.sizeOnlyWidth.Value / sprite.InitialSize.X);
                else if (drawSettings.sizeOnlyHeight != null)
                    size = sprite.InitialSize * (drawSettings.sizeOnlyHeight.Value / sprite.InitialSize.Y);
                else if (sprite.DefaultDrawSettings.sizeExact != null)
                    size = sprite.DefaultDrawSettings.sizeExact.Value;
                else if (sprite.DefaultDrawSettings.sizeOnlyWidth != null)
                    size = sprite.InitialSize * (sprite.DefaultDrawSettings.sizeOnlyWidth.Value / sprite.InitialSize.X);
                else if (sprite.DefaultDrawSettings.sizeOnlyHeight != null)
                    size = sprite.InitialSize * (sprite.DefaultDrawSettings.sizeOnlyHeight.Value / sprite.InitialSize.Y);
                else
                    size = sprite.InitialSize;
            }
            //go to position
            {
                double angleDeg = sprite.DefaultDrawSettings.angleDeg ?? 0;
                if (drawSettings.angleDeg != null)
                    angleDeg += drawSettings.angleDeg.Value;
                else if (drawSettings.angleLookToPoint != null)
                    angleDeg += (drawSettings.angleLookToPoint.Value - new Vector2d(drawSettings.x, drawSettings.y)).AngleDeg();//now test
               
                GL.Translate(new Vector3d(drawSettings.x, drawSettings.y, 0));
                GL.Rotate(-angleDeg, 0, 0, 1);
                //  GL.Translate(new Vector3d(new Vector2d(drawSettings.originX-0.5, drawSettings.originY-0.5).MultEach(size)));
            }

            //select texture part by frame and draw it
            {
                var frameNumber = drawSettings.frameNumber ?? 0;
                if (frameNumber < 0)
                    frameNumber = 0;
                if (frameNumber > sprite.FrameCountVertical * sprite.FrameCountHorizontal - 1)
                    frameNumber = sprite.FrameCountHorizontal * sprite.FrameCountVertical - 1;
                int hor = sprite.FrameCountHorizontal;
                int vert = sprite.FrameCountVertical;

                double horPart = 1d / hor, vertPart = 1d / vert;
                double bottom = 1 - (frameNumber / hor + 1) * vertPart;
                double top = 1 - frameNumber / hor * vertPart;
                double right = (frameNumber % hor + 1) * horPart;
                double left = frameNumber % hor * horPart;


                var topLeft = -size.MultEach(new Vector2d(drawSettings.anchorX, drawSettings.anchorY));
                GL.Begin(PrimitiveType.Quads);
                // указываем поочередно вершины и текстурные координаты
                GL.TexCoord2(left, top);
                GL.Vertex2(topLeft);
                GL.TexCoord2(right, top);
                GL.Vertex2(topLeft.X + size.X, topLeft.Y);
                GL.TexCoord2(right, bottom);
                GL.Vertex2(topLeft.X + size.X, topLeft.Y + size.Y);
                GL.TexCoord2(left, bottom);
                GL.Vertex2(topLeft.X, topLeft.Y + size.Y);
                GL.End();

                //draw rect to brighten sprite

                if (brightenFactor.Equal(1) == false)
                {
                    double bright = Math.Min(2, brightenFactor) - 1;
                    GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.One);
                    GL.Color4(ColorByBright(bright));
                    GL.Begin(PrimitiveType.Quads);
                    // указываем поочередно вершины и текстурные координаты
                    GL.Vertex2(topLeft);
                    GL.Vertex2(topLeft.X + size.X, topLeft.Y);
                    GL.Vertex2(topLeft.X + size.X, topLeft.Y + size.Y);
                    GL.Vertex2(topLeft.X, topLeft.Y + size.Y);
                    GL.End();

                    if (brightenFactor > 2)
                    {

                        GL.Color4(ColorByBright((BrightenFactor - 2) / 2));
                        GL.Begin(PrimitiveType.Quads);
                        // указываем поочередно вершины и текстурные координаты
                        GL.Vertex2(topLeft);
                        GL.Vertex2(topLeft.X + size.X, topLeft.Y);
                        GL.Vertex2(topLeft.X + size.X, topLeft.Y + size.Y);
                        GL.Vertex2(topLeft.X, topLeft.Y + size.Y);
                        GL.End();
                    }
                }
            }


                GL.PopMatrix();
                GL.Disable(EnableCap.Texture2D);
        }
        

        static void DrawText(IFramePainterInfo frame, int index)
        {
            QFont.Begin();
            var text = frame.textList[index];
            var font = FontList.All[text.Item1];

            //загружаем все шрифты размером 40 (для достаточной четкости), а потом растягиваем перед выводом

            float defaultEmSize = 40.0f;
            var fontState = _textManager.LoadOrCheckFont(font.fontFamily, defaultEmSize /*(float)font.emSize*/, font.fontStyle, text.Item2);
            fontState.QFont.Options.Colour = font.color;

            double maxWidth = text.Item6 == null ? double.MaxValue : text.Item6.Value;

            var fontInfo = new Font(font.fontFamily, defaultEmSize, font.fontStyle, GraphicsUnit.Pixel);
            var fontHeight = fontInfo.Height;
            double userWantsHeightInPixels = font.emSize; //in pixels??
            double scale = userWantsHeightInPixels / fontInfo.Height;
            double userWantsMaxWidthInPixels = maxWidth / scale; //* weHavePerPixel.X;
            var sizeOnbitmap = fontState.QFont.Measure(text.Item2, (float)userWantsMaxWidthInPixels, text.Item5); //todo размер шрифта , если выбираем маленький вьюпорт, не тот
            var realSize = (new Vector2d(sizeOnbitmap.Width, sizeOnbitmap.Height)) * scale;// rect.size 
                                                                                           //.DivEach(new Vector2d(control.Width, control.Height))
                                                                                           //.MultEach(new Vector2d(sizeOnbitmap.Width, sizeOnbitmap.Height));

            //  if (maxWidth != null)
            //      maxWidth *= (rect.size.X / control.Width);
            // GL.Scale()
            GL.PushMatrix();
            var pos = (text.Item4 - text.Item3.MultEach(realSize));
            GL.Translate(pos.X, pos.Y, 0);
            GL.Scale(scale, scale, 1);
            fontState.QFont.Print(text.Item2, (float)userWantsMaxWidthInPixels, text.Item5);
            GL.PopMatrix();
            QFont.End();
        }

        public static void DrawPolygon(List<Vector2d> points, Color color)
        {
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Begin(PrimitiveType.Polygon);
            GL.Color4(color);
            points.ForEach(p => GL.Vertex2((Vector2)p));
            GL.End();
        }

        public static void DrawLineStrip(List<Vector2d> points, Color color, double lineWidth)
        {
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.LineWidth((float)lineWidth);
            GL.Begin(PrimitiveType.LineStrip);
            GL.Color4(color);
            points.ForEach(p => GL.Vertex2((Vector2)p));
            GL.End();
        }
        

        static Color ColorByBright(double e)
        {
            int x = 1 + (int)(253 * e);
            return Color.FromArgb(255, x, x, x);
        }

    }
}
