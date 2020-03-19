using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Custom",
		"FreezePlayer",
		"A costum script to freeze the player to prevent it from moving when in dialog or something or another")]
	[AddComponentMenu("")]
	public class FreezePlayer : Command
	{
		[SerializeField] protected string stopPlayerString;

		[SerializeField] protected bool setPlayerActive;

		#region public members

		public override void OnEnter()
		{
			PlayerStatic.FreezePlayer(stopPlayerString);
			if (setPlayerActive)
			{
				PlayerStatic.playerInstance.SetActive(!PlayerStatic.playerInstance.activeSelf);
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

