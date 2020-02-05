using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2d : MonoBehaviour
{
    public int MovementSpeed;
    CharacterController Player;
	public KeyCode forwardKey = KeyCode.W;
	public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode interactionButton = KeyCode.E;
    public KeyCode interactionButtonGamepad = KeyCode.Joystick1Button1;
    public Animator Ani;
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
        getInteractionKey();
    }

    private void keyboardMovement()
    {
        if (Input.GetKey(leftKey))
        {
            transform.position += Vector3.left * MovementSpeed * Time.deltaTime;
            Ani.SetFloat("HorizontalAni", -1);
            Ani.SetFloat("VerticalAni", 0);
        }
        if (Input.GetKey(rightKey))
        {
            transform.position += Vector3.right * MovementSpeed * Time.deltaTime;
            Ani.SetFloat("HorizontalAni", 1);
            Ani.SetFloat("VerticalAni", 0);
        }
        if (Input.GetKey(forwardKey))
        {
            transform.position += Vector3.up * MovementSpeed * Time.deltaTime;
            Ani.SetFloat("VerticalAni", 1);
            Ani.SetFloat("HorizontalAni", 0);
        }
        if (Input.GetKey(backwardKey))
        {
            transform.position += Vector3.down * MovementSpeed * Time.deltaTime;
            Ani.SetFloat("VerticalAni", -1);
            Ani.SetFloat("HorizontalAni", 0);
        }
    }

    private void gamepadMovement()
    {
        float x = Input.GetAxis("Horizontal") * MovementSpeed * Time.deltaTime;
       // Ani.SetFloat("HorizontalAni", Input.GetAxis("Horizontal"));
        float y = Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime;
        //Ani.SetFloat("VerticalAni", Input.GetAxis("Vertical"));
        transform.Translate(x, y, 0);
    }

    public bool getInteractionKey()
    {
        if (Input.GetKey(interactionButton) || Input.GetKey(interactionButtonGamepad))
            return true;
        else
        {
            return false;
        }
    }
}
