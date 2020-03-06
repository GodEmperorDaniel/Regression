using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Fungus;

//Custom Property Drawer for interaction settings (by EH)
[CustomPropertyDrawer(typeof(PuzzleSolution))]
public class PuzzleSolutionEditor : PropertyDrawer {
	protected int selectedIndex = 0;
	protected float spaceBetweenLines = 2;
	const int propertyCount = 3;

	//We are forced to handle layout manually as GUIEditorLayout is not available to Property Drawers :(
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		var foldoutProperty = property.FindPropertyRelative("_foldout");
		var showPopupProperty = property.FindPropertyRelative("_showPopup");
		var isDefaultProperty = property.FindPropertyRelative("_defaultSolution");

		var c = 1;

		if (foldoutProperty.boolValue) {
			c = propertyCount;
			if (showPopupProperty.boolValue) {
				c++;
			}
			if (isDefaultProperty.boolValue) {
				c--;
			}
		}

		return EditorGUIUtility.singleLineHeight * c + (c - 1) * spaceBetweenLines;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);

		var parent = property.serializedObject.targetObject;
		var foldoutProperty = property.FindPropertyRelative("_foldout");
		var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
		foldoutProperty.boolValue = EditorGUI.Foldout(foldoutRect, foldoutProperty.boolValue, label);

		if (foldoutProperty.boolValue) {
			EditorGUI.indentLevel++;

			var defaultProperty = property.FindPropertyRelative("_defaultSolution");
			var showPopupProperty = property.FindPropertyRelative("_showPopup");
			showPopupProperty.boolValue = false;

			//Draw normal fields
			var flowchartProperty = property.FindPropertyRelative("flowchart");
			var solutionProperty = property.FindPropertyRelative("solution");
			var isDefaultSolution = false;

			FieldInfo[] fields = parent.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			foreach (var field in fields)
			{
				if (field.Name == property.propertyPath) {
					if (Attribute.GetCustomAttribute(field, typeof(DefaultSolution)) != null) {
						isDefaultSolution = true;
						break;
					}
				}
			}

			Rect flowchartRect;
			Rect popupRect;

			if (isDefaultSolution) {
				flowchartRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + spaceBetweenLines, position.width, EditorGUIUtility.singleLineHeight);
				popupRect = new Rect(position.x, position.y + 2 * (EditorGUIUtility.singleLineHeight + spaceBetweenLines), position.width, EditorGUIUtility.singleLineHeight);
			} else {
				var solutionRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + spaceBetweenLines, position.width, EditorGUIUtility.singleLineHeight);
				flowchartRect = new Rect(position.x, position.y + 2 * (EditorGUIUtility.singleLineHeight + spaceBetweenLines), position.width, EditorGUIUtility.singleLineHeight);
				popupRect = new Rect(position.x, position.y + 3 * (EditorGUIUtility.singleLineHeight + spaceBetweenLines), position.width, EditorGUIUtility.singleLineHeight);
				EditorGUI.PropertyField(solutionRect, solutionProperty);
			}

			defaultProperty.boolValue = isDefaultSolution;
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
