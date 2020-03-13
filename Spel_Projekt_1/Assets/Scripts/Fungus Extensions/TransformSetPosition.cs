using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus {
	[CommandInfo("Transform",
				 "Set Local Position",
				 "Sets the position of a GameObject relative to its parent.")]
	[AddComponentMenu("")]
	public class TransformSetPosition : Command {
		[SerializeField] protected GameObjectData targetGameObject;
		[SerializeField] protected Vector3Data position;

		public override void OnEnter() {
			if (targetGameObject.Value) {
				targetGameObject.Value.transform.localPosition = position.Value;
			}

			Continue();
		}

		public override string GetSummary() {
			if (targetGameObject.Value == null) {
				return "Error: No game object selected";
			}

			return targetGameObject.Value.name + " = " + position.GetDescription();
		}

		public override Color GetButtonColor() {
			return new Color32(172, 224, 130, 255);
		}
	}
}
