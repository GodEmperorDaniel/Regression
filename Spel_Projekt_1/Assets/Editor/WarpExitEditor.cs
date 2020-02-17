using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WarpExit))]
public class WarpExitEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		var id = serializedObject.FindProperty("id").longValue;
		EditorGUILayout.SelectableLabel("Id: " + id);
	}
}
