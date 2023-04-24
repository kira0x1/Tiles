using UnityEngine;
using UnityEngine.U2D;

namespace Kira
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
            else if (mapGenerator.mapType == MapType.Biomes)
            {
                GenerateBiomes();
            }
        }

        private void GenerateNoisePreview()
        {
            int width = mapGenerator.width;
            int height = mapGenerator.height;
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

        private void GenerateBiomes()
        {
            int width = mapGenerator.width;
            int height = mapGenerator.height;

            mapGenerator.GenerateWaves(out float[,] heightMap, out float[,] moistureMap, out float[,] heatMap);

            Sprite[] sprites = new Sprite[spriteAtlas.spriteCount];
            Color[] pixelMap = new Color[(width * height) * 16];
            spriteAtlas.GetSprites(sprites);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Sprite tileSprite = mapGenerator.GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]).GetTileSprite();
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

            float scale = Mathf.Min(texture.width, texture.height);
            Vector2 pos = Vector2.zero;
            Vector2 size = new Vector2(scale, scale);
            Rect rect = new Rect(pos, size);

            Sprite terrainSprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

            previewSprite.transform.localScale = size;
            previewSprite.sprite = terrainSprite;
        }


        private void GenerateColors()
        {
            int width = mapGenerator.width;
            int height = mapGenerator.height;

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