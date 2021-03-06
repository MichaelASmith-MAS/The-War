﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Terrain/Mesh Settings")]
public class MeshSettings : UpdatableSettings
{
    public float meshScale = 1f;
    public bool useFlatShading;

    public const int numSupportedLODs = 5;
    public const int numSupportedChunkSizes = 9;
    public const int numSupportedFlatshadedChunkSizes = 3;

    public static readonly int[] supportedChunkSizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };
    public static readonly int[] supportedFlatshadedChunkSizes = { 48, 72, 96 };

    [Range(0, numSupportedChunkSizes - 1)]
    public int chunkSizeIndex;

    [Range(0, numSupportedFlatshadedChunkSizes - 1)]
    public int flatshadedChunkSizeIndex;

    // Number of vertices per line of mesh rendered at LOD 0; Includes 2 extra verts that are excluded from mesh, but used for calculating normals
    public int NumVertsPerLine { get { return supportedChunkSizes[useFlatShading ? flatshadedChunkSizeIndex : chunkSizeIndex] + 1; } }

    public float MeshWorldSize { get { return (NumVertsPerLine - 3) * meshScale; } }

}
