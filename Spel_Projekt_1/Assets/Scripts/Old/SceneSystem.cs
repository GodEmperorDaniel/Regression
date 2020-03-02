using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("")]
public class SceneSystem : MonoBehaviour
{
    public string SceneName;
    public bool lockDoor;
    private Transform door;
    Vector3 SpawnPos;
    public int ThisDoorIndex;
    public int SpawnToIndex;
    private BoxCollider2D collider;
    [Header("Side of door to spawn player")]
    public bool leftofdoor;
    public bool rightofdoor;
    public bool underdoor;
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
            if (leftofdoor)
            {
                Vector3 offset = new Vector3(-0.8f, 0, 0);
                SpawnPos = door.position + offset;
            }
            else if(rightofdoor)
            {
                Vector3 offset = new Vector3(0.8f, 0, 0);
                SpawnPos = door.position + offset;
            }
            else if(underdoor)
            {
                Vector3 offset = new Vector3(0, -0.8f, 0);
                SpawnPos = door.position + offset;
            }
            else
            {
                SpawnPos = door.position;
                Debug.Log("default");
            }
       
            PlayerStatic.playerInstance.transform.position = SpawnPos;
        }
        return SpawnPos;
    }
}
