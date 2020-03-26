using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallFootstepsEvent : MonoBehaviour
{
    private GameObject footsteps = null;
    private GroundDetection footstepManager = null;
    private Collider2D col;

    // Update is called once per frame
    void Update()
    {
        if (!footsteps)
        {
            footsteps = GameObject.FindGameObjectWithTag("footsteps");
            footstepManager = footsteps.GetComponent<GroundDetection>();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            col = other;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            col = null;
        }
    }

    public void GetFootstep()
    {
        if (col)
        { 
            footstepManager.CheckSound(col);
        }
    }
}
