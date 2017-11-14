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
    public class Misha2
    {

        int hp, gold, pos, round;
        int enemyHp, enemyGold, enemyPos,cnt;
        int[] house = new int[18], houseHp = new int[18];

        void GoToTile(int tile, int act)
        {
            if (pos > tile)
            {
                Write("L");
            }
            else if (pos < tile)
            {
                Write("R");
            }
            else
            {
                Write("S");
            }
            if (Math.Abs(pos - tile) <= 1)
                Write(act);
            else
                Write(0);
        }

        void Solve()
        {
            //changle line in Program.cs to use this solver
            //code here. use Read...() and Write(...,...,...)
            round = ReadInt(); hp = ReadInt(); gold = ReadInt(); pos = ReadInt(); enemyHp = ReadInt(); enemyGold = ReadInt(); enemyPos = ReadInt(); cnt = ReadInt();
            for (int i = 0; i < cnt; i++)
            {
                int homeType = ReadInt(),  homeHp = ReadInt(), homePos = ReadInt() ;
                house[homePos] = homeType;
                houseHp[homePos] = homeHp;
            }

            if (round < 80)
            {
                int nearestEmpty = -1;
                for(int i= 0; i < 9;i++){
                    if (house[i] == 0)
                    {
                        if (nearestEmpty == -1 || Math.Abs(pos - i) < Math.Abs(pos - nearestEmpty))
                            nearestEmpty = i;
                    }
                }
                if (nearestEmpty == -1)
                    GoToTile(8, 0);
                else
                    GoToTile(nearestEmpty, 1);
            }
            else
            {
                int eee = round / 20;
                int aimPos = eee % 2 == 0 ? 0 : 8;
                if (Math.Abs(pos - aimPos) <= 1)
                {
                    if (house[aimPos] == 5)
                        GoToTile(aimPos, 6);
                    else
                        GoToTile(aimPos, 5);
                }
                else
                {
                    int nextPos = aimPos == 0 ? pos - 1 : pos + 1;
                    GoToTile(aimPos, (house[nextPos] < 2 || house[nextPos] > 4) ? new Random().Next(2,5) : (0));
                }
            }
        }



        #region Main

        protected static TextReader reader;
        protected static TextWriter writer;
        public static void Run()
        {
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

            new Misha2().Solve();

            reader.Close();
            writer.Close();
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