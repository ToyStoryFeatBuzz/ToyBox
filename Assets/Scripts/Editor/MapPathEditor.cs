using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(MapPath))]
public class MapPathEditor : Editor
{
    private void OnSceneGUI()
    {
        MapPath path = (MapPath)target;

        Handles.color = Color.green;

        for (int i = 0; i < path.points.Count; i++)
        {
            Vector3 worldPos = path.transform.position + (Vector3)path.points[i];

            EditorGUI.BeginChangeCheck();
            Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);
            newWorldPos.z = 0f;

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(path, "Move 2D Path Point");
                path.points[i] = newWorldPos - path.transform.position;
            }

            Handles.SphereHandleCap(0, worldPos, Quaternion.identity, 0.1f, EventType.Repaint);
        }

        Handles.color = Color.cyan;
        for (int i = 0; i < path.points.Count - 1; i++)
        {
            Vector3 p1 = path.transform.position + (Vector3)path.points[i];
            Vector3 p2 = path.transform.position + (Vector3)path.points[i + 1];
            Handles.DrawLine(p1, p2);
        }
    }

    
}
