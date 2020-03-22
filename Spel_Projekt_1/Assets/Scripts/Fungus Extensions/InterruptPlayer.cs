using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus {
	[CommandInfo("Player",
				 "Interrupt",
				 "Cancels the player's current movement.")]
	[AddComponentMenu("")]
	public class InterruptPlayer : Command {

		public override void OnEnter() {
			base.OnEnter();

			PlayerStatic.ControllerInstance.InterruptMove();

			Continue();
		}

		public override Color GetButtonColor() {
			return new Color32(130, 154, 226, 255);
		}
	}
}
