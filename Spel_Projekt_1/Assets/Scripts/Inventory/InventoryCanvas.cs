using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class InventoryCanvas : MonoBehaviour
{
	[Header("References")]
	public GameObject cursor;
	public GameObject combineCursor;
	public InventoryPopup popup;
	public CanvasGroup canvasGroup;
	public List<Image> itemIcons = new List<Image>();

	[Header("Appearance")]
	public int rowLength;
	public float fadeInTime;
	public float fadeOutTime;

	[Header("Input")]
	public string horizontalAxis = "Horizontal";
	public string verticalAxis = "Vertical";
	public string selectAxis = "InteractionButton";
	public string closeAxis = "InventoryButton";
	public string closeAxis2 = "Cancel";
	[Range(0,1)]
	public float deadZone;

	public float delayBeforeKeyRepeat = 1;
	public float keyRepeatTime = 0.25f;

	[Header("Automatic Itemframe Generation")]
	[FormerlySerializedAs("itemFrame")]
	public GameObject itemFramePrefab;
	public float horizontalSpacing = 72;
	public float verticalSpacing = 80;
	public int minimumFramesShown;

	private int cursorPos;
	private Inventory _inventory;
	private float _delay = 0;
	private int _dirPressed = 0;
	private int _shownFrames;
	private bool _closeButtonPressed;
	private bool _selectButtonPressed;
	private bool _focused;
	private bool _combining;
	private int _combiningWith;
	private GameObject _currentCursor;
	private float _fadeInTimeLeft = 0;
	private float _fadeOutTimeLeft = 0;

	// Start is called before the first frame update
	void Start()
    {
		DontDestroyOnLoad(transform.parent.gameObject);
		Hide(true);
	}

	public void Show(Inventory inventory, bool skipFadeIn = false) {
		transform.parent.gameObject.SetActive(true);
		combineCursor.SetActive(false);
		PlayerStatic.FreezePlayer("Inventory");

		_currentCursor = cursor;
		_inventory = inventory;
		_shownFrames = Mathf.Max(minimumFramesShown, inventory.Count);

		for (var i = itemIcons.Count; i < inventory.Count; i++) {
			itemIcons.Add(CreateFrame(i));
		}

		for (var i = 0; i < itemIcons.Count; i++) {
			if (i < _shownFrames) {
				var frame = itemIcons[i];
				frame.rectTransform.parent.gameObject.SetActive(true);

				/*if (x >= rowLength) {
					x = 0;
					y++;
				}
				
				frame.rectTransform.parent.localPosition = new Vector3(xOffset + x * horizontalSpacing, yOffset - y * verticalSpacing, 0);
				x++*/

				if (i < inventory.Count) {
					frame.sprite = inventory.Items[i].sprite;
					frame.enabled = true;
				} else {
					frame.sprite = null;
					frame.enabled = false;
				};
			} else {
				itemIcons[i].transform.parent.gameObject.SetActive(false);
			}
		}

		if (!skipFadeIn) {
			_fadeInTimeLeft = fadeInTime;
		} else {
			canvasGroup.alpha = 1;
		}

		Focus();
		MoveCursor(0);
	}

	private void Update() {
		if (_focused) {
			MoveCursor();

			if (Input.GetAxisRaw(selectAxis) > deadZone) {
				if (!_selectButtonPressed) {
					_selectButtonPressed = true;

					if (cursorPos < _inventory.Count) {
						var item = _inventory.Items[cursorPos];

						if (_combining) {
							if (_combiningWith != cursorPos) {
								var otherItem = _inventory.Items[_combiningWith];
								_inventory.TryCombine(item, otherItem);
								Show(_inventory, true);//Check for any changes
							}
						} else {
							UnFocus();
							popup.Show(_inventory, this, item);
						}
					}

					if (_combining) {
						EndCombine();
					}
				}
			} else {
				_selectButtonPressed = false;
			}

			if (Input.GetAxisRaw(closeAxis) > deadZone || Input.GetAxisRaw(closeAxis2) > deadZone) {
				if (!_closeButtonPressed) {
					if (_combining) {
						EndCombine();
					} else {
						Hide();
					}
				}
			} else {
				_closeButtonPressed = false;
			}
		}

		if (_fadeInTimeLeft > 0 && canvasGroup) {
			_fadeInTimeLeft -= Time.deltaTime;
			canvasGroup.alpha = 1 - (_fadeInTimeLeft / fadeInTime);
		} else if (_fadeOutTimeLeft > 0 && canvasGroup) {
			_fadeOutTimeLeft -= Time.deltaTime;
			canvasGroup.alpha = _fadeOutTimeLeft / fadeInTime;
			if (_fadeOutTimeLeft <= 0) {
				Hide(true);
			}
		}
	}

	private void MoveCursor() {
		var v = Input.GetAxisRaw(verticalAxis);
		var h = Input.GetAxisRaw(horizontalAxis);

		if (h * h > v * v) {
			v = 0;
		} else {
			h = 0;
		}

		if (_dirPressed == 0) {
			if (v > deadZone) {
				MoveCursor(-rowLength);
				_dirPressed = 1;
				_delay = delayBeforeKeyRepeat;
			} else if (v < -deadZone) {
				MoveCursor(rowLength);
				_dirPressed = 2;
				_delay = delayBeforeKeyRepeat;
			} else if (h > deadZone) {
				MoveCursor(1);
				_dirPressed = 3;
				_delay = delayBeforeKeyRepeat;
			} else if (h < -deadZone) {
				MoveCursor(-1);
				_dirPressed = 4;
				_delay = delayBeforeKeyRepeat;
			}
		} else if (_dirPressed == 1 && v > deadZone) {
			_delay -= Time.deltaTime;
			if (_delay < 0) {
				_delay = keyRepeatTime;
				MoveCursor(-rowLength);
			}
		} else if (_dirPressed == 2 && v < -deadZone) {
			_delay -= Time.deltaTime;
			if (_delay < 0) {
				_delay = keyRepeatTime;
				MoveCursor(rowLength);
			}
		} else if (_dirPressed == 3 && h > deadZone) {
			_delay -= Time.deltaTime;
			if (_delay < 0) {
				_delay = keyRepeatTime;
				MoveCursor(1);
			}
		} else if (_dirPressed == 4 && h < -deadZone) {
			_delay -= Time.deltaTime;
			if (_delay < 0) {
				_delay = keyRepeatTime;
				MoveCursor(-1);
			}
		} else {
			_dirPressed = 0;
		}
	}

	private void MoveCursor(int byAmount) {
		cursorPos += byAmount;
		if (cursorPos >= _shownFrames) {
			//cursorPos = _shownFrames - 1;
			cursorPos -= _shownFrames;
		}
		if (cursorPos < 0)
		{
			cursorPos += _shownFrames;
		}

		if (cursorPos < itemIcons.Count) {
			_currentCursor.SetActive(true);
			_currentCursor.transform.position = itemIcons[cursorPos].transform.position;
		} else {
			_currentCursor.SetActive(false);
		}
	}

	protected Image CreateFrame(int i) {
		var newObj = Instantiate(itemFramePrefab);
		newObj.transform.SetParent(transform);
		return newObj.GetComponentInChildren<Image>();
	}

	public void Hide(bool skipFadeOut = false) {
		if (!skipFadeOut && canvasGroup && fadeOutTime > 0) {
			_fadeOutTimeLeft = fadeOutTime;
		} else {
			if (_combining) {
				EndCombine();
			}
			transform.parent.gameObject.SetActive(false);
			cursor.SetActive(false);
			combineCursor.SetActive(false);
			PlayerStatic.ResumePlayer("Inventory");
			if (PlayerStatic.IsFrozen())
			{
				PlayerStatic.ResumePlayer("", true);
			}
		}
	}

	public void Focus() {
		_focused = true;
		_selectButtonPressed = true;
		_closeButtonPressed = true;
	}

	public void UnFocus() {
		_focused = false;
	}

	public void StartCombine() {
		_combiningWith = cursorPos;
		cursor.SetActive(false);
		combineCursor.SetActive(true);
		_currentCursor = combineCursor;
		_combining = true;
		MoveCursor(0);
	}

	public void EndCombine() {
		cursorPos = _combiningWith;
		cursor.SetActive(true);
		combineCursor.SetActive(false);
		_currentCursor = cursor;
		_combining = false;
		MoveCursor(0);
	}
}
