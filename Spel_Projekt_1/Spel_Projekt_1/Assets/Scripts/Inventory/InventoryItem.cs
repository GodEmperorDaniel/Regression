using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Inventory Item")]
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
public class InventoryItem : ScriptableObject {
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
	public string title;
	public string description;
	public Sprite sprite;
	[HideInInspector]
	public int itemId;

	public static bool operator ==(InventoryItem item1, InventoryItem item2) {
		var o1 = (object)item1;
		var o2 = (object)item2;

		if (o1 == null && o2 == null) {
			return true;
		} else if (o1 == null || o2 == null) {
			return false;
		} else {
			throw new InvalidOperationException("Items should not be directly compared. Compare their itemId instead. If you do intend to compare directly, use Equals() instead.");
		}
	}

	public static bool operator !=(InventoryItem item1, InventoryItem item2) {
		var o1 = (object)item1;
		var o2 = (object)item2;

		if (o1 == null && o2 == null) {
			return false;
		} else if (o1 == null || o2 == null) {
			return true;
		} else {
			throw new InvalidOperationException("Items should not be directly compared. Compare their itemId instead. If you do intend to compare directly, use Equals() instead.");
		}
	}
}
