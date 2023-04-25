using UnityEngine;

namespace Kira.Grid
{
    public class Grid
    {
        private int width;
        private int height;
        private float cellSize;
        private int[,] gridValues;
        private TextMesh[,] gridText;
        private Vector3 originPos;

        public Grid(int width, int height, float cellSize, Vector3 originPos)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPos = originPos;

            gridValues = new int[width, height];
            gridText = new TextMesh[width, height];

            for (int x = 0; x < gridValues.GetLength(0); x++)
            {
                for (int y = 0; y < gridValues.GetLength(1); y++)
                {
                    Vector3 textPos = GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f;
                    gridText[x, y] = GridUtils.CreateWorldText(gridValues[x, y].ToString(), null, textPos, 20, Color.white);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            SetValue(2, 1, 56);
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + originPos;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originPos).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPos).y / cellSize);
        }

        public void SetValue(int x, int y, int value)
        {
            if (x < 0 || y < 0) return;
            if (x >= width || y >= height) return;

            gridValues[x, y] = value;
            gridText[x, y].text = value.ToString();
        }

        public void SetValue(Vector3 worldPosition, int value)
        {
            GetXY(worldPosition, out int x, out int y);
            SetValue(x, y, value);
        }

        public void AddValue(Vector3 worldPosition, int value)
        {
            GetXY(worldPosition, out int x, out int y);

            if (x < 0 || y < 0) return;
            if (x >= width || y >= height) return;

            int v = gridValues[x, y] + value;
            SetValue(x, y, v);
        }

        public void SubtractValue(Vector3 worldPosition, int value)
        {
            GetXY(worldPosition, out int x, out int y);

            if (x < 0 || y < 0) return;
            if (x >= width || y >= height) return;

            int v = gridValues[x, y] - value;
            SetValue(x, y, v);
        }

        public void AddValue(int x, int y, int value)
        {
            if (x < 0 || y < 0) return;
            if (x >= width || y >= height) return;

            gridValues[x, y] += value;
            gridText[x, y].text = gridValues[x, y].ToString();
        }

        public void SubtractValue(int x, int y, int value)
        {
            if (x < 0 || y < 0) return;
            if (x >= width || y >= height) return;

            gridValues[x, y] -= value;
            gridText[x, y].text = gridValues[x, y].ToString();
        }

        public int GetValue(int x, int y)
        {
            if (x < 0 || y < 0) return -1;
            if (x >= width || y >= height) return -1;
            return gridValues[x, y];
        }

        public int GetValue(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetValue(x, y);
        }
    }
}