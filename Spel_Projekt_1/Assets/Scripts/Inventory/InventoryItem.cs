using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Inventory Item")]
public class InventoryItem : ScriptableObject
{
	public string name;
	public string description;
	public Sprite sprite;

    public int items { get; internal set; }

}
