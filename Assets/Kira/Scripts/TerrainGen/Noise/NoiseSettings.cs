using UnityEngine;

namespace Kira.Noise
{
    [System.Serializable]
    public struct NoiseSettings
    {
        public int width;
        public int height;
        public float scale;
        public Vector2 offset;
        public int octaves;
        public float lacunarity;
        public float persistance;
        public int seed;
        public float heightMultiplier;

        public NoiseSettings(int width, int height, float scale, Vector2 offset, int octaves, float lacunarity, float persistance, int seed, float heightMultiplier)
        {
            this.persistance = persistance;
            this.lacunarity = lacunarity;
            this.octaves = octaves;
            this.scale = scale;
            this.height = height;
            this.width = width;
            this.offset = offset;
            this.seed = seed;
            this.heightMultiplier = heightMultiplier;
        }
    }
}