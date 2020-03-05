/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Fungus;

public class PuzzleScript : MonoBehaviour {
	[SerializeField] protected Button startButton = null;

	[SerializeField] protected TextMeshProUGUI showPuzzleGuess = null;

	[SerializeField] protected List<PuzzleAndScene> sceneAndNumber = new List<PuzzleAndScene>();

	[Range(1, 10)]
	[SerializeField] protected int maxCharacters = 9;

	private string puzzleCombination = null;

	private GameObject lastSelectedButton = null;


	private void OnEnable() {
		startButton.Select();
		PlayerStatic.FreezePlayer("Puzzle");
	}

	private void OnDisable() {
		PlayerStatic.ResumePlayer("Puzzle");
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			gameObject.SetActive(false);
			return;
		}

		AlwaysSelected();

		if (showPuzzleGuess && !string.IsNullOrEmpty(puzzleCombination)) {
			showPuzzleGuess.text = puzzleCombination;
		}
	}

	private void AlwaysSelected() {
		foreach (Selectable button in Button.allSelectablesArray) {
			if (button.gameObject == EventSystem.current.currentSelectedGameObject) {
				lastSelectedButton = button.gameObject;
			}
		}
		if (!EventSystem.current.alreadySelecting) {
			EventSystem.current.SetSelectedGameObject(lastSelectedButton);
		}
	}

	public void AddSceneAndPuzzle(PuzzleAndScene number) {
		sceneAndNumber.Add(number);
	}

	public void RemoveSceneAndPuzzle(PuzzleAndScene number) {
		sceneAndNumber.Remove(number);
	}

	public void AddPuzzlePiece(string puzzleCharacter) {
		if (string.IsNullOrEmpty(puzzleCombination)) {
			puzzleCombination += puzzleCharacter;
		} else if (puzzleCombination.Length < maxCharacters) {
			puzzleCombination += puzzleCharacter;
		}

	}

	public void CheckNumber() {
		for (int i = 0; i < sceneAndNumber.Count; i++) {
			sceneAndNumber[i].flowchart.ExecuteBlock(sceneAndNumber[i].block);
		}
		puzzleCombination = null;
	}

	public string PassCombination() {
		return puzzleCombination;
	}

	private void CallBlock(InteractionSettings settings) {
		if (settings.flowchart && settings.block) {
			settings.flowchart.ExecuteBlock(settings.block);
		}
	}
}
*/