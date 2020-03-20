using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus {
	/// <summary>
	/// Character variable type.
	/// </summary>
	[VariableInfo("Other", "Character")]
	[AddComponentMenu("")]
	[System.Serializable]
	public class CharacterVariable : VariableBase<Character> {
		public static readonly CompareOperator[] compareOperators = { CompareOperator.Equals, CompareOperator.NotEquals };
		public static readonly SetOperator[] setOperators = { SetOperator.Assign };

		public virtual bool Evaluate(CompareOperator compareOperator, Character value) {
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

		public override void Apply(SetOperator setOperator, Character value) {
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
			return Value.gameObject.name;
		}
	}

	/// <summary>
	/// Container for an Character variable reference or constant value.
	/// </summary>
	[System.Serializable]
	public struct CharacterData {
		[SerializeField]
		[VariableProperty("<Value>", typeof(CharacterVariable))]
		public CharacterVariable characterRef;

		[SerializeField]
		public Character characterVal;

		public static implicit operator Character(CharacterData characterData) {
			return characterData.Value;
		}

		public CharacterData(Character v) {
			characterVal = v;
			characterRef = null;
		}

		public Character Value {
			get { return (characterRef == null) ? characterVal : characterRef.Value; }
			set { if (characterRef == null) { characterVal = value; } else { characterRef.Value = value; } }
		}

		public string GetDescription() {
			if (characterRef == null) {
				return characterVal.ToString();
			} else {
				return characterRef.Key;
			}
		}
	}
}