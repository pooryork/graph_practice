﻿using System;
using System.Collections.Generic;

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
            Console.WriteLine(graph1.ToString());
            Console.WriteLine();
            Console.WriteLine(graph2.ToString());
            Console.WriteLine();
            #endregion

            #region task Ia-6
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
            List<string> isolatedVertexes2 = graph1.Ia6();
            Console.Write("2: ");
            if (isolatedVertexes2.Count > 0)
            {
                foreach (var i in isolatedVertexes2)
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
            #endregion

        }

    }
}
