using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    [SerializeField] Transform followTarget = null;
    void Awake() {
        if (followTarget == null) gameObject.GetComponent<SimpleFollow>().enabled = false;
    }

    void Update() {
        transform.position = followTarget.position;
    }
}

