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
    public const float stepDuration = 0.5f;
    private Coroutine playerMovement;

    public Animator Ani;
    private Vector2 movement;
    private Vector2 oldPos = Vector2.zero;
    private Vector2 dir;
    
    public float stepTimer;
    public bool isStepping = false;
    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<CharacterController>();
        Ani = GetComponentInChildren<Animator>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!PlayerStatic.freezePlayer)
        {
            keyboardMovement();
            gamepadMovement();
            getInteractionKey();
            fixAnimator();
        }
    }
    private void keyboardMovement()
    {
        if (playerMovement == null)
        {
            if (Input.GetKey(leftKey))
            {
                TransPos(Vector2.left);
            }
            if (Input.GetKey(rightKey))
            {
                TransPos(Vector2.right);
            }
            if (Input.GetKey(forwardKey))
            {
                TransPos(Vector2.up);
            }
            if (Input.GetKey(backwardKey))
            {
                TransPos(Vector2.down);
            }
        }

    }

    private void TransPos(Vector3 mDir)
    {
        if (isStepping)
        {
            playerMovement = StartCoroutine(Steps(mDir));
        }
        else
        {
            transform.position += mDir * MovementSpeed * Time.deltaTime;
        }
    }

    private void gamepadMovement()
    {
        if (playerMovement == null)
        {
            if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && Input.GetAxisRaw("Vertical") != 0)
            {
                TransPos(new Vector2(0, Input.GetAxisRaw("Vertical")).normalized);
                //inputY = Input.GetAxisRaw("Vertical");
            }

            if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && Input.GetAxisRaw("Horizontal") != 0)
            {
                TransPos(new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized);
            }
        }
        //float inputX = Input.GetAxisRaw("Horizontal");
        //float inputY = Input.GetAxisRaw("Vertical");
        //movement = new Vector2(inputX, inputY);
        //transform.Translate(movement.normalized * MovementSpeed * Time.deltaTime);
    }

    private void fixAnimator()
    {
        if(oldPos == (Vector2)transform.position)
        {
            Ani.SetBool("movement", false);
        }else
        {
            Ani.SetBool("movement", true);
            dir = (oldPos - (Vector2)transform.position).normalized;
            oldPos = transform.position;
            Ani.SetFloat("horimovement", Mathf.RoundToInt(dir.x));
            Ani.SetFloat("vertimovement", Mathf.RoundToInt(dir.y));
        }
    }

    IEnumerator Steps(Vector3 mDir)
    {
        Vector2 startPos = transform.position;
        Vector2 destinationPos = transform.position + mDir;
        float t = 0.0f;

        while(t < stepTimer)
        {
            transform.position = Vector2.Lerp(startPos, destinationPos, t);
            t += Time.deltaTime / stepDuration;
            yield return new WaitForEndOfFrame();
        }
        transform.position = destinationPos;
        playerMovement = null;
    }

    public bool getInteractionKey()
    {
        if (!PlayerStatic.freezePlayer && (Input.GetKey(interactionButton) || Input.GetKey(interactionButtonGamepad)))
            return true;
        else
        {
            return false;
        }
    }
}
