using System;
using System.Collections.Generic;
using System.Numerics;

//REFERÊNCIA CÓDIGO
//https://github.com/mburst/dijkstras-algorithm/blob/master/dijkstras.cs

namespace Dijkstra
{
    class Graph
    {
        Dictionary<UnityEngine.Vector3, Dictionary<UnityEngine.Vector3, int>> vertices = new Dictionary<UnityEngine.Vector3, Dictionary<UnityEngine.Vector3, int>>();

        public void add_vertex(UnityEngine.Vector3 name, Dictionary<UnityEngine.Vector3, int> edges)
        {
            vertices[name] = edges;
        }

        public List<UnityEngine.Vector3> shortest_path(UnityEngine.Vector3 start, UnityEngine.Vector3 finish)
        {
            var previous = new Dictionary<UnityEngine.Vector3, UnityEngine.Vector3>();
            var distances = new Dictionary<UnityEngine.Vector3, int>();
            var nodes = new List<UnityEngine.Vector3>();

            List<UnityEngine.Vector3> path = null;

            foreach (var vertex in vertices)
            {
                if (vertex.Key == start)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == finish)
                {
                    path = new List<UnityEngine.Vector3>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            return path;
        }
    }

    class MainClass
    {
        static Graph g = new Graph();

        public static void AddDraph(UnityEngine.Vector3[,] posicao, int[,] peso, int distance)
        {
            for (int x = 1; x < distance - 1; x++)
            {
                for (int y = 1; y < distance - 1; y++)
                {
                    g.add_vertex(posicao[x, y], new Dictionary<UnityEngine.Vector3, int>() { { posicao[x + 1, y], peso[x + 1,y] },
                                                                                            { posicao[x - 1, y], peso[x - 1, y] },
                                                                                            { posicao[x , y + 1], peso[x, y + 1] },
                                                                                            { posicao[x, y - 1], peso[x, y - 1] } });
                }
            }
        }

        public static List<UnityEngine.Vector3> GetPath(UnityEngine.Vector3 start, UnityEngine.Vector3 finish)
        {
            return g.shortest_path(start, finish);
        }
    }
}