using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenyUi;

    public static CharacterController2d player;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }
    }

    public void Resume()
    {
        pauseMenyUi.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenyUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadInventory()
    {
        //Time.timeScale = 0f;
        //player = playerInstance.GetComponent<Inventory>().ShowUi();
    }

    //public void SaveGame()
    //{
    //    Debug.Log("Save game, please wait");
    //}

    //public void LoadGame()
    //{
    //    Debug.Log("Load Game...");
        
    //}

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }




}
