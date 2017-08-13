using OpenTK;
using QuickFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Framework
{
    public enum Align { left, right, center, justify };

    public interface IFramePainterInfo
    {
        Rect2d cameraViewport { get; set; }
        double cameraRotationDeg { get; set; }
        List<SpriteDrawSettings> spriteList { get; set; }
        List<Tuple<Enum, string, Vector2d, Vector2d, QFontAlignment, double?, double?>> textList { get; set; }
        List<Tuple<List<Vector2d>, Color, double>> polygonList { get; set; }
        List<Tuple<List<Vector2d>, Color, double, double>> pathList { get; set; }
    }
    /// <summary>
    /// Кадр, который игра обязана вернуть контроллеру для отрисовки
    /// </summary>
    public class Frame : IFramePainterInfo //todo скрыть все всопмогательное в неявный интерейс
    {
        public Frame()
        {
            ((IFramePainterInfo)this).cameraViewport = new Rect2d(0, 0, 800, 600);
            ((IFramePainterInfo)this).cameraRotationDeg = 0;
            ((IFramePainterInfo)this).pathList = new List<Tuple<List<Vector2d>,Color,double,double>>();
            ((IFramePainterInfo)this).polygonList = new List<Tuple<List<Vector2d>,Color,double>>();
            ((IFramePainterInfo)this).spriteList = new List<SpriteDrawSettings>();
            ((IFramePainterInfo)this).textList = new List<Tuple<Enum, string, Vector2d, Vector2d, QFontAlignment, double?, double?>>();
        }
        Rect2d IFramePainterInfo.cameraViewport { get; set; }
        double IFramePainterInfo.cameraRotationDeg { get; set; }
        List<SpriteDrawSettings> IFramePainterInfo.spriteList { get; set; }
        List<Tuple<Enum, string, Vector2d, Vector2d, QFontAlignment, double?, double?>> IFramePainterInfo.textList { get; set; }
        /// <summary>
        /// точки, цвет, глубина, толщина
        /// </summary>
        List<Tuple<List<Vector2d>, Color, double, double>> IFramePainterInfo.pathList { get; set; }
        List<Tuple<List<Vector2d>, Color, double>> IFramePainterInfo.polygonList { get; set; }

        
        /// <summary>
        /// в этой перегрузке только размер, в остальных - еще и левый верхний угол
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CameraViewport(double width, double height)
        {
            CameraViewport(0, 0, width, height);
        }
        public void CameraViewport(double x, double y, double width, double height)
        {
            ((IFramePainterInfo)this).cameraViewport = new Rect2d(x, y, width, height);
        }
        public void CameraViewport(Vector2d position, Vector2d size)
        {
            ((IFramePainterInfo)this).cameraViewport = new Rect2d(position, size);
        }
        public void CameraViewport(Vector2d position, double width, double height)
        {
            ((IFramePainterInfo)this).cameraViewport = new Rect2d(position, width, height);
        }
        public void CameraRotation(double angleDegrees)
        {
            ((IFramePainterInfo)this).cameraRotationDeg = angleDegrees;
        }

        //----text-----
        public void TextTopLeft(Enum font, string text, Vector2d position, double? widthLimit = null, Align align = Align.left, double? depth = null)
        {
            TextCustomAnchor(font, text, 0, 0, position, widthLimit, align, depth);
        }
        public void TextTopLeft(Enum font, string text, double x, double y, double? widthLimit = null, Align align = Align.left, double? depth = null)
        {
            TextCustomAnchor(font, text, 0, 0, new Vector2d(x, y), widthLimit, align, depth);
        }
        public void TextBottomLeft(Enum font, string text, Vector2d position, double? widthLimit = null, Align align = Align.left, double? depth = null)
        {
            TextCustomAnchor(font, text, 0, 1, position, widthLimit, align, depth);
        }
        public void TextBottomLeft(Enum font, string text, double x, double y, double? widthLimit = null, Align align = Align.left, double? depth = null)
        {
            TextCustomAnchor(font, text, 0, 1, new Vector2d(x, y), widthLimit, align, depth);
        }
        public void TextCenter(Enum font, string text, Vector2d position, double? widthLimit = null, Align align = Align.left, double? depth = null)
        {
            TextCustomAnchor(font, text, 0.5, 0.5, position, widthLimit, align, depth);
            
        }
        public void TextCenter(Enum font, string text, double x, double y, double? widthLimit = null, Align align = Align.left, double? depth = null)
        {
            TextCustomAnchor(font, text, 0.5, 0.5, new Vector2d(x, y), widthLimit, align, depth);

        }
        public void TextCustomAnchor(Enum font, string text, double anchorX, double anchorY, double x, double y, double? widthLimit = null, Align align = Align.left, double? depth = null)
        {
            TextCustomAnchor(font, text, anchorX, anchorY, new Vector2d(x, y), widthLimit, align, depth);
        }
        public void TextCustomAnchor(Enum font, string text, double anchorX, double anchorY, Vector2d position, double? widthLimit = null, Align align = Align.left, double? depth = null)
        {
            text = text ?? "";
            ((IFramePainterInfo)this).textList.Add(Tuple.Create(font, text, new Vector2d(anchorX, anchorY), position, (QFontAlignment)(int)align, widthLimit, depth));
        }
        //-------------------------------

        #region sprite

        //public void SpriteTopLeft(ISprite sprite){}
        //public void SpriteTopLeft(ISpriteSpecial sprite){}
        //public void SpriteTopLeft(Enum sprite, double x, double y) { SpriteCustom(sprite, 0.0, 0.0, x, y, 0, null); }
        //public void SpriteTopLeft(Enum sprite, double x, double y, double angleInDegrees) { }
        //public void SpriteTopLeft(Enum sprite, double x, double y, SpecialDraw specialDraw) { }
        //public void SpriteTopLeft(Enum sprite, double x, double y, Vector2d direction) { }
        //public void SpriteTopLeft(Enum sprite, double x, double y, double angleInDegrees, SpecialDraw specialDraw) { }
        //public void SpriteTopLeft(Enum sprite, double x, double y, Vector2d direction, SpecialDraw specialDraw) { }
        //public void SpriteTopLeft(Enum sprite, Vector2d position) { SpriteCustom(sprite, 0.0, 0.0, position.X, position.Y, 0, null); }
        //public void SpriteTopLeft(Enum sprite, Vector2d position, double angleInDegrees) { }
        //public void SpriteTopLeft(Enum sprite, Vector2d position, Vector2d direction) { }
        //public void SpriteTopLeft(Enum sprite, Vector2d position, SpecialDraw specialDraw) { }
        //public void SpriteTopLeft(Enum sprite, Vector2d position, double angleInDegrees, SpecialDraw specialDraw) { }
        //public void SpriteTopLeft(Enum sprite, Vector2d position, Vector2d direction, SpecialDraw specialDraw) { }

        public void SpriteCorner(Enum ESprite,
           double x, double y,
           double? angleDeg = null, Vector2d? angleLookToPoint = null,
           Vector2d? sizeExact = null, double? sizeOnlyWidth = null, double? sizeOnlyHeight = null,
           double? depth = null,
           double? opacity = null,
           int? frameNumber = null
           )
        {
            SpriteCustomAnchor(ESprite, 0, 0, x, y, angleDeg, angleLookToPoint, sizeExact, sizeOnlyWidth, sizeOnlyHeight,
                depth, opacity, frameNumber);
        }
        public void SpriteCorner(Enum ESprite,
            Vector2d position,
            double? angleDeg = null, Vector2d? angleLookToPoint = null,
            Vector2d? sizeExact = null, double? sizeOnlyWidth = null, double? sizeOnlyHeight = null,
            double? depth = null,
            double? opacity = null,
            int? frameNumber = null
            )
        {
            SpriteCustomAnchor(ESprite, 0, 0, position.X, position.Y, angleDeg, angleLookToPoint, sizeExact, sizeOnlyWidth, sizeOnlyHeight,
                depth, opacity, frameNumber);
        }

        public void SpriteCenter(Enum ESprite,
            double x, double y,
            double? angleDeg = null, Vector2d? angleLookToPoint = null,
            Vector2d? sizeExact = null, double? sizeOnlyWidth = null, double? sizeOnlyHeight = null,
            double? depth = null,
            double? opacity = null,
            int? frameNumber = null
            )
        {
            SpriteCustomAnchor(ESprite, 0.5, 0.5, x, y, angleDeg, angleLookToPoint, sizeExact, sizeOnlyWidth, sizeOnlyHeight,
                depth, opacity, frameNumber);
        }
        public void SpriteCenter(Enum ESprite,
            Vector2d position,
            double? angleDeg = null, Vector2d? angleLookToPoint = null,
            Vector2d? sizeExact = null, double? sizeOnlyWidth = null, double? sizeOnlyHeight = null,
            double? depth = null,
            double? opacity = null,
            int? frameNumber = null
            )
        {
            SpriteCustomAnchor(ESprite, 0.5, 0.5, position.X, position.Y, angleDeg, angleLookToPoint, sizeExact, sizeOnlyWidth, sizeOnlyHeight,
                depth, opacity, frameNumber);
        }

        public void SpriteCustomAnchor(Enum ESprite,
            double anchorX, double anchorY,
           double x, double y,
           double? angleDeg = null, Vector2d? angleLookToPoint = null,
           Vector2d? sizeExact = null, double? sizeOnlyWidth = null, double? sizeOnlyHeight = null,
           double? depth = null,
           double? opacity = null,
           int? frameNumber = null
           )
        {
            ((IFramePainterInfo)this).spriteList.Add(new Framework.SpriteDrawSettings
            {
                angleDeg = angleDeg,
                angleLookToPoint = angleLookToPoint,
                depth = depth,
                frameNumber = frameNumber,
                opacity = opacity,
                anchorX = anchorX,
                anchorY = anchorY,
                sizeExact = sizeExact,
                sizeOnlyHeight = sizeOnlyHeight,
                sizeOnlyWidth = sizeOnlyWidth,
                SpriteEnum = ESprite,
                x = x,
                y = y
            });
        }
        public void SpriteCustomAnchor(Enum ESprite,
             double anchorX, double anchorY,
            Vector2d position,
            double? angleDeg = null, Vector2d? angleLookToPoint = null,
            Vector2d? sizeExact = null, double? sizeOnlyWidth = null, double? sizeOnlyHeight = null,
            double? depth = null,
            double? opacity = null,
            int? frameNumber = null
            )
        {
            SpriteCustomAnchor(ESprite, anchorX, anchorY, position.X, position.Y, angleDeg, angleLookToPoint, sizeExact, sizeOnlyWidth, sizeOnlyHeight,
                depth, opacity, frameNumber);
        }

        
        //public void SpriteCenter(ISprite sprite) { }
        //public void SpriteCenter(ISpriteSpecial sprite) { }
        //public void SpriteCenter(Enum sprite, Vector2d position) { SpriteCustom(sprite, 0.5, 0.5, position.X, position.Y, 0, null); }
        //public void SpriteCenter(Enum sprite, Vector2d position, SpecialDraw specialDraw) { SpriteCustom(sprite, 0.5, 0.5, position.X, position.Y, 0, specialDraw); }
        //public void SpriteCenter(Enum sprite, Vector2d position, double angleInDegrees) { }
        //public void SpriteCenter(Enum sprite, Vector2d position, Vector2d direction) { }
        //public void SpriteCenter(Enum sprite, Vector2d position, double angleInDegrees, SpecialDraw specialDraw) { }
        //public void SpriteCenter(Enum sprite, Vector2d position, Vector2d direction, SpecialDraw specialDraw) { SpriteCustom(sprite, 0.5, 0.5, position.X, position.Y, direction.AngleDeg(), specialDraw); }
        //public void SpriteCenter(Enum sprite, double x, double y) { }
        //public void SpriteCenter(Enum sprite, double x, double y, double angleInDegrees) { }
        //public void SpriteCenter(Enum sprite, double x, double y, SpecialDraw specialDraw) { }
        //public void SpriteCenter(Enum sprite, double x, double y, Vector2d direction) { }
        //public void SpriteCenter(Enum sprite, double x, double y, Vector2d direction, SpecialDraw specialDraw) { }
        //public void SpriteCenter(Enum sprite, double x, double y, double angleInDegrees, SpecialDraw specialDraw)  { }

        //public void SpriteCustom(double originPointX, double originPointY, ISprite sprite) { }
        //public void SpriteCustom(double originPointX, double originPointY, ISpriteSpecial sprite) { }
        //public void SpriteCustom(Enum sprite, double originPointX, double originPointY, double x, double y) { }
        //public void SpriteCustom(Enum sprite, double originPointX, double originPointY, double x, double y, double angleInDegrees) { }
        //public void SpriteCustom(Enum sprite, double originPointX, double originPointY, double x, double y, Vector2d direction) { }
        //public void SpriteCustom(Enum sprite, double originPointX, double originPointY, double x, double y, SpecialDraw specialDraw){ }
        //public void SpriteCustom(Enum sprite, double originPointX, double originPointY, Vector2d position, double angleInDegrees, SpecialDraw specialDraw) { }
        //public void SpriteCustom(Enum sprite, double originPointX, double originPointY, Vector2d position, Vector2d direction, SpecialDraw specialDraw) { SpriteCustom(sprite, originPointX, originPointY, position.X, position.Y, direction.AngleDeg() , specialDraw);}
        //public void SpriteCustom(Enum sprite, double originPointX, double originPointY, Vector2d position) { SpriteCustom(sprite, originPointX, originPointY, position.X, position.Y, 0, null); }
        //public void SpriteCustom(Enum sprite, double originPointX, double originPointY, Vector2d position, double angleInDegrees) { }
        //public void SpriteCustom(Enum sprite, double originPointX, double originPointY, Vector2d position, SpecialDraw specialDraw) { SpriteCustom(sprite, originPointX, originPointY, position.X, position.Y, 0, specialDraw); }
        //public void SpriteCustom(Enum sprite, double originPointX, double originPointY, Vector2d position, Vector2d direction) {/* SpriteCustom(sprite, originPointX, originPointY, position.X, position.Y, )*/ }
        //public void SpriteCustom(Enum sprite, double originPointX, double originPointY, double x, double y, Vector2d direction, SpecialDraw specialDraw) { }
        //public void SpriteCustom(Enum sprite, double originPointX, double originPointY, double x, double y, double angleInDegrees, SpecialDraw specialDraw) 
        //{
        //    ((IFramePainterInfo)this).spriteList.Add(Tuple.Create(sprite, new Vector2d(originPointX, originPointY), new Vector2d(x, y), angleInDegrees, specialDraw));
        //}

        #endregion

        //polygon and line strip
        public void Polygon(Color color, Rect2d rect) { PolygonWithDepth(color, 0, rect.pointsClosed); }
        public void Polygon(Color color, List<Vector2d> pointList) { PolygonWithDepth(color, 0, pointList); }
        public void Polygon(Color color, params Vector2d[] pointList) { PolygonWithDepth(color, 0, pointList.ToList()); }
        public void Polygon(Color color, params double[] pointList) { PolygonWithDepth(color, 0, DoubleListToPointList(pointList.ToList())); }
        public void PolygonWithDepth(Color color, double depth, params Vector2d[] pointList) { PolygonWithDepth(color, depth, pointList.ToList()); }
        public void PolygonWithDepth(Color color, double depth, params double[] pointList) { PolygonWithDepth(color, depth, DoubleListToPointList(pointList.ToList())); }
        public void PolygonWithDepth(Color color, double depth , Rect2d rect ) { PolygonWithDepth(color, depth, rect.pointsClosed); }
        public void PolygonWithDepth(Color color, double depth, List<Vector2d> pointList)
        {
            ((IFramePainterInfo)this).polygonList.Add(Tuple.Create(pointList, color, depth));    
        }
        List<Vector2d> DoubleListToPointList(List<double> list)
        {
            var res = new List<Vector2d>();
            for (int one = 0, two = 1; one < list.Count; one += 2, two += 2)
                res.Add(new Vector2d(list[one], list[two]));
            return res;
        }

        public void Circle(Color color, double x, double y, double radius, double startAngleDeg=0, double sectorAngleDeg=360, int circleConsistsOfLines = 64)
        {
             Circle(color, new Vector2d(x, y), radius, startAngleDeg, sectorAngleDeg, circleConsistsOfLines);
        }
        //todo test circle
        public void Circle(Color color, Vector2d center, double radius, double startAngleDeg=0, double sectorAngleDeg=360, int circleConsistsOfLines = 64)
        {
            double singleangleRad = Math.PI * 2 / circleConsistsOfLines;
            var res = new List<Vector2d>();
            var startAngleRad = startAngleDeg / 180 * Math.PI;
            for (int i = 0; i < circleConsistsOfLines; i++)
            {

                double angleRad = i * singleangleRad;
                double angleDeg = angleRad * 180 / Math.PI;
                if (angleDeg.DoubleGreaterOrEqual( sectorAngleDeg)) //todo add all extensions to geomhelper to say they are exists
                {
                    angleRad = sectorAngleDeg / 180 * Math.PI;//todo test all extensions
                    res.Add(   (new Vector2d( radius,0)).RotateRad(startAngleRad + angleRad) + center); //дорисовали последнюю точку

                    break;
                }
                res.Add((new Vector2d( radius,0)).RotateRad(startAngleRad + angleRad) + center);
            }

            Polygon(color, res); //todo preserve depth as order of adding
            
        }
        public void Path(Color color, double lineWidth, Rect2d rect) { PathWithDepth(color, lineWidth, 0, rect.pointsClosed); }
        public void Path(Color color, double lineWidth, List<Vector2d> pointList) { PathWithDepth(color, lineWidth, 0, pointList); }
        public void Path(Color color, double lineWidth, params Vector2d[] pointList) { PathWithDepth(color, lineWidth, 0, pointList.ToList()); }
        public void Path(Color color, double lineWidth, params double[] pointList) { PathWithDepth(color, lineWidth, 0, DoubleListToPointList(pointList.ToList())); }
        public void PathWithDepth(Color color, double lineWidth, double depth, params Vector2d[] pointList) { PathWithDepth(color, lineWidth, depth, pointList.ToList()); }
        public void PathWithDepth(Color color, double lineWidth, double depth, params double[] pointList) { PathWithDepth(color, lineWidth, depth, DoubleListToPointList(pointList.ToList())); }
        public void PathWithDepth(Color color, double lineWidth, double depth, Rect2d rect) { PathWithDepth(color, lineWidth, depth, rect.pointsClosed); }
        public void PathWithDepth(Color color, double lineWidth, double depth, List<Vector2d> pointList)
        {
            ((IFramePainterInfo)this).pathList.Add(Tuple.Create(pointList, color, depth, lineWidth));
        }


       
    }
}
