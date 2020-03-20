using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEyes : MonoBehaviour
{

    Animator anim = null;
    Transform player = null;

    bool eyesOpen = false;

    [Header("After what x-position should the eyes open?")]
    int eyeOpenPos = 40;

    [Header("How often should the eyes blink?")]
    [SerializeField] float blinkFrequency = 10f;
    float timeSinceLastBlink = 0f;

    void Awake() {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void Update() {
        OpenEyesAfterAWhile();
        Blink();
    }

    private void OpenEyesAfterAWhile() {
        if (eyesOpen) return;
        if (player.position.x > eyeOpenPos) {
            anim.SetTrigger("open");
            eyesOpen = true;
        }
    }

    private void Blink() {
        if (!eyesOpen) return;
        if (timeSinceLastBlink >= blinkFrequency) {
            anim.SetTrigger("blink");
            timeSinceLastBlink = 0f;
        } else {
            timeSinceLastBlink += Time.deltaTime;
        }
    }
}
