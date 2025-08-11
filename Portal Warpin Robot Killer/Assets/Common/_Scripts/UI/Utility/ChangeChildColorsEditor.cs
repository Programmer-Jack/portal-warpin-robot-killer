#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChangeChildColors))]
public class ChangeChildColorsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChangeChildColors srcComponent = (ChangeChildColors)target;
        if (GUILayout.Button("Apply"))
        {
            Undo.RecordObject(target, "Apply colors");
            PrefabUtility.RecordPrefabInstancePropertyModifications(target);
            srcComponent.ApplyColorToChildren();
        }
    }
}

#endif