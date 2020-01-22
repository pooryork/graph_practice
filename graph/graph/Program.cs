using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            #region constructors
            Graph graph = new Graph("test.txt");
            Graph graph1 = new Graph("test1.txt");
            Graph graph2 = new Graph("test2.txt");

            Console.WriteLine(graph.ToString());
            Console.WriteLine();
            /*Console.WriteLine(graph1.ToString());
            Console.WriteLine();
            Console.WriteLine(graph2.ToString());
            Console.WriteLine();*/
            #endregion

            /*#region task Ia-6
            Console.WriteLine("Ia-6. Вывести все изолированные вершины орграфа (степени 0).");
            List<string> isolatedVertexes1 = graph.Ia6();
            Console.Write("1: ");
            if (isolatedVertexes1.Count > 0)
            {
                foreach (var i in isolatedVertexes1)
                {
                    Console.Write(i + " ");
                }
            }
            else
            {
                Console.Write("no isolated vertexes");
            }
            Console.WriteLine();
            List<string> isolatedVertexes3 = graph2.Ia6();
            Console.Write("3: ");
            if (isolatedVertexes3.Count > 0)
            {
                foreach (var i in isolatedVertexes3)
                {
                    Console.Write(i + " ");
                }

            }
            else
            {
                Console.Write("no isolated vertexes");
            }
            Console.WriteLine();
            Console.WriteLine();

            #endregion

            #region task Ia-7
            Console.WriteLine("Ia-7. Вывести на экран те вершины, полустепень исхода которых больше, чем у заданной вершины.");
            List<string> vertexes = graph2.Ia7("Rostov");
            Console.Write("3: ");
            if (vertexes.Count > 0)
            {
                foreach (var i in vertexes)
                {
                    Console.Write(i + " ");
                }
            }
            else
            {
                Console.Write(" such vertexes do not exist");
            }
            Console.WriteLine();
            Console.WriteLine();
            #endregion

            #region task Iб-7
            Console.WriteLine("Iб-7. Вывести список смежности подграфа данного графа, полученного удалением вершин с чётными номерами.");
            Console.WriteLine("1: ");
            graph.Ib7();
            Console.WriteLine("2: ");
            graph = new Graph("test.txt");
            graph1 = new Graph("test1.txt");
            graph1.Ib7();
            Console.WriteLine();
            //Console.WriteLine(graph.ToString());
            #endregion*/

            #region Обход 1 (DFS)
            Console.WriteLine("16. Найти сильно связные компоненты орграфа (DFS)");
            /*graph.stronglyConnectedWithDFS();            
            Console.WriteLine();*/
            #endregion

            #region Алгоритм Дейкстры
            /*Console.WriteLine("Вывести длины кратчайших путей от всех вершин до u (Алгоритм Дейкстры)");
            Dictionary<string, double?> graph_ways = graph.Dijkstra("4");
            Console.WriteLine("Из вершины 4: ");
            foreach (KeyValuePair<string, double?> i in graph_ways)
            {
                Console.WriteLine(i.ToString());
            }
            Console.WriteLine();*/
            #endregion

            #region Прим
            Console.WriteLine("Дан взвешенный неориентированный граф из N вершин и M ребер.");
            Console.WriteLine("Требуется найти в нем каркас минимального веса. Алгоритм Прима");
            Graph connected_oriented = new Graph("test.txt");
            //Console.WriteLine(connected_oriented.ToString());
            Console.WriteLine(connected_oriented.Prim());
            Console.WriteLine();
            #endregion

            #region IVb
            Console.WriteLine("Найти радиус графа — минимальный из эксцентриситетов его вершин.");
            Graph g = new Graph("test.txt");
            List<double?> max_distance = new List<double?>();
            foreach (var vertex in g.GetListVertex())
            {
                max_distance.Add(g.BellmanFord(vertex).
                    Where(elem => elem.Value != double.MaxValue).
                    Max(elem => elem.Value));
                //Console.WriteLine(vertex);
                /*foreach (KeyValuePair<string, double?> item in g.BellmanFord(vertex))
                {
                    Console.WriteLine(item);
                }*/
            }
            double? radius = max_distance.Min();
            Console.WriteLine("Радиус: " + radius);
            Console.WriteLine();
            #endregion

            #region IVc
            Console.WriteLine("Определить, есть ли в графе вершина, каждая из минимальных стоимостей пути от которой до остальных не превосходит N.");
            Console.Write("N = ");
            int n = int.Parse(Console.ReadLine());

            foreach (var vertex in g.GetListVertex())
            {
                Dictionary<string, double?> dist = g.BellmanFord(vertex);

                var max_dist = dist.Values.Max();

                if (max_dist <= n)
                {
                    Console.Write(vertex + " ");
                }
            }
            //Console.WriteLine();
            Console.WriteLine();
            #endregion
        }
    }
}
