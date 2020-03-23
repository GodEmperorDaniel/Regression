using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Custom",
		"UnFreezePlayer",
		"A costum script to unfreeze the player to prevent it from moving when in dialog or something or another")]
	[AddComponentMenu("")]
	public class UnFreezePlayer : Command
	{
		[SerializeField] protected string stopPlayerString;

		[SerializeField] protected bool setPlayerActive;

		#region public members

		public override void OnEnter()
		{
			PlayerStatic.ResumePlayer(stopPlayerString);
			if (setPlayerActive)
			{
				PlayerStatic.PlayerInstance.SetActive(!PlayerStatic.PlayerInstance.activeSelf);
			}
			Continue();
		}

		public override Color GetButtonColor()
		{
			return new Color32(235, 191, 217, 255);
		}

		#endregion
	}

}

