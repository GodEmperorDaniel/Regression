using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Fungus;

public class PuzzleScript : MonoBehaviour {
	public Selectable startButton;
	public TMP_Text showPuzzleGuess;
	public List<PuzzleSolution> solutions = new List<PuzzleSolution>();
	[DefaultSolution]
	public PuzzleSolution defaultSolution = new PuzzleSolution();
	[Min(1)]
	public int maxCharacters = 9;
	public string escapeAxis = "Cancel";
	public string previewString = "";

    private string puzzleCombination = "";
	private Selectable lastSelectedButton = null;
	private bool escapePressed;

    private void OnEnable() {
		if (startButton) {
			startButton.Select();
		}
		puzzleCombination = string.Empty;
		PlayerStatic.FreezePlayer("Puzzle Script");
	}

	private void OnDisable() {
		PlayerStatic.ResumePlayer("Puzzle Script");
	}

	void Update()
	{
		if (Input.GetAxisRaw(escapeAxis) > 0) {
			if (!escapePressed) {
				Cancel();
			}
		} else {
			escapePressed = false;
		}

		AlwaysSelected();

		if (showPuzzleGuess)
		{
			if (string.IsNullOrEmpty(puzzleCombination)) {
				showPuzzleGuess.text = previewString;
			} else {
				showPuzzleGuess.text = puzzleCombination;
			}
		}
	}

	private void AlwaysSelected()
	{
		foreach (Selectable button in Button.allSelectablesArray)
		{
			if (button.gameObject == EventSystem.current.currentSelectedGameObject)
			{
				lastSelectedButton = button;
			}
		}
		if (!EventSystem.current.alreadySelecting && lastSelectedButton)
		{
			lastSelectedButton.Select();
		}
	}

	public void Cancel() {
		gameObject.SetActive(false);
	}

	public void AddPuzzlePiece(string puzzleCharacter) {
		if (puzzleCombination.Length < maxCharacters) {
			puzzleCombination += puzzleCharacter;
		}
	}

	public void RemovePuzzlePiece(string puzzleCharacter) {
		if (puzzleCombination.Length < maxCharacters) {
			puzzleCombination += puzzleCharacter;
		}
	}

	public void CheckNumber() {
		for (int i = 0; i < solutions.Count; i++) {
			if (puzzleCombination == solutions[i].solution) {
				RunSolution(solutions[i]);
				return;
			}
		}

		RunSolution(defaultSolution);
	}

	public string PassCombination()
	{
		return puzzleCombination;
	}

	private void RunSolution(PuzzleSolution solution)
	{
		if (solution.flowchart && solution.block)
		{
			solution.flowchart.ExecuteBlock(solution.block);
		}
	}
}


