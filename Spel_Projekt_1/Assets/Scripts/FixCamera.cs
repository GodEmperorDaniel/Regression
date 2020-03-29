using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCamera : MonoBehaviour
{
	private void OnEnable()
	{
		StartCoroutine(FixIt());
	}

	private IEnumerator FixIt()
	{
		yield return new WaitForEndOfFrame();
		GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = PlayerStatic.PlayerInstance.transform;
	}
}
