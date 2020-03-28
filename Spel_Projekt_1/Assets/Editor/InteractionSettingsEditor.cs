using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Fungus;

//Custom Property Drawer for interaction settings (by EH)
[CustomPropertyDrawer(typeof(InteractionSettings))]
public class InteractionSettingsEditor : PropertyDrawer {
	protected int selectedIndex = 0;
	protected float spaceBetweenLines = 2;
	const int propertyCount = 5;

	//We are forced to handle layout manually as GUIEditorLayout is not available to Property Drawers :(
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		var foldoutProperty = property.FindPropertyRelative("_foldout");
		var showPopupProperty = property.FindPropertyRelative("_showPopup");

		if (foldoutProperty.boolValue) {
			if (showPopupProperty.boolValue) {
				return EditorGUIUtility.singleLineHeight * (propertyCount + 1) + propertyCount * spaceBetweenLines;
			} else {
				return EditorGUIUtility.singleLineHeight * propertyCount + (propertyCount - 1) * spaceBetweenLines;
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
			var offsetRect = new Rect(position.x, position.y + 2* EditorGUIUtility.singleLineHeight + spaceBetweenLines, position.width, EditorGUIUtility.singleLineHeight);
			var angleRect = new Rect(position.x, position.y + 3 * EditorGUIUtility.singleLineHeight + spaceBetweenLines, position.width, EditorGUIUtility.singleLineHeight);
			var flowchartRect = new Rect(position.x, position.y + 4 * (EditorGUIUtility.singleLineHeight + spaceBetweenLines), position.width, EditorGUIUtility.singleLineHeight);
			var popupRect = new Rect(position.x, position.y + 5 * (EditorGUIUtility.singleLineHeight + spaceBetweenLines), position.width, EditorGUIUtility.singleLineHeight);

			EditorGUI.indentLevel++;

			var showPopupProperty = property.FindPropertyRelative("_showPopup");
			showPopupProperty.boolValue = false;

			//Draw normal fields
			var flowchartProperty = property.FindPropertyRelative("flowchart");
			var offsetProperty = property.FindPropertyRelative("offset");
			var angleProperty = property.FindPropertyRelative("interactableAngle");
			var angleDotProperty = property.FindPropertyRelative("interactableAngleDot");
			EditorGUI.PropertyField(activeRect, property.FindPropertyRelative("Active"));
			EditorGUI.PropertyField(offsetRect, offsetProperty);
			
			//Angle defaults to 0 (because struct) but we want it to default to 360
			var angle = angleProperty.floatValue;
			if (angle == 0) {
				angle = 360;
			} else if (angle == 361) {
				angle = 0;
			}
			angle = EditorGUI.Slider(angleRect, "Interactable Angle", angle, 0, 360);
			angleDotProperty.floatValue = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
			if (angle == 0) {
				angle = 361;
			}
			angleProperty.floatValue = angle;

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
