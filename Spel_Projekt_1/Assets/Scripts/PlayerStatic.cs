using UnityEngine;

public class PlayerStatic : MonoBehaviour
{
    public static Transform player;
    void Start()
    {
        player = gameObject.transform;
    }
}
