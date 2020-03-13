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
	protected int selectedIndex = -1;
	protected int selectedVariableIndex = -1;
	protected float spaceBetweenLines = 2;
	const int propertyCount = 3;

	//We are forced to handle layout manually as GUIEditorLayout is not available to Property Drawers :(
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		var foldoutProperty = property.FindPropertyRelative("_foldout");
		var showPopupProperty = property.FindPropertyRelative("_showPopup");
		var showVariableProperty = property.FindPropertyRelative("_showVariablePopup");
		var isDefaultProperty = property.FindPropertyRelative("_defaultSolution");

		var c = 1;

		if (foldoutProperty.boolValue) {
			c = propertyCount;
			if (showPopupProperty.boolValue) {
				c++;
			}
			if (showVariableProperty.boolValue) {
				c++;
			}
			if (isDefaultProperty.boolValue) {
				c--;
			}
		}

		return EditorGUIUtility.singleLineHeight * c + (c - 1) * spaceBetweenLines;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		label = EditorGUI.BeginProperty(position, label, property);

		var parent = property.serializedObject.targetObject;
		var foldoutProperty = property.FindPropertyRelative("_foldout");
		var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
		foldoutProperty.boolValue = EditorGUI.Foldout(foldoutRect, foldoutProperty.boolValue, label, true);

		if (foldoutProperty.boolValue) {
			EditorGUI.indentLevel++;

			//Editor only properties
			var defaultProperty = property.FindPropertyRelative("_defaultSolution");
			var showVariableProperty = property.FindPropertyRelative("_showVariablePopup");
			var showPopupProperty = property.FindPropertyRelative("_showPopup");
			showPopupProperty.boolValue = false;
			showVariableProperty.boolValue = false;

			//Normal properties
			var flowchartProperty = property.FindPropertyRelative("flowchart");
			var solutionProperty = property.FindPropertyRelative("solution");

			//Find the DefaultSolution Attribute to determine whether to draw the solution field
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

			//Draw the solution field only if we are not the default solution
			defaultProperty.boolValue = isDefaultSolution;
			int defaultSolutionRect = isDefaultSolution ? 0 : 1;
			if (!isDefaultSolution) {
				var solutionRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + spaceBetweenLines, position.width, EditorGUIUtility.singleLineHeight);
				EditorGUI.PropertyField(solutionRect, solutionProperty);
			}

			//Draw normal fields
			Rect flowchartRect = new Rect(position.x, position.y + (defaultSolutionRect + 1) * (EditorGUIUtility.singleLineHeight + spaceBetweenLines), position.width, EditorGUIUtility.singleLineHeight);
			Rect popupRect = new Rect(position.x, position.y + (defaultSolutionRect + 2) * (EditorGUIUtility.singleLineHeight + spaceBetweenLines), position.width, EditorGUIUtility.singleLineHeight);
			Rect variablePopupRect = new Rect(position.x, position.y + (defaultSolutionRect + 3) * (EditorGUIUtility.singleLineHeight + spaceBetweenLines), position.width, EditorGUIUtility.singleLineHeight);

			EditorGUI.PropertyField(flowchartRect, flowchartProperty);
			var flowchart = flowchartProperty.objectReferenceValue as Flowchart;

			if (flowchart) {
				//Find blocks to show in the popup
				var blockProperty = property.FindPropertyRelative("block");
				var block = blockProperty.objectReferenceValue;
				var blocks = flowchart.GetComponents<Block>();

				var variableProperty = property.FindPropertyRelative("output");
				var variable = variableProperty.objectReferenceValue;
				var variables = flowchart.GetComponents<StringVariable>();

				//Show Blocks
				if (blocks.Length > 0) {
					showPopupProperty.boolValue = true;
					var blockNames = new string[blocks.Length + 1];
					blockNames[0] = "< None >";

					for (var i = 0; i < blocks.Length; i++) {
						blockNames[i + 1] = blocks[i].BlockName;

						if (blocks[i] == block) {
							selectedIndex = i;
						}
					}

					selectedIndex = EditorGUI.Popup(popupRect, "Block To Execute", selectedIndex + 1, blockNames) - 1;

					if (selectedIndex >= 0 && selectedIndex < blocks.Length) {
						blockProperty.objectReferenceValue = blocks[selectedIndex];
					} else {
						blockProperty.objectReferenceValue = null;
					}
				} else {
					variablePopupRect = popupRect;
				}

				//Show variables
				if (variables.Length > 0) {
					showVariableProperty.boolValue = true;
					var variableNames = new string[variables.Length + 1];
					variableNames[0] = "< None >"; 

					for (var i = 0; i < variables.Length; i++) {
						variableNames[i + 1] = variables[i].Key;

						if (variables[i] == variable) {
							selectedVariableIndex = i;
						}
					}

					selectedVariableIndex = EditorGUI.Popup(variablePopupRect, "Output Variable", selectedVariableIndex + 1, variableNames) - 1;

					if (selectedVariableIndex >= 0 && selectedVariableIndex < variables.Length) {
						variableProperty.objectReferenceValue = variables[selectedVariableIndex];
					} else {
						variableProperty.objectReferenceValue = null;
					}
				}
			}

			property.serializedObject.ApplyModifiedProperties();

			EditorGUI.indentLevel--;
		}

		EditorGUI.EndProperty();
	}
}
