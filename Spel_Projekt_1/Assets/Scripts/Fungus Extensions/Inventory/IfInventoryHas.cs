using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus {
	[CommandInfo("Inventory",
				 "If Inventory Contains",
				 "Checks whether the player's inventory contains an item. If the test expression is true, execute the following command block.")]
	[AddComponentMenu("")]
	public class IfInventoryHas : Condition {
		[Tooltip("Item to find")]
		[SerializeField] protected InventoryItemData testItem;

		[Tooltip("Boolean value to compare against")]
		[SerializeField] protected BooleanData condition;

		protected Inventory _inventory;
		public Inventory Inventory { get {
				if (_inventory == null) {
					if (PlayerStatic.playerInstance != null) {
						_inventory = PlayerStatic.playerInstance.GetComponent<Inventory>();
					}
				}

				return _inventory;
			}
		}

		protected override bool EvaluateCondition() {
			if (testItem.Value == null) {
				return false;
			}
			if (Inventory == null) {
				return false;
			}

			return Inventory.HasItem(testItem.Value);
		}

		protected override bool HasNeededProperties() {
			return testItem.Value != null;
		}

		#region Public members

		public override string GetSummary() {
			if (testItem.Value == null) {
				return "Error: No item selected";
			}

			string summary = "Inventory contains item " + testItem.Value.name + " == " + condition.Value;

			return summary;
		}

		public override bool HasReference(Variable variable) {
			return variable == this.testItem || base.HasReference(variable) || condition.booleanRef == variable; ;
		}

		public override Color GetButtonColor() {
			return new Color32(253, 253, 150, 255);
		}

		#endregion
	}
}