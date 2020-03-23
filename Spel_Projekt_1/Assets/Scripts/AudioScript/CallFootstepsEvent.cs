using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallFootstepsEvent : MonoBehaviour
{
    private GameObject footsteps = null;
    private GroundDetection footstepManager = null;

    // Update is called once per frame
    void Update()
    {
        if (!footsteps)
        {
            footsteps = GameObject.FindGameObjectWithTag("footsteps");
            footstepManager = footsteps.GetComponent<GroundDetection>();
        }
    }

    public void GetFootstep()
    {
        footstepManager.CheckSound();
    }
}
