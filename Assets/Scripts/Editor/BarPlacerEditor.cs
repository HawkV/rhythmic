using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BarPlacer))]
public class BarPlacerEditor : Editor
{
    public override void OnInspectorGUI() {
        BarPlacer barPlacer = (BarPlacer)target;

        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        var buttonDescription = new GUIContent("Init", "Re-creates the instantiated objects. Use if any of the prefabs changes");
        if (GUILayout.Button(buttonDescription)) {
            barPlacer.Init();   
        }
        EditorGUILayout.EndHorizontal();
    }
}
