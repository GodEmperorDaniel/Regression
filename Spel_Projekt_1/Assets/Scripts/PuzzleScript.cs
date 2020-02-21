using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PuzzleScript : MonoBehaviour
{
    [SerializeField] protected Button startButton = null;

    [SerializeField] protected string nameOfThisScene;

	[SerializeField] protected TextMeshProUGUI showPuzzleGuess = null;

    [SerializeField] protected List<puzzleAndScene> sceneAndNumber = new List<puzzleAndScene>();

    private string puzzleCombination;

	private GameObject lastSelectedButton = null;


    private void Start()
    {
        startButton.Select();
    }

    void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PlayerStatic.freezePlayer = false;
			SceneManager.UnloadSceneAsync(nameOfThisScene);
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

	public void AddSceneAndPuzzle(puzzleAndScene number)
    {
        sceneAndNumber.Add(number);
    }

    public void RemoveSceneAndPuzzle(puzzleAndScene number)
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
            if (puzzleCombination == sceneAndNumber[i].puzzleSolution)
            {
                PlayerStatic.freezePlayer = false;
                SceneManager.LoadScene(sceneAndNumber[i].nameOfNextScene);
            }
            else
            {
                Debug.Log("No signal on this number... Try another");
            }
        }
        puzzleCombination = null;
    }
}

[System.Serializable]
public struct puzzleAndScene
{
    public string puzzleSolution;

    public string nameOfNextScene;
}
