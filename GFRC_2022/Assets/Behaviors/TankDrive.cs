using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class TankDrive : MonoBehaviour
{
	public Transform drive_base = null;
	public Transform drive_head = null; // @TODO@ What if there was more parts than just the head?
	public Wheel[]   wheels     = new Wheel[6];
	public Vector2   dims       = new Vector2(0.5f, 0.7f);

	void OnValidate()
	{
		dims.x                      = Mathf.Clamp(dims.x, 0.25f, 1.0f);
		dims.y                      = Mathf.Clamp(dims.y, 0.25f, 1.0f);
		drive_base.localScale       = new Vector3(dims.x, 0.05f, dims.y);
		drive_head.position         = drive_base.position + drive_base.forward * (dims.y + drive_head.localScale.z) * 0.5f;

		for (int i = 0; i < wheels.Length; i += 1)
		{
			wheels[i].transform.position = drive_base.position + drive_base.right * dims.x * (i / (wheels.Length / 2) - 0.5f) + drive_base.forward * dims.y * (i % (wheels.Length / 2) / (wheels.Length / 2 - 1.0f) - 0.5f);
		}
	}

	void Update()
	{
		const float GREASE = 0.000001f; // @NOTE@ How quickly the movement and steering changes.

		Vector2 movement = left_stick();
		if (movement == new Vector2(0.0f, 0.0f))
		{
			movement = wasd_normalized();
		}

		float steering = right_stick().x;
		if (steering == 0.0f)
		{
			if (Keyboard.current[Key.Q].isPressed) { steering += -1.0f; }
			if (Keyboard.current[Key.E].isPressed) { steering +=  1.0f; }
		}

		foreach (var wheel in wheels)
		{
			wheel.activation = dampen(wheel.activation, movement.y + Vector3.Dot(wheel.transform.forward, Vector3.Cross(transform.position - wheel.transform.position, transform.up)) * steering, GREASE);
		}
	}
}
