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
   
    public void Update()
    {
    }

    public void NewGame()
    {
        if (PlayerStatic.PlayerInstance)
        {
            Destroy(PlayerStatic.PlayerInstance);
        }
        FungusSaver.Instance.ClearSave();
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

}
