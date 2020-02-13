using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEntrance : MonoBehaviour
{
	public SceneReference warpToScene;
	[HideInInspector]
	public long exitId;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			SceneWarpManager.instance.WarpPlayer(warpToScene,exitId,collision.gameObject);
		}
	}
}
