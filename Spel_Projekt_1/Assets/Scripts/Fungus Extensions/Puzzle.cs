using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Custom", 
		"PuzzleScript",
		"A custom puzzle script that checks if input was right")]

	[AddComponentMenu("")]
	public class Puzzle : Command
	{
		[SerializeField] protected string correctPassword;

		[SerializeField] protected string testedPassword;

		[SerializeField] protected InteractionSettings ifCorrect;

		[SerializeField] protected InteractionSettings ifUncorrect;

		#region All Functions

		public override void OnEnter()
		{
			testedPassword = GetComponentInChildren<PuzzleScript>().PassCombination();

			if (testedPassword == correctPassword)
			{
				PlayerStatic.ResumePlayer("Puzzle");
				ifCorrect.block.StartExecution();
			}
			else if (testedPassword != correctPassword)
			{
				ifUncorrect.block.StartExecution();
			}
			else
			{
				Continue();
			}
		}

		public override Color GetButtonColor()
		{
			return new Color32(235, 191, 217, 255);
		}

		#endregion

	}
}