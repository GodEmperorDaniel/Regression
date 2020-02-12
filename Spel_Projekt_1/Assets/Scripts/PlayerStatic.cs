using UnityEngine;

public class PlayerStatic : MonoBehaviour
{
    public static Transform player;
    public static int DoorIndex;
    void Awake()
    {
		player = gameObject.transform;
		DontDestroyOnLoad(gameObject);
    }
}
