/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SceneWarpManager : MonoBehaviour
{
	private static SceneWarpManager _instance;
	private long waitingForId = 0;
	private System.Action<WarpExit> onFoundExit;

	public static SceneWarpManager instance {
		get {
			if (!_instance) {
				_instance = FindObjectOfType<SceneWarpManager>();
			}
			if (!_instance) {
				var obj = new GameObject();
				obj.name = "Scene Warp Manager";
				_instance = obj.AddComponent<SceneWarpManager>();
				DontDestroyOnLoad(obj);
			}
			return _instance;
		}
	}
	public static bool hasInstance {
		get {
			return _instance != null;
		}
	}

	public void OnWarpAwaken(WarpExit warpExit) {
		if (warpExit.GetID() == waitingForId && waitingForId != 0) {
			onFoundExit(warpExit);
			onFoundExit = null;
			waitingForId = 0;
		}
	}

	public void WaitForExitWithId(long id, System.Action<WarpExit> onComplete) {
		waitingForId = id;
		onFoundExit = onComplete;
	}

	public void Awake() {
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad(gameObject);
		} else if (_instance != this) {
			Destroy(gameObject);
		}
	}

	public void WarpPlayer(SceneReference warpToScene, long exitId, GameObject player) {
		var findWarpExit = WarpExit.GetWarp(exitId);
		
		if (findWarpExit != null) {
			if (findWarpExit.gameObject.scene.isLoaded) {
				OnFoundExit(findWarpExit, player);
				return;
			}
		}

		DontDestroyOnLoad(player);
		waitingForId = exitId;
		onFoundExit = delegate (WarpExit warpExit) {
			OnFoundExit(warpExit, player);
		};
		SceneManager.LoadScene(warpToScene);
	}

	private void OnFoundExit(WarpExit warpExit, GameObject player) {
		player.transform.position = warpExit.transform.position;
	}
}*/
