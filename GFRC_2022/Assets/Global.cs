using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

static class Global
{
	static public float TAU = 2.0f * Mathf.PI;

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

	static public Vector3 v2_on_plane(Vector3 axis_x, Vector3 axis_y, Vector2 position)
	{
		return axis_x * position.x + axis_y * position.y;
	}

	static public Vector2 rotate(Vector2 v, float degrees)
	{
		return new Vector2
			(
				v.x * Mathf.Cos(degrees / 180.0f * Mathf.PI) - v.y * Mathf.Sin(degrees / 180.0f * Mathf.PI),
				v.x * Mathf.Sin(degrees / 180.0f * Mathf.PI) + v.y * Mathf.Cos(degrees / 180.0f * Mathf.PI)
			);
	}

	static public float mod(float a, float b)
	{
		return (a % b + b) % b;
	}

	static public float min_degree_arc(float a, float b)
	{
		float delta_0 = mod(b, 360.0f) - mod(a, 360.0f);
		float delta_1 = delta_0 - Mathf.Sign(delta_0) * 360.0f;
		return Mathf.Abs(delta_0) < Mathf.Abs(delta_1) ? delta_0 : delta_1;
	}

	static public float hypot(float a, float b)
	{
		return Mathf.Sqrt(a * a + b * b);
	}
}
