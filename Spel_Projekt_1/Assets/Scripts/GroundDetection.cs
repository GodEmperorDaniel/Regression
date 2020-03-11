using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GroundDetection : MonoBehaviour
{
	public List<SoundAndGround> sounds = new List<SoundAndGround>();
	[Min(0)]
	public int speed;
	public Animator ani;
	public Tilemap tileMap;
	private Vector3Int gridCoords;
	private GridLayout grid;
	private Tile tile;
	private GameObject player;

	private void Start()
	{
		InvokeRepeating("PlaySound", 0, speed);
		player = PlayerStatic.playerInstance;
		grid = tileMap.layoutGrid;
	}

	private void Update()
	{
		if (ani.GetBool("moving"))
		{
			gridCoords = grid.WorldToCell(player.transform.position);
			tile = tileMap.GetTile(gridCoords) as Tile;
			for (int i = 0; i < sounds.Count; i++)
			{
				if (tile.sprite == sounds[i].spriteOfTile)
				{
					PlaySound(sounds[i].nameOfSound);
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
public struct SoundAndGround
{
	public string nameOfSound;
	public Sprite spriteOfTile;
}