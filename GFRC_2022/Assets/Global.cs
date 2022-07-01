using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

static class Global
{
	// @NOTE@ Framrate independent dampening. Use this to gradually move one value to other smoothly over each update tick.
	// `friction` of 0.0f will cause an instanteous transition from `start` to `end`.
	// `friction` of 0.5f will close the gap between `start` and `end` by half after one second.
	// `friction` of 1.0f will always make the value stay at `start`, e.g. infinite friction.
	static public float   dampen(float   start, float   end, float friction) { return end + (start - end) * Mathf.Pow(friction, Time.deltaTime); }
	static public Vector2 dampen(Vector2 start, Vector2 end, float friction) { return end + (start - end) * Mathf.Pow(friction, Time.deltaTime); }

	static public Vector2 wasd()
	{
		Vector2 v = new Vector2(0.0f, 0.0f);
		if (Keyboard.current[Key.A].isPressed) { v.x -= 1.0f; }
		if (Keyboard.current[Key.D].isPressed) { v.x += 1.0f; }
		if (Keyboard.current[Key.S].isPressed) { v.y -= 1.0f; }
		if (Keyboard.current[Key.W].isPressed) { v.y += 1.0f; }
		return v;
	}

	// @NOTE@ Will not be normalized if no input is given.
	static public Vector2 wasd_normalized()
	{
		Vector2 v = wasd();
		return v == new Vector2(0.0f, 0.0f) ? new Vector2(0.0f, 0.0f) : v.normalized;
	}

	static public Vector2 arrow_keys()
	{
		Vector2 v = new Vector2(0.0f, 0.0f);
		if (Keyboard.current[Key.LeftArrow ].isPressed) { v.x -= 1.0f; }
		if (Keyboard.current[Key.RightArrow].isPressed) { v.x += 1.0f; }
		if (Keyboard.current[Key.DownArrow ].isPressed) { v.y -= 1.0f; }
		if (Keyboard.current[Key.UpArrow   ].isPressed) { v.y += 1.0f; }
		return v;
	}

	static public Vector2 gamepad_buttons()
	{
		Vector2 v = new Vector2(0.0f, 0.0f);
		if (Gamepad.current != null)
		{
			if (Gamepad.current.buttonWest .isPressed) { v.x -= 1.0f; }
			if (Gamepad.current.buttonEast .isPressed) { v.x += 1.0f; }
			if (Gamepad.current.buttonSouth.isPressed) { v.y -= 1.0f; }
			if (Gamepad.current.buttonNorth.isPressed) { v.y += 1.0f; }
		}
		return v;
	}

	static public Vector2 left_stick()
	{
		return Gamepad.current == null ? new Vector2(0.0f, 0.0f) : Gamepad.current.leftStick.ReadValue();
	}

	static public Vector2 right_stick()
	{
		return Gamepad.current == null ? new Vector2(0.0f, 0.0f) : Gamepad.current.rightStick.ReadValue();
	}

	static public float argument(Vector2 v)
	{
		return v.magnitude < 0.001f ? 0.0f : Mathf.Atan2(v.y, v.x);
	}

	static public void apply_wheel_physics(Rigidbody rigidbody, Wheel[] wheels)
	{
		foreach (Wheel wheel in wheels)
		{
			wheel.drive_activation = Mathf.Clamp(wheel.drive_activation, -1.0f, 1.0f);
			Vector3 force =
				(wheel.transform.forward + wheel.transform.right * wheel.strafe_k)
					* wheel.drive_activation
					* wheel.drive_force
					* Mathf.Max(1.0f - rigidbody.velocity.magnitude / wheel.max_speed, 0.0f);
			rigidbody.AddForce (force                                                                         / wheels.Length);
			rigidbody.AddTorque(Vector3.Cross(wheel.transform.position - rigidbody.transform.position, force) / wheels.Length);
		}
	}
}
