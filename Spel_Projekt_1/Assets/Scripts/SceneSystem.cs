using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    public string SceneName;
    public bool lockDoor;
    public bool isSpawner = false; //vi behöver inte denna, alla dörrar 'kan vara' spawners om en dörr är kopplad till dess index sedan
    private Transform door;
    private Vector3 SpawnPos;

    private void Start()
    {
		//if !PlayerStatic.Player --> instantiate(Player) (dvs om ingen spelare finns skapa en) kanske borde använda load resource om inte public variabel funkar för prefaben
		door = gameObject.transform;
        SpawnPosition();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") //kan du göra om till layermask enligt mig - JB
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
	private void SpawnPlayer()
	{

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
