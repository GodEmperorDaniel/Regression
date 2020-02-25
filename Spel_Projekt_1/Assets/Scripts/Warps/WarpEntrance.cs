using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpEntrance : MonoBehaviour
{
	public SceneReference warpToScene;
	[HideInInspector]
	public long exitId;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			var player = collision.gameObject;
			var findWarpExit = WarpExit.GetWarp(exitId);

			if (findWarpExit != null) {
				if (findWarpExit.gameObject.scene.isLoaded) {
					findWarpExit.WarpPlayer();
					return;
				}
			}

			PlayerStatic.exitID = exitId;
			SceneManager.LoadScene(warpToScene);
			//SceneWarpManager.instance.WarpPlayer(warpToScene,exitId,collision.gameObject);
		}
	}

	private void OnFoundExit(WarpExit warpExit, GameObject player) {
		player.transform.position = warpExit.transform.position;
	}
}
