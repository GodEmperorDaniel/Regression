using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatic : MonoBehaviour {
    public static GameObject playerInstance;
    public static int DoorIndex;
	public static long exitID;
	private static HashSet<string> freezeStack = new HashSet<string>();

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

	public static void FreezePlayer(string key) {
		freezeStack.Add(key);
		if (playerInstance) {
			var c = playerInstance.GetComponent<CharacterController2d>();
			if (c) {
				c.enabled = false;
			}
		}
	}

	public static void ResumePlayer(string key) {
		freezeStack.Remove(key);
		if (playerInstance && freezeStack.Count <= 0) {
			var c = playerInstance.GetComponent<CharacterController2d>();
			if (c) {
				c.enabled = true;
			}
		}
	}

    public void Update()
    {
        Debug.Log(freezeStack.Count);
    }
}
