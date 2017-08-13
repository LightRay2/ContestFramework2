using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class SpriteDrawSettings
    {
        public Enum SpriteEnum;
        public double x, y;
        public double anchorX, anchorY;
        public double? angleDeg = null;
        public Vector2d? angleLookToPoint = null;
        public Vector2d? sizeExact = null;
        public double? sizeOnlyWidth = null;
        public double? sizeOnlyHeight = null;
        public double? depth = null;
        public double? opacity = null;
        public int? frameNumber = null;
    }
    public class SpriteList
    {
        public static Dictionary<Enum, SpriteList> All = new Dictionary<Enum, SpriteList>();

        //public Enum SpriteEnum { get; set; }
        //public Vector2d? Size { get; set; }
        //public Vector2d? ScaleFromOriginal { get; set; }
        //public double Opacity { get; set; }


        public int FrameCountHorizontal { get; set; }
        public int FrameCountVertical { get; set; }
        
        //дальше служебные
        public SpriteDrawSettings DefaultDrawSettings = new SpriteDrawSettings();
        
        public Vector2d InitialSize { get; set; }

        //todo отрисовщик не забыть
        /// <summary>
        /// это должен учитывать отрисовщик
        /// </summary>
        public Vector2d ScaleToPowerOf2 { get; set; }
        public int OpenglTexture { get; set; }
        private SpriteList() { }

        public static void Load(Enum ESprite,
             double prerotateAngleDeg = 0,
            Vector2d? defaultSizeExact = null, double? defaultSizeOnlyWidth = null, double? defaultSizeOnlyHeight = null,
              double? defaultDepth = null, double? defaultOpacity = null,
            int frameCountHorizontal = 1, int frameCountVertical = 1)
        {
            var s = new SpriteList();
            s.FrameCountHorizontal = frameCountHorizontal;
            s.FrameCountVertical = frameCountVertical;
            s.DefaultDrawSettings.SpriteEnum = ESprite;
            s.DefaultDrawSettings.angleDeg = prerotateAngleDeg;
            s.DefaultDrawSettings.sizeExact = defaultSizeExact;
            s.DefaultDrawSettings.sizeOnlyWidth = defaultSizeOnlyWidth;//todo draw even not loaded sprite
            s.DefaultDrawSettings.sizeOnlyHeight = defaultSizeOnlyHeight;
            s.DefaultDrawSettings.depth = defaultDepth;
            s.DefaultDrawSettings.opacity = defaultOpacity;
            All.Add(ESprite, s);
        }
        //public static void LoadDefaultSize(Enum sprite, int frameCountHorizontal=1, int frameCountVertical=1 , double depth = 0, double opacity=1)
        //{
        //    var s = new SpriteList();
        //    s.SpriteEnum = sprite;
        //    s.FrameCountHorizontal = frameCountHorizontal;
        //    s.FrameCountVertical = frameCountVertical;
        //    s.Depth = depth;
        //    s.Opacity = opacity;
        //    All.Add(sprite, s);
        //}

        //public static void LoadSetSize(Enum sprite, Vector2d size, int frameCountHorizontal = 1, int frameCountVertical = 1, double depth = 0, double opacity = 1)
        //{
        //    var s = new SpriteList();
        //    s.SpriteEnum = sprite;
        //    s.Size = size;
        //    s.FrameCountHorizontal = frameCountHorizontal;
        //    s.FrameCountVertical = frameCountVertical;
        //    s.Depth = depth;
        //    s.Opacity = opacity;
        //    All.Add(sprite, s);
        //}

        //public static void LoadSetScale(Enum sprite, double scaleFromOriginal = 1, int frameCountHorizontal = 1, int frameCountVertical = 1, double depth = 0, double opacity = 1)
        //{
        //    var s = new SpriteList();
        //    s.SpriteEnum = sprite;
        //    s.ScaleFromOriginal = new Vector2d( scaleFromOriginal);
        //    s.FrameCountHorizontal = frameCountHorizontal;
        //    s.FrameCountVertical = frameCountVertical;
        //    s.Depth = depth;
        //    s.Opacity = opacity;
        //    All.Add(sprite, s);
        //}

        //public static void LoadSetScale(Enum sprite, Vector2d scaleFromOriginal, int frameCountHorizontal = 1, int frameCountVertical = 1, double depth = 0, double opacity = 1)
        //{
        //    var s = new SpriteList();
        //    s.SpriteEnum = sprite;
        //    s.ScaleFromOriginal = scaleFromOriginal;
        //    s.FrameCountHorizontal = frameCountHorizontal;
        //    s.FrameCountVertical = frameCountVertical;
        //    s.Depth = depth;
        //    s.Opacity = opacity;
        //    All.Add(sprite, s);
        //}

    }
}
