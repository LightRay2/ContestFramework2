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

    public class Misha3
    {


        int hp, gold, pos, round;
        int enemyHp, enemyGold, enemyPos, cnt;
        int[] house = new int[25], houseHp = new int[25];


        bool GoToTile(int tile, int act, bool force = false)
        {
            if ( Math.Abs(pos - tile) > 1)
                act = 0;
            if (pos > tile)
            {
                Write("L", act);
            }
            else if (pos < tile)
            {
                Write("R", act);
            }
            else
            {
                Write("S", act);
            }

            return Math.Abs(pos - tile) <= 1;
        }

        void Solve()
        {
            //changle line in Program.cs to use this solver
            //code here. use Read...() and Write(...,...,...)
            round = ReadInt(); hp = ReadInt(); gold = ReadInt(); pos = ReadInt(); enemyHp = ReadInt(); enemyGold = ReadInt(); enemyPos = ReadInt();
            for (int i = 0; i < 18; i++)
            {
                house[i] = ReadInt();
            }
            for (int i = 0; i < 18; i++) { houseHp[i] = ReadInt(); }
            int memoryOne = ReadInt(), memoryTwo = memoryOne == -1 ? -1 : ReadInt();


            if (memoryOne == 6 && house[memoryTwo] != 2)
                memoryOne = -1;

            
            if(memoryOne ==-1)
            {
                //facts

                bool gameStarted = round < 15;
                bool gameFinish = round > 80;
                int farmCount = house.Take(9).Count(x => x == 1);
                int gunCount = house.Take(9).Count(x => x > 2);

                int ballistasOnlastTiles = house.Skip(6).Take(3).Count(x => x == 5);
                bool towerBeforeBallista = house[5] == 2;

                int aimsUnderGuns = 0; // 3 ok
                int tileForGun8 = -1, tileForGun10 = -1, tileForGun12 = -1;
                int nearestEmpty = -1;

                AnalyzeGuns(ref aimsUnderGuns, ref tileForGun8, ref tileForGun10, ref tileForGun12, ref nearestEmpty, farmCount);
                int nearestNotFarm = house.Take(9).ToList().FindIndex(x => x != 1);
                int nearestTower = Enumerable.Range(0,9).OrderBy(x=>Math.Abs(x-pos)).Cast<int?>().ToList().FirstOrDefault(x => house[ x.Value] == 2)??-1;
                int nearestNotGun = Enumerable.Range(0, 9).OrderBy(x => Math.Abs(x - pos)).Cast<int?>().ToList().FirstOrDefault(x => house[x.Value] <= 2)??-1;

                //action select
                if ((gameStarted || (gold < 1000 && farmCount < 3)) &&farmCount <9)
                {
                    memoryOne = 1;
                    if (nearestEmpty == -1) memoryTwo = nearestNotFarm;
                    else memoryTwo = nearestEmpty;
                }
                else if (gameFinish/* || gold > 10000*/)
                {
                    
                    if (pos >=6 && ballistasOnlastTiles>0)
                    {
                        for (int i = pos; i >= 6; i--)
                        {
                            if (house[i] != 5)
                            {
                                memoryOne = 5;
                                memoryTwo = i;
                            }
                        }
                    }
                    if (memoryOne == -1)
                    {
                        if(ballistasOnlastTiles ==0)
                        {
                            memoryOne = 5;
                            memoryTwo = 8;
                        }
                        else if (towerBeforeBallista)
                        {
                            memoryOne = 6;
                            memoryTwo = 5;
                        }
                        else
                        {
                            memoryOne = 2;
                            memoryTwo = 5;
                        }
                    }
                }
                else if(aimsUnderGuns >= 5 )
                {
                    if(nearestTower == -1)
                    {
                        if(nearestNotGun == -1)
                        {
                            memoryOne = 2;
                            memoryTwo = nearestNotFarm;
                        }
                        else
                        {
                            memoryOne = 2;
                            memoryTwo = nearestNotGun;
                        }
                    }
                    else
                    {
                        memoryOne = 6;
                        memoryTwo = nearestTower;
                    }
                }
                else if(tileForGun8 != -1)
                {
                    memoryOne = 3; memoryTwo = tileForGun8;
                }
                else if(tileForGun10 != -1)
                {
                    memoryOne = 4; memoryTwo = tileForGun10;
                }
                else if(tileForGun12 != -1)
                {
                    memoryOne = 5; memoryTwo = tileForGun12;
                }
                else
                {
                    Write("S", 0);
                }
            }

            if (memoryOne >= 0)
            {
                //action 
                bool finished = GoToTile(memoryTwo, memoryOne);
                if (finished)
                    memoryOne = -1;
            }

            Write("memory", memoryOne, memoryTwo);

        }

        private void AnalyzeGuns(ref int aimsUnderGuns, ref int tileForGun8, ref int tileForGun10, ref int tileForGun12, ref int nearestEmpty, int farmCount)
        {
            List<int> gun8 = new List<int>();
            List<int> gun10 = new List<int>();
            List<int> gun12 = new List<int>();
            

            for (int i = 0; i < 9; i++)
            {
                if (i > 1)
                {
                    if (tileForGun8 == -1 && house[i + 8] > 0 && (house[i] == 0 || house[i] == 2 || (house[i]==1 && farmCount>3)))
                        gun8.Add( i);
                    if (tileForGun10 == -1 && house[i + 10] > 0 && (house[i] == 0 || house[i] == 2 || (house[i] == 1 && farmCount > 3)))
                        gun10.Add( i);
                    if (tileForGun12 == -1 && house[i + 12] > 0 && (house[i] == 0 || house[i] == 2 || (house[i] == 1 && farmCount > 3)))
                        gun12 .Add( i);
                }
                if (house[i] == 3)
                {
                    if (house[i + 8] != 0) aimsUnderGuns += 2;
                    if (house[i + 7] != 0) aimsUnderGuns++;
                    if (house[i + 9] != 0) aimsUnderGuns++;
                }
                if (house[i] == 4)
                {
                    if (house[i + 10] != 0) aimsUnderGuns += 2;
                    if (house[i + 9] != 0) aimsUnderGuns++;
                    if (house[i + 11] != 0) aimsUnderGuns++;
                }
                if (house[i] == 5)
                {
                    if (house[i + 12] != 0) aimsUnderGuns += 2;
                    if (house[i + 11] != 0) aimsUnderGuns++;
                    if (house[i + 13] != 0) aimsUnderGuns++;
                }

                if (house[i] == 0)
                {
                    if (nearestEmpty == -1) nearestEmpty = i;
                    else if (Math.Abs(pos - i) <= Math.Abs(pos - nearestEmpty)) nearestEmpty = i;
                }
            }

            gun8 = gun8.OrderBy(x => Enumerable.Range(0, 9).Where(i => house[i] > 2).DefaultIfEmpty().Min(i => Math.Abs(i - x))).ToList();
            tileForGun8 = gun8.Count == 0 ? -1 : gun8.Last();
            gun10 = gun10.OrderBy(x => Enumerable.Range(0, 9).Where(i => house[i] > 2).DefaultIfEmpty().Min(i => Math.Abs(i - x))).ToList();
            tileForGun10 = gun10.Count == 0 ? -1 : gun10.Last();
            gun12 = gun12.OrderBy(x => Enumerable.Range(0, 9).Where(i => house[i] > 2).DefaultIfEmpty().Min(i => Math.Abs(i - x))).ToList();
            tileForGun12 = gun12.Count == 0 ? -1 : gun12.Last();
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

            var game = new Misha3();
            game.Solve();
            
            reader.Close();
            writer.Close();

            return game.GetType().Name;
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