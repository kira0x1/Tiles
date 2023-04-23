namespace Kira
{
    public class BiomeTemp
    {
        public BiomePreset biome;

        public BiomeTemp(BiomePreset biome)
        {
            this.biome = biome;
        }

        public float GetDiffValue(float height, float moisture, float heat)
        {
            return height - biome.minHeight + (moisture - biome.minMoisture) + (heat - biome.minHeat);
        }
    }
}