using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTest : MonoBehaviour
{
    public Transform player;
    public float distanceToInteract;

    void Update()
    {
        if (player)
        {
            Vector2 angle = transform.TransformDirection(Vector2.right);
            Vector2 toOther = player.position - transform.position;

            if (Vector2.Dot(angle, toOther) <= distanceToInteract)
            {
                //press e to interact GUI;
                Debug.Log("The object has been interacted with");
            }
        }
    }
}