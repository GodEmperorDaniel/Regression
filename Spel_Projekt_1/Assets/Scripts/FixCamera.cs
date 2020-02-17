using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCamera : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = PlayerStatic.playerInstance.transform;
	}
}
