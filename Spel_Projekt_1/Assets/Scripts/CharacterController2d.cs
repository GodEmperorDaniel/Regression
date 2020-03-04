using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

[System.Serializable]
[DisallowMultipleComponent]
public class CharacterController2d : MonoBehaviour, ISaveable {
	static readonly float _r = Mathf.Cos(Mathf.PI / 8) * Mathf.Cos(Mathf.PI / 8) / (Mathf.Sin(Mathf.PI / 8) * Mathf.Sin(Mathf.PI / 8));
	static readonly float _invSqr2 = 1 / Mathf.Sqrt(2);
  
	public enum DirectionType {
		full360,
		directions8,
		directions4,
	}
	public enum SpeedType {
		smooth,
		toggle
	}

	[FormerlySerializedAs("MovementSpeed")]
	public float movementSpeed = 2;
	public float stepSize = 0.5f;
	[Range(0,1)]
	public float deadZone = 0.1f;
	public string horizontalAxis = "Horizontal";
	public string verticalAxis = "Vertical";
	public string interactionButton = "InteractionButton";
	public string inventoryButton = "InventoryButton";
	public DirectionType directionType = DirectionType.directions4;
	public SpeedType speedType = SpeedType.toggle;
	[HideInInspector]
	public Vector2 forward;

	[FormerlySerializedAs("Ani")]
	public Animator animator;
	public string animatorMovementBool = "movement";
	public string animatorHorizontalFloat = "horimovement";
	public string animatorVerticalFloat = "vertimovement";

	private bool _inventoryPressed = false;
	private int _interactionPressed = 0;
	private float _stepLeft;
	private Vector2 _stepDir;


	private void Start() {
		if (animator == null) {
			animator = GetComponent<Animator>();
		}
	}

	private void OnEnable()
	{
		_interactionPressed = 2;
	}

	private void Update() {
		if (_stepLeft > 0) {
			_stepLeft -= Time.deltaTime;
		} else {
			ReadInput();
		}

		Translate();
		CheckInventoryButton();
		if (Input.GetAxisRaw(interactionButton) > deadZone) {
			_interactionPressed++;
		} else {
			_interactionPressed = 0;
		}
	}

	private void ReadInput() {
		var h = Input.GetAxisRaw(horizontalAxis);
		var v = Input.GetAxisRaw(verticalAxis);

		if (h * h + v * v > deadZone * deadZone) {
			_stepLeft = stepSize;
			switch (directionType) {
				case DirectionType.full360:
					_stepDir = new Vector2(h, v);
					_stepDir.Normalize();
					break;
				case DirectionType.directions4:
					if (h * h > v * v) {
						_stepDir = new Vector2(Mathf.Sign(h), 0);
					} else {
						_stepDir = new Vector2(0, Mathf.Sign(v));
					}
					break;
				case DirectionType.directions8:
					if (h * h > _r * v * v) {
						_stepDir = new Vector2(Mathf.Sign(h), 0);
					} else if (v * v > _r * h * h) {
						_stepDir = new Vector2(0, Mathf.Sign(v));
					} else {
						_stepDir = new Vector2(Mathf.Sign(h) * _invSqr2, Mathf.Sign(v) * _invSqr2);
					}
					break;
			}

			forward = _stepDir;

			if (speedType == SpeedType.smooth) {
				_stepDir *= Mathf.Sqrt(h * h + v * v);
			}

			SetAnimatorVariables(true);
		} else {
			_stepDir = Vector2.zero;
			SetAnimatorVariables(false);
		}
	}

	private void Translate() {
		transform.Translate(_stepDir * movementSpeed * Time.deltaTime);
	}

	private void SetAnimatorVariables(bool moving) {
		if (animator != null)
		{
			animator.SetBool(animatorMovementBool, moving);
			animator.SetFloat(animatorHorizontalFloat, forward.x);
			animator.SetFloat(animatorVerticalFloat, forward.y);
		}
	}

	private void CheckInventoryButton() {
		if (!_inventoryPressed && Input.GetAxisRaw(inventoryButton) > deadZone) {
			_inventoryPressed = true;
			var inventory = GetComponent<Inventory>();
			if (inventory != null) {
				inventory.ShowUI();
			}
		} else if (_inventoryPressed && Input.GetAxisRaw(inventoryButton) <= deadZone) {
			_inventoryPressed = false;
		}
	}

	public bool GetInteractionKey() {
		return isActiveAndEnabled && _interactionPressed > 0;
	}

	public bool GetInteractionKeyDown() {
		return isActiveAndEnabled && _interactionPressed == 1;
	}

	public byte[] Save() {
		var scene = SceneManager.GetActiveScene();
		var sceneName = FungusSaver.StringEncoding.GetBytes(scene.name);

		var data = new byte[9 * 4 + sceneName.Length];

		BitConverter.GetBytes(transform.position.x).CopyTo(data, 0);
		BitConverter.GetBytes(transform.position.y).CopyTo(data, 4);
		BitConverter.GetBytes(transform.position.z).CopyTo(data, 8);
		BitConverter.GetBytes(forward.x).CopyTo(data, 12);
		BitConverter.GetBytes(forward.y).CopyTo(data, 16);
		BitConverter.GetBytes(_stepLeft).CopyTo(data, 20);
		BitConverter.GetBytes(_stepDir.x).CopyTo(data, 24);
		BitConverter.GetBytes(_stepDir.y).CopyTo(data, 28);
		BitConverter.GetBytes(sceneName.Length).CopyTo(data, 32);
		sceneName.CopyTo(data, 36);

		return data;
	}

	public void Load(byte[] data, int version) {
		transform.position = new Vector3(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
		forward = new Vector2(BitConverter.ToSingle(data, 12), BitConverter.ToSingle(data, 16));
		_stepLeft = BitConverter.ToSingle(data, 20);
		_stepDir = new Vector2(BitConverter.ToSingle(data, 24), BitConverter.ToSingle(data, 28));

		SetAnimatorVariables(_stepDir.sqrMagnitude > deadZone * deadZone);

		if (version >= 3) {
			var sceneNameLength = BitConverter.ToInt32(data, 32);
			var sceneName = FungusSaver.StringEncoding.GetString(data, 36, sceneNameLength);
			var currentScene = SceneManager.GetActiveScene();
			if (currentScene.name != sceneName) {
				SceneManager.LoadScene(sceneName);
			}
		}
	}
}
