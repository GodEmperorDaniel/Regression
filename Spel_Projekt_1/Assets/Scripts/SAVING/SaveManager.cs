using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public object Player { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Save();
        }
    }

    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.OpenOrCreate);

            SaveData data = new SaveData();

            SavePlayer(data);

            file.Close();


        }
        catch (System.Exception)
        {
            //this is for handling errors
        }
    }


    public void SavePlayer(SaveData data)
    {
        
    }
}
