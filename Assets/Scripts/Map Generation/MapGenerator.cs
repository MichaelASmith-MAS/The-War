using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenerationType { SingleChunk, MultiChunk, Infinite }
public enum TerrainType { Water, Sand, Grass, Rock }

public class MapGenerator : MonoBehaviour
{
    public GenerationType generationType;

    public NoiseSettings noiseSettings;
    public HeightMapSettings heightMapSettings;
    public MeshSettings meshSettings;
    public TextureSettings textureSettings;
    public TerrainObjectSettings terrainObjectSettings;

    public Material gameMapMaterial;

    public int colliderLODIndex;
    public LODInfo[] detailLevels;

    public Transform viewer;

    public static int verticesPerLine;
    public static MeshData meshData;
    public static Node[,] graph;

    Vector2 viewerPosition;
    Vector2 viewerPositionOld;

    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    float meshWorldSize;
    int chunksVisibleInViewDistance;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

    TerrainType[,] terrainTypes;

    void Awake()
    {
        noiseSettings.seed = Random.Range(int.MinValue, int.MaxValue); //Randomize seed

        if (generationType == GenerationType.SingleChunk)
        {
            GenerateSingleTerrain();

        }
        else
        {
            GenerateStartTerrain();

        }

    }

    void Update()
    {
        if (generationType == GenerationType.Infinite)
        {
            viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

            if (viewerPosition != viewerPositionOld)
            {
                foreach (TerrainChunk chunk in visibleTerrainChunks)
                {
                    chunk.UpdateCollisionMesh();
                }
            }

            if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
            {
                viewerPositionOld = viewerPosition;
                UpdateVisibleChunks();
            }
        }
    }

    void GenerateSingleTerrain()
    {
        float[,] terrainNoiseMap;
        HeightMap heightMap;
        GameObject gameMap;
        MeshRenderer gameMapMeshRenderer;
        MeshFilter gameMapMeshFilter;
        MeshCollider gameMapMeshCollider;

        noiseSettings.offset.x = Random.Range(-100000, 100000); //Randomize map offset
        noiseSettings.offset.y = Random.Range(-100000, 100000); //Randomize map offset

        terrainNoiseMap = NoiseGenerator.GenerateNoiseMap(meshSettings.NumVertsPerLine, meshSettings.NumVertsPerLine, noiseSettings, Vector2.zero); //Generate the noise map itself

        heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.NumVertsPerLine, meshSettings.NumVertsPerLine, heightMapSettings, terrainNoiseMap); //Normalizes noise map

        terrainTypes = new TerrainType[meshSettings.NumVertsPerLine, meshSettings.NumVertsPerLine]; //Sets array of terrain types to be equal to the width and height of the map
        
        // run through every column and row in the map to determine the type of terrain at the location
        for (int y = 0; y < meshSettings.NumVertsPerLine; y++)
        {
            for (int x = 0; x < meshSettings.NumVertsPerLine; x++)
            {
                for (int i = textureSettings.layers.Length - 1; i >= 0; i--) //Start at the end of the list and work backwards for simplification
                {
                    if(heightMap.values[x,y] > textureSettings.layers[i].startHeight)
                    {
                        terrainTypes[x, y] = textureSettings.layers[i].terrainType;
                        break;
                    }
                }
            }
        }

        meshData = MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, 0, out verticesPerLine); //Generates the mesh information necessary to create a new mesh object 
        
        //Create the mesh and set up all necessary values
        gameMap = new GameObject("Game Map");
        gameMap.transform.parent = transform;
        gameMapMeshRenderer = gameMap.AddComponent<MeshRenderer>();
        gameMapMeshFilter = gameMap.AddComponent<MeshFilter>();
        gameMapMeshCollider = gameMap.AddComponent<MeshCollider>();

        gameMapMeshRenderer.sharedMaterial = gameMapMaterial;

        Mesh gameMapMesh = meshData.CreateMesh();

        gameMapMeshFilter.mesh = gameMapMesh;
        gameMapMeshCollider.sharedMesh = gameMapMesh;

        gameMap.tag = Tags.Ground;
        gameMap.layer = gameObject.layer;

        // Apply texture data to map material
        textureSettings.ApplyToMaterial(gameMapMaterial);
        textureSettings.UpdateMeshHeights(gameMapMaterial, heightMapSettings.MinHeight, heightMapSettings.MaxHeight);

        try
        {
            terrainObjectSettings.noiseSettings.seed = Random.Range(int.MaxValue, int.MinValue);
            terrainObjectSettings.noiseSettings.offset = new Vector2(Random.Range(-100000, 100000), Random.Range(-100000, 100000));

            float[,] objectPlacementNoise = NoiseGenerator.GenerateNoiseMap(meshSettings.NumVertsPerLine, meshSettings.NumVertsPerLine, terrainObjectSettings.noiseSettings, Vector2.zero);

            TerrainObjects[,] terrainPlacementObjects = TerrainObjectGenerator.GenerateObjectPlacement(terrainObjectSettings, heightMap, heightMapSettings, objectPlacementNoise);

            int objectCount = 0;

            for (int y = 0; y < verticesPerLine; y++)
            {
                for (int x = 0; x < verticesPerLine; x++)
                {
                    if (terrainPlacementObjects[x, y] != null)
                    {
                        objectCount++;
                        GameObject tempObject = Instantiate(terrainPlacementObjects[x, y].objectToPlace, meshData.Vertices[y * verticesPerLine + x], Quaternion.identity, gameMap.transform);
                        if (terrainPlacementObjects[x, y].randomizeRotation)
                        {
                            float xAngle = terrainPlacementObjects[x, y].angleToWorld ? meshData.Normals[y * verticesPerLine + x].x : 0;
                            float zAngle = terrainPlacementObjects[x, y].angleToWorld ? meshData.Normals[y * verticesPerLine + x].z : 0;
                            tempObject.transform.rotation = Quaternion.Euler(xAngle, Random.Range(0, 360), zAngle);
                        }
                        if (terrainPlacementObjects[x, y].randomizeSize)
                        {
                            float scale = Random.Range(terrainPlacementObjects[x, y].minimumSize, terrainPlacementObjects[x, y].maximumSize);
                            tempObject.transform.localScale = new Vector3(scale, scale, scale);
                        }
                    }
                }
            }
        }
        catch
        { }

        graph = PathfindingGenerators.GenerateGraph(verticesPerLine, terrainTypes);
    }

    void GenerateStartTerrain()
    {
        

        Debug.Log("Infinite terrain not yet implemented.");

        meshWorldSize = meshSettings.MeshWorldSize - 1;

        float maxViewDist = detailLevels[detailLevels.Length - 1].visibleDistanceThreshold;
        chunksVisibleInViewDistance = Mathf.RoundToInt(maxViewDist / meshWorldSize);

        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

        textureSettings.ApplyToMaterial(gameMapMaterial);
        textureSettings.UpdateMeshHeights(gameMapMaterial, heightMapSettings.MinHeight, heightMapSettings.MaxHeight);

        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {
        HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();

        for (int i = visibleTerrainChunks.Count - 1; i >= 0; i--)
        {
            alreadyUpdatedChunkCoords.Add(visibleTerrainChunks[i].coord);
            visibleTerrainChunks[i].UpdateTerrainChunk();

        }

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / meshWorldSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / meshWorldSize);

        for (int yOffset = -chunksVisibleInViewDistance; yOffset <= chunksVisibleInViewDistance; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDistance; xOffset <= chunksVisibleInViewDistance; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoord))
                {
                    if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                    {
                        terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();

                    }
                    //else
                    //{
                    //    TerrainChunk newChunk = new TerrainChunk(viewedChunkCoord, heightMapSettings, meshSettings, detailLevels, colliderLODIndex, transform, viewer, mapMaterial);
                    //    terrainChunkDictionary.Add(viewedChunkCoord, newChunk);

                    //    newChunk.OnVisibilityChanged += OnTerrainChunkVisibilityChanged;
                    //    newChunk.Load();
                    //}
                }
            }
        }

    }

    void OnTerrainChunkVisibilityChanged(TerrainChunk chunk, bool isVisible)
    {
        if (isVisible)
        {
            visibleTerrainChunks.Add(chunk);
        }
        else
        {
            visibleTerrainChunks.Remove(chunk);
        }
    }
}


[System.Serializable]
public struct LODInfo
{
    [Range(0, MeshSettings.numSupportedLODs - 1)]
    public int lod;
    public float visibleDistanceThreshold;

    public float SqrVisibleDistanceThreshold { get { return visibleDistanceThreshold * visibleDistanceThreshold; } }
}