using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindScript : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public Text up, left, down, right, interactionButton, inventoryButton;
    void Start()
    {
        keys.Add("Up", KeyCode.W);
        keys.Add("Down", KeyCode.S);
        keys.Add("Left", KeyCode.S);
        keys.Add("Right", KeyCode.R);
        keys.Add("InteractionButton", KeyCode.E);
        keys.Add("InventoryButton", KeyCode.Q);

        up.text = keys["Up"].ToString();
        down.text = keys["Down"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        interactionButton.text = keys["InteractionButton"].ToString();
        inventoryButton.text = keys["InventoryButton"].ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
