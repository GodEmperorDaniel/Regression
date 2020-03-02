using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus {
	[CommandInfo("Inventory",
				 "Count Item",
				 "Counts the number of copies of an item the player has in their inventory and outputs it to a variable.")]
	[AddComponentMenu("")]
	public class CountItem : Command {
		[Tooltip("Item to count")]
		[SerializeField] protected InventoryItemData testItem;

		[HideInInspector]
		[Tooltip("Variable to output result to")]
		[SerializeField] protected string key;

		protected Inventory _inventory;
		public Inventory Inventory {
			get {
				if (_inventory == null) {
					if (PlayerStatic.inventoryInstance != null) {
						_inventory = PlayerStatic.inventoryInstance;
					}
				}

				return _inventory;
			}
		}

		public override void OnEnter() {
			base.OnEnter();

			if (testItem.Value != null && key != "") {
				var v = Inventory.CountItem(testItem.Value);
				var flowChart = GetFlowchart();

				flowChart.GetVariable<IntegerVariable>(key).Value = v;
			}

			Continue();
		}

		public override string GetSummary() {
			if (testItem.Value == null) {
				return "Error: No item selected";
			}
			if (key == "") {
				return "Error: No output selected";
			}

			return testItem.Value.title + " => " + key;
		}
	}
}