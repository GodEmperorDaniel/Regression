using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallFootstepsEvent : MonoBehaviour
{
    private GameObject footsteps = null;
    private GroundDetection footstepManager = null;

    public void GetFootstep() {
		if (!footstepManager) {
			if (!footsteps) {
				footsteps = GameObject.FindGameObjectWithTag("footsteps");
			}
			if (footsteps) {
				footstepManager = footsteps.GetComponent<GroundDetection>();
			}
		}

		if (footstepManager) {
			footstepManager.CheckSound();
		}
    }
}
