using UnityEngine;

namespace Kira.Map
{
    [CreateAssetMenu(fileName = "New Biome Preset", menuName = "Kira/New Biome Preset")]
    public class BiomePreset : ScriptableObject
    {
        public Sprite[] tiles;
        public float minHeight;
        public float minMoisture;
        public float minHeat;

        public Sprite GetTileSprite()
        {
            return tiles[Random.Range(0, tiles.Length)];
        }

        public bool MatchCondition(float height, float moisture, float heat)
        {
            return height >= minHeight && moisture >= minMoisture && heat >= minHeat;
        }
    }
}