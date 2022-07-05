using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class CarDrive : MonoBehaviour
{
	public Transform drive_base = null;
	public Transform drive_head = null; // @TODO@ What if there was more parts than just the head?
	public Wheel     wheel_bl   = null;
	public Wheel     wheel_br   = null;
	public Wheel     wheel_fl   = null;
	public Wheel     wheel_fr   = null;
	public Vector2   dims       = new Vector2(0.5f, 0.7f);

	float steer_angle = 0.0f;

	void OnValidate()
	{
		dims.x                      = Mathf.Clamp(dims.x, 0.25f, 1.0f);
		dims.y                      = Mathf.Clamp(dims.y, 0.25f, 1.0f);
		drive_base.localScale       = new Vector3(dims.x, 0.05f, dims.y);
		drive_head.position         = drive_base.position + drive_base.forward * (dims.y + drive_head.localScale.z) * 0.5f;
		wheel_bl.transform.position = drive_base.position + drive_base.right * dims.x * -0.5f + drive_base.forward * dims.y * -0.5f;
		wheel_br.transform.position = drive_base.position + drive_base.right * dims.x *  0.5f + drive_base.forward * dims.y * -0.5f;
		wheel_fl.transform.position = drive_base.position + drive_base.right * dims.x * -0.5f + drive_base.forward * dims.y *  0.5f;
		wheel_fr.transform.position = drive_base.position + drive_base.right * dims.x *  0.5f + drive_base.forward * dims.y *  0.5f;
	}

	void Update()
	{
		const float GREASE = 0.000001f; // @NOTE@ How quickly the movement and steering changes.

		//
		// Movement.
		//

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
		steer_angle = dampen(steer_angle, steering * 60.0f, GREASE);

		wheel_bl.activation = dampen(wheel_bl.activation, Mathf.Clamp(movement.y, -1.0f, 1.0f), GREASE);
		wheel_br.activation = dampen(wheel_br.activation, Mathf.Clamp(movement.y, -1.0f, 1.0f), GREASE);

		wheel_fl.angle      = steer_angle;
		wheel_fl.activation = dampen(wheel_fl.activation, Mathf.Clamp(movement.y, -1.0f, 1.0f), GREASE);
		wheel_fr.angle      = steer_angle;
		wheel_fr.activation = dampen(wheel_fr.activation, Mathf.Clamp(movement.y, -1.0f, 1.0f), GREASE);
	}
}

