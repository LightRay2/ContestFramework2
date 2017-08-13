using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Framework
{
    public class FontList
    {
        public static Dictionary<Enum, FontList> All = new Dictionary<Enum, FontList>();

        public Enum fontEnum;
        public string fontFamily;
        public double emSize;
        public FontStyle fontStyle;
        public Color color;
        public double depth; //todo учитывать
        private FontList() { }

        public static void Load(Enum font, string fontFamily, double emSize, Color color, FontStyle fontStyle=FontStyle.Regular, double depth = 1)
        {
            var f = new FontList
            {
                color = color,
                depth = depth,
                emSize = emSize,
                fontEnum = font,
                fontFamily = fontFamily,
                fontStyle = fontStyle
            };
            All.Add(font, f);
        }

        public static void Load(Enum font, string fontFamily, double emSize, FontStyle fontStyle, double depth = 1)
        {
            Load(font, fontFamily, emSize, Color.Black, fontStyle, depth);
        }
        public static void Load(Enum font, string fontFamily, double emSize)
        {
            Load(font, fontFamily, emSize, Color.Black);
        }
    }
}
