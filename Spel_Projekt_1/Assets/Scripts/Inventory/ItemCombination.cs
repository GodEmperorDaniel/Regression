using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemCombination {
	public InventoryItem partA;
	public InventoryItem partB;
	public List<InventoryItem> result;
}
