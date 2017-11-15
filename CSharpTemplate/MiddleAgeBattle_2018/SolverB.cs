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
    public class SolverB
    {
        class Elem
        {
            public int type;
            public int pos;
            public int hp;
            public Elem()
            {

            }
        }
        void Solve()
        {
            //changle line in Program.cs to use this solver
            //code here. use Read...() and Write(...,...,...)
            List<Elem> a = new List<Elem>();
            int n = ReadInt();
            int hp = ReadInt();
            int g = ReadInt();
            int pos = ReadInt();
            int[] ddd = ReadIntArray();
            int count = 0;
            for (int i = 0; i < 18; i++)
            {

                int a1 = ReadInt();
                if(a1!=0)
                {
                    count++;
                    a.Add(new Elem());
                    a[count - 1].pos = i;
                    a[count - 1].type = a1;
                }
            }
            count = 0;
            for (int i = 0; i < 18; i++)
            {
                int a1 = ReadInt();
                if(a1!=0)
                {
                    count++;
                    a[count - 1].hp = hp;
                }
            }
            if (n < 30 )
            {
                if(g>600 && pos<3)                
                {
                    Write('R', 1);
                }
                else
                {
                    Write('S', 0);
                }
            }
            else
            { 

                    if (n < 38)
                        Write('R', 3 + n % 3);
                    else {
                    if(n<40)
                    {
                        Write('R', 0);
                    }
                    else
                    {
                        int k = -1;
                        for (int i = 0; i < count; i++)
                        {
                            if (a[i].type == 2 && a[i].pos < 9)
                            {
                                k = a[i].pos;
                                break;
                            }
                        }
                        if (k == -1)
                        {
                            Write('S', 2);
                        }
                        else
                        {
                            if (k > pos)
                            {
                                Write('R', 0);
                            }
                            else
                            {
                                if (k < pos)
                                {
                                    Write('L', 0);
                                }
                                else
                                {
                                    Write('S', 6);
                                }
                            }
                        }

                    }
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
            
                
            new SolverB().Solve();

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