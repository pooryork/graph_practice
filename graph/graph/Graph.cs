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
        public bool IsWeighted { get; set; }

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
                                vertex_info_arr = vertex_info_arr[1].Split(" ");
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
                       
        //переопределение метода ToString
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

            List<string> list = this.GetListVertex();

            foreach (KeyValuePair<string, Dictionary<string, double?>> item_vertex in graph)
            {
                if (item_vertex.Value.Count == 1)
                {
                    Dictionary<string, double?> item = item_vertex.Value;
                    if (item.TryGetValue("null", out double? key))
                    {
                            bool noEdges = true;
                            foreach (KeyValuePair<string, Dictionary<string, double?>> sub_item_vertex in graph)
                            {                                
                                foreach (KeyValuePair<string, double?> sub_item_edge in sub_item_vertex.Value)
                                {
                                    if (sub_item_edge.Key == item_vertex.Key)
                                    {
                                    noEdges = false;
                                    }
                                }                    
                            }
                            if (noEdges)
                            {
                                isolatedVertexes.Add(item_vertex.Key);
                            }
                                                
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

        //7. Вывести список смежности подграфа данного графа, полученного удалением вершин с чётными номерами.
        public void Ib7()
        {
            List<string> vertexesToDelete = new List<string>();
            //удаляем вершину из списка вершин
            foreach (KeyValuePair<string, Dictionary<string, double?>> item_vertex in graph)
            {
                //выясняем какие вершины нужно удалить
                bool success = Int32.TryParse(item_vertex.Key, out int number);
                if (success && number % 2 == 0)
                {
                    vertexesToDelete.Add(item_vertex.Key);
                }
                else if (!success)
                {
                    throw new Exception("Некорректная операция! Название вершины не является числом!");
                }
            }

            //выясняем какие вершины нужно удалить
            foreach (var i in vertexesToDelete)
            {
                //удалить вершину из списка вершин
                graph.Remove(i);

            }

            Graph tmp_graph = new Graph();
            tmp_graph.IsWeighted = IsWeighted;
            //удалить вершину из списка вершин
            foreach (KeyValuePair<string, Dictionary<string, double?>> item_vertex in graph)
            {
                Dictionary<string, double?> new_vertex = new Dictionary<string, double?>();
                foreach (KeyValuePair<string, double?> item_edge in item_vertex.Value)
                {
                    if (vertexesToDelete.IndexOf(item_edge.Key) == -1)
                    {
                        new_vertex.Add(item_edge.Key, item_edge.Value);
                    }                    
                }
                if (new_vertex.Count > 0)
                {
                    tmp_graph.Add(item_vertex.Key, new_vertex);
                }
                else
                {
                    new_vertex.Add("null", null);
                    tmp_graph.Add(item_vertex.Key, new_vertex);
                }

            }
            Console.WriteLine(tmp_graph.ToString());
        }

        private void Add(string key, Dictionary<string, double?> new_vertex)
        {
            graph.Add(key, new_vertex);
        }

        //16. Найти сильно связные компоненты орграфа.
        public int II1()
        {
            return -1;
        }

        public List<string> GetListVertex()
        {
            return new List<string>(graph.Keys);
        }

        public Dictionary<string, double?> GetListWeightedEdges(string vertex)
        {
            try
            {
                return graph[vertex];
            }
            catch
            {
                throw new Exception("Указанная вершина отсутсвует.");
            }
        }

        public List<string> GetListEdges(string vertex)
        {
            try
            {
                return new List<string>(graph[vertex].Keys);
            }
            catch
            {
                throw new Exception("Указанная вершина " + vertex + " отсутсвует.");
            }
        }

        public List<string> DFS(string vertex)
        {
            Stack<string> NoProcessedVertex = new Stack<string>();
            List<string> ProcessedVertex = new List<string>();

            if (graph.ContainsKey(vertex))
            {
                ProcessedVertex.Add(vertex);
                foreach (string item in GetListEdges(vertex))
                {
                    NoProcessedVertex.Push(item);
                }

                while (NoProcessedVertex.Count > 0)
                {
                    string temp_vertex = NoProcessedVertex.Pop();
                    if (temp_vertex != "null")
                    {
                        if (ProcessedVertex.Contains(temp_vertex))
                        {
                            continue;
                        }
                        ProcessedVertex.Add(temp_vertex);
                        foreach (string item in GetListEdges(temp_vertex))
                        {
                            NoProcessedVertex.Push(item);
                        }
                    }
                }

                return ProcessedVertex;
            }
            else
            {
                throw new Exception("Указанная вершина отсутсвует в графе.");
            }
        }

        public Dictionary<string, List<string>> stronglyConnectedWithDFS()
        {
            Dictionary<string, List<string>> dfs_mas = new Dictionary<string, List<string>>();

            foreach (KeyValuePair<string, Dictionary<string, double?>> item_vertex in graph)
            {
                dfs_mas.Add(item_vertex.Key, this.DFS(item_vertex.Key));
            }

            Dictionary<string, List<string>> stronglyConnected = new Dictionary<string, List<string>>();

            foreach (KeyValuePair<string, List<string>> i in dfs_mas)
            {
                List<string> tmp = new List<string>();
                foreach (var j in i.Value)
                {                    
                    if (i.Key != j)
                    {
                        //Console.WriteLine(i.Key + " - " + j);
                        //KeyValuePair<string, string> connecetedVertexes = new KeyValuePair<string, string>();
                        //connecetedVertexes.Key = i.Key;

                        tmp.Add(j);                        
                    }
                }
                stronglyConnected.Add(i.Key, tmp);
            }

            return stronglyConnected;

        }

        //30. Вывести длины кратчайших путей от всех вершин до u.
        public Dictionary<string, double?> Dijkstra(string cur_vertex)
        {
            Dictionary<string, double?> distance = new Dictionary<string, double?>();
            Dictionary<string, string> path = new Dictionary<string, string>();

            foreach (string vertex in GetListVertex())
            {
                if (vertex == cur_vertex)
                {
                    distance.Add(vertex, 0);
                }
                else
                {
                    distance.Add(vertex, double.MaxValue);
                }
            }

            List<string> not_visited_vertex = GetListVertex();

            while (not_visited_vertex.Count > 0)
            {
                var min_value = distance.Where(elem => not_visited_vertex.Contains(elem.Key)).Min(t => t.Value);
                var min_vertex_distance = distance.Where(elem => not_visited_vertex.Contains(elem.Key)).Where(t => t.Value == min_value).ToList();

                not_visited_vertex.Remove(min_vertex_distance[0].Key);

                foreach (KeyValuePair<string, double?> edge in GetListWeightedEdges(min_vertex_distance[0].Key))
                {
                    //Console.WriteLine("edge: " + edge.ToString());
                    if (edge.Key != "null")
                    {
                        if (distance[edge.Key] > distance[min_vertex_distance[0].Key] + edge.Value)
                        {
                            distance[edge.Key] = distance[min_vertex_distance[0].Key] + edge.Value;
                            //Console.WriteLine("edge: " + edge.Value);
                            if (path.ContainsKey(edge.Key))
                            {
                                path[edge.Key] = min_vertex_distance[0].Key;
                            }
                            else
                            {
                                path.Add(edge.Key, min_vertex_distance[0].Key);
                            }
                        }
                    }
                }
            }
            
            return distance;
        }
    }
}
