using UnityEngine;

[CreateAssetMenu(menuName = "Terrain/Noise Settings")]
public class NoiseSettings : UpdatableSettings
{
    public NormalizeMode normalizeMode;

    public int seed = 0;
    public int octaves = 6;
    public float scale = 40;
    public float lacunarity = 2;

    [Range(0, 1)]
    public float persistance = .585f;

    public Vector2 offset;

    public void ValidateValues()
    {
        scale = Mathf.Max(scale, 0.01f);
        octaves = Mathf.Max(octaves, 1);
        lacunarity = Mathf.Max(lacunarity, 1);
        persistance = Mathf.Clamp01(persistance);
    }
}
