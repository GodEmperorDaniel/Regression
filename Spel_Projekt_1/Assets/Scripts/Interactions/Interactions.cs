using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Interactions : MonoBehaviour {
	[System.Serializable]
	public struct ItemInteraction {
		public InventoryItem requiredItem;
		public InteractionSettings interaction;
	}

	public static HashSet<Interactions> canUseItemOn = new HashSet<Interactions>();

	public Collider2D objectCollider = null;

	[Header("Interaction Setting")]


	[FormerlySerializedAs("onEnter")]
	public InteractionSettings OnEnter;
	[FormerlySerializedAs("onStay")]
	public InteractionSettings OnKeyPressed;
	public InteractionSettings OnStay;
	[FormerlySerializedAs("onExit")]
	public InteractionSettings OnExit;
	[FormerlySerializedAs("onItemUse")]
	public List<ItemInteraction> OnItemUse;
	[System.NonSerialized]
	public bool insideTrigger;

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

	private void OnDisable() {
		canUseItemOn.Remove(this);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			var controller = other.gameObject.GetComponentInParent<CharacterController2d>();
			canUseItemOn.Add(this);
			insideTrigger = true;
			Interact(controller, OnEnter);
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			var controller = other.gameObject.GetComponentInParent<CharacterController2d>();
			insideTrigger = false;
			canUseItemOn.Remove(this);
			Interact(controller, OnExit);
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag ==	"Player") {
			var controller = other.gameObject.GetComponentInParent<CharacterController2d>();
			if (controller.isActiveAndEnabled) {
				Interact(controller, OnStay);
			}
			if (controller.GetInteractionKeyDown()) {
				Interact(controller, OnKeyPressed);
			}
		}
	}

	private bool Interact(CharacterController2d controller, InteractionSettings settings)
	{
		if (settings.Active) {
			var facingAngle = controller.forward;
			var dirAngle = (transform.position - controller.transform.position).normalized;
			if (Vector2.Dot(facingAngle, dirAngle) >= settings.interactableAngleDot) {
				if (settings.flowchart && settings.block) {
					settings.flowchart.ExecuteBlock(settings.block);
				}

				return true;
			}
		}

		return false;
	}

	public bool UseItem(CharacterController2d controller, InventoryItem item) {
		foreach (var setting in OnItemUse) {
			if (setting.interaction.Active && item.itemId == setting.requiredItem.itemId) {
				return Interact(controller, setting.interaction);
			}
		}

		return false;
	}
}