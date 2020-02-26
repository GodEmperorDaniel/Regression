using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenyUi;
    public GameObject lastSelectedButton = null;

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
        AlwaysSelected();
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
        PlayerStatic.playerInstance.GetComponent<Inventory>().ShowUI();
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
        SceneManager.LoadScene("BT_MAINMENU");
        Application.Quit();
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


}
