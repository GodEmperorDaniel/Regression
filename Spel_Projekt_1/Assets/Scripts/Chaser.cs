using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ChaserSettings {
	public float baseSpeed;
	public float baseAcceleration;
	public float playerDistanceSpeedScaling;
	public float playerDistanceAccelerationScaling;
	public float playerSpeedSpeedScaling;
	public float playerSpeedAccelerationScaling;
	public float minSpeed;
	public float maxSpeed;

	public ChaserSettings(float baseSpeed, float baseAcceleration, float playerDistanceSpeedScaling,
		float playerDistanceAccelerationScaling, float playerSpeedSpeedScaling, float playerSpeedAccelerationScaling, 
		float minSpeed, float maxSpeed) {

		this.baseSpeed = baseSpeed;
		this.baseAcceleration = baseAcceleration;
		this.playerDistanceSpeedScaling = playerDistanceSpeedScaling;
		this.playerDistanceAccelerationScaling = playerDistanceAccelerationScaling;
		this.playerSpeedSpeedScaling = playerSpeedSpeedScaling;
		this.playerSpeedAccelerationScaling = playerSpeedAccelerationScaling;
		this.maxSpeed = maxSpeed;
		this.minSpeed = minSpeed;
	}
}

public class Chaser : MonoBehaviour {
	public ChaserSettings playerStationary = new ChaserSettings(1, 2, 0, 0, 0, 0, 0, 3);
	public ChaserSettings playerMovingAway = new ChaserSettings(1, 2, 0.25f, 0, 0.01f, 0, 0, 3);
	public ChaserSettings playerMovingCloser = new ChaserSettings(0.8f, 1, -0.1f, 0, 0, 0, 0, 3);
	public float startSpeed;
	public float speed;
	public Animator ani;

	private CharacterController2d c;

	void Start()
    {
		speed = startSpeed;
		c = PlayerStatic.ControllerInstance;
	}

	void Update()
	{
		float targetSpd = 0;
		float accel = playerStationary.baseAcceleration;


		if (c) {
			var deltaPos = new Vector3(c.transform.position.x - 1, c.transform.position.y) - transform.position;
			var v = c.velocity;
			var d = Vector3.Dot(deltaPos, Vector2.right);
			Debug.Log(d);

			ChaserSettings s;

			if (d > 0) {
				s = playerMovingAway;
			} else if (d < 0) {
				s = playerMovingCloser;
			} else {
				s = playerStationary;
			}

			targetSpd = Mathf.Clamp(s.baseSpeed + deltaPos.magnitude * s.playerDistanceSpeedScaling + v.magnitude * s.playerSpeedSpeedScaling, s.minSpeed, s.maxSpeed);
			accel = s.baseAcceleration + deltaPos.magnitude * s.playerDistanceAccelerationScaling + v.magnitude * s.playerSpeedAccelerationScaling;
		}

		if (speed < targetSpd) {
			speed += accel * Time.deltaTime;
			if (speed > targetSpd) {
				speed = targetSpd;
			}
		} else if (speed > targetSpd) {
			speed -= accel * Time.deltaTime;
			if (speed < targetSpd) {
				speed = targetSpd;
			}
		}

		transform.position += new Vector3(speed * Time.deltaTime, 0);


		if (speed < 0.1)
		{
			ani.speed = 0;
		}
		else
		{
			ani.speed = 1;
		}
		//if (transform.position.x > 8) {
		//	transform.position -= new Vector3(16, 0);
		//}
	}
}
