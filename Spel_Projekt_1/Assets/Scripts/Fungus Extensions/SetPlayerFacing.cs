using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus {
	[CommandInfo("Player",
				 "Set Facing",
				 "Sets the player's facing and movement direction.")]
	[AddComponentMenu("")]
	public class SetPlayerFacing : Command {
		public enum FacingDirection {
			up,
			down,
			right,
			left
		}

		[Tooltip("Direction to face the player to.")]
		[SerializeField] protected FacingDirection facingDirection;

		public override void OnEnter() {
			base.OnEnter();

			switch (facingDirection) {
				case FacingDirection.up:
					PlayerStatic.ControllerInstance.SetFacing(Vector2.up);
					break;
				case FacingDirection.down:
					PlayerStatic.ControllerInstance.SetFacing(Vector2.down);
					break;
				case FacingDirection.left:
					PlayerStatic.ControllerInstance.SetFacing(Vector2.left);
					break;
				case FacingDirection.right:
					PlayerStatic.ControllerInstance.SetFacing(Vector2.right);
					break;
			}

			Continue();
		}

		public override string GetSummary() {
			var s = facingDirection.ToString();
			return char.ToUpper(s[0]) + s.Substring(1);
		}

		public override Color GetButtonColor() {
			return new Color32(130, 154, 226, 255);
		}
	}
}
