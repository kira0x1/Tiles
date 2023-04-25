using UnityEngine;

namespace Kira.Grid
{
    public class GridTest : MonoBehaviour
    {
        private Grid grid;
        private Camera cam;

        private void Start()
        {
            cam = FindObjectOfType<Camera>();

            const int width = 4;
            const int height = 2;
            const float scale = 10f;

            grid = new Grid(width, height, scale, new Vector3(-(width * scale) / 2f, -(height * scale) / 2f));
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                grid.AddValue(GridUtils.GetMouseWorldPosition(cam), 1);
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                grid.SubtractValue(GridUtils.GetMouseWorldPosition(cam), 1);
            }

            if (Input.GetMouseButtonDown(4))
            {
                Debug.Log(grid.GetValue(GridUtils.GetMouseWorldPosition(cam)));
            }
        }
    }
}