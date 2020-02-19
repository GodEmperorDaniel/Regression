using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus {
	[CommandInfo("Inventory",
				 "Remove Item",
				 "Removes an item from the player's inventory.")]
	[AddComponentMenu("")]
	public class RemoveItem : Command {
		[Tooltip("Item to add")]
		[SerializeField] protected InventoryItemData item;

		protected Inventory _inventory;
		public Inventory Inventory {
			get {
				if (_inventory == null) {
					if (PlayerStatic.playerInstance != null) {
						_inventory = PlayerStatic.playerInstance.GetComponent<Inventory>();
					}
				}

				return _inventory;
			}
		}

		public override void OnEnter() {
			base.OnEnter();

			if (item.Value != null) {
				Inventory.RemoveItem(item.Value);
			}

			Continue();
		}

		public override string GetSummary() {
			if (item.Value == null) {
				return "Error: No item selected";
			}

			return item.Value.name;
		}
	}
}