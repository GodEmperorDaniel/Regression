
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
[DisallowMultipleComponent]
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenyUi;
	public Selectable firstSelected;
	[Header("Music Parameters")]
	public GameObject inMenu;
	public GameObject outMenu;

	private EventSystem eventSystem;
    private Selectable lastSelectedButton = null;

    // Update is called once per frame
    void Update() {
		if (!eventSystem) {
			eventSystem = EventSystem.current;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
				FixSettings();
                Resume();
            } else if (!PlayerStatic.IsFrozen()) {
                Pause();
            }
        }
		if (GameIsPaused) {
			AlwaysSelected();
		}
    }

    public void Resume()
    {
        pauseMenyUi.SetActive(false);
		FixSettings();
        PlayerStatic.ResumePlayer("pause");
        GameIsPaused = false;
    }

    void Pause()
    {
        FixSettings();
		pauseMenyUi.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
        lastSelectedButton = null;
        PlayerStatic.FreezePlayer("pause");
        GameIsPaused = true;
    }

    public void LoadInventory()
    {
        PlayerStatic.playerInstance.GetComponent<Inventory>().ShowUI();
    }


    
    public void BackToMainMenu()
    {
		Destroy(PlayerStatic.playerInstance);
        SceneManager.LoadScene("BT_MAINMENU");
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
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
            lastSelectedButton = firstSelected;
        }
        if (!EventSystem.current.alreadySelecting)
        {
            lastSelectedButton.Select();
        }
    }

    void FixSettings()
    {
        if (inMenu && outMenu)
        {
            inMenu.SetActive(!inMenu.activeSelf);
            outMenu.SetActive(!outMenu.activeSelf);
        }
    }
}
