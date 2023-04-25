using UnityEngine;
using UnityEngine.U2D;

namespace Kira.Map
{
    public class MapPreview : MonoBehaviour
    {
        [SerializeField]
        private MapGenerator mapGenerator;

        [SerializeField]
        private SpriteRenderer previewSprite;

        [SerializeField]
        private SpriteAtlas spriteAtlas;

        [Space(10)]
        public bool autoUpdate;

        [ContextMenu("Create Texture")]
        public void GeneratePreview()
        {
            if (mapGenerator.mapType == MapType.Noise)
            {
                GenerateNoisePreview();
            }
            else if (mapGenerator.mapType == MapType.Color)
            {
                GenerateColors();
            }
        }

        private void GenerateNoisePreview()
        {
            int width = mapGenerator.noiseSettings.width;
            int height = mapGenerator.noiseSettings.height;
            float[,] heightMap = mapGenerator.GenerateNoiseMap();

            Color[] colorMap = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colorMap[y * width + x] = Color.Lerp(Color.white, Color.black, heightMap[x, y]);
                }
            }

            Texture2D texture = new Texture2D(width, height);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels(colorMap);
            texture.Apply();

            float scale = Mathf.Min(texture.width, texture.height);
            Vector2 pos = Vector2.zero;
            Vector2 size = new Vector2(scale, scale);
            Rect rect = new Rect(pos, size);

            Sprite terrainSprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            previewSprite.sprite = terrainSprite;
        }


        private void GenerateColors()
        {
            int width = mapGenerator.noiseSettings.width;
            int height = mapGenerator.noiseSettings.height;

            float[,] heightMap = mapGenerator.GenerateNoiseMap();

            Sprite[] sprites = new Sprite[spriteAtlas.spriteCount];
            Color[] colorMap = new Color[width * height];

            spriteAtlas.GetSprites(sprites);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float curHeight = heightMap[x, y];
                    TerrainType[] regions = mapGenerator.regions;

                    for (int i = 0; i < regions.Length; i++)
                    {
                        if (curHeight >= regions[i].height)
                        {
                            colorMap[y * width + x] = regions[i].color;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }


            Texture2D texture = new Texture2D(width, height);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels(colorMap);
            texture.Apply();

            float scale = Mathf.Min(texture.width, texture.height);
            Vector2 pos = Vector2.zero;
            Vector2 size = new Vector2(scale, scale);
            Rect rect = new Rect(pos, size);

            Sprite terrainSprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

            // previewSprite.transform.localScale = size;
            previewSprite.sprite = terrainSprite;
        }
    }
}