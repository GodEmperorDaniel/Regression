using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
[SerializeField]
public class VectorWrapper
{
    public float x;
    public float y;
    public float z;

    public VectorWrapper(Vector3 vec)
    {
        x = vec.x;
        y = vec.y;
        z = vec.z;
    }
}

[System.Serializable]
[SerializeField]
public class RegressionSave
{
    public CharacterController2d controller;
    public Inventory inventory;
    public VectorWrapper position;
}

[System.Serializable]
[SerializeField]
public class GameSaveManager : MonoBehaviour
{

    [SerializeField]
    public static GameSaveManager instance;
    public static CharacterController2d player
    {
        get
        {
            return PlayerStatic.playerInstance.GetComponent<CharacterController2d>();
        }
    }
    public GameObject SaveGameUi;
    [SerializeField]
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

    [SerializeField]
    public bool IsSaveFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/game_save");
    }
    [SerializeField]
    public void SaveGame()
    {
        FindObjectOfType<SavePlayerPos>().PlayerPosSave();
        Debug.Log(Application.persistentDataPath);
        PauseMenu.FindObjectOfType<PauseMenu>().Resume();
        Debug.Log("Save game, please wait");
        if (!IsSaveFile())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/game_save/player_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save/player_data");
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/game_save/player_data/player_save.data");
        var data = new RegressionSave();
        data.controller = player;
        data.inventory = player.GetComponent<Inventory>();
        data.position = new VectorWrapper(player.transform.position);
        var json = JsonUtility.ToJson(data);
        Debug.Log(bf);
        Debug.Log(file);
        Debug.Log(json);
        bf.Serialize(file,json);
        

        file.Close();
    }
    [SerializeField]
    public void LoadGame()
    {
        FindObjectOfType<SavePlayerPos>().PlayerPosLoad();

        Debug.Log(Application.persistentDataPath);
        FindObjectOfType<PauseMenu>().Resume();
        Debug.Log("Load Game...");
        if (!Directory.Exists(Application.persistentDataPath + "/game_save/player_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save/player_data");
            
        }

        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/game_save/player_data/player_save.data"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/game_save/player_data/player_save.data", FileMode.Open);
            var data = new RegressionSave();
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), data);

            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(data.controller), player);
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(data.inventory), player.GetComponent<Inventory>());
            player.transform.position = new Vector3(data.position.x, data.position.y, data.position.z);
            file.Close();
        }
    }

    /*public object HeaderHandle(Header[] headers)
    {
        foreach (var h in headers)
        {
            Debug.Log(h.Name + " " + h.Value.ToString());
            if (h.Name == "Pos")
            {
                var v = (VectorWrapper)h.Value;
                player.transform.position = new Vector3(v.x, v.y, v.z);

            }
        }

        return null;
    }*/
}
