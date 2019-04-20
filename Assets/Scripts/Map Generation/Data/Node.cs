using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int xLoc, yLoc;
    public List<Node> neighbors;
    public TerrainType terrainType;

    public Node()
    {
        xLoc = 0;
        yLoc = 0;
        neighbors = new List<Node>();
        terrainType = TerrainType.Grass;
    }

    public Node(int xLoc, int yLoc, List<Node> neighbors, TerrainType terrainType)
    {
        this.xLoc = xLoc;
        this.yLoc = yLoc;
        this.neighbors = neighbors;
        this.terrainType = terrainType;
    }

}
