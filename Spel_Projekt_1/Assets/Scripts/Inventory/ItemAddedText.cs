using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemAddedText : MonoBehaviour
{
    [SerializeField] float fadeSpeed = 1f;
    [SerializeField] float textAliveTime = 2f;
    [SerializeField] TextMeshProUGUI text;
	private static ItemAddedText _instance;
	private InventoryItem _item;
    private string defaultText;

    void Awake() {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
		_instance = this;
        defaultText = text.text;
	}

	public static void DisplayItem(InventoryItem item) {
		if (_instance) {
			_instance._item = item;
			_instance.DisplayText();
		}
	}

	public void DisplayText() {
        text.text = _instance._item.title + defaultText;
		StartCoroutine(FadeText(textAliveTime, fadeSpeed, text));
	}

	private IEnumerator FadeText(float textAliveTime, float fadeSpeed, TextMeshProUGUI text) {
        yield return FadeIn(fadeSpeed, text);
        yield return new WaitForSeconds(textAliveTime);
        yield return FadeOut(fadeSpeed, text);
    }

    private IEnumerator FadeIn(float fadeSpeed, TextMeshProUGUI text) {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        while (text.color.a < 1.0f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (fadeSpeed * Time.deltaTime));
            yield return null;
        }
    }
    private IEnumerator FadeOut(float fadeSpeed, TextMeshProUGUI text) {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        while (text.color.a > 0.0f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (fadeSpeed * Time.deltaTime));
            yield return null;
        }
    }

}
