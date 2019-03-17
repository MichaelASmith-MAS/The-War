using System;
using UnityEngine;

public static class HeightMapGenerator
{
    public static HeightMap GenerateHeightMap(int mapWidth, int mapHeight, HeightMapSettings settings, float[,] noiseMap)
    {
        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        for (int y = 0; y < mapWidth; y++)
        {
            for (int x = 0; x < mapHeight; x++)
            {
                noiseMap[x, y] *= settings.heightMultiplier;

                if(noiseMap[x, y] > maxValue)
                {
                    maxValue = noiseMap[x, y];
                }
                if (noiseMap[x, y] < minValue)
                {
                    minValue = noiseMap[x, y];
                }
            }
        }

        return new HeightMap(noiseMap, minValue, maxValue);

    }

}

public struct HeightMap
{
    public readonly float[,] values;
    public readonly float minValue;
    public readonly float maxValue;

    public HeightMap(float[,] values, float minValue, float maxValue)
    {
        this.values = values ?? throw new ArgumentNullException(nameof(values));
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
}