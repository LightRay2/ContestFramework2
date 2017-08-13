using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class DoubleExtensions
    {
        public static bool Equal(this double a, double b)
        {
            return Math.Abs(a - b) < eps;
        }
        

        /// <summary>
        /// везде, где нужен eps, используем этот, чтобы потом найти концы. Менять с осторожносьтю
        /// </summary>
        public static double eps = 0.000000000001;//10e-12

        public static bool DoubleEqual(this double a, double b, double multEpsilon = 1E-13)
        {
            double epsilon = Math.Max(Math.Abs(a), Math.Abs(b)) * multEpsilon;
            return Math.Abs(a - b) <= Math.Max(epsilon, eps);
            //return Math.Abs(a - b) < eps;
        }
        public static bool DoubleLess(this double a, double b)
        {
            return !DoubleEqual(a, b) && a < b;
        }
        public static bool DoubleLessOrEqual(this double a, double b)
        {
            return a < b || DoubleEqual(a, b);
        }
        public static bool DoubleGreater(this double a, double b)
        {
            return !DoubleEqual(a, b) && a > b;
        }
        public static bool DoubleGreaterOrEqual(this double a, double b)
        {
            return a > b || DoubleEqual(a, b);
        }

        public static double RoundTo14SignificantDigits(this double d)
        {
            if (d == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, 14);
        }

        public static double Rounded(this double d, int digitsAfterPoint)
        {
            return Math.Round(d, digitsAfterPoint);
        }

        /// <summary>
        /// вращает по часовой
        /// </summary>
        /// <param name="v"></param>
        /// <param name="angleRad"></param>
        /// <returns></returns>
        public static Vector2d RotateRad(this Vector2d v, double angleRad)
        {
            return Vector2d.Transform(v, Quaterniond.FromAxisAngle(new Vector3d(0, 0, 1), -angleRad));
        }
        /// <summary>
        /// вращает по часовой
        /// </summary>
        /// <param name="v"></param>
        /// <param name="angleDeg"></param>
        /// <returns></returns>
        public static Vector2d RotateDeg(this Vector2d v, double angleDeg)
        {
            return Vector2d.Transform(v, Quaterniond.FromAxisAngle(new Vector3d(0, 0, 1), -angleDeg / 180 * Math.PI));
        }

        /// <summary>
        /// против часовой, [0; 360) от оси ох
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double AngleRad(this Vector2d v)
        {
            var res = -Math.Atan2(v.Y, v.X);
            if (res < 0)
                res += Math.PI * 2;
            return res;
        }
        /// <summary>
        /// против часовой, [0; 360) от оси ох
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double AngleDeg(this Vector2d v)
        {
            return v.AngleRad() / Math.PI * 180;
        }
        /// <summary>
        /// на сколько надо прокрутить данный вектор против часовой, чтобы попасть в other,[0; 360) 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double AngleRad(this Vector2d v, Vector2d other)
        {
            var res = Math.Atan2(v.X, v.Y) - Math.Atan2(other.X, other.Y);
            if (res < 0)
                res += Math.PI * 2;
            else if (res >= Math.PI * 2)
                res -= Math.PI * 2;
            return res;
        }
        /// <summary>
        /// на сколько надо прокрутить данный вектор против часовой, чтобы попасть в other,[0; 360) 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double AngleDeg(this Vector2d v, Vector2d other)
        {
            return v.AngleRad(other) / Math.PI * 180;
        }

        /// <summary>
        /// не важно, больше Х чем Y или нет
        /// </summary>
        /// <returns></returns>
        public static int ToRange(this int a, int x, int y)
        {
            if (x > y)
            {
                int p = x; x = y; y = p;
            }
            if (a < x)
                return x;

            if (a > y)
                return y;

            return a;
        }
        /// <summary>
        /// не важно, больше Х чем Y или нет
        /// </summary>
        /// <param name="a"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double ToRange(this double a, double x, double y)
        {
            if (x > y)
            {
                double p = x; x = y; y = p;
            }
            if (a < x)
                return x;

            if (a > y)
                return y;

            return a;
        }
        public static Vector2d MultEach(this Vector2d one, Vector2d other)
        {
            return new Vector2d(one.X * other.X, one.Y * other.Y);
        }

        public static Vector2d DivEach(this Vector2d one, Vector2d other, double assignOtherIfZero = double.MinValue)
        {
            if (other.X == 0)
                other.X = assignOtherIfZero;
            if (other.Y == 0)
                other.Y = assignOtherIfZero;
            return new Vector2d(one.X / other.X, one.Y / other.Y);
        }

    }
}
