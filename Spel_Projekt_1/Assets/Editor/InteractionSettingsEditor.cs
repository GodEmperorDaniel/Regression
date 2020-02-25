using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Fungus;

//Custom Property Drawer for interaction settings (by EH)
[CustomPropertyDrawer(typeof(InteractionSettingsEditor))]
public class InteractionSettingsEditor : PropertyDrawer {
	protected int selectedIndex = 0;
	protected float spaceBetweenLines = 2;

	//We are forced to handle layout manually as GUIEditorLayout is not available to Property Drawers :(
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		var foldoutProperty = property.FindPropertyRelative("_foldout");
		var showPopupProperty = property.FindPropertyRelative("_showPopup");

		if (foldoutProperty.boolValue) {
			if (showPopupProperty.boolValue) {
				return EditorGUIUtility.singleLineHeight * 4 + 3 * spaceBetweenLines;
			} else {
				return EditorGUIUtility.singleLineHeight * 3 + 2 * spaceBetweenLines;
			}
		} else {
			return EditorGUIUtility.singleLineHeight;
		}
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);

		var foldoutProperty = property.FindPropertyRelative("_foldout");
		var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
		foldoutProperty.boolValue = EditorGUI.Foldout(foldoutRect, foldoutProperty.boolValue, label);

		if (foldoutProperty.boolValue) {
			var activeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + spaceBetweenLines, position.width, EditorGUIUtility.singleLineHeight);
			var flowchartRect = new Rect(position.x, position.y + 2 * (EditorGUIUtility.singleLineHeight + spaceBetweenLines), position.width, EditorGUIUtility.singleLineHeight);
			var popupRect = new Rect(position.x, position.y + 3 * (EditorGUIUtility.singleLineHeight + spaceBetweenLines), position.width, EditorGUIUtility.singleLineHeight);

			EditorGUI.indentLevel++;

			var showPopupProperty = property.FindPropertyRelative("_showPopup");
			showPopupProperty.boolValue = false;

			//Draw normal fields
			var flowchartProperty = property.FindPropertyRelative("flowchart");
			EditorGUI.PropertyField(activeRect, property.FindPropertyRelative("Active"));
			EditorGUI.PropertyField(flowchartRect, flowchartProperty);

			var flowchart = flowchartProperty.objectReferenceValue as Flowchart;

			if (flowchart) {
				//Find blocks to show in the popup
				var blockProperty = property.FindPropertyRelative("block");
				var block = blockProperty.objectReferenceValue;
				var blocks = flowchart.GetComponents<Block>();

				if (blocks.Length > 0) {
					showPopupProperty.boolValue = true;
					var blockNames = new string[blocks.Length];

					for (var i = 0; i < blocks.Length; i++) {
						blockNames[i] = blocks[i].BlockName;

						if (blocks[i] == block) {
							selectedIndex = i;
						}
					}

					selectedIndex = EditorGUI.Popup(popupRect, "Block", selectedIndex, blockNames);

					if (selectedIndex < blocks.Length) {
						blockProperty.objectReferenceValue = blocks[selectedIndex];
					} else {
						blockProperty.objectReferenceValue = null;
					}
				}
			}

			property.serializedObject.ApplyModifiedProperties();

			EditorGUI.indentLevel--;
		}

		EditorGUI.EndProperty();
	}
}
