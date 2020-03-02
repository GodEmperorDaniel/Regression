using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[System.Serializable]
public struct InteractionSettings
{
	public bool Active;
	public float interactableAngle;
	public float interactableAngleDot;
	//public string text;

	public Flowchart flowchart;

	[HideInInspector]
	public Block block;

	//Helpers for the custom Property Drawer
	[HideInInspector]
	public bool _foldout;
	[HideInInspector]
	public bool _showPopup;
}

