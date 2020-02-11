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
        SpawnPosition();
        DontDestroyOnLoad(player);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") //kan du göra om till layermask
        {
            PlayerStatic.DoorIndex = SpawnToIndex;
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
        if(PlayerStatic.DoorIndex == ThisDoorIndex) 
        {
            SpawnPos = door.position;
            PlayerStatic.player.position = SpawnPos;
        }
        return SpawnPos;
    }
}
