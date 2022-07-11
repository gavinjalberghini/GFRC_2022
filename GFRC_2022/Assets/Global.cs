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
	static public float   dampen(float   start, float   end, float friction) => end + (start - end) * Mathf.Pow(friction, Time.deltaTime);
	static public Vector2 dampen(Vector2 start, Vector2 end, float friction) => end + (start - end) * Mathf.Pow(friction, Time.deltaTime);

	static public bool key_down(Key k) =>
		Keyboard.current != null && Keyboard.current[k].isPressed;

	static public bool key_now_down(Key k) =>
		Keyboard.current != null && Keyboard.current[k].wasPressedThisFrame;

	static public Vector2 wasd()
	{
		Vector2 v = new Vector2(0.0f, 0.0f);
		if (key_down(Key.A)) { v.x -= 1.0f; }
		if (key_down(Key.D)) { v.x += 1.0f; }
		if (key_down(Key.S)) { v.y -= 1.0f; }
		if (key_down(Key.W)) { v.y += 1.0f; }
		return v;
	}

	// @NOTE@ Will not be normalized if no input is given.
	static public Vector2 wasd_normalized() =>
		wasd() == new Vector2(0.0f, 0.0f) ? new Vector2(0.0f, 0.0f) : wasd().normalized;

	static public Vector2 arrow_keys()
	{
		Vector2 v = new Vector2(0.0f, 0.0f);
		if (key_down(Key.LeftArrow )) { v.x -= 1.0f; }
		if (key_down(Key.RightArrow)) { v.x += 1.0f; }
		if (key_down(Key.DownArrow )) { v.y -= 1.0f; }
		if (key_down(Key.UpArrow   )) { v.y += 1.0f; }
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

	// @TODO@ Is this frame rate independent already?
	static public Vector2 mouse_delta() =>
		Mouse.current == null ? new Vector2(0.0f, 0.0f) : Mouse.current.delta.ReadValue();

	static public Vector2 left_stick() =>
		Gamepad.current == null ? new Vector2(0.0f, 0.0f) : Gamepad.current.leftStick.ReadValue();

	static public Vector2 right_stick() =>
		Gamepad.current == null ? new Vector2(0.0f, 0.0f) : Gamepad.current.rightStick.ReadValue();

	static public float argument(Vector2 v) =>
		v.magnitude < 0.001f ? 0.0f : Mathf.Atan2(v.y, v.x);

	static public Vector3 v2_on_plane(Vector3 axis_x, Vector3 axis_y, Vector2 position) =>
		axis_x * position.x + axis_y * position.y;

	static public Vector2 rotate(Vector2 v, float degrees) =>
		new Vector2
			(
				v.x * Mathf.Cos(degrees / 180.0f * Mathf.PI) - v.y * Mathf.Sin(degrees / 180.0f * Mathf.PI),
				v.x * Mathf.Sin(degrees / 180.0f * Mathf.PI) + v.y * Mathf.Cos(degrees / 180.0f * Mathf.PI)
			);

	static public float mod(float a, float b) =>
		(a % b + b) % b;

	static public float min_degree_arc(float a, float b)
	{
		float delta_0 = mod(b, 360.0f) - mod(a, 360.0f);
		float delta_1 = delta_0 - Mathf.Sign(delta_0) * 360.0f;
		return Mathf.Abs(delta_0) < Mathf.Abs(delta_1) ? delta_0 : delta_1;
	}

	static public float hypot(float a, float b) =>
		Mathf.Sqrt(a * a + b * b);

	static public Vector2 polar(float rad) =>
		new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
}
