using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairTreeEye : MonoBehaviour
{
    public GameObject leftObject;
    public BoxCollider2D right;
    private BoxCollider2D leftCollider;
    public Vector2 eyePositionLeft;
    public Vector2 eyePositionMiddle;
    public Vector2 eyePositionRight;
    private Transform eye;

    void Start()
    {
        eye = GetComponent<Transform>();
        leftCollider = leftObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered");
        if (collision == leftCollider)
        {
            eye.position = eyePositionLeft;
            Debug.Log("Trigger LEFT");
        }
        if(collision == right)
        {
            eye.position = eyePositionRight;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger exited");
        if (collision == leftCollider)
        {
            eye.position = eyePositionMiddle;
        }
        if(collision == right)
        {
            eye.position = eyePositionMiddle;
        }
    }
}
