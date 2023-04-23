using System.Collections.Generic;
using UnityEngine;

namespace Kira
{
    public class Map : MonoBehaviour
    {
        public BiomePreset[] biomes;
        public GameObject tilePrefab;

        [Header("Dimensions")]
        public int width = 50;
        public int height = 50;
        public float scale = 1.0f;
        public Vector2 offset;

        [Header("Height Map")]
        public Wave[] heightWaves;
        public float[,] heightMap;

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

        private void GenerateMap()
        {
            heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset);
            moistureMap = NoiseGenerator.Generate(width, height, scale, moistureWaves, offset);
            heatMap = NoiseGenerator.Generate(width, height, scale, heatWaves, offset);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    GameObject tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    tile.GetComponent<SpriteRenderer>().sprite = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]).GetTileSprite();
                }
            }
        }

        public void GenerateWaves(out float[,] heightMap, out float[,] moistureMap, out float[,] heatMap)
        {
            heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset);
            moistureMap = NoiseGenerator.Generate(width, height, scale, moistureWaves, offset);
            heatMap = NoiseGenerator.Generate(width, height, scale, heatWaves, offset);
        }

        public Sprite[] GenerateSpriteMap()
        {
            heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset);
            moistureMap = NoiseGenerator.Generate(width, height, scale, moistureWaves, offset);
            heatMap = NoiseGenerator.Generate(width, height, scale, heatWaves, offset);
            Sprite[] spriteMap = new Sprite[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    spriteMap[y * width + x] = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]).GetTileSprite();
                }
            }

            return spriteMap;
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