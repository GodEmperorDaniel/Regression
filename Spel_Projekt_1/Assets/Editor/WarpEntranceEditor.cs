using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(WarpEntrance))]
public class WarpEntranceEditor : Editor {
	//Must match what is written in SceneReference!
	const string sceneAssetPropertyString = "sceneAsset";
	//Must match what is written in SceneReference!
	const string scenePathPropertyString = "scenePath";

	string lastScenePath = "";
	List<string> exitNames;
	List<long> ids;
	int selectedIndex = -2;
	long rememberedId = 0;
	GUIContent[] guiContent;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		var exitIdProperty = serializedObject.FindProperty("exitId");
		var sceneProperty = serializedObject.FindProperty("warpToScene");
		var scenePathProperty = sceneProperty.FindPropertyRelative(scenePathPropertyString);
		var scenePath = scenePathProperty.stringValue;

		if (selectedIndex == -2) {
			rememberedId = exitIdProperty.longValue;
		}

		if (EditorApplication.isPlaying) {
			EditorGUILayout.HelpBox("Warps cannot be edited during play", MessageType.Warning);
		} else {
			if (scenePath != lastScenePath || selectedIndex == -2) {
				lastScenePath = scenePath;
				selectedIndex = -1;
				exitNames = new List<string>();
				ids = new List<long>();

				if (!string.IsNullOrEmpty(scenePath)) {
					Scene scene = SceneManager.GetSceneByPath(scenePath);
					bool newScene = false;
					if (scene == null || !scene.IsValid()) {
						newScene = true;
						if (!EditorApplication.isPlaying) {
							scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
						} else {
							Debug.LogWarning("Warps cannot be edited during play");
							return;
							//SceneManager.LoadScene(scenePath, LoadSceneMode.Additive);
							//scene = SceneManager.GetSceneByPath(scenePath);
						}
					}

					if (scene.IsValid()) {
						foreach (var obj in scene.GetRootGameObjects()) {
							var children = obj.GetComponentsInChildren<WarpExit>();
							foreach (var c in children) {
								var id = c.GetID();
								if (id != 0) {
									exitNames.Add(string.Format("{0} (id: {1})", c.gameObject.name, id));
									ids.Add(id);
									if (id == exitIdProperty.longValue || id == rememberedId) {
										exitIdProperty.longValue = id;
										selectedIndex = exitNames.Count - 1;
									}
								}
							}
						}

						guiContent = new GUIContent[exitNames.Count];

						for (var i = 0; i < exitNames.Count; i++) {
							guiContent[i] = new GUIContent(exitNames[i]);
						}

						if (newScene) {
							SceneManager.UnloadSceneAsync(scene);
						}
					} else {
						guiContent = new GUIContent[0];
						exitIdProperty.longValue = 0;
					}
				} else {
					guiContent = new GUIContent[0];
					exitIdProperty.longValue = 0;
				}
			}

			selectedIndex = EditorGUILayout.Popup(new GUIContent("Exit Location"), selectedIndex, guiContent);
			exitIdProperty.longValue = 0;
			if (selectedIndex >= 0 && selectedIndex < ids.Count) {
				exitIdProperty.longValue = ids[selectedIndex];
				rememberedId = ids[selectedIndex];
			}
		}

		serializedObject.ApplyModifiedProperties();
		EditorGUILayout.SelectableLabel("Exit Id: " + exitIdProperty.longValue);
	}
}
