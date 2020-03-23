using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawns : MonoBehaviour
{
    public List<Chances> randomSpawns = new List<Chances>();

    private int rnd;

    void Update()
    {
        for (int i = 0; i < randomSpawns.Count; i++)
        {
            if (randomSpawns[i].keepTime > randomSpawns[i].timeRange)
            {
                randomSpawns[i].keepTime = 0;
                rnd = Random.Range(1, 100);

                if (randomSpawns[i].chance >= rnd)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(randomSpawns[i].nameOfSound);
                }
            }
            else
            {
                randomSpawns[i].keepTime = randomSpawns[i].keepTime + Time.deltaTime;
            }
        }
    }
}


[System.Serializable]

public class Chances
{
    [Tooltip("The timerange used to calculate the sound, in seconds")]
    [Min(1)]
    public int timeRange;

    [Tooltip("Chance for sound to spawn in designated 'Time Range'")]
    [Range(1, 100)]
    [Min(1)]
    public int chance;

    [FMODUnity.EventRef]
    public string nameOfSound;

    [HideInInspector]
    public float keepTime = 0;
}
