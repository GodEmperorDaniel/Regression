using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactions : MonoBehaviour
{
	private Collider2D objectCollider = null;

	[Header("Interaction Setting")]

	public InteractionSettings onEnter;
	public InteractionSettings onStay;
	public InteractionSettings onExit;

	private void Awake()
	{
		if (objectCollider == null)
		{
			objectCollider = gameObject.GetComponent<Collider2D>();

			if (objectCollider == null)
			{
				Debug.LogError("Could not find any colliders, it will not work");
				Debug.Break();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Interact(other, onEnter);
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		Interact(other, onExit);
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag ==	"Player")
		{
			if (other.gameObject.GetComponentInParent<CharacterController2d>().getInteractionKey())
			{
				Debug.Log("Interacted");

				Interact(other, onStay);
			}
		}
	}

	private void Interact(Collider2D other, InteractionSettings settings)
	{
		if (settings.Active)
		{
			if (other.gameObject.tag == "Player") //enbart en fråga men finns det någon anledning att använda tag över layermasks? - JB
			{
				if (settings.flowchart && settings.block)
				{
					settings.flowchart.ExecuteBlock(settings.block);
				}
			}
		}
	}

	IEnumerator showImage(InteractionSettings settings)
	{
		//float timer = 0;

		//sätt upp bilden på canvas

		/*while (timer < settings.image.imageTimer)
		{
			timer += Time.deltaTime;
			yield return null;
		}

		timer = 0;
		Color t = scareImage.color;
		while (timer < settings.image.fadeTimer)
		{
			timer += Time.deltaTime;
			t.a = Mathf.Lerp(255, 0, timer / settings.image.fadeTimer);
			scareImage.color = t;
			yield return null;
		}

		//ta bort bilden från canvas
	}

	private void SkickaText()
	{ 
		//connecta till flowcharten och skicka string till specifikt block
		}*/
		yield return null;
	}
}
