using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRaycast : MonoBehaviour
{
    public GameObject player;
    private GameObject playerCopy;
    public float offset;


    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerCopy = Instantiate(player, new Vector3(PlayerStatic.PlayerInstance.transform.position.x, offset, 0f), Quaternion.identity);
            playerCopy.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(playerCopy);
    }
}
