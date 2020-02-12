using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatic : MonoBehaviour
{
    public static GameObject playerInstance;
    public static int DoorIndex;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (playerInstance == null)
        {
            playerInstance = gameObject;
        }
        else
        {
           Destroy(gameObject);
        }
    }
}
