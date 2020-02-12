using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    public string SceneName;
    public bool lockDoor;
    public GameObject player;
    private Transform door;
    Vector3 SpawnPos;
    public int ThisDoorIndex;
    public int SpawnToIndex;
    private BoxCollider2D collider;
    private void Awake()
    {

        door = gameObject.transform;
        if (PlayerStatic.DoorIndex == ThisDoorIndex)
        {
            SpawnPosition();
        }
    }
    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        SpawnPosition();
        DontDestroyOnLoad(player);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStatic.DoorIndex = SpawnToIndex;
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

    private void Update()
    {
        if (!lockDoor)
        {
            collider.isTrigger = true;
        }
        else
        {
            collider.isTrigger = false;
        }
    }
    private Vector3 SpawnPosition()
    {
        if(PlayerStatic.DoorIndex == ThisDoorIndex) 
        {
            SpawnPos = door.position;
            PlayerStatic.player.position = SpawnPos;
        }
        return SpawnPos;
    }
}
