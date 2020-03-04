using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor {
	bool foldOut = false;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		foldOut = EditorGUILayout.Foldout(foldOut, "Info");
		if (foldOut) {
			var itemsProp = serializedObject.FindProperty("_items");

			itemsProp.Next(true); // skip generic field
			itemsProp.Next(true); // advance to array size field

			// Get the array size
			int arrayLength = itemsProp.intValue;

			itemsProp.Next(true); // advance to first array index

			// Write values to list
			int lastIndex = arrayLength - 1;
			for (int i = 0; i < arrayLength; i++) {
				var item = (InventoryItem)itemsProp.objectReferenceValue; // copy the value to the list

				if (item) {
					EditorGUILayout.LabelField(string.Format("{0} ({1})", item.title, item.itemId));
				}

				if (i < lastIndex) itemsProp.Next(false); // advance without drilling into children
			}
		}
	}
}
	