using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    public string SceneName;
    public string doorToSpawn;
    Vector3 SpawnPos;
    public GameObject player;

    private void Start()
    {
        DontDestroyOnLoad(player);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SceneManager.LoadScene(SceneName);
            SpawnPosition();
            collision.transform.position = SpawnPos;
        }
    }
    private Vector3 SpawnPosition()
    {
        SpawnPos = GameObject.FindWithTag(doorToSpawn).transform.position;
        return SpawnPos;
    }
}
