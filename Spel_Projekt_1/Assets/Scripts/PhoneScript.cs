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
        for (int i = 0; i < sceneAndNumber.Count; i++)
        {
            if (phonedNumber == sceneAndNumber[i].numberCombinations)
            {
                PlayerStatic.freezePlayer = false;
                SceneManager.LoadScene(sceneAndNumber[i].nameOfNextScene);
            }
            else
            {
                Debug.Log("No signal on this number... Try another");
            }
        }
        phonedNumber = null;
    }
}

[System.Serializable]
public struct numberAndScene
{
    public string numberCombinations;

    public string nameOfNextScene;
}
