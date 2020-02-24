using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

////[CustomPropertyDrawer(typeof(PuzzleAndScene))]
//public class PuzzleEditor : PropertyDrawer
//{
//	protected int selectedIndex = 0;
//	protected float spaceBetweenLines = 2;

//	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//	{
//		bool foldoutProterty = property.FindPropertyRelative("_foldout").boolValue;
//		bool showPopupProperty = property.FindPropertyRelative("_showPopup").boolValue;

//		if (foldoutProterty)
//		{
//			if (showPopupProperty)
//			{
//				return EditorGUIUtility.singleLineHeight * 3 + 2 * spaceBetweenLines;
//			}
//			else
//			{
//				return EditorGUIUtility.singleLineHeight * 2 + 1 * spaceBetweenLines;
//			}
//		}
//		else
//		{
//			return EditorGUIUtility.singleLineHeight;
//		}
//	}

//	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//	{
//		//EditorGUI.BeginProperty()
//	}
//}
