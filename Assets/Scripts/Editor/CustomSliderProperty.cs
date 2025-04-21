using UnityEngine;
using UnityEditor;
using ToyBox.LevelDesign;

[CustomEditor(typeof(PlayerCamera))]
public class CustomSliderProperty : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        PlayerCamera script = (PlayerCamera)target;


        EditorGUILayout.LabelField("Players Attraction", EditorStyles.boldLabel);

        for (int i = 0; i < script.playersImpact.Count; i++)
        {
            script.playersImpact[i] = EditorGUILayout.Slider($"Player {i+1} attraction : ", script.playersImpact[i], 0f, 1f);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("+ Ajouter"))
        {
            script.playersImpact.Add(0f);
        }

        if (GUILayout.Button("- Supprimer"))
        {
            if (script.playersImpact.Count > 0)
                script.playersImpact.RemoveAt(script.playersImpact.Count - 1);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
    }
}
