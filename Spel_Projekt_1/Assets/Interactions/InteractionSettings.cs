using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct InteractionSettings
{
	public bool Active;
	public string text;

	public ImageSettings image;
	public AudioClip audioClip;
	public OtherEvents moreEvents;
}

[System.Serializable]
public struct ImageSettings
{
	public Image screenImage;
	public float fadeTimer;
	public float imageTimer;
}

[System.Serializable]
public struct OtherEvents
{
	public bool enableOrDisable;
	public GameObject gObject;
}
