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
        if (GUILayout.Button("Init")) {
            barPlacer.Init();   
        }
        EditorGUILayout.EndHorizontal();
    }
}
