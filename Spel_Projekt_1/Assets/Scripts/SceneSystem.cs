using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    public string SceneName;
    public bool lockDoor;
    public bool isSpawner = false;
    public GameObject player;
    private Transform door;
    Vector3 SpawnPos;

    private void Start()
    {
        door = gameObject.transform;
        SpawnPosition();
        DontDestroyOnLoad(player);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") //kan du göra om till layermask
        {
            if (!lockDoor) //kan ha två colliders på dörren, en aktiveras när den är låst --> för att man inte ska kunna gå igenom den
            {
                SceneManager.LoadScene(SceneName);
            }
            else
            {
                Debug.Log("Door is Locked");
            }
        }
    }
    private void Update()
    {
       // Debug.Log(SpawnPos);
    }
    private Vector3 SpawnPosition()
    {
        if(isSpawner) 
        {
            SpawnPos = door.position;
            PlayerStatic.player.position = SpawnPos;
        }
        return SpawnPos;
    }
}
