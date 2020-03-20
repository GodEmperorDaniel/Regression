using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRaycast : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject player;
    private GameObject playerCopy;
    private bool spawned; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerCopy = Instantiate(player, new Vector3(collision.GetComponent<Transform>().position.x, 2.5f, 0f), Quaternion.identity);
            playerCopy.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(playerCopy);
    }

    /* void FixedUpdate()
     {
         RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, -1f), 1f);

         if (hit && hit.collider.tag == "Player")
         {
             while (!spawned) 
                 { 
                 playerCopy = Instantiate(player, new Vector3(0f, player.transform.position.y + 1.5f, 0f), Quaternion.identity);
                 playerCopy.GetComponent<SpriteRenderer>().sortingOrder = 0;
                 spawned = true;
                 }
         }
         else
         {
             spawned = false;
             Destroy(playerCopy);
         }
     }
     */
}
