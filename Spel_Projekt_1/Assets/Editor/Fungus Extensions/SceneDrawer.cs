using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fungus.EditorUtils {
	[CustomPropertyDrawer(typeof(SceneReferenceData))]
	public class SceneDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);

			SerializedProperty referenceProp = property.FindPropertyRelative("sceneRef");
			SerializedProperty valueProp = property.FindPropertyRelative("sceneVal");

			var origLabel = new GUIContent(label);

			DrawSingleLineProperty(position, origLabel, referenceProp, valueProp);

			EditorGUI.EndProperty();
		}

		private void DrawSingleLineProperty(Rect rect, GUIContent label, SerializedProperty referenceProp, SerializedProperty valueProp) {
			const int popupWidth = 17;

			Rect controlRect = EditorGUI.PrefixLabel(rect, label);
			Rect valueRect = controlRect;
			valueRect.width = controlRect.width - popupWidth - 5;
			Rect popupRect = controlRect;

			if (referenceProp.objectReferenceValue == null) {
				SceneReferencePropertyDrawer.DrawSceneProperty(valueRect, valueProp, new GUIContent(""));
				popupRect.x += valueRect.width + 5;
				popupRect.width = popupWidth;
			}

			EditorGUI.PropertyField(popupRect, referenceProp, new GUIContent(""));
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUIUtility.singleLineHeight;
		}
	}
}