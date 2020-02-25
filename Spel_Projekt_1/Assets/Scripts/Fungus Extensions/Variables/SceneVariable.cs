using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus {
	/// <summary>
	/// Scene variable type.
	/// </summary>
	[VariableInfo("Other", "Scene")]
	[AddComponentMenu("")]
	[System.Serializable]
	public class SceneVariable : VariableBase<SceneReference> {
		public static readonly CompareOperator[] compareOperators = { CompareOperator.Equals, CompareOperator.NotEquals };
		public static readonly SetOperator[] setOperators = { SetOperator.Assign };

		public virtual bool Evaluate(CompareOperator compareOperator, SceneReference value) {
			bool condition = false;

			switch (compareOperator) {
				case CompareOperator.Equals:
					condition = Value == value;
					break;
				case CompareOperator.NotEquals:
					condition = Value != value;
					break;
				default:
					Debug.LogError("The " + compareOperator.ToString() + " comparison operator is not valid.");
					break;
			}

			return condition;
		}

		public override void Apply(SetOperator setOperator, SceneReference value) {
			switch (setOperator) {
				case SetOperator.Assign:
					Value = value;
					break;
				default:
					Debug.LogError("The " + setOperator.ToString() + " set operator is not valid.");
					break;
			}
		}

		public override string ToString() {
			return Value;
		}
	}

	/// <summary>
	/// Container for a Scene variable reference or constant value.
	/// </summary>
	[System.Serializable]
	public struct SceneReferenceData {
		[SerializeField]
		[VariableProperty("<Value>", typeof(SceneVariable))]
		public SceneVariable sceneRef;

		[SerializeField]
		public SceneReference sceneVal;

		public static implicit operator SceneReference(SceneReferenceData sceneData) {
			return sceneData.Value;
		}

		public SceneReferenceData(SceneReference v) {
			sceneVal = v;
			sceneRef = null;
		}

		public SceneReference Value {
			get { return (sceneRef == null) ? sceneVal : sceneRef.Value; }
			set { if (sceneRef == null) { sceneVal = value; } else { sceneRef.Value = value; } }
		}

		public string GetDescription() {
			if (sceneRef == null) {
				if (sceneVal == null) {
					return "Null";
				} else {
					return sceneVal.ToString();
				}
			} else {
				return sceneRef.Key;
			}
		}
	}
}