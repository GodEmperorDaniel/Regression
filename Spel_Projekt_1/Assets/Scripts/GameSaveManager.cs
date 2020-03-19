using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSaveManager : MonoBehaviour {
	//The version number must be updated every time the save format changes
	public const int version = 4;

	//private static ISaveable[] _saveables;

	public ISaveable[] GetAllSaveables(int version) {
		if (version >= 4) {
			return new ISaveable[] {
				ScenSaver.Instance,
				PlayerStatic.Saveable,
				FungusSaver.Instance
			};
		} else if (version >= 2) {
			return new ISaveable[] {
				PlayerStatic.controllerInstance,
				PlayerStatic.inventoryInstance,
				FungusSaver.Instance
			};
		} else {
			return new ISaveable[] {
				PlayerStatic.controllerInstance,
				PlayerStatic.inventoryInstance,
			};
		}
	}

	public void SaveGame() {
		var saveableObjects = GetAllSaveables(version);

		//Signature is 4 bytes for the version number and 4 bytes for the number of blocks
		//Each block is prefixed with 4 bytes determining the block's size, prefix not included
		int count = saveableObjects.Length;
		int totalSize = 8;
		var saveData = new byte[count][];

		//Collect all the data to save from the saveable objects
		for (var i = 0; i < count; i++) {
			var obj = saveableObjects[i];
			var objData = obj.Save();
			saveData[i] = objData;
			totalSize += 4 + objData.Length;
		}

		var output = new byte[totalSize];
		BitConverter.GetBytes(version).CopyTo(output, 0);
		BitConverter.GetBytes(count).CopyTo(output, 4);
		var index = 8;

		for (var i = 0; i < count; i++) {
			var objData = saveData[i];
			BitConverter.GetBytes(objData.Length).CopyTo(output, index);
			index += 4;
			objData.CopyTo(output, index);
			index += objData.Length;
		}

		//Write the data to disc
		CreateDirectory();
		FileStream file = File.Create(Application.persistentDataPath + "/game_save/player_data/player_save.data");

		file.Write(output,0,totalSize);
		file.Close();
	}

	public void LoadGame() {
		PlayerStatic.ResumePlayer("pause");
		CreateDirectory();
		if (File.Exists(Application.persistentDataPath + "/game_save/player_data/player_save.data")) {
			FileStream file = File.Open(Application.persistentDataPath + "/game_save/player_data/player_save.data", FileMode.Open);


			var saveData = new byte[file.Length];
			file.Read(saveData, 0, (int)file.Length);

			var ver = BitConverter.ToInt32(saveData, 0);
			var count = BitConverter.ToInt32(saveData, 4);
			var index = 8;

			var saveableObjects = GetAllSaveables(ver);

			for (var i = 0; i < count; i++) {
				var size = BitConverter.ToInt32(saveData, index);
				index += 4;
				var obj = saveableObjects[i];
				var objData = new byte[size];
				Array.Copy(saveData,index,objData,0,size);

				obj.Load(objData, ver);

				index += size;
			}
			
			file.Close();
		}
	}

	private void CreateDirectory() {
		if (!Directory.Exists(Application.persistentDataPath + "/game_save")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
		}
		if (!Directory.Exists(Application.persistentDataPath + "/game_save/player_data")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/game_save/player_data");
		}
	}
}
