using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Fungus;

public class PuzzleScript : MonoBehaviour {
	[Header("Puzzle Logic")]
	[Tooltip("These are combinations of the puzzle that each runs a certain Fungus behaviour when entered.\nThe combination entered is stored in the optional string variable.")]
	public List<PuzzleSolution> solutions = new List<PuzzleSolution>();
	[DefaultSolution]
	[Tooltip("This is the default Fungus behaviour to run when none of the above combinations match.\nThe combination entered is stored in the optional string variable.")]
	public PuzzleSolution defaultSolution = new PuzzleSolution();
	[Min(1)]
	public int maxCharacters = 9;
	[DefaultSolution]
	[Tooltip("This is the Fungus behaviour to run when the index of the next puzzle piece to enter is changed.\nThe index is converted to a string and stored in the optional string variable.")]
	public PuzzleSolution onIndexSet;
	[DefaultSolution]
	[Tooltip("This is the Fungus behaviour to run when a puzzle piece is added.\nThe piece is stored in the optional string variable.\nUse On Index Set to determine the position of the puzzle piece.")]
	public PuzzleSolution onPuzzlePieceAdded;

	[Header("Interface and Appearance")]
	public string escapeAxis = "Cancel";
	public bool resetPuzzleOnCancel = true;
	public string previewString = "";
	public Selectable startButton;
	public TMP_Text showPuzzleGuess;
	public List<GameObject> autoEnable;
	public List<GameObject> autoDisable;
	public bool clearOnCheck = true;

	private string puzzleCombination = "";
	private Selectable lastSelectedButton = null;
	private bool escapePressed;
	private int selectedIndex;

	private int hour;
	private int minute;

	[Header("Eyepuzzle")]
	public List<GameObject> allButtons = new List<GameObject>();

	private void OnEnable()
	{
		StartCoroutine(WaitOnGui());
	}

	private IEnumerator WaitOnGui()
	{
		yield return new WaitForEndOfFrame();

		if (startButton)
		{
			if (EventSystem.current)
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
			startButton.Select();
		}
		foreach (var o in autoEnable)
		{
			o.SetActive(true);
		}
		foreach (var o in autoDisable)
		{
			o.SetActive(false);
		}

		if (resetPuzzleOnCancel)
		{
			puzzleCombination = string.Empty;
		}

		PlayerStatic.FreezePlayer("Puzzle Script");
	}

	private void OnDisable() {
		PlayerStatic.ResumePlayer("Puzzle Script");
	}

	void Update()
	{
		if (Input.GetAxisRaw(escapeAxis) > 0)
		{
			if (!escapePressed)
			{
				Cancel();
			}
		}
		else
		{
			escapePressed = false;
		}

		AlwaysSelected();

		if (showPuzzleGuess)
		{
			if (string.IsNullOrEmpty(puzzleCombination))
			{
				showPuzzleGuess.text = previewString;
			}
			else
			{
				showPuzzleGuess.text = puzzleCombination;
			}
		}
		if (puzzleCombination.Length >= maxCharacters)
		{
			CheckNumber();
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
			RunSolution(onIndexSet, puzzleCombination.Length.ToString());
			puzzleCombination += puzzleCharacter;
			RunSolution(onPuzzlePieceAdded, puzzleCharacter);
		}
	}
	public void SetCurrentIndex(int index) {
		selectedIndex = index;
		RunSolution(onIndexSet, index.ToString());
	}

	public void SetPuzzlePieceAtPreviouslySelectedIndex(string puzzleCharacter) {
		Debug.Log(puzzleCombination);
		puzzleCombination = puzzleCombination.PadRight(selectedIndex + 1);
		var charArray = puzzleCombination.ToCharArray();
		charArray[selectedIndex] = puzzleCharacter[0];
		puzzleCombination = new string(charArray);
		RunSolution(onPuzzlePieceAdded, puzzleCharacter);
		Debug.Log(puzzleCombination);
	}

	public void RemovePuzzlePiece() {
		if (puzzleCombination.Length > 0) {
			puzzleCombination = puzzleCombination.Remove(puzzleCombination.Length - 1);
		}
	}

	public void RemovePuzzlePiece(int index) {
		if (puzzleCombination.Length > index) {
			puzzleCombination = puzzleCombination.Remove(index, 1);
		}
	}

	public void CheckNumber() {
		for (int i = 0; i < solutions.Count; i++)
		{
			if (puzzleCombination == solutions[i].solution)
			{
				puzzleCombination = "";
				RunSolution(solutions[i], puzzleCombination);
				return;
			}
		}
		if (clearOnCheck)
		{ 
			puzzleCombination = "";
		}
		RunSolution(defaultSolution, puzzleCombination);
	}

	public string PassCombination()
	{
		return puzzleCombination;
	}

	public void SelectAButton() {
		EventSystem.current.SetSelectedGameObject(null);
		for (int i = 0; i < allButtons.Count; i++)
		{
			if (allButtons[i].transform.parent.gameObject.activeSelf && allButtons[i].activeSelf)
			{
				EventSystem.current.SetSelectedGameObject(allButtons[i]);
				break;
			}
		}
	}

	private void RunSolution(PuzzleSolution solution, string combination)
	{
		if (solution.flowchart)
		{
			if (solution.output) {
				solution.output.Value = combination;
			}

			if (solution.block) {
				solution.flowchart.ExecuteBlock(solution.block);
			}
		}
	}

	public void SetHour(int puzzlePiece)
	{
		hour += puzzlePiece;
		if (hour > 12)
		{
			hour = 1;
		}
		else if (hour < 1)
		{
			hour = 12;
		}

		FixTime();
	}
	public void SetMinute(int puzzlePiece)
	{
		minute += puzzlePiece;
		if (minute > 59)
		{
			minute = 0;
		}
		else if (minute < 0)
		{
			minute = 55;
		}

		FixTime();
	}

	private void FixTime()
	{
		puzzleCombination = hour.ToString() + ":" + minute.ToString();
		RunSolution(onIndexSet, hour.ToString());
		RunSolution(onPuzzlePieceAdded, minute.ToString());
	}
}


