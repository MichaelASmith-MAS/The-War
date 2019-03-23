using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainObjectGenerator
{
    public static TerrainObjects[,] GenerateObjectPlacement(TerrainObjectSettings terrainObjectSettings, HeightMap heightMap, HeightMapSettings heightMapSettings, float[,] objectPlacementNoise)
    {
        TerrainObjects[,] gameObjects = new TerrainObjects[heightMap.values.GetLength(0), heightMap.values.GetLength(0)];

        for (int y = 0; y < heightMap.values.GetLength(0); y++)
        {
            for (int x = 0; x < heightMap.values.GetLength(0); x++)
            {
                for (int i = 0; i < terrainObjectSettings.terrainObjects.Length; i++)
                {
                    if(heightMap.values[x,y] >= terrainObjectSettings.terrainObjects[i].minimumHeight * heightMapSettings.heightMultiplier && heightMap.values[x,y] <= terrainObjectSettings.terrainObjects[i].maximumHeight * heightMapSettings.heightMultiplier)
                    {
                        if (terrainObjectSettings.terrainObjects[i].usePerlinNoise)
                        {
                            if (objectPlacementNoise[x, y] >= terrainObjectSettings.terrainObjects[i].minimumNoiseLevel && objectPlacementNoise[x, y] <= terrainObjectSettings.terrainObjects[i].maximumNoiseLevel)
                            {
                                int randomCheck = Random.Range(0, 100);

                                if (randomCheck < terrainObjectSettings.terrainObjects[i].placementLikelihood)
                                {
                                    gameObjects[x, y] = terrainObjectSettings.terrainObjects[i];
                                }
                            }
                        }
                        else
                        {
                            int randomCheck = Random.Range(0, 100);

                            if (randomCheck < terrainObjectSettings.terrainObjects[i].placementLikelihood)
                            {
                                gameObjects[x, y] = terrainObjectSettings.terrainObjects[i];
                            }
                        }
                    }
                }
            }
        }

        return gameObjects;
    }
}
