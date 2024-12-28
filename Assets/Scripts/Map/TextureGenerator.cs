//Tutorial Used: Procedural Terrain Generation by Sebastian Lague
//Youtube: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
//Github: https://github.com/SebLague/Procedural-Landmass-Generation

using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromColourMap(Color32[] colourMap, int width, int height) {
        Texture2D texture = new(width, height)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };
        texture.SetPixels32(colourMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color32[] colorMap = new Color32[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        return TextureFromColourMap(colorMap, width, height);
    }
}
