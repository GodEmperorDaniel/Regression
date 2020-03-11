using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Custom" +
		"FreezePlayer",
		"A costum script to freeze the player to prevent it from moving when in dialog or something or another")]
	[AddComponentMenu("")]
	public class FreezePlayer : Command
	{
		[SerializeField] protected string stopPlayerString;

		protected PlayerStatic playerStatic;

		#region public members

		public override void OnEnter()
		{
			PlayerStatic.FreezePlayer(stopPlayerString);
		}

		public override Color GetButtonColor()
		{
			return new Color32(235, 191, 217, 255);
		}

		//public override bool HasReference(Variable variable)
		//{
		//	return sceneName.stringRef == variable ||
		//		base.HasReference(variable);
		//}

		#endregion
	}

}

