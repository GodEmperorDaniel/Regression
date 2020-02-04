using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour
{
	[Header("Object Settings")]

	public Sprite objectSprite = null;

	[Header("Collider Settings")]

	[Range(0.001f, 10)]
	public float colliderRadius;

	private Collider2D objectCollider = null;

	[Header("Interaction Setting")]

	public InteractionSettings onEnter;
	public InteractionSettings onStay;
	public InteractionSettings onExit;
	private AudioSource source;

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
			else
			{
				(objectCollider as CircleCollider2D).radius = colliderRadius;
			}
		}
		if (objectSprite != null)
		{
			gameObject.GetComponentInChildren<SpriteRenderer>().sprite = objectSprite;
		}
		else
		{
			Debug.LogWarning("No sprite set for " + gameObject.name + ", it will not be visable");
		}
		if (source == null)
		{
			source = gameObject.GetComponentInChildren<AudioSource>();
			if (source == null)
			{
				Debug.LogWarning("No AudioSource found in children, if you deleted child revert to prefab to fix");
			}
		}

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (onEnter.Active)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
			{

			}
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if (onExit.Active)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
			{

			}
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (onStay.Active)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
			{

			}
		}
	}
}
