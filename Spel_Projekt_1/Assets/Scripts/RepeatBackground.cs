using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    [SerializeField] GameObject bgParent = null;
    Camera cam = null;
    Vector2 cameraBorders = Vector2.zero;

    void Awake() {
        cam = gameObject.GetComponent<Camera>();
        cameraBorders = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
    }

    void LateUpdate() {
        if (bgParent == null) return;
        Transform[] _bgs = bgParent.GetComponentsInChildren<Transform>();
        if (_bgs.Length > 1) {
            Transform _firstSibling = _bgs[1];
            Transform _lastSibling = _bgs[_bgs.Length - 1];
            float _offset = _lastSibling.GetComponent<SpriteRenderer>().bounds.extents.x; // Returns half sprite width, used to find right edge.
            if (transform.position.x + cameraBorders.x > _lastSibling.position.x + _offset) { // If camera goes past rightmost edge, reorder children and move furthest bg to the front.
                _firstSibling.SetAsLastSibling();
                _firstSibling.position = new Vector3(_lastSibling.position.x + (_offset * 2f), _lastSibling.position.y, _lastSibling.position.z);
            }
        }
    }
}
