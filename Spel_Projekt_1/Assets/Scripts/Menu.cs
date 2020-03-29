using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
[DisallowMultipleComponent]
public class Menu : MonoBehaviour
{
    public Selectable firstSelected;
    public GameObject startMenyUi;
    public EventSystem eventSystem;
    private Selectable lastSelectedButton = null;
    public void Update()
    {
        AlwaysSelected();
    }

    public void NewGame()
    {
        if (PlayerStatic.PlayerInstance)
        {
            Destroy(PlayerStatic.PlayerInstance);
        }
		GameSaveManager.instance.ClearSave();
        SceneManager.LoadScene("Start");
    }

    public void Options()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadScene("BT_CREDITS");
    }



    private void AlwaysSelected()
    {
        if (!eventSystem.currentSelectedGameObject)
        {
            lastSelectedButton.Select();
        }
        else
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
        }
    }

}
