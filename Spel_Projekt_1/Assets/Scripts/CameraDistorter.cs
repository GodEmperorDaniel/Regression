using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraDistorter : MonoBehaviour
{
	public Material material;
	public float delay = 2;

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
		Graphics.Blit(source, destination, material);
	}

	private void OnEnable() {
		if (material) {
			material.SetFloat("_startT", Time.time + delay);
			material.SetFloat("_screenRatio", Camera.main.aspect);
		}
	}

	private void Update() {

	}
}
