using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int speedValue = 10;

    public Speed speed;
    public Node currentPosition;

    public int maxActionPoints, currentActionPoints;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = MapGenerator.meshData.Vertices[MapGenerator.meshData.Vertices.Length / 2];
        currentPosition = MapGenerator.graph[MapGenerator.verticesPerLine / 2, MapGenerator.verticesPerLine / 2];

        speed = new Speed(speedValue);

        maxActionPoints = Mathf.FloorToInt(speed.currentSpeed / 2);
        currentActionPoints = maxActionPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        PathfindingGenerators.GeneratePaths(currentPosition, MapGenerator.graph, out Dictionary<Node, float> distance, out Dictionary<Node, Node> previous);

    }
}
