using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[System.Serializable]
public struct InteractionSettings
{
	public bool Active;
	//public string text;

	public Flowchart flowchart;
	[HideInInspector]
	public Block block;

	//Helpers for the custom Property Drawer
	[HideInInspector]
	public bool _foldout;
	[HideInInspector]
	public bool _showPopup;

	//Commented out these for simplicity, as they are not used right now (same thing can be accomplished with Fungus) -EH
	//public ImageSettings image;
	//public AudioClip audioClip;
	//public OtherEvents moreEvents;
}

//[System.Serializable]
//public struct ImageSettings
//{
//	public Image screenImage;
//	public float fadeTimer;
//	public float imageTimer;
//}

//[System.Serializable]
//public struct OtherEvents
//{
//	public bool enableOrDisable;
//	public GameObject gObject;
//}
