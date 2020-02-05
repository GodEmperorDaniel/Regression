using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2d : MonoBehaviour
{
    public int MovementSpeed;
    CharacterController Player;
	public KeyCode forwardKey = KeyCode.W;
	public KeyCode backwardKey = KeyCode.S;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        keyboardMovement();
        gamepadMovement();
    }

    private void keyboardMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * MovementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * MovementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(forwardKey))
        {
            transform.position += Vector3.up * MovementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * MovementSpeed * Time.deltaTime;
        }
    }

    private void gamepadMovement()
    {
        float x = Input.GetAxis("Horizontal") * MovementSpeed * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime;
        transform.Translate(x, y, 0);
    }
}
