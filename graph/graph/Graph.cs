using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Graph
{
    public class Graph
    {
        //Структура:
        //1) Вершина (string - имя вершины)
        //2) Словарь (Вершина/Вес)
        //2.1) Имя вершины к которой существует путь
        //2.2) Вес если он есть иначе использовать null!
        private Dictionary<string, Dictionary<string, double?>> graph = new Dictionary<string, Dictionary<string, double?>>();
        //Свойство указывающее взвешенный ли граф
        public bool IsWeighted { get; }

        //Конструктор по умолчанию инициализирует пустой граф
        public Graph() { }
        //Конструктор читающий данные из файла
        public Graph(string FileName)
        {
            //Проверяем существует ли файл
            if (File.Exists(FileName))
            {
                using (StreamReader file_info = new StreamReader(FileName))
                {
                    string type_graph = file_info.ReadLine();

                    //Если граф взвешенный
                    if (type_graph.ToString() == "Weighted")
                    {
                        //Устанавливаем что граф взвешенный
                        this.IsWeighted = true;
                        //счётчик номера вершины
                        int counter = 1;
                        while (!file_info.EndOfStream)
                        {
                            //Вся информация о вершине в строке
                            string info = file_info.ReadLine();
                            //Информация разбитая на массив
                            string[] vertex_info_arr = info.Split(" - ");
                            string vertex_num = vertex_info_arr[0];
                            //если из вершины выходит хотя бы одна дуга
                            if (vertex_info_arr.Length == 2)
                            {
                                vertex_info_arr = vertex_info_arr[1].Split(" ");
                                //Словарь для добавления дуг и веса
                                Dictionary<string, double?> vertex_info_pairs = new Dictionary<string, double?>();
                                //Добавляем все дуги
                                for (int i = 0; i < vertex_info_arr.Length; i++)
                                {
                                    string[] temp = vertex_info_arr[i].Split(':');
                                    try
                                    {
                                        vertex_info_pairs.Add(temp[0], double.Parse(temp[1]));
                                    }
                                    catch
                                    {
                                        throw new Exception("Неверно указан вес ребра в файле");
                                    }

                                }
                                //Добвляем в граф
                                graph.Add(vertex_num, vertex_info_pairs);
                            }
                            else
                            {
                                Dictionary<string, double?> vertex_info_pairs = new Dictionary<string, double?>();
                                vertex_info_pairs.Add("null", null);
                                graph.Add(vertex_num, vertex_info_pairs);
                                //throw new Exception("Неверно указана вершина. Не нужно указывать вершины, из которых не выходит ни одна дуга");
                            }
                            counter++;

                        }
                    }
                    //Если граф не взвешенный
                    else if (type_graph.ToString() == "Not Weighted")
                    {
                        while (!file_info.EndOfStream)
                        {
                            //Вся информация о вершине в строке
                            string info = file_info.ReadLine();
                            //Информация разбитая на массив
                            string[] vertex_info_arr = info.Split(" - ");
                            string vertex_num = vertex_info_arr[0];
                            //если из вершины выходит хотя бы одна дуга
                            if (vertex_info_arr.Length == 2)
                            {
                                vertex_info_arr = vertex_info_arr[1].Split("  ");
                                //Словарь для добавления дуг и веса
                                Dictionary<string, double?> vertex_info_pairs = new Dictionary<string, double?>();
                                //Добавляем все дуги
                                for (int i = 0; i < vertex_info_arr.Length; i++)
                                {
                                    vertex_info_pairs.Add(vertex_info_arr[i], null);
                                }
                                //Добвляем в граф
                                graph.Add(vertex_num, vertex_info_pairs);
                            }
                            else
                            {
                                Dictionary<string, double?> vertex_info_pairs = new Dictionary<string, double?>();
                                vertex_info_pairs.Add("null", null);
                                graph.Add(vertex_num, vertex_info_pairs);
                                //throw new Exception("Неверно указана вершина. Не нужно указывать вершины, из которых не выходит ни одна дуга");
                            }
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Файл с таким именем не найден.");
            }
        }
               
        public override string ToString()
        {
            StringBuilder info_graph = new StringBuilder();
            //Информация о том взвешенный ли граф
            switch (IsWeighted)
            {
                case true:
                    info_graph.Append("Weighted");
                    foreach (KeyValuePair<string, Dictionary<string, double?>> item_vertex in graph)
                    {   
                        //Добавляем вершину
                        info_graph.Append(Environment.NewLine + item_vertex.Key + " - ");
                        //Добавлем ребра
                        foreach (KeyValuePair<string, double?> item_edge in item_vertex.Value)
                        {
                            if (item_edge.Key == "null" && item_edge.Value == null) {
                                info_graph.Append("no edges ");
                            }
                            else {
                                info_graph.Append(item_edge.Key + ":" + item_edge.Value + " ");
                            }                            
                        }
                    }

                    break;
                case false:
                    info_graph.Append("Not Weighted");
                    foreach (KeyValuePair<string, Dictionary<string, double?>> item_vertex in graph)
                    {
                        //Добавляем вершину
                        info_graph.Append(Environment.NewLine + item_vertex.Key + " - ");
                        //Добавлем ребра
                        foreach (KeyValuePair<string, double?> item_edge in item_vertex.Value)
                        {
                            if (item_edge.Key == "null")
                            {
                                info_graph.Append("no edges ");
                            }
                            else
                            {
                                info_graph.Append(item_edge.Key + " ");
                            }                            
                        }
                    }
                    break;
            }

            return info_graph.ToString();
        }


        //6. Вывести все изолированные вершины орграфа (степени 0).
        public List<string> Ia6()
        {
            List<string> isolatedVertexes = new List<string>();

            foreach (KeyValuePair<string, Dictionary<string, double?>> item_vertex in graph)
            {
                //Добавляем вершину
                //info_graph.Append(Environment.NewLine + item_vertex.Key + " - ");
                //Добавлем ребра
                foreach (KeyValuePair<string, double?> item_edge in item_vertex.Value)
                {
                    if (item_edge.Key == "null" && item_edge.Value == null)
                    {
                        isolatedVertexes.Add(item_vertex.Key);
                    }
                }
            }

            return isolatedVertexes;
        }

        //7. Вывести на экран те вершины, полустепень исхода которых больше, чем у заданной вершины.
        public List<string> Ia7(string vertexName)
        {
            List<string> vertexes = new List<string>();

            //KeyValuePair<string, Dictionary<string, double?>> vertex = new KeyValuePair<string, Dictionary<string, double?>>();
            int vertexNameOutcomeDegree = 0;
            //считаем степеь исхода заданной вершины
            foreach (KeyValuePair<string, Dictionary<string, double?>> item_vertex in graph)
            {
                if (item_vertex.Key == vertexName)
                {
                    foreach (KeyValuePair<string, double?> item_edge in item_vertex.Value)
                    {
                        if (item_edge.Key != "null")
                        {
                            vertexNameOutcomeDegree++;
                        }
                    }
                    Console.WriteLine("Степень исхода вершины " + vertexName + " - " + vertexNameOutcomeDegree);
                }
            }

            //проходим по всем остальным вершинам графа и считаем степень исхода
            foreach (KeyValuePair<string, Dictionary<string, double?>> item_vertex in graph)
            {
                int curVertexOutcomeDegree = 0;
                if (item_vertex.Key != vertexName)
                {
                    foreach (KeyValuePair<string, double?> item_edge in item_vertex.Value)
                    {
                        if (item_edge.Key != "null")
                        {
                            curVertexOutcomeDegree++;
                        }
                    }
                    if (curVertexOutcomeDegree > vertexNameOutcomeDegree)
                    {
                        vertexes.Add(item_vertex.Key);
                    }
                    //Console.WriteLine("Степень исхода вершины " + vertexName + " - " + vertexNameOutcomeDegree);
                }
            }

            return vertexes;
        }

    }
}
