using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathfindingGenerators
{
    public static Node[,] GenerateGraph(int verticesPerLine, TerrainType[,] terrainTypes)
    {
        Node[,] graph = new Node[verticesPerLine, verticesPerLine];

        for (int x = 0; x < verticesPerLine; x++)
        {
            for (int y = 0; y < verticesPerLine; y++)
            {
                graph[x, y] = new Node();
                graph[x, y].xLoc = x;
                graph[x, y].yLoc = y;
                graph[x, y].terrainType = terrainTypes[x, y];
            }
        }

        for (int x = 0; x < verticesPerLine; x++)
        {
            for (int y = 0; y < verticesPerLine; y++)
            {
                if (x > 0)
                {
                    graph[x, y].neighbors.Add(graph[x - 1, y]);
                }
                if (x < verticesPerLine - 1)
                {
                    graph[x, y].neighbors.Add(graph[x + 1, y]);
                }
                if (y > 0)
                {
                    graph[x, y].neighbors.Add(graph[x, y - 1]);
                }
                if (y < verticesPerLine - 1)
                {
                    graph[x, y].neighbors.Add(graph[x, y + 1]);
                }
                if (x > 0 && y > 0)
                {
                    graph[x, y].neighbors.Add(graph[x - 1, y - 1]);
                }
                if (x > 0 && y < verticesPerLine - 1)
                {
                    graph[x, y].neighbors.Add(graph[x - 1, y + 1]);
                }
                if (x < verticesPerLine - 1 && y > 0)
                {
                    graph[x, y].neighbors.Add(graph[x + 1, y - 1]);
                }
                if (x < verticesPerLine - 1 && y < verticesPerLine - 1)
                {
                    graph[x, y].neighbors.Add(graph[x + 1, y + 1]);
                }
            }
        }

        return graph;
    }

    public static void GeneratePaths(Node source, Node[,] graph, out Dictionary<Node, float> distance, out Dictionary<Node, Node> previous)
    {
        distance = new Dictionary<Node, float>();
        previous = new Dictionary<Node, Node>();

        List<Node> unvisted = new List<Node>();

        distance[source] = 0;
        previous[source] = null;

        foreach (Node v in graph)
        {
            if (v != source)
            {
                distance[v] = Mathf.Infinity;
                previous[v] = null;
            }

            unvisted.Add(v);
        }

        while (unvisted.Count > 0)
        {
            Node u = null;

            foreach (Node possible in unvisted)
            {
                if(u == null || distance[possible] < distance[u])
                {
                    u = possible;
                }
            }

            unvisted.Remove(u);

            foreach (Node v in u.neighbors)
            {
                float alt = distance[u] + CostToEnter(v);

                if (alt < distance[v])
                {
                    distance[v] = alt;
                    previous[v] = u;
                }
            }
        }
    }

    static float CostToEnter(Node node)
    {
        switch (node.terrainType)
        {
            case TerrainType.Water:
                return 5f;
            case TerrainType.Sand:
                return 2f;
            case TerrainType.Grass:
                return 1f;
            case TerrainType.Rock:
                return 10f;
            default:
                return 1f;
        }
    }
}
