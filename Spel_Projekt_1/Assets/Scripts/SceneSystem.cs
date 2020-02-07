using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    public string SceneName;
    public bool lockDoor;
    public GameObject player;
    Vector3 SpawnPos;
    public string doorToSpawn;

    private void Start()
    {
        SpawnPosition();
        PlayerStatic.player.position = SpawnPos;
        DontDestroyOnLoad(player);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!lockDoor)
            {
                SceneManager.LoadScene(SceneName);
            }
            else
            {
                Debug.Log("Door is Locked");
            }
        }
    }

    private Vector3 SpawnPosition()
    {
        SpawnPos = GameObject.FindWithTag(doorToSpawn).transform.position;
        return SpawnPos;
    }
}
