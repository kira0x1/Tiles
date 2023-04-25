using UnityEditor;

namespace Kira.Map
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            HandleAutoUpdate();
        }

        private void HandleAutoUpdate()
        {
            if (!DrawDefaultInspector()) return;
            MapGenerator map = (MapGenerator)target;
            if (map.mapPreview.autoUpdate) map.mapPreview.GeneratePreview();
        }
    }
}