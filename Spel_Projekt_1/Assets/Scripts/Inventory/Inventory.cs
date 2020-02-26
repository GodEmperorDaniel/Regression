using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	public List<InventoryItem> items;
	public List<ItemCombination> possibleCombinations;
	public GameObject uiPrefab;

	protected InventoryCanvas canvas;

    public void Start()
    {
        if (uiPrefab != null)
        {
            canvas = Instantiate(uiPrefab).GetComponentInChildren<InventoryCanvas>();
        }
    }

    public bool HasItem(InventoryItem item) {
		foreach (var i in items) {
			if (i == item) {
				return true;
			}
		}

		return false;
	}

	public int CountItem(InventoryItem item) {
		var count = 0;

		foreach (var i in items) {
			if (i == item) {
				count++;
			}
		}

		return count;
	}

	public void GiftItem(InventoryItem item) {
		items.Add(item);
	}

	public void RemoveItem(InventoryItem item) {
		items.Remove(item);
	}

	public bool TryCombine(InventoryItem partA, InventoryItem partB) {
		foreach (var combination in possibleCombinations) {
			if (combination.partA == partA && combination.partB == partB || combination.partA == partB && combination.partB == partA) {
				RemoveItem(partA);
				RemoveItem(partB);
				foreach (var newItem in combination.result) {
					GiftItem(newItem);
				}

				return true;
			}
		}

		return false;
	}

	public void ShowUI() {
		if (canvas != null) {
			canvas.Show(this);
		}
	}
}
