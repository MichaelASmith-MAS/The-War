using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitStates { None, Move }

public class Unit : MonoBehaviour
{
    public FiniteStateMachine<UnitStates> finiteStateMachine;
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
        finiteStateMachine.RunState();
    }

    public void Move()
    {
        

    }
}
