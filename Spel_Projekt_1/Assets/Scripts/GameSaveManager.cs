using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager instance;
    public static CharacterController player;
    public GameObject SaveGameUi;
    //static Player player;

    void awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }


    public bool IsSaveFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/game_save");
    }

    public void SaveGame()
    {
        Debug.Log("Save game, please wait");
        if (!IsSaveFile())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/game_save/player_data"));
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save/player_data");
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/game_save/player_data/player_save.txt");
        var json = JsonUtility.ToJson(player);
        bf.Serialize(file, json);
        file.Close();
    }

    public void LoadGame()
    {
        Debug.Log("Load Game...");
        if (!Directory.Exists(Application.persistentDataPath + "/game_save/player_data")) ;
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save/player_data");
        }
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/game_save/player_data/player_save.txt"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/game_save/player_data/player_save.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), player);
            file.Close();
        }
    }
}
