using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController2d))]
[RequireComponent(typeof(Inventory))]
public class PlayerStatic : MonoBehaviour {
	public static GameObject playerInstance;
	public static CharacterController2d controllerInstance;
	public static Inventory inventoryInstance;
	public static int DoorIndex;
	public static long exitID;
	private static HashSet<string> freezeStack = new HashSet<string>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (playerInstance == null)
        {
            playerInstance = gameObject;
			controllerInstance = GetComponent<CharacterController2d>();
			inventoryInstance = GetComponent<Inventory>();
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
		if (controllerInstance && freezeStack.Count <= 0) {
			controllerInstance.enabled = true;
		}
	}

}
