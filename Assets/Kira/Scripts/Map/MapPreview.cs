using UnityEngine;
using UnityEngine.U2D;

namespace Kira
{
    [ExecuteInEditMode]
    public class MapPreview : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private Map map;

        [SerializeField]
        private MeshRenderer previewMesh;

        [SerializeField]
        private SpriteAtlas spriteAtlas;

        [Header("Dimensions")]
        public int width = 50;
        public int height = 50;
        public float scale = 1.0f;
        public Vector2 offset;

        [Header("Height Map")]
        public Wave[] heightWaves;

        [Header("Moisture Map")]
        public Wave[] moistureWaves;

        [Header("Heat Map")]
        public Wave[] heatWaves;

        [SerializeField, Space(10)]
        private bool autoUpdate;

        private void OnEnable()
        {
            map = GetComponent<Map>();
        }

        [ContextMenu("Sync Values")]
        private void SyncValues()
        {
            heightWaves = map.heightWaves;
            moistureWaves = map.moistureWaves;
            heatWaves = map.heatWaves;
            width = map.width;
            height = map.height;
            scale = map.scale;
            offset = map.offset;
        }

        private void OnValidate()
        {
            if (autoUpdate) GenerateTexture();
        }

        [ContextMenu("Create Texture")]
        private void GenerateTexture()
        {
            Sprite[] sprites = new Sprite[spriteAtlas.spriteCount];
            Color[] pixelMap = new Color[(width * height) * 16];
            spriteAtlas.GetSprites(sprites);

            // map.GenerateWaves(out float[,] heightMap, out float[,] moistureMap, out float[,] heatMap);
            float[,] heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset);
            float[,] moistureMap = NoiseGenerator.Generate(width, height, scale, moistureWaves, offset);
            float[,] heatMap = NoiseGenerator.Generate(width, height, scale, heatWaves, offset);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Sprite tileSprite = map.GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]).GetTileSprite();
                    var sprite = spriteAtlas.GetSprite(tileSprite.name);

                    for (int y1 = 0; y1 < 16; y1++)
                    {
                        for (int x1 = 0; x1 < 16; x1++)
                        {
                            pixelMap[(y1 + y) * width + (x1 + x)] = sprite.texture.GetPixel((int)sprite.textureRect.xMin + x1, (int)sprite.textureRect.yMin + y1);
                        }
                    }
                }
            }


            Texture2D texture = new Texture2D(width, height);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels(pixelMap);
            texture.Apply();

            previewMesh.sharedMaterial.mainTexture = texture;
        }
    }
}