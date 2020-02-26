using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Fungus;

public class PuzzleScript : MonoBehaviour
{
    [SerializeField] protected Button startButton = null;

	[SerializeField] protected TextMeshProUGUI showPuzzleGuess = null;

   [SerializeField] protected List<PuzzleAndScene> sceneAndNumber = new List<PuzzleAndScene>();

    private string puzzleCombination;

	private GameObject lastSelectedButton = null;


    private void Start()
    {
        startButton.Select();
		PlayerStatic.FreezePlayer("Puzzle");
	}

    void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PlayerStatic.ResumePlayer("Puzzle");
			SceneManager.UnloadSceneAsync(gameObject.scene);
		}

		AlwaysSelected();

		if (showPuzzleGuess != null)
		{
			showPuzzleGuess.text = puzzleCombination;
		}
	}

	private void AlwaysSelected()
	{
		foreach (Selectable button in Button.allSelectablesArray)
		{
			if (button.gameObject == EventSystem.current.currentSelectedGameObject)
			{
				lastSelectedButton = button.gameObject;
			}
		}
		if (!EventSystem.current.alreadySelecting)
		{
			EventSystem.current.SetSelectedGameObject(lastSelectedButton);
		}
	}

	public void AddSceneAndPuzzle(PuzzleAndScene number)
	{
		sceneAndNumber.Add(number);
	}

	public void RemoveSceneAndPuzzle(PuzzleAndScene number)
	{
		sceneAndNumber.Remove(number);
	}

	public void AddPuzzlePiece(string puzzleCharacter)
	{
		puzzleCombination += puzzleCharacter;
	}

	public void CheckNumber()
	{
		for (int i = 0; i < sceneAndNumber.Count; i++)
		{
			sceneAndNumber[i].flowchart.ExecuteBlock(sceneAndNumber[i].block);
		}
		puzzleCombination = null;
	}

	public string PassCombination()
	{
		return puzzleCombination;
	}

	private void CallBlock(InteractionSettings settings)
	{
		if (settings.flowchart && settings.block)
		{
			settings.flowchart.ExecuteBlock(settings.block);
		}
	}
}


