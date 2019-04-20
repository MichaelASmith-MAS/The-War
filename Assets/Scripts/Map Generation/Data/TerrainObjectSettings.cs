using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Terrain/Object Placement Settings")]
public class TerrainObjectSettings : UpdatableSettings
{
    [Tooltip("Objects, such as grass and trees, to place in the world. The first object in the list will be replaced by the second if both are calculated to the same location.")]
    public TerrainObjects[] terrainObjects;

    public NoiseSettings noiseSettings;

}

[System.Serializable]
public class TerrainObjects
{
    public GameObject objectToPlace;

    [Range(0,100), Tooltip("Percentage chance an object will be placed at a calculated position. Higher values denote higher likelihood.")]
    public int placementLikelihood;

    [Range(0, 1)]
    public float minimumHeight = 0;

    [Range(0, 1)]
    public float maximumHeight = 1;

    public bool randomizeRotation;
    public bool randomizeSize;

    [Range(.5f, 2f)]
    public float minimumSize;

    [Range(.5f, 2f)]
    public float maximumSize;

    public bool angleToWorld;

    [Tooltip("Checking this box will use perlin noise to map the object location. If unchecked, random noise is used instead.")]
    public bool usePerlinNoise;

    [Range(0, 1), Tooltip("The minimum value from the generated noise that the objects will spawn at.")]
    public float minimumNoiseLevel;

    [Range(0, 1), Tooltip("The maximum value from the generated noise that the objects will spawn at.")]
    public float maximumNoiseLevel;

}