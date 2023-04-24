using System.Collections.Generic;
using UnityEngine;

namespace Kira
{
    public class MapGenerator : MonoBehaviour
    {
        public MapPreview mapPreview;
        public MapType mapType;
        public BiomePreset[] biomes;
        public SpriteRenderer tilePrefab;

        public TerrainType[] regions;

        [Header("Dimensions")]
        public int width = 50;
        public int height = 50;
        public float scale = 1.0f;
        public Vector2 offset;
        public int octaves = 4;
        public float lacunarity = 0.5f;
        public float persistance = 2f;
        public int seed;

        [Header("Height Map")]
        public Wave[] heightWaves;
        private float[,] heightMap;

        [Header("Moisture Map")]
        public Wave[] moistureWaves;
        private float[,] moistureMap;

        [Header("Heat Map")]
        public Wave[] heatWaves;
        private float[,] heatMap;


        private void Start()
        {
            GenerateMap();
        }

        private void OnValidate()
        {
            if (width < 1) width = 1;
            if (height < 1) height = 1;
            if (scale <= 0.0f) scale = 0.01f;
        }

        private void GenerateMap()
        {
            heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset);
            moistureMap = NoiseGenerator.Generate(width, height, scale, moistureWaves, offset);
            heatMap = NoiseGenerator.Generate(width, height, scale, heatWaves, offset);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    SpriteRenderer tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    if (mapType == MapType.Noise)
                    {
                        tile.color = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
                    }
                    else if (mapType == MapType.Biomes)
                    {
                        tile.sprite = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]).GetTileSprite();
                    }
                }
            }
        }

        public void GenerateWaves(out float[,] heightMap, out float[,] moistureMap, out float[,] heatMap)
        {
            heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset);
            moistureMap = NoiseGenerator.Generate(width, height, scale, moistureWaves, offset);
            heatMap = NoiseGenerator.Generate(width, height, scale, heatWaves, offset);
        }

        public float[,] GenerateNoiseMap()
        {
            float[,] result = NoiseGenerator.GenerateNoiseMap(width, height, seed, scale, octaves, persistance, lacunarity, offset);
            return result;
        }

        public BiomePreset GetBiome(float height, float moisture, float heat)
        {
            List<BiomeTemp> biomeTemps = new List<BiomeTemp>();

            foreach (BiomePreset biome in biomes)
            {
                if (biome.MatchCondition(height, moisture, heat))
                {
                    biomeTemps.Add(new BiomeTemp(biome));
                }
            }


            BiomePreset closestBiome = biomeTemps.Count > 0 ? biomeTemps[0].biome : biomes[0];
            float curVal = biomeTemps.Count > 0
                ? biomeTemps[0].GetDiffValue(height, moisture, heat)
                : 0.0f;

            foreach (BiomeTemp biome in biomeTemps)
            {
                if (biome.GetDiffValue(height, moisture, heat) >= curVal) continue;
                closestBiome = biome.biome;
                curVal = biome.GetDiffValue(height, moisture, heat);
            }

            return closestBiome;
        }
    }
}