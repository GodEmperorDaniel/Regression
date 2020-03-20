using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LjudManager : MonoBehaviour
{
	public Animator animator;

	public string soundName;

	public int speed;

	private void Start()
	{
		InvokeRepeating("PlaySound", 0, speed);
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player" && animator.GetBool("moving"))
		{
			PlaySound();
		}
	}

	private void PlaySound()
	{
		FMODUnity.RuntimeManager.PlayOneShot(soundName);
	}

	
}