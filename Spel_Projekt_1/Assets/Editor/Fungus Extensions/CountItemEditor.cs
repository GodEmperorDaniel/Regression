using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace Fungus.EditorUtils {
	[CustomEditor(typeof(CountItem))]
	public class CountItemEditor : CommandEditor {
		public override void DrawCommandGUI() {
			base.DrawCommandGUI();

			var keyProp = serializedObject.FindProperty("key");
			var t = target as CountItem;

			var vars = (from v in t.GetFlowchart().Variables where v.GetType() == typeof(IntegerVariable) select v.Key).ToList();
			int index = vars.IndexOf(keyProp.stringValue);
			index = EditorGUILayout.Popup("Output", index, vars.ToArray());

			if (index != -1) {
				keyProp.stringValue = vars[index];
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}
