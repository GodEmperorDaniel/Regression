using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[DisallowMultipleComponent]
public class InventoryPopup : MonoBehaviour
{
	public Selectable firstSelectable;
	public Flowchart flowchart;
	private Inventory _inventory;
	private InventoryCanvas _canvas;
	private InventoryItem _item;

	// Start is called before the first frame update
	void Start()
    {
		Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Show(Inventory inventory, InventoryCanvas canvas, InventoryItem item) {
		PlayerStatic.FreezePlayer("Inventory Popup");
		_inventory = inventory;
		_canvas = canvas;
		_item = item;
		gameObject.SetActive(true);
		StartCoroutine(WaitOnGui());
	}

	private IEnumerator WaitOnGui()
	{
		yield return new WaitForEndOfFrame();

		firstSelectable.Select();
	}

		public void Hide() {
		PlayerStatic.ResumePlayer("Inventory Popup");
		gameObject.SetActive(false);
		_item = null;
		if (_canvas) {
			_canvas.Focus();
		}
	}

	public void Use() {
		foreach (var i in Interactions.canUseItemOn) {
			if (i) {
				i.UseItem(_inventory.GetComponent<CharacterController2d>(), _item);
			}
		}
		Hide();
		_canvas.Hide();
	}

	public void Combine() {
		_canvas.StartCombine();
		Hide();
	}

	public void Inspect() {
		flowchart.StopAllBlocks();
		flowchart.GetVariable<InventoryItemVariable>("SelectedItem").Value = _item;
		flowchart.SetStringVariable("Name", _item.title);
		flowchart.SetStringVariable("Description", _item.description);
		Hide();
		_canvas.Hide();
		flowchart.ExecuteBlock("On Inspect");
	}
}
