using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GroundDetection : MonoBehaviour
{
	public List<SoundAndGround> sounds = new List<SoundAndGround>();

	public void CheckSound(Collider2D col)
	{
		for (int i = 0; i < sounds.Count; i++)
		{
			for (int j = 0; j < sounds[i].ColliderForSound.Count; j++)
			{
				if (col == sounds[i].ColliderForSound[j])
				{
					PlaySound(sounds[i].nameOfSound);
					break;
				}
			}
		}
	}

	private void PlaySound(string sound)
	{
		FMODUnity.RuntimeManager.PlayOneShot(sound);
	}
}

[System.Serializable]
public class SoundAndGround
{
	[FMODUnity.EventRef]
	public string nameOfSound;
	public List<Collider2D> ColliderForSound = new List<Collider2D>();
}