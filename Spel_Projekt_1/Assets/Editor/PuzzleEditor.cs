using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Fungus;

[CustomPropertyDrawer(typeof(PuzzleAndScene))]
public class PuzzleEditor : PropertyDrawer
{
	protected int selectedIndex = 0;
	protected float spaceBetweenLines = 2;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		bool foldoutProterty = property.FindPropertyRelative("_foldout").boolValue;
		bool showPopupProperty = property.FindPropertyRelative("_showPopup").boolValue;

		if (foldoutProterty)
		{
			if (showPopupProperty)
			{
				return EditorGUIUtility.singleLineHeight * 3 + 2 * spaceBetweenLines;
			}
			else
			{
				return EditorGUIUtility.singleLineHeight * 2 + 1 * spaceBetweenLines;
			}
		}
		else
		{
			return EditorGUIUtility.singleLineHeight;
		}
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		SerializedProperty foldoutProperty = property.FindPropertyRelative("_foldout");
		Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
		foldoutProperty.boolValue = EditorGUI.Foldout(foldoutRect, foldoutProperty.boolValue, "Puzzle");

		if (foldoutProperty.boolValue)
		{
			Rect flowchartRect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight + spaceBetweenLines), position.width, EditorGUIUtility.singleLineHeight);
			Rect popupRect = new Rect(position.x, position.y + 2 * (EditorGUIUtility.singleLineHeight + spaceBetweenLines), position.width, EditorGUIUtility.singleLineHeight);

			EditorGUI.indentLevel++;

			SerializedProperty showPopupProperty = property.FindPropertyRelative("_showPopup");
			showPopupProperty.boolValue = false;

			SerializedProperty flowchartProperty = property.FindPropertyRelative("flowchart");
			EditorGUI.PropertyField(flowchartRect, flowchartProperty);

			Flowchart flowchart = flowchartProperty.objectReferenceValue as Flowchart;

			if (flowchart)
			{
				SerializedProperty blockProperty = property.FindPropertyRelative("block");
				Object block = blockProperty.objectReferenceValue;
				Block[] blocks = flowchart.GetComponents<Block>();

				if (blocks.Length > 0)
				{
					showPopupProperty.boolValue = true;
					string[] blockNames = new string[blocks.Length];

					for (int i = 0; i < blocks.Length; i++)
					{
						blockNames[i] = blocks[i].BlockName;

						if (blocks[i] == block)
						{
							selectedIndex = i;
						}
					}

					selectedIndex = EditorGUI.Popup(popupRect, "Block", selectedIndex, blockNames);

					if (selectedIndex < blocks.Length)
					{
						blockProperty.objectReferenceValue = blocks[selectedIndex];
					}
					else
					{
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
