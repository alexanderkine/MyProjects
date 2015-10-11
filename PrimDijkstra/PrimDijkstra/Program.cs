using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PrimDijkstra
{
    class Program
    {
        static void Main(string[] args)
        {
            var tops = Reader();
            var matrix = ToMatrix(tops);
            var ostov = Algoritm(matrix, tops);
            for (int i = 0; i < ostov.Graph.Count; i++)
            {
                ostov.Graph[i] = new LinkedList<int>(ostov.Graph[i].OrderBy(x => x));
                ostov.Graph[i].AddFirst(i + 1);
                ostov.Graph[i].AddLast(0);
            }
            WriteOutput(ostov);
            var haha = "";
        }

        static List<Top> Reader()
        {
            var tops = new List<Top>();
            using (var reader = new StreamReader("in.txt"))
            {
                var input = reader.ReadToEnd().Split('\n');
                for (var i = 0; i < int.Parse(input[0]); i++)
                {
                    var haha = input[i + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
                    tops.Add(new Top(haha[0], haha[1], i));
                }
            }
            return tops;
        }

        static int GetDistance(Top u, Top v)
        {
            return Math.Abs(u.X - v.X) + Math.Abs(u.Y - v.Y);
        }

        static List<List<int>> ToMatrix(List<Top> tops)
        {
            var matrix = new List<List<int>>();
            for (int i = 0; i < tops.Count; i++)
                matrix.Add(new List<int>());
            for (int i = 0; i < tops.Count; i++)
                for (int j = 0; j < tops.Count; j++)
                    if (j == i)
                        matrix[i].Add(0);
                    else
                        matrix[i].Add(GetDistance(tops[i], tops[j]));
            return matrix;
        }

        static Ostov Algoritm(List<List<int>> matrix, List<Top> tops)
        {
            var near = new Dictionary<Top, Top>();
            var w = new List<Top>(tops);
            var distance = new Dictionary<Top, int>();
            var top = tops[0];
            w.Remove(top);
            var ostov = new Ostov(Enumerable.Range(0, tops.Count).Select(x => new LinkedList<int>()).ToList(), 0);
            for (int i = 0; i < tops.Count; i++)
            {
                near.Add(tops[i], top);
                distance.Add(tops[i], matrix[i][0]);
            }
            while (w.Count != 0)
            {
                var v = distance.Where(x => x.Value != 0 && w.Contains(x.Key)).OrderBy(x => x.Value).First();
                var u = near[v.Key];
                ostov.Graph[v.Key.Number].AddLast(u.Number + 1);
                ostov.Graph[u.Number].AddLast(v.Key.Number + 1);
                ostov.Weight += v.Value;
                w.Remove(v.Key);
                foreach (var versh in w)
                {
                    if (distance[versh] > matrix[versh.Number][v.Key.Number])
                    {
                        near[versh] = v.Key;
                        distance[versh] = matrix[versh.Number][v.Key.Number];
                    }
                }
            }
            return ostov;
        }

        static void WriteOutput(Ostov ostov)
        {
            using (var writer = new StreamWriter("out.txt"))
            {
                for (int i=0;i<ostov.Graph.Count;i++)
                {
                    for (int j = 0; j < ostov.Graph[i].Count; j++)
                    {
                        writer.Write(ostov.Graph[i].ElementAt(j));
                        writer.Write(' ');
                    }
                    writer.WriteLine();
                }
                writer.Write(ostov.Weight);
            }
        }
    }

    class Top
    {
        public int X;
        public int Y;
        public int Number;
        public Top(int x, int y, int number)
        {
            X = x;
            Y = y;
            Number = number;
        }
    }

    class Ostov
    {
        public List<LinkedList<int>> Graph;
        public int Weight;
        public Ostov(List<LinkedList<int>> graph, int weight)
        {
            Graph = graph;
            Weight = weight;
        }
    }  
}