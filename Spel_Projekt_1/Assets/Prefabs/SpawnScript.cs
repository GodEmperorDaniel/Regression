using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    Vector3 SpawnPos;
    public string doorToSpawn;
    public Transform Player;
    void Start()
    {
        SpawnPosition();
        Player.transform.position = SpawnPos;
    }

    private Vector3 SpawnPosition()
    {
        SpawnPos = GameObject.FindWithTag(doorToSpawn).transform.position;
        Debug.Log("working");
        Debug.Log(SpawnPos);
        return SpawnPos;
    }
}
