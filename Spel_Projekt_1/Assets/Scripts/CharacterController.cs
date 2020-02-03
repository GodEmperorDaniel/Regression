using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public int MovementSpeed = 3;
    public Vector3 LeftInput = Vector3.up;
    CharacterController Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        keyboardMovement();
    }

    private void keyboardMovement()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.))
        {
            transform.position += Vector3.left * MovementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * MovementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * MovementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.down * MovementSpeed * Time.deltaTime;
        }
    }
}
