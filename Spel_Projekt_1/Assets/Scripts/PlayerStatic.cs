using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2d))]
[RequireComponent(typeof(Inventory))]
[DisallowMultipleComponent]
public class PlayerStatic : MonoBehaviour {
	public class SaveablePlayerStatic : ISaveable {
		public void Load(byte[] data, int version) {
			var controllerDataLength = BitConverter.ToInt32(data, 0);
			var inventoryDataLength = BitConverter.ToInt32(data, 4);
			var controllerData = new byte[controllerDataLength];
			var inventoryData = new byte[inventoryDataLength];
			Array.Copy(data, 8, controllerData, 0, controllerDataLength);
			Array.Copy(data, 8 + controllerDataLength, inventoryData, 0, inventoryDataLength);

			if (playerInstance == null) {
				_controllerData = controllerData;
				_inventoryData = inventoryData;
				_version = version;
			} else {
				controllerInstance.Load(controllerData, version);
				inventoryInstance.Load(inventoryData, version);
			}
		}

		public byte[] Save() {
			var controllerData = controllerInstance.Save();
			var inventoryData = inventoryInstance.Save();

			var data = new byte[8 + controllerData.Length + inventoryData.Length];

			BitConverter.GetBytes(controllerData.Length).CopyTo(data, 0);
			BitConverter.GetBytes(inventoryData.Length).CopyTo(data, 4);
			controllerData.CopyTo(data, 8);
			inventoryData.CopyTo(data, 8 + controllerData.Length);

			return data;
		}
	}

	public static int DoorIndex;
	public static long exitID;

	public static GameObject playerInstance;
	public static CharacterController2d controllerInstance;
	public static Inventory inventoryInstance;
	public static SaveablePlayerStatic Saveable {
		get {
			if (_saveable == null) {
				_saveable = new SaveablePlayerStatic();
			}
			return _saveable;
		}
	}

	private static SaveablePlayerStatic _saveable;
	private static byte[] _controllerData;
	private static byte[] _inventoryData;
	private static int _version;
	private static HashSet<string> freezeStack = new HashSet<string>();

    void Awake()
    {
		Cursor.visible = false;
		DontDestroyOnLoad(gameObject);
        if (playerInstance == null)
        {
            playerInstance = gameObject;
			controllerInstance = GetComponent<CharacterController2d>();
			inventoryInstance = GetComponent<Inventory>();
			if (_controllerData != null) {
				controllerInstance.Load(_controllerData, _version);
				inventoryInstance.Load(_inventoryData, _version);
				_controllerData = null;
				_inventoryData = null;
			}
        }
        else
        {
           Destroy(gameObject);
        }
    }

	public static void FreezePlayer(string key) {
		freezeStack.Add(key);
		if (controllerInstance) {
			controllerInstance.enabled = false;
		}
	}

	public static void ResumePlayer(string key) {
		freezeStack.Remove(key);
		if (controllerInstance && !IsFrozen()) {
			controllerInstance.enabled = true;
		}
	}

	public static bool IsFrozen() {
		return freezeStack.Count > 0;
	}
}
