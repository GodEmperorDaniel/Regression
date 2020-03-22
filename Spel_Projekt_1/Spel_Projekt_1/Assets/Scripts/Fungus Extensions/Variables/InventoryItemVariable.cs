using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus {
	/// <summary>
	/// InventoryItem variable type.
	/// </summary>
	[VariableInfo("Other", "InventoryItem")]
	[AddComponentMenu("")]
	[System.Serializable]
	public class InventoryItemVariable : VariableBase<InventoryItem> {
		public static readonly CompareOperator[] compareOperators = { CompareOperator.Equals, CompareOperator.NotEquals };
		public static readonly SetOperator[] setOperators = { SetOperator.Assign };

		public virtual bool Evaluate(CompareOperator compareOperator, InventoryItem value) {
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

		public override void Apply(SetOperator setOperator, InventoryItem value) {
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
			return Value.title;
		}
	}

	/// <summary>
	/// Container for an InventoryItem variable reference or constant value.
	/// </summary>
	[System.Serializable]
	public struct InventoryItemData {
		[SerializeField]
		[VariableProperty("<Value>", typeof(InventoryItemVariable))]
		public InventoryItemVariable inventoryItemRef;

		[SerializeField]
		public InventoryItem inventoryItemVal;

		public static implicit operator InventoryItem(InventoryItemData characterData) {
			return characterData.Value;
		}

		public InventoryItemData(InventoryItem v) {
			inventoryItemVal = v;
			inventoryItemRef = null;
		}

		public InventoryItem Value {
			get { return (inventoryItemRef == null) ? inventoryItemVal : inventoryItemRef.Value; }
			set { if (inventoryItemRef == null) { inventoryItemVal = value; } else { inventoryItemRef.Value = value; } }
		}

		public string GetDescription() {
			if (inventoryItemRef == null) {
				if (inventoryItemVal == null) {
					return "Null";
				} else {
					return inventoryItemVal.title;
				}
			} else {
				return inventoryItemRef.Key;
			}
		}
	}
}