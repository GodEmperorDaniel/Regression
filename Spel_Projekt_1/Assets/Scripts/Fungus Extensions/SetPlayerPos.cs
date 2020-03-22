using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Custom",
		"SetPlayerPos",
		"A costum script to change the players position")]
	[AddComponentMenu("")]
	public class SetPlayerPos : Command
	{
		[SerializeField] protected Vector2 position;

		#region public members

		public override void OnEnter()
		{
			PlayerStatic.playerInstance.transform.position = position;
			Continue();
		}

		public override Color GetButtonColor()
		{
			return new Color32(235, 191, 217, 255);
		}

		#endregion
	}

}