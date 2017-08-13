using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class GeomHelper
    {
        public static double eps = 0.000000000001;//10e-12
        public static bool DoubleEqual(double a, double b)
        {
            return Math.Abs(a - b) < eps;
        }
        public static bool DoubleLess(double a, double b)
        {
            return !DoubleEqual(a, b) && a < b;
        }
        public static bool DoubleLessOrEqual(double a, double b)
        {
            return a < b || DoubleEqual(a, b);
        }
        public static double CrossProductModule(Vector2d one, Vector2d two)
        {
            return Math.Abs(one.X * two.Y - one.Y * two.X);
        }


        #region point inside polygon and rect, on line or segment

        //public static bool PointInsideRect(Vector2d point, Vector2d rectLocation, Vector2d rectSize)
        //{

        //    Vector2d a = rectLocation,
        //        b = new Vector2d(rectLocation.X + rectSize.X, rectLocation.Y),
        //        c = new Vector2d(rectLocation.X + rectSize.X, rectLocation.Y + rectSize.Y),
        //        d = new Vector2d(rectLocation.X, rectLocation.Y + rectSize.Y);
        //    double area1 = CrossProductModule(a - point, b - point) / 2;
        //    double area2 = CrossProductModule(b - point, c - point) / 2;
        //    double area3 = CrossProductModule(c - point, d - point) / 2;
        //    double area4 = CrossProductModule(d - point, a - point) / 2;
        //    double rectArea = rectSize.X * rectSize.Y;


        //    return !DoubleLess(rectArea, (area1 + area2 + area3 + area4));
        //}

        static double isLeft(Vector2d P0, Vector2d P1, ref Vector2d P2)
        {
            return ((P1.X - P0.X) * (P2.Y - P0.Y)
                    - (P2.X - P0.X) * (P1.Y - P0.Y));
        }

        /// <summary>
        /// полигоны можно передавать как замкнутые, так и не замкнутые. строго или нет - не проверял
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool PointInPolygon(List<List<Vector2d>> polygon, Vector2d point)
        {
            int insideCount = 0;
            foreach (var poly in polygon)
                if (PointInPolygon(poly, point))
                    insideCount++;
            return (insideCount & 1) == 1;
        }
        /// <summary>
        /// полигон можно передавать как замкнутый, так и не замкнутый. строго или нет - не проверял
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool PointInPolygon(List<Vector2d> polygon, Vector2d point)
        {
            if (polygon.Count == 0)
                return false;
            bool added = false;
            if (polygon[0] != polygon[polygon.Count - 1])
            {
                added = true;
                polygon.Add(polygon[0]);
            }

            //сам алгоритм Winding Count - http://geomalgorithms.com/a03-_inclusion.html
            int n = polygon.Count - 1;
            int wn = 0;    // the  winding number counter

            // loop through all edges of the polygon
            for (int i = 0; i < n; i++)
            {   // edge from V[i] to  V[i+1]
                if (polygon[i].Y <= point.Y)
                {          // start y <= P.y
                    if (polygon[i + 1].Y > point.Y)      // an upward crossing
                        if (isLeft(polygon[i], polygon[i + 1], ref point) > 0)  // P left of  edge
                            ++wn;            // have  a valid up intersect
                }
                else
                {                        // start y > P.y (no test needed)
                    if (polygon[i + 1].Y <= point.Y)     // a downward crossing
                        if (isLeft(polygon[i], polygon[i + 1], ref point) < 0)  // P right of  edge
                            --wn;            // have  a valid down intersect
                }
            }
            //==



            if (added)
                polygon.RemoveAt(polygon.Count - 1);

            return (wn & 1) == 1;
        }

        /// <summary>
        /// не строго
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool PointInRotatedRect(List<Vector2d> rect, Vector2d point)
        {
            if (rect.Count != 4)
                throw new Exception("Количество точек должно быть равно 4");

            //(0<AM⋅AB<AB⋅AB)∧(0<AM⋅AD<AD⋅AD)
            Vector2d AM = point - rect[0];
            Vector2d AB = rect[1] - rect[0];
            Vector2d AD = rect[3] - rect[0];
            double one = Vector2d.Dot(AM, AB);
            double two = Vector2d.Dot(AB, AB);
            double three = Vector2d.Dot(AM, AD);
            double four = Vector2d.Dot(AD, AD);
            return one > 0 && one < two && three > 0 && three < four;
        }

        /// <summary>
        /// то есть со сторонами, параллельными осям (для не параллельных испльзуем PointInRotatedRect). Не строго
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool PointInSimpleRect(Vector2d point, Rect2d rect)
        {
            return (point.X - rect.left) * (point.X - rect.right) <=0  
                && (point.Y - rect.top)*(  point.Y - rect.bottom) <=0;
        }

        public static bool PointInSimpleRect(Vector2d point, params Vector2d[] rect)
        {
            return PointInSimpleRect(point, Rect2d.FromBoundingBox(rect));
        }

        public static List<bool> PointsInConvexPolygon( List<Vector2d> polygon, List<Vector2d> pointList)
        {
            if (polygon.Count <= 2)
                return pointList.Select(x => false).ToList();

            List<bool> res = new List<bool>(pointList.Count);

            for (int pointIndex = 0; pointIndex < pointList.Count; pointIndex++)
            {
                var pos = 0;
                var neg = 0;

                for (var i = 0; i < polygon.Count; i++)
                {

                    //Form a segment between the i'th point
                    var x1 = polygon[i].X;
                    var y1 = polygon[i].Y;

                    //And the i+1'th, or if i is the last, with the first point
                    var i2 = i < polygon.Count - 1 ? i + 1 : 0;

                    var x2 = polygon[i2].X;
                    var y2 = polygon[i2].Y;

                    var x = pointList[pointIndex].X;
                    var y = pointList[pointIndex].Y;

                    //Compute the cross product
                    var d = (x - x1) * (y2 - y1) - (y - y1) * (x2 - x1);

                    if (d > 0) pos++;
                    if (d < 0) neg++;

                    //If the sign changes, then point is outside
                    if (pos > 0 && neg > 0)
                    {
                        res[pointIndex] = false;
                        continue;
                    }
                }
                res[pointIndex] = true;
            }
            return res;
        }

        public static bool IsPointOnLine(Vector2d point, Vector2d A, Vector2d B)
        {
            return DoubleEqual(CrossProductModule ((A - point), (B - point)), 0);
        }
        public static bool IsPointOnSegment(Vector2d point, Vector2d A, Vector2d B)
        {
            
            return IsPointOnLine(point, A, B) &&  DoubleLessOrEqual( Vector2d.Dot( (A - point), (B - point)), 0);

        }

        #endregion

        public static Tuple<double, double, double> LineEquation(Vector2d one, Vector2d two)
        {
            //A = y2-y1
            //B = x1-x2
            //C = A*x1+B*y1
            double a = two.Y - one.Y;
            double b = one.X - two.X;
            double c = a * one.X + b * one.Y;
            return Tuple.Create<double, double, double>(a, b, c);
        }

        public static Vector2d? LineCrossLine(Tuple<Vector2d, Vector2d> one, Tuple<Vector2d, Vector2d> two)
        {
            var first = LineEquation(one.Item1, one.Item2);
            var second = LineEquation(two.Item1, two.Item2);

            //double det = A1*B2 - A2*B1
            //if(det == 0){
            //    //Lines are parallel
            //}else{
            //    double x = (B2*C1 - B1*C2)/det
            //    double y = (A1*C2 - A2*C1)/det
            //}

            double det = first.Item1 * second.Item2 - second.Item1 * first.Item2;
            if (DoubleEqual(det, 0))
            {
                return null;
            }
            else
            {
                double x = (second.Item2 * first.Item3 - first.Item2 * second.Item3) / det;
                double y = (first.Item1 * second.Item3 - second.Item1 * first.Item3) / det;
                return new Vector2d(x, y);
            }
        }

        public static Vector2d? SegmentCrossSegment(Tuple<Vector2d, Vector2d> one, Tuple<Vector2d, Vector2d> two)
        {
            var linesCross = LineCrossLine(one, two);
            if (linesCross != null &&
                DoubleLessOrEqual(Vector2d.Dot((one.Item1 - linesCross.Value), (one.Item2 - linesCross.Value)), 0)
                && DoubleLessOrEqual(Vector2d.Dot((two.Item1 - linesCross.Value), (two.Item2 - linesCross.Value)), 0))
            {
                return linesCross;
            }
            return null;
        }

        



        /// <summary>
        /// алгоритм не заметит точки , лежащей вне перпендикуляров концов отрезка
        /// </summary>
        /// <returns></returns>
        public static Vector2d? PointOnSegmentClosestToPointApproximate(Vector2d point, Vector2d a, Vector2d b, double closerThan)
        {
            var segment = b - a;
            var pointVector = point - a;

            double scalarProgection = Vector2d.Dot(segment, pointVector) / segment.Length;

            if (scalarProgection >= 0 && scalarProgection < segment.Length)
            {
                //значит указатель в границах отрезка(может быть, где то рядом), остается посчитать расстояние от прямой до точки 
                var pointOnSegment = a + segment.Normalized() * scalarProgection;
                var distSqr = (point - pointOnSegment).LengthSquared;
                if (distSqr <= closerThan * closerThan)
                {
                    return pointOnSegment;
                }
            }

            return null;
        }

        public static Vector2d AveragePoint(List<Vector2d> list)
        {
            var midX = list.Sum(p => p.X) / list.Count;
            var midY = list.Sum(p => p.Y) / list.Count;
            return new Vector2d(midX, midY);
        }

        public static List<Vector2d> Circle(Vector2d center, double radius, int partCountPerFullCircle, double startAngle = 0, double sectorAngleDeg = 360)
        {
            double singleangleRad = Math.PI * 2 / partCountPerFullCircle;
            var res = new List<Vector2d>();
            var startAngleRad = startAngle / 180 * Math.PI;
            for (int i = 0; i < partCountPerFullCircle; i++)
            {

                double angleRad = i * singleangleRad;
                double angleDeg = angleRad * 180 / Math.PI;
                if (angleDeg.DoubleGreaterOrEqual(sectorAngleDeg))
                {
                    angleRad = sectorAngleDeg / 180 * Math.PI;
                    res.Add((Vector2d.UnitX * radius).RotateRad(startAngleRad + angleRad) + center); //дорисовали последнюю точку

                    break;
                }
                res.Add((Vector2d.UnitX * radius).RotateRad(startAngleRad + angleRad) + center);
            }



            return res;
        }

        public static bool IsCircleIntersectsRect(Vector2d center, double radius, Rect2d rect, int partCountPerFullCircle = 64)
        {
            var circle = Circle(center, radius, partCountPerFullCircle);
            return (circle.Any(x => GeomHelper.PointInSimpleRect(x, rect)));
        }

        public static bool IsCircleIntersectsRect(Vector2d position, object sHELL_RADIUS, Rect2d wall)
        {
            throw new NotImplementedException();
        }
    }
}
