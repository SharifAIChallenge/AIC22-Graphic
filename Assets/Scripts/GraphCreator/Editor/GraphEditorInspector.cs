using UnityEditor;
using UnityEngine;

namespace GraphCreator
{
    [CustomEditor(typeof(Graph))]
    public class GraphEditorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Graph graph = (Graph)target;
            if(GUILayout.Button("Build YAML File!"))
            {
                graph.BuildYaml();
            }
        }
    }
}
