using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newButtonNav : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    private List<Button> activeButtons = new List<Button>();
    private Navigation navi;
    private void OnEnable()
    {
        activeButtons = new List<Button>();

        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].gameObject.activeSelf)
            {
                activeButtons.Add(buttons[i]);
            }
        }

        for (int i = 0; i < activeButtons.Count; i++)
        {
            navi.mode = Navigation.Mode.Explicit;
            if (i == 0)
            {
                navi.selectOnDown = activeButtons[1];
                navi.selectOnUp = activeButtons[activeButtons.Count - 1];
            }
            else if (i == activeButtons.Count - 1)
            {
                navi.selectOnDown = activeButtons[0];
                navi.selectOnUp = activeButtons[i-1];
            }
            else
            {
                navi.selectOnDown = activeButtons[i+1];
                navi.selectOnUp = activeButtons[i-1];
            }
                activeButtons[i].navigation = navi;
        }
    }
}
