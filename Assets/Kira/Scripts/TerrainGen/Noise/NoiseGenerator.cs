using UnityEngine;

namespace Kira.Noise
{
    public class NoiseGenerator : MonoBehaviour
    {
        public static float[,] Generate(int width, int height, float scale, Wave[] waves, Vector2 offset)
        {
            float[,] noiseMap = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float samplePosX = x * scale + offset.x;
                    float samplePosY = y * scale + offset.y;

                    float normalization = 0.0f;

                    foreach (Wave wave in waves)
                    {
                        float nX = samplePosX * wave.frequency + wave.seed;
                        float nY = samplePosY * wave.frequency + wave.seed;

                        noiseMap[x, y] += wave.amplitude * Mathf.PerlinNoise(nX, nY);
                        normalization += wave.amplitude;
                    }

                    noiseMap[x, y] /= normalization;
                }
            }

            return noiseMap;
        }

        public static float[,] GenerateNoiseMap(NoiseSettings noiseSettings, Wave[] waves)
        {
            int mapWidth = noiseSettings.width;
            int mapHeight = noiseSettings.height;
            float scale = noiseSettings.scale;
            Vector2 offset = noiseSettings.offset;
            int seed = noiseSettings.seed;
            int octaves = noiseSettings.octaves;
            float persistance = noiseSettings.persistance;
            float lacunarity = noiseSettings.lacunarity;

            float[,] noiseMap = new float[mapWidth, mapHeight];

            System.Random prng = new System.Random(seed);
            Vector2[] octaveOffsets = new Vector2[octaves];

            float amplitude = 1f;

            for (int i = 0; i < octaves; i++)
            {
                float offsetX = prng.Next(-100000, 100000) + offset.x;
                float offsetY = prng.Next(-100000, 100000) - offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);

                amplitude *= persistance;
            }

            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            float maxLocalNoiseHeight = float.MinValue;
            float minLocalNoiseHeight = float.MaxValue;

            float halfWidth = mapWidth / 2f;
            float halfHeight = mapHeight / 2f;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    amplitude = 1f;
                    var frequency = 1f;
                    float noiseHeight = 0f;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                        float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;


                        foreach (Wave wave in waves)
                        {
                            float wX = sampleX * wave.frequency + wave.seed;
                            float wY = sampleY * wave.frequency + wave.seed;
                            noiseHeight += Mathf.PerlinNoise(wX, wY) * wave.amplitude;
                        }
                    }


                    if (noiseHeight > maxLocalNoiseHeight) maxLocalNoiseHeight = noiseHeight;
                    else if (noiseHeight < minLocalNoiseHeight) minLocalNoiseHeight = noiseHeight;

                    noiseMap[x, y] = noiseHeight;
                }
            }

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float inverseLerp = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                    noiseMap[x, y] = Mathf.Floor(inverseLerp * noiseSettings.heightMultiplier);
                }
            }


            return noiseMap;
        }
    }
}