using Kira.Noise;
using UnityEngine;

namespace Kira.Map
{
    public class MapGenerator : MonoBehaviour
    {
        public MapPreview mapPreview;
        public MapType mapType;

        public TerrainType[] regions;
        public NoiseSettings noiseSettings;
        public Wave[] heightWaves;

        private void OnValidate()
        {
            if (noiseSettings.width < 1) noiseSettings.width = 1;
            if (noiseSettings.height < 1) noiseSettings.height = 1;
            if (noiseSettings.scale <= 0.0f) noiseSettings.scale = 0.01f;
        }

        public float[,] GenerateNoiseMap()
        {
            float[,] result = NoiseGenerator.GenerateNoiseMap(noiseSettings, heightWaves);
            return result;
        }
    }
}