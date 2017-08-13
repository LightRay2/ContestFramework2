using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    /// <summary>
    /// Дополнительные настройки. Используйте статические методы для создания.
    /// </summary>
    public class SpecialDraw
    {
        public Vector2d? scaleSize { get; set; }
        public Vector2d? size { get; set; }
        public int? frame { get; set; }
        public double? depth { get; set; }
        public double? opacity { get; set; }
        private SpecialDraw(Vector2d? size, Vector2d? scaleSize, int? frame, double? depth, double? opacity)
        {
            this.scaleSize = scaleSize;
            this.size = size;
            this.frame = frame;
            this.depth = depth;
            this.opacity = opacity;
        }

        public static SpecialDraw All(Vector2d? size = null, int? frame = null, double? depth = null, double? opacity = null)
        {
            return new SpecialDraw(size, null, frame, depth, opacity);
        }
        public static SpecialDraw All(double scaleSize , int? frame = null, double? depth = null, double? opacity = null)
        {
            return new SpecialDraw(null, new Vector2d(scaleSize), frame, depth, opacity);
        }
        public static SpecialDraw All(double scaleSizeX, double scaleSizeY, int? frame = null, double? depth = null, double? opacity = null)
        {
            return new SpecialDraw(null, new Vector2d(scaleSizeX, scaleSizeY), frame, depth, opacity);
        }

        public static SpecialDraw Opacity(double opacity)
        {
            return new SpecialDraw(null, null, null, null, opacity);
        }
        public static SpecialDraw Frame(int frame)
        {
            return new SpecialDraw(null, null, frame, null, null);
        }
        public static SpecialDraw Frame(double frame)
        {
            //todo
            return null; // return new SpecialDraw(null, null, frame, null, null);//todo
        }
        public static SpecialDraw Depth(double depth)
        {
            return new SpecialDraw(null, null, null, depth, null);
        }
        public static SpecialDraw Size(Vector2d size)
        {
            return new SpecialDraw(size, null, null, null, null);
        }
        public static SpecialDraw Size(double width, double height)
        {
            return new SpecialDraw(new Vector2d(width, height), null, null, null, null);
        }
        /// <summary>
        /// масштаб по сравнению со стандартным (как если бы без SpecialDraw)
        /// </summary>
        /// <param name="scaleSize"></param>
        /// <returns></returns>
        public static SpecialDraw SizeScale(Vector2d scaleSize)
        {
            return new SpecialDraw(null, scaleSize, null, null, null);
        }
        public static SpecialDraw SizeScale(double scaleSizeX, double scaleSizeY)
        {
            return new SpecialDraw(null, new Vector2d(scaleSizeX, scaleSizeY), null, null, null);
        }
        /// <summary>
        /// масштаб по сравнению со стандартным (как если бы без SpecialDraw)
        /// </summary>
        /// <param name="scaleSize"></param>
        /// <returns></returns>
        public static SpecialDraw SizeScale(double scaleSize)
        {
            return new SpecialDraw(null, new Vector2d(scaleSize), null, null, null);
        }
    }
}
