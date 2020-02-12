using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Custom script to load our scenes the way we want to. LoadScene (built into fungus used as referens)

namespace Fungus
{
	[CommandInfo("Flow",
		"Custom Interaction",
		"A custom made interaction block for ease of use with fungus")] 
	[AddComponentMenu("")]
	public class CustomInteraction : Command
	{
		[Tooltip("Name of the scene to load")]
		[SerializeField] protected StringData sceneName = new StringData(""); //kan nog göras om till scenereferens

		[Tooltip("The loadingscreen displayed while loading next scene")]
		[SerializeField] protected Texture2D loadingScreen;

		[Tooltip("Select the mode to load the scene, true addative och false singular")]
		[SerializeField] protected bool loadMode;

		#region Public members
		public override void OnEnter()
		{
			SceneLoader.LoadScene(sceneName, loadingScreen, loadMode);
		}

		public override string GetSummary()
		{
			if (sceneName.Value.Length == 0)
			{
				return "Error: No scene name selected";
			}

			return sceneName;
		}

		public override Color GetButtonColor()
		{
			return new Color32(235, 191, 217, 255);
		}

		public override bool HasReference(Variable variable)
		{
			return sceneName.stringRef == variable ||
				base.HasReference(variable);
		}

		#endregion
	}
}
