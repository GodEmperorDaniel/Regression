using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Transform player;
    public float distanceToInteract;
    public Canvas canvas;
    private bool insideTrigger;

    void Update()
    {
        if (player)
        {
            Vector2 angle = transform.TransformDirection(Vector2.right);
            Vector2 toOther = player.position - transform.position;

            if (Vector2.Dot(angle, toOther) <= distanceToInteract)
            {
                if (insideTrigger)
                {
                    canvas.gameObject.SetActive(true);
                    Debug.Log("in range");
                }
                else
                {
                    canvas.gameObject.SetActive(false);
                    Debug.Log("in range");
                }

            }

        }

        Debug.Log(insideTrigger);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            insideTrigger = true;
            Debug.Log("test true");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("test false");
            insideTrigger = false;
        }
    }
}