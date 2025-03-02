// Tutorial Used: Procedural Terrain Generation by Sebastian Lague
// Youtube: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
// Github: https://github.com/SebLague/Procedural-Landmass-Generation

using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap, Mesh, FalloffMap };
    [Header("Settings")]
    public DrawMode drawMode;

    public Noise.NormalizeMode normalizeMode;

    public bool useFlatShading;

    [Range(0, 6)]
    public int editorPreviewLOD;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool useFalloff;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;
    static MapGenerator instance;

    float[,] falloffMap;

    float[,] noiseMap;

    Color32[] colourMap;

    void Awake()
    {
        falloffMap = FalloffGenerator.GenerateFalloffMap(MapChunkSize);
        seed = UnityEngine.Random.Range(0, 100000);
    }

    public static int MapChunkSize
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<MapGenerator>();
            }
            if (instance.useFlatShading)
            {
                return 95;
            }
            else
            {
                return 239;
            }
        }
    }

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData(Vector2.zero);
        MapDisplay display = FindFirstObjectByType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(mapData.colourMap, MapChunkSize, MapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editorPreviewLOD, useFlatShading), TextureGenerator.TextureFromColourMap(mapData.colourMap, MapChunkSize, MapChunkSize));
        }
        else if (drawMode == DrawMode.FalloffMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(MapChunkSize)));
        }
    }

    public MeshData GenerateMeshData(MapData mapData, int lod)
    {
        return MeshGenerator.GenerateTerrainMesh(
            mapData.heightMap,
            meshHeightMultiplier,
            meshHeightCurve,
            lod,
            useFlatShading
        );
    }

    public MapData GenerateMapData(Vector2 center)
    {
        noiseMap = Noise.GenerateNoiseMap(MapChunkSize + 2, MapChunkSize + 2, seed, noiseScale, octaves, persistance, lacunarity, center + offset, normalizeMode);

        colourMap = new Color32[MapChunkSize * MapChunkSize];
        for (int y = 0; y < MapChunkSize; y++)
        {
            for (int x = 0; x < MapChunkSize; x++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight >= regions[i].height)
                    {
                        colourMap[y * MapChunkSize + x] = regions[i].color;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        return new MapData(noiseMap, colourMap);
    }

    void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

        falloffMap = FalloffGenerator.GenerateFalloffMap(MapChunkSize);
    }

    readonly struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}

[Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}

public readonly struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color32[] colourMap;

    public MapData(float[,] heightMap, Color32[] colourMap)
    {
        this.heightMap = heightMap;
        this.colourMap = colourMap;
    }
}