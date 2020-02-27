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
    public EventSystem eventSystem;
    private Selectable lastSelectedButton = null;

    SavePlayerPos PlayerPosData;

    private void Start()
    {
        PlayerPosData = FindObjectOfType<SavePlayerPos>();
    }
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
        PlayerStatic.ResumePlayer("pause");
        GameIsPaused = false;
    }

    void Pause()
    {
        if (eventSystem == null)
        {
            eventSystem = EventSystem.current;
        }
        pauseMenyUi.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
        lastSelectedButton = null;
        PlayerStatic.FreezePlayer("pause");
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
        PlayerPosData.PlayerPosSave();
        SceneManager.LoadScene("BT_MAINMENU");
    }

    private void AlwaysSelected()
    {
        foreach (Selectable button in Button.allSelectablesArray)
        {
            if (button.gameObject == eventSystem.currentSelectedGameObject)
            {
                lastSelectedButton = button;
            }
        }
        if (!lastSelectedButton)
        {
            lastSelectedButton = eventSystem.firstSelectedGameObject.GetComponent<Selectable>();
        }
        if (!EventSystem.current.alreadySelecting)
        {
            lastSelectedButton.Select();
        }
    }


}
