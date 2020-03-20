// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Fungus
{
    /// <summary>
    /// Loads a new Unity scene and displays an optional loading image. This is useful
    /// for splitting a large game across multiple scene files to reduce peak memory
    /// usage. Previously loaded assets will be released before loading the scene to free up memory.
    /// The scene to be loaded must be added to the scene list in Build Settings.")]
    /// </summary>
    [CommandInfo("Flow", 
                 "Load Scene", 
                 "Loads a new Unity scene and displays an optional loading image. This is useful " +
                 "for splitting a large game across multiple scene files to reduce peak memory " +
                 "usage. Previously loaded assets will be released before loading the scene to free up memory." +
                 "The scene to be loaded must be added to the scene list in Build Settings.")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class LoadScene : Command
    {
        [Tooltip("Name of the scene to load. The scene must also be added to the build settings.")]
        [SerializeField] protected SceneReferenceData scene;

        [Tooltip("Image to display while loading the scene")]
        [SerializeField] protected Texture2D loadingImage;

        #region Public members

        public override void OnEnter()
        {
            SceneLoader.LoadScene(scene.Value, loadingImage, false);
        }

        public override string GetSummary()
        {
            if (scene.Value == null || scene.Value.ScenePath.Length == 0)
            {
                return "Error: No scene selected";
            }

#if UNITY_EDITOR
			var assetGUID = new GUID(AssetDatabase.AssetPathToGUID(scene.Value));

			for (int index = 0; index < EditorBuildSettings.scenes.Length; ++index) {
				if (assetGUID.Equals(EditorBuildSettings.scenes[index].guid)) {
					return scene.Value;
				}
			}

			return "Error: Scene not enabled in Build Settings";
#else
			return scene.Value;
#endif
		}

		public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        public override bool HasReference(Variable variable)
        {
            return scene.sceneRef == variable ||
                base.HasReference(variable);
        }

		#endregion
    }
}