using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAnim : MonoBehaviour {
	public Vector2 waterMovementLayer1;
	//public Vector2 waterMovementLayer2;
	private Material mat;
	private Vector2 offset1;
	//private Vector2 offset2;

	// Start is called before the first frame update
	void Start()
    {
		mat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update() {
		offset1 += waterMovementLayer1 * Time.deltaTime;
		//offset2 += waterMovementLayer2 * Time.deltaTime;
		mat.SetTextureOffset("_NormalTex", offset1);
		//mat.SetTextureOffset("_NormalTex2", offset2);
	}
}
