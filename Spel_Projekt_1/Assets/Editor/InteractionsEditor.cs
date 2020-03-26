using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(Interactions))]
public class InteractionsEditor : Editor {
	void OnSceneGUI() {
		Interactions myObj = (Interactions)target;

		float r = 0.5f;

		r = Draw(myObj, myObj.OnEnter, r);
		r = Draw(myObj, myObj.OnKeyPressed, r);
		r = Draw(myObj, myObj.OnStay, r);
		r = Draw(myObj, myObj.OnExit, r);
		foreach (var s in myObj.OnItemUse) {
			r = Draw(myObj, s.interaction, r);
		}

		//myObj.shieldArea = (float)Handles.ScaleValueHandle(myObj.shieldArea, myObj.transform.position + myObj.transform.forward * myObj.shieldArea, myObj.transform.rotation, 1, Handles.ConeCap, 1);
	}

	float Draw(Interactions myObj, InteractionSettings settings, float r) {
		if (!settings.Active) {
			return r;
		}

		if (settings.interactableAngle > 0) {
			float d = -0.5f * Mathf.PI;
			var p = myObj.transform.position + (Vector3)settings.offset;

			if (PlayerStatic.ControllerInstance) {
				var pl = PlayerStatic.ControllerInstance.transform.position;
				d = Mathf.Atan2(p.y - pl.y, p.x - pl.x);
				var invD = Mathf.Atan2(pl.y - p.y, pl.x - p.x);
				Handles.color = new Color32(0, 255, 0, 64);
				Handles.DrawSolidArc(p,Vector3.forward, new Vector3(Mathf.Cos(invD - Mathf.Deg2Rad * settings.interactableAngle * 0.5f), Mathf.Sin(invD - Mathf.Deg2Rad * settings.interactableAngle * 0.5f)), settings.interactableAngle, r * 0.5f);
				p = pl;
			}

			Handles.color = new Color32(255, 0, 0, 64);
			Handles.DrawSolidArc(p, Vector3.forward, new Vector3(Mathf.Cos(d - Mathf.Deg2Rad * settings.interactableAngle * 0.5f), Mathf.Sin(d - Mathf.Deg2Rad * settings.interactableAngle * 0.5f)), settings.interactableAngle, r);
		}

		return r + 0.125f;
	}
}
