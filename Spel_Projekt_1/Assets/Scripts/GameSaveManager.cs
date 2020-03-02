using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSaveManager : MonoBehaviour
{
	public const int version = 1;

	public void SaveGame() {
		var saveableObjects = new ISaveable[] {
			PlayerStatic.controllerInstance,
			PlayerStatic.inventoryInstance,
		};

		int totalSize = 8;
		int count = saveableObjects.Length;
		var saveData = new byte[count][];

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

		CreateDirectory();
		FileStream file = File.Create(Application.persistentDataPath + "/game_save/player_data/player_save.data");

		file.Write(output,0,totalSize);
		file.Close();
	}

	public void LoadGame() {
		CreateDirectory();
		if (File.Exists(Application.persistentDataPath + "/game_save/player_data/player_save.data")) {
			FileStream file = File.Open(Application.persistentDataPath + "/game_save/player_data/player_save.data", FileMode.Open);

			var saveableObjects = new ISaveable[] {
				PlayerStatic.controllerInstance,
				PlayerStatic.inventoryInstance,
			};

			var saveData = new byte[file.Length];
			file.Read(saveData, 0, (int)file.Length);

			var ver = BitConverter.ToInt32(saveData, 0);
			var count = BitConverter.ToInt32(saveData, 4);
			var index = 8;

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
