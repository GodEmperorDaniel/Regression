using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[DisallowMultipleComponent]
public class Inventory : MonoBehaviour, ISaveable
{
	public List<ItemCombination> possibleCombinations;
	public GameObject uiPrefab;

	protected static InventoryCanvas canvas;
	[SerializeField]
	protected List<InventoryItem> _items;

	public int Count { get { return _items.Count; } }
	public InventoryItem[] Items { get { return _items.ToArray(); } }

	ItemAddedText itemAddedText; // Simons skit, remove if bad

    public void Start()
    {
		itemAddedText = GameObject.FindObjectOfType<ItemAddedText>(); // Simons skit, remove if bad

		if (!canvas && uiPrefab != null)
        {
            canvas = Instantiate(uiPrefab).GetComponentInChildren<InventoryCanvas>();
        }

		for (var i = _items.Count - 1; i >= 0; i--) {
			var item = _items[i];
			if (item == null) {
				_items.RemoveAt(i);
			} else {
				CheckId(item);

				_items[i] = Instantiate(item);
				_items[i].itemId = item.itemId;
			}
		}
	}
    
    public bool HasItem(InventoryItem item) {
		CheckId(item);
		foreach (var i in _items) {
			if (i.itemId == item.itemId) {
				return true;
			}
		}

		return false;
	}
    
    public int CountItem(InventoryItem item) {
		var count = 0;

		foreach (var i in _items) {
			if (i.itemId == item.itemId) {
				count++;
			}
		}

		return count;
	}
    
    public void GiftItem(InventoryItem item) {
		CheckId(item);
		var newItem = Instantiate(item);
		newItem.itemId = item.itemId;
		_items.Add(newItem);
		itemAddedText?.DisplayText(); // Simons skit, remove if bad
	}
    
    public bool RemoveItem(InventoryItem item) {
		CheckId(item);
		for (var i = 0; i < _items.Count; i++) {
			if (item.itemId == _items[i].itemId) {
				_items.RemoveAt(i);
				return true;
			}
		}

		return false;
	}
    
    public bool TryCombine(InventoryItem partA, InventoryItem partB) {
		CheckId(partA);
		CheckId(partB);
		foreach (var combination in possibleCombinations) {
			CheckId(combination.partA);
			CheckId(combination.partB);

			if (combination.partA.itemId == partA.itemId && combination.partB.itemId == partB.itemId || combination.partA.itemId == partB.itemId && combination.partB.itemId == partA.itemId) {
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
		if (canvas) {
			canvas.Show(this);
		}
	}

	private void CheckId(InventoryItem item) {
		if (item != null && item.itemId == 0) {
			item.itemId = item.GetInstanceID();
		}
	}

	public byte[] Save() {
		var data = new byte[4 * (1 + Count)];

		var count = BitConverter.GetBytes(Count);

		count.CopyTo(data, 0);

		for (var i = 0; i < Count; i++) {
			var itemId = BitConverter.GetBytes(_items[i].itemId);
			itemId.CopyTo(data, 4 * (i + 1));
		}

		return data;
	}

	public void Load(byte[] data, int version) {
		var allItems = Resources.LoadAll<InventoryItem>("Items");

		var count = BitConverter.ToInt32(data,0);

		_items = new List<InventoryItem>(count);

		for (var i = 0; i < count; i++) {
			var itemId = BitConverter.ToInt32(data, 4 * (i + 1));
			foreach (var item in allItems) {
				if (item.itemId == itemId || item.GetInstanceID() == itemId) {
					GiftItem(item);

					break;
				}
			}
		}
	}

	public void ClearSave() {}
}
