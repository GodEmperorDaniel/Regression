using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShot : MonoBehaviour
{
    public List<sound> listOfSounds = new List<sound>();
    public void PlaySound(int indexOfSound)
    {
        FMODUnity.RuntimeManager.PlayOneShot(listOfSounds[indexOfSound].nameOfSound);
    }

    [System.Serializable]
    public struct sound
    {
        [FMODUnity.EventRef]
        public string nameOfSound;
    }
}
