using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class InventoryCanvas : MonoBehaviour
{
	public int rowLength;
	public int minimumFramesShown;
	public GameObject itemFrame;
	public GameObject cursor;
	public GameObject combineCursor;
	public InventoryPopup popup;

	public float horizontalSpacing = 72;
	public float verticalSpacing = 80;

	//protected List<Image> itemFrames;
	private List<Image> itemIcons = new List<Image>();
	private int cursorPos;

	public string horizontalAxis = "Horizontal";
	public string verticalAxis = "Vertical";
	public string selectAxis = "InteractionButton";
	public string closeAxis = "InventoryButton";
	[Range(0,1)]
	public float deadZone;

	public float delayBeforeKeyRepeat = 1;
	public float keyRepeatTime = 0.25f;

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

	// Start is called before the first frame update
	void Start()
    {
		DontDestroyOnLoad(transform.parent.gameObject);
		Hide();
	}

	public void Show(Inventory inventory) {
		gameObject.SetActive(true);
		combineCursor.SetActive(false);
		PlayerStatic.FreezePlayer("Inventory");

		_currentCursor = cursor;
		_inventory = inventory;
		_shownFrames = Mathf.Max(minimumFramesShown, inventory.Count);

		for (var i = itemIcons.Count; i < inventory.Count; i++) {
			itemIcons.Add(CreateFrame());
		}

		var xOffset = -0.5f * horizontalSpacing * (Mathf.Min(rowLength, _shownFrames) - 1);
		var yOffset = 0.5f * verticalSpacing * (Mathf.Ceil((float)_shownFrames / rowLength) - 1);
		var x = 0;
		var y = 0;

		for (var i = 0; i < itemIcons.Count; i++) {
			if (i < _shownFrames) {
				if (x >= rowLength) {
					x = 0;
					y++;
				}
				var frame = itemIcons[i];
				frame.rectTransform.parent.gameObject.SetActive(true);
				frame.rectTransform.parent.localPosition = new Vector3(xOffset + x * horizontalSpacing, yOffset - y * verticalSpacing, 0);
				if (i < inventory.Count) {
					frame.sprite = inventory.Items[i].sprite;
				} else {
					frame.sprite = null;
				}
				x++;
			} else {
				itemIcons[i].transform.parent.gameObject.SetActive(false);
			}
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
								Show(_inventory);//Check for any changes
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

			if (Input.GetAxisRaw(closeAxis) > deadZone) {
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
			cursorPos = _shownFrames - 1;
		}
		if (cursorPos < 0) {
			cursorPos = 0;
		}

		if (cursorPos < itemIcons.Count) {
			_currentCursor.SetActive(true);
			_currentCursor.transform.position = itemIcons[cursorPos].transform.position;
		} else {
			_currentCursor.SetActive(false);
		}
	}

	protected Image CreateFrame() {
		var newObj = Instantiate(itemFrame);
		newObj.transform.SetParent(transform);
		return newObj.GetComponentInChildren<Image>();
	}

	public void Hide() {
		if (_combining) {
			EndCombine();
		}
		gameObject.SetActive(false);
		cursor.SetActive(false);
		combineCursor.SetActive(false);
		PlayerStatic.ResumePlayer("Inventory");
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
