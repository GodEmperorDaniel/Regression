using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[AttributeUsage(AttributeTargets.Field)]
public class DefaultSolution : Attribute {

}

[Serializable]
public struct PuzzleSolution {
	public string solution;

	public Flowchart flowchart;

	[HideInInspector]
	public Block block;

	[HideInInspector]
	public StringVariable output;

#if UNITY_EDITOR
	//Helpers for the custom Property Drawer
	[HideInInspector]
	public bool _foldout;
	[HideInInspector]
	public bool _showPopup;
	[HideInInspector]
	public bool _showVariablePopup;
	[HideInInspector]
	public bool _defaultSolution;
#endif
}
