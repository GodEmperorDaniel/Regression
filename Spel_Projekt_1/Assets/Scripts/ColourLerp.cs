using UnityEngine;
using System.Collections;

public class ColourLerp : MonoBehaviour
{
    public Color A;
    public Color B;
    public float speed = 1.0f;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.color = Color.Lerp(A, B, Mathf.PingPong(Time.time * speed, 1.0f));
    }
}