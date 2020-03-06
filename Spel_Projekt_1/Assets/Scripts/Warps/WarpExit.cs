using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
#endif

[ExecuteInEditMode]
public class WarpExit : MonoBehaviour {
	public enum FaceDirection {
		dontChange,
		up,
		down,
		right,
		left
	}

	public bool interruptMove = true;
	public FaceDirection faceDirection;
	private static Dictionary<long, WarpExit> allIds;
	[HideInInspector]
	[SerializeField]
	private long id = 0;

	public void WarpPlayer() {
		PlayerStatic.playerInstance.transform.position = transform.position;
		switch (faceDirection) {
			case FaceDirection.up:
				PlayerStatic.controllerInstance.SetFacing(Vector2.up);
				break;
			case FaceDirection.down:
				PlayerStatic.controllerInstance.SetFacing(Vector2.down);
				break;
			case FaceDirection.right:
				PlayerStatic.controllerInstance.SetFacing(Vector2.right);
				break;
			case FaceDirection.left:
				PlayerStatic.controllerInstance.SetFacing(Vector2.left);
				break;
		}
		if (interruptMove) {
			PlayerStatic.controllerInstance.InterruptMove();
		}
	}

	void Awake() {
		AssignID();
		if (GetID() == PlayerStatic.exitID && PlayerStatic.exitID != 0) {
			PlayerStatic.exitID = 0;
			WarpPlayer();
		}
	}

	#region id assignement
	public static WarpExit GetWarp(long id) {
		if (allIds == null) {
			return null;
		}

		if (allIds.ContainsKey(id)) {
			return allIds[id];
		}

		return null;
	}

	public long GetID() {
		return id;
	}

	void AssignID() {
		if (allIds == null) {
			allIds = new Dictionary<long, WarpExit>();
		}

		if (ShouldAssignID()) {
			while (!HasValidId()) {
				var guid = System.Guid.NewGuid().ToByteArray();
				id = System.BitConverter.ToInt64(guid, 0);
			}

			if (!allIds.ContainsKey(id)) {
				allIds.Add(id, this);
			}
		} else {
			if (allIds.ContainsKey(id) && allIds[id] == this) {
				allIds.Remove(id);
			}
			id = 0;
		}
	}

	bool HasValidId() {
		if (id == 0) {
			return false;
		}

		if (allIds.ContainsKey(id) && allIds[id] != this) {
			return false;
		}

		return true;
	}

	void OnValidate() {
		AssignID();
	}

	private void OnDestroy() {
		if (allIds.ContainsKey(id) && allIds[id] == this) {
			allIds.Remove(id);
		}
	}

	bool ShouldAssignID() {
#if UNITY_EDITOR
		if (PrefabUtility.IsPartOfPrefabAsset(this) || EditorUtility.IsPersistent(this) ) {
			return false;
		}

		var mainStage = StageUtility.GetMainStageHandle();
		var currentStage = StageUtility.GetStageHandle(gameObject);
		if (currentStage != mainStage) {
			var prefabStage = PrefabStageUtility.GetPrefabStage(gameObject);
			if (prefabStage != null) {
				return true;
			}
		}
#endif
		return true;
	}
	#endregion
}
