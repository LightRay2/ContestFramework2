
using System;
using OpenTK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK
{
    public class Rect2d
    {
        public double left, top, right, bottom;
        public Vector2d lefttop { get { return new Vector2d(left, top); } set { left = value.X; top = value.Y; } }
        public Vector2d righttop { get { return new Vector2d(right, top); } set { right = value.X; top = value.Y; } }
        public Vector2d leftbottom { get { return new Vector2d(left, bottom); } set { left = value.X; bottom = value.Y; } }
        public Vector2d rightbottom { get { return new Vector2d(right, bottom); } set { right = value.X; bottom = value.Y; } }
        public Vector2d size { get { return rightbottom - lefttop; } set { rightbottom = lefttop + value; } }
        public List<Vector2d> points { get { return new List<Vector2d> { lefttop, righttop, rightbottom, leftbottom }; } }
        public List<Vector2d> pointsClosed { get { return new List<Vector2d> { lefttop, righttop, rightbottom, leftbottom,lefttop }; } }
        public Rect2d(double x, double y, double width, double height)
        {
            this.left = x;
            this.top = y;
            this.right = x + width;
            this.bottom = y + height;
        }
        public Rect2d() { }

        public Rect2d(Vector2d origin, Vector2d size)
        {
            this.lefttop = origin;
            this.size = size;
        }
        public Rect2d(double x, double y, Vector2d size) : this(new Vector2d(x,y), size){}
        public Rect2d(Vector2d origin, double width, double height) : this(origin, new Vector2d(width, height)) { }
        
        public static Rect2d FromCenterAndSize(double centerX, double centerY, double width, double height){return Rect2d.FromCenterAndSize(new Vector2d(centerX, centerY), new Vector2d(width, height));}
        public static Rect2d FromCenterAndSize(Vector2d center,  double width, double height){return Rect2d.FromCenterAndSize(center, new Vector2d(width, height));}
        public static Rect2d FromCenterAndSize(double centerX, double centerY, Vector2d size) { return Rect2d.FromCenterAndSize(new Vector2d(centerX, centerY), size); }
        public static Rect2d FromCenterAndSize(Vector2d center, Vector2d size)
        {
            return new Rect2d(center - size / 2, size);
        }
        public static Rect2d FromBoundingBox(params Vector2d[] pointList)
        {
            if (pointList.Length == 0)
                return new Rect2d();

            Rect2d rect = Rect2d.FromCenterAndSize(pointList[0], new Vector2d(0));

            foreach (var point in pointList.Skip(1))
            {
                rect.left = System.Math.Min(rect.left, point.X);
                rect.right = System.Math.Max(rect.right, point.X);
                rect.top = System.Math.Min(rect.top, point.Y);
                rect.bottom = System.Math.Max(rect.bottom, point.Y);

            }
            return rect;
        }
        public static Rect2d FromSize(double width, double height)
        {
            return new Rect2d(new Vector2d(0), new Vector2d(width, height));
        }
        public static Rect2d FromSize(Vector2d size) {
            return FromSize(size.X, size.Y);
        }

        public Vector2d center { get { return new Vector2d(left + size.X / 2, top + size.Y / 2); } }

        public static Rect2d operator +(Rect2d rect, Vector2d vector)
        {
            return new Rect2d(rect.lefttop + vector, rect.size);
        }
        public static Rect2d operator -(Rect2d rect, Vector2d vector)
        {
            return new Rect2d(rect.lefttop - vector, rect.size);
        }
    }
}
