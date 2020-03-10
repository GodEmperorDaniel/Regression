using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class MaterialAnimator : MonoBehaviour
{
	public Material material;
	public Vector2 waterMovement;
	public float delay = 2;

	private SpriteRenderer _spr;
	//[SerializeField][HideInInspector] private Material _mat;
	private Vector2 offset;

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
		Graphics.Blit(source, destination, material);
	}

	private void OnEnable() {
		_spr = GetComponent<SpriteRenderer>();
		if (_spr && !material) {
			material = _spr.material;
		}

		if (material) {
			/*if (material != _mat) {
				material = Instantiate(material);
				_mat = material;
			}*/

			material.SetFloat("_startT", Time.time + delay);
			material.SetFloat("_screenRatio", Camera.main.aspect);
		}
	}

	/*private void OnValidate() {
		if (material && material != _mat) {
			material = Instantiate(material);
			_mat = material;
			if (_spr) {
				_spr.material = material;
			}
		}
	}*/

	private void Update() {
		offset += Time.deltaTime * waterMovement;
		if (material && material.HasProperty("_NormalTex")) {
			material.SetTextureOffset("_NormalTex", offset);

		}
	}
}
