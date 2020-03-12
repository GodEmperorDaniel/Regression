using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleEyeController : MonoBehaviour
{
    Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            anim.SetBool("playerInRange", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            anim.SetBool("playerInRange", false);
        }
    }
}
