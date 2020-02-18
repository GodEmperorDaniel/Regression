using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	public List<InventoryItem> items;
	public List<ItemCombination> possibleCombinations;

	public void Start() {
		for (var i = 0; i < items.Count; i++) {
			items[i] = Instantiate(items[i]);
		}
	}

	public bool HasItem(InventoryItem item) {
		return false;
	}

	public bool TryCombine(InventoryItem partA, InventoryItem partB) {
		foreach (var combination in possibleCombinations) {
			if (combination.partA == partA && combination.partB == partB || combination.partA == partB && combination.partB == partA) {
				items.Remove(partA);
				items.Remove(partB);
				foreach (var newItem in combination.result) {
					items.Add(newItem);
				}

				return true;
			}
		}

		return false;
	}
}
