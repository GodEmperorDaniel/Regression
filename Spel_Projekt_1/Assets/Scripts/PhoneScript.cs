using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhoneScript : MonoBehaviour
{
    [SerializeField] protected Button startButton = null;

    [SerializeField] protected string nameOfThisScene;

    [SerializeField] protected List<numberAndScene> sceneAndNumber = new List<numberAndScene>();

    [SerializeField] private string phonedNumber;


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
    }

    public void AddSceneAndNumber(numberAndScene number)
    {
        sceneAndNumber.Add(number);
    }

    public void RemoveSceneAndNumber(numberAndScene number)
    {
        sceneAndNumber.Remove(number);
    }

    public void AddNumber(int number)
    { 
        phonedNumber += number.ToString();
    }

    public void CheckNumber()
    {
        bool match = false;
        for (int i = 0; i < sceneAndNumber.Count; i++)
        {
            if (!match && phonedNumber == sceneAndNumber[i].numberCombinations)
            {
                match = true;
                Debug.Log("You got a signal!");
                SceneManager.LoadScene(sceneAndNumber[i].nameOfNextScene);
            }
            else if (match)
            {
                break;
            }
        }
        if (!match)
        {
            Debug.Log("No signal on this number... Try another");
            phonedNumber = null;
        }
    }
}

[System.Serializable]
public struct numberAndScene
{
    public string numberCombinations;

    public string nameOfNextScene;
}
