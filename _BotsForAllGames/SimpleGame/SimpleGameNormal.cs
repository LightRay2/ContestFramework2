using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSharpTemplate
{
    public class SimpleGameNormal
    {

        public class Point { public double x, y; }
        void Go()
        {
            //read

            int time = ReadInt(), scoreWe = ReadInt(), scoreEnemy = ReadInt();

            Point we = new Point { x = ReadDouble(), y = ReadDouble() };
            Point enemy = new Point { x = ReadDouble(), y = ReadDouble() };
            int ballcount = ReadInt();
            var balls = new List<Point>();
            for (int i = 0; i < ballcount; i++)
            {
                balls.Add(new Point { x = ReadDouble(), y = ReadDouble() });
            }
            Point aim = new Point { x = we.x, y = we.y };




            if (ballcount < 4)
            {
                if (ballcount > 0)
                    aim = balls.OrderBy(ball => Dist(we, ball)).First();

            }
            else
            {
                int bestone = 0, besttwo = 1, bestthree = 2;
                double bestDist = double.MaxValue;
                //  double bestToCenter = double.MaxValue;
                for (int one = 0; one < balls.Count; one++)
                {
                    for (int two = 0; two < balls.Count; two++)
                    {
                        for (int three = 0; three < balls.Count; three++)
                        {
                            for (int four = 0; four < balls.Count; four++)
                            {
                                var list = new List<int> { one, two, three, four };
                                if (list.Count != list.Distinct().Count())
                                    continue;

                                double curDist = Dist(we, balls[one]) + Dist(balls[one], balls[two]) + Dist(balls[two], balls[three]) + Dist(balls[three], balls[four]);
                                //    if(Math.Abs(curDist - bestDist ) < 0.000001)
                                // {
                                //      if(Dist)
                                //  }
                                if (curDist < bestDist)
                                {
                                    bestone = one;
                                    bestDist = curDist;
                                }
                            }
                        }
                    }
                }

                aim = balls[bestone];

            }

            Write(aim.x, aim.y);

        }


        double Dist(Point one, Point two)
        {
            return Math.Sqrt((one.x - two.x) * (one.x - two.x) + (one.y - two.y) * (one.y - two.y));
        }


        #region Main

        protected static TextReader reader;
        protected static TextWriter writer;
        public static string Run()
        {
            var typeName = "_";
            if (Debugger.IsAttached)
            {
                reader = new StreamReader("..\\..\\input.txt");
                //reader = new StreamReader(Console.OpenStandardInput());
                //writer = Console.Out;
                writer = new StreamWriter("..\\..\\output.txt");
            }
            else
            {
                //     reader = new StreamReader(Console.OpenStandardInput());
                //     writer = new StreamWriter(Console.OpenStandardOutput());
                reader = new StreamReader("input.txt");
                //reader = new StreamReader(Console.OpenStandardInput());
                //writer = Console.Out;
                writer = new StreamWriter("output.txt");
            }
            try
            {
                //var thread = new Thread(new Solver().Solve, 1024 * 1024 * 128);
                //thread.Start();
                //thread.Join();
                var game = new SimpleGameNormal();
                typeName = game.GetType().Name;
                game.Go();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
#if DEBUG
#else
            throw;
#endif
            }
            reader.Close();
            writer.Close();
            return typeName;
        }

        #endregion

        #region Read / Write
        private static Queue<string> currentLineTokens = new Queue<string>();
        private static string[] ReadAndSplitLine() { return reader.ReadLine().Split(new[] { ' ', '\t', }, StringSplitOptions.RemoveEmptyEntries); }
        public static string ReadToken() { while (currentLineTokens.Count == 0) currentLineTokens = new Queue<string>(ReadAndSplitLine()); return currentLineTokens.Dequeue(); }
        public static int ReadInt() { return int.Parse(ReadToken()); }
        public static long ReadLong() { return long.Parse(ReadToken()); }
        public static double ReadDouble() { return double.Parse(ReadToken(), CultureInfo.InvariantCulture); }
        public static int[] ReadIntArray() { return ReadAndSplitLine().Select(int.Parse).ToArray(); }
        public static long[] ReadLongArray() { return ReadAndSplitLine().Select(long.Parse).ToArray(); }
        public static double[] ReadDoubleArray() { return ReadAndSplitLine().Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray(); }
        public static int[][] ReadIntMatrix(int numberOfRows) { int[][] matrix = new int[numberOfRows][]; for (int i = 0; i < numberOfRows; i++) matrix[i] = ReadIntArray(); return matrix; }
        public static int[][] ReadAndTransposeIntMatrix(int numberOfRows)
        {
            int[][] matrix = ReadIntMatrix(numberOfRows); int[][] ret = new int[matrix[0].Length][];
            for (int i = 0; i < ret.Length; i++) { ret[i] = new int[numberOfRows]; for (int j = 0; j < numberOfRows; j++) ret[i][j] = matrix[j][i]; }
            return ret;
        }
        public static string[] ReadLines(int quantity) { string[] lines = new string[quantity]; for (int i = 0; i < quantity; i++) lines[i] = reader.ReadLine().Trim(); return lines; }
        public static void WriteArray<T>(IEnumerable<T> array) { writer.WriteLine(string.Join(" ", array)); }
        public static void Write(params object[] array) { WriteArray(array); }
        public static void WriteLines<T>(IEnumerable<T> array) { foreach (var a in array) writer.WriteLine(a); }
        private class SDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        {
            public new TValue this[TKey key]
            {
                get { return ContainsKey(key) ? base[key] : default(TValue); }
                set { base[key] = value; }
            }
        }
        private static T[] Init<T>(int size) where T : new() { var ret = new T[size]; for (int i = 0; i < size; i++) ret[i] = new T(); return ret; }
        #endregion
    }
}