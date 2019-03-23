using UnityEngine;

public class MapPreview : MonoBehaviour
{
    public GameObject previewMesh;
    
    public NoiseSettings noiseSettings;
    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureSettings textureSettings;

    public Material terrainMaterial;

    [Range(0, MeshSettings.numSupportedLODs - 1)]
    public int editorPreviewLevelOfDetail;

    public bool autoUpdate;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    public void DrawMesh(MeshData meshData)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
    }

    public void DrawMapInEditor()
    {
        meshFilter = previewMesh.GetComponent<MeshFilter>();
        meshRenderer = previewMesh.GetComponent<MeshRenderer>();

        textureSettings.ApplyToMaterial(terrainMaterial);
        textureSettings.UpdateMeshHeights(terrainMaterial, heightMapSettings.MinHeight, heightMapSettings.MaxHeight);

        float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(meshSettings.NumVertsPerLine, meshSettings.NumVertsPerLine, noiseSettings, Vector2.zero);
        HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.NumVertsPerLine, meshSettings.NumVertsPerLine, heightMapSettings, noiseMap);
        int vertsPerLine;
        DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, editorPreviewLevelOfDetail, out vertsPerLine));
    }

    private void OnValidate()
    {
        if (noiseSettings != null)
        {
            noiseSettings.OnValuesUpdated -= OnValuesUpdated;
            noiseSettings.OnValuesUpdated += OnValuesUpdated;
        }
        if (meshSettings != null)
        {
            meshSettings.OnValuesUpdated -= OnValuesUpdated;
            meshSettings.OnValuesUpdated += OnValuesUpdated;
        }
        if (heightMapSettings != null)
        {
            heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
            heightMapSettings.OnValuesUpdated += OnValuesUpdated;
        }
        if (textureSettings != null)
        {
            textureSettings.OnValuesUpdated -= OnTextureValuesUpdated;
            textureSettings.OnValuesUpdated += OnTextureValuesUpdated;
        }
    }

    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    void OnTextureValuesUpdated()
    {
        textureSettings.ApplyToMaterial(terrainMaterial);
    }

}
