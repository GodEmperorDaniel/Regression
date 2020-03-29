using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangeNoteScript : MonoBehaviour
{
    private static GameObject _instance;

    private void Start()
    {
        PlayerStatic.FreezePlayer("Note");
       
        if (StrangeNoteScript._instance)
        {
            Destroy(StrangeNoteScript._instance);
        }
        else
        {
            _instance = gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Cancel") > 0 || Input.GetKeyDown(KeyCode.E))
        {
            PlayerStatic.ResumePlayer("Note");
            gameObject.SetActive(false);
        }
    }
}
