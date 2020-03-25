using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class ScenSaver : MonoBehaviour, ISaveable
{
	private static ScenSaver _instance;
	public static ScenSaver Instance {
		get {
			if (!_instance) {
				var go = new GameObject("Scene Saver");
				_instance = go.AddComponent<ScenSaver>();
				DontDestroyOnLoad(_instance);
			}

			return _instance;
		}
	}

	public void Load(byte[] data, int version) {
		var sceneNameLength = BitConverter.ToInt32(data, 0);
		var sceneName = FungusSaver.StringEncoding.GetString(data, 4, sceneNameLength);
		var currentScene = SceneManager.GetActiveScene();
		if (currentScene.name != sceneName) {
			SceneManager.LoadScene(sceneName);
		}
	}

	public byte[] Save() {
		var scene = SceneManager.GetActiveScene();
		var sceneName = FungusSaver.StringEncoding.GetBytes(scene.name);

		var data = new byte[4 + sceneName.Length];

		BitConverter.GetBytes(sceneName.Length).CopyTo(data, 0);
		sceneName.CopyTo(data, 4);

		return data;
	}

	public void ClearSave() {}
}
