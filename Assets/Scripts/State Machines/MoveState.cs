using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IState
{
    public Node movementLocation = null;

    Unit unit;

    Dictionary<Node, float> distances;
    Dictionary<Node, Node> previous;
    bool countDisplayed = false;

    public MoveState(Unit unit)
    {
        this.unit = unit;
    }

    public void Enter()
    {
        PathfindingGenerators.GeneratePaths(unit.currentPosition, MapGenerator.graph, out distances, out previous);

        List<Node> possibleMovements = new List<Node>();

        foreach (var pair in distances)
        {
            if (pair.Value <= unit.currentActionPoints)
            {
                possibleMovements.Add(pair.Key);
            }
        }

        // TODO: Instantiate movement orb prefabs based on possibleMovements list locations
    }

    public void Execute()
    {
        // TODO: Add execute code
        // check for movement location change

        if (!countDisplayed)
        {
            Debug.Log(distances.Count);
            Debug.Log(unit.currentPosition.terrainType);
            countDisplayed = true;
        }

        if (movementLocation != null)
        {
            Node current = movementLocation;
            List<Node> path = new List<Node>();

            while (current != null)
            {
                if (current != unit.currentPosition)
                {
                    path.Add(current);
                    current = previous[current];
                }
                else
                {
                    current = null;
                }
            }

            // TODO: Instantiate lines between orbs displaying path
            // Wait for verification
            // Tell unit to move to location

            //unit.finiteStateMachine.ChangeState(UnitStates.None);
        }
    }

    public void Exit()
    {
        distances = new Dictionary<Node, float>();
        previous = new Dictionary<Node, Node>();

    }
}
