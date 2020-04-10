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

			if (PlayerInstance == null) {
				_controllerData = controllerData;
				_inventoryData = inventoryData;
				_version = version;
			} else {
				ControllerInstance.Load(controllerData, version);
				InventoryInstance.Load(inventoryData, version);
			}
		}

		public byte[] Save() {
			var controllerData = ControllerInstance.Save();
			var inventoryData = InventoryInstance.Save();

			var data = new byte[8 + controllerData.Length + inventoryData.Length];

			BitConverter.GetBytes(controllerData.Length).CopyTo(data, 0);
			BitConverter.GetBytes(inventoryData.Length).CopyTo(data, 4);
			controllerData.CopyTo(data, 8);
			inventoryData.CopyTo(data, 8 + controllerData.Length);

			return data;
		}

		public void ClearSave() {
			freezeStack.Clear();
			if (PlayerInstance) {
				//ControllerInstance.ClearSave();
				//InventoryInstance.ClearSave();
				Destroy(_player);
				_player = null;
				_controller = null;
				_inventory = null;
			}
		}
	}

	public static int DoorIndex;
	public static long exitID;

	public static GameObject PlayerInstance { get { return _player; } }
	public static CharacterController2d ControllerInstance { get { return _controller; } }
	public static Inventory InventoryInstance { get { return _inventory; } }

	public static SaveablePlayerStatic Saveable {
		get {
			if (_saveable == null) {
				_saveable = new SaveablePlayerStatic();
			}
			return _saveable;
		}
	}

	private static GameObject _player;
	private static CharacterController2d _controller;
	private static Inventory _inventory;
	private static SaveablePlayerStatic _saveable;
	private static byte[] _controllerData;
	private static byte[] _inventoryData;
	private static int _version;
	private static HashSet<string> freezeStack = new HashSet<string>();

    void Awake()
    {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		DontDestroyOnLoad(gameObject);
        if (_player == null)
        {
            _player = gameObject;
			_controller = GetComponent<CharacterController2d>();
			_inventory = GetComponent<Inventory>();
			if (_controllerData != null) {
				ControllerInstance.Load(_controllerData, _version);
				InventoryInstance.Load(_inventoryData, _version);
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
		if (ControllerInstance) {
			ControllerInstance.enabled = false;
		}
	}

	public static void ResumePlayer(string key) {
		freezeStack.Remove(key);
		if (ControllerInstance && !IsFrozen()) {
			ControllerInstance.enabled = true;
		}
	}

	public static bool IsFrozen() {
		return freezeStack.Count > 0;
	}
}
