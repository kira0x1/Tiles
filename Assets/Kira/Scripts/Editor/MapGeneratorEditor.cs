using UnityEditor;

namespace Kira
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (DrawDefaultInspector())
            {
                MapGenerator map = (MapGenerator)target;
                if (map.mapPreview.autoUpdate)
                {
                    map.mapPreview.GeneratePreview();
                }
            }
        }
    }
}