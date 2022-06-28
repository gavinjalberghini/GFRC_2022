using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class mechanum_drive : MonoBehaviour
{
	public float movement_force = 64.0f;
	public float torque         = 64.0f;
	public float max_speed      = 1.0f;

	GameObject robot_base;
	GameObject robot_pivot_indicator;
	Rigidbody  robot_base_rigid_body;
	Vector2    pivot_offset;

	// @TODO@ Either Unity is buggy or I'm using the Input System API wrong. `Gamepad.current.buttonWest.wasPressedThisFrame` may trigger multiple times, instead of once.
	// State of the button will have to be saved in order to side step this issue, although it's pretty crufty.
	bool button_west;
	bool button_east;
	bool button_south;
	bool button_north;

	void Start()
	{
		robot_base            = transform.Find("Base").gameObject;
		robot_pivot_indicator = transform.Find("Pivot Indicator").gameObject;
		robot_base_rigid_body = robot_base.GetComponent<Rigidbody>();
	}

	void Update()
	{
		//
		// Pivot change.
		//

		// @TODO@ Make the pivot change depending on the orientation of the camera (e.g. pressing "up" would make the pivot go up, even when it is going lower on the base).
		if (Keyboard.current[Key.LeftArrow ].wasPressedThisFrame || Gamepad.current.buttonWest.isPressed  && !button_west ) { pivot_offset.x -= 1.0f; }
		if (Keyboard.current[Key.RightArrow].wasPressedThisFrame || Gamepad.current.buttonEast.isPressed  && !button_east ) { pivot_offset.x += 1.0f; }
		if (Keyboard.current[Key.DownArrow ].wasPressedThisFrame || Gamepad.current.buttonSouth.isPressed && !button_south) { pivot_offset.y -= 1.0f; }
		if (Keyboard.current[Key.UpArrow   ].wasPressedThisFrame || Gamepad.current.buttonNorth.isPressed && !button_north) { pivot_offset.y += 1.0f; }
		pivot_offset.x = Mathf.Clamp(pivot_offset.x, -1.0f, 1.0f);
		pivot_offset.y = Mathf.Clamp(pivot_offset.y, -1.0f, 1.0f);

		Vector3 pivot_indicator_position =
			robot_base.transform.position
				+ robot_base.transform.right   * robot_base.transform.localScale.x * 0.5f * pivot_offset.x
				+ robot_base.transform.forward * robot_base.transform.localScale.z * 0.5f * pivot_offset.y;

		//
		// Cardinal movement.
		//

		Vector2 movement = Gamepad.current.leftStick.ReadValue();
		if (movement == new Vector2(0.0f, 0.0f))
		{
			if (Keyboard.current[Key.A].isPressed) { movement.x -= 1.0f; }
			if (Keyboard.current[Key.D].isPressed) { movement.x += 1.0f; }
			if (Keyboard.current[Key.S].isPressed) { movement.y -= 1.0f; }
			if (Keyboard.current[Key.W].isPressed) { movement.y += 1.0f; }
			if (movement != new Vector2(0.0f, 0.0f))
			{
				movement = Vector3.Normalize(movement);
			}
		}
		movement *= movement_force;
		movement *= Mathf.Max(max_speed - Vector3.Magnitude(robot_base_rigid_body.velocity), 0.0f); // @NOTE@ Stops from forever accelerating.
		robot_base_rigid_body.AddForce(robot_base.transform.right * movement.x + robot_base.transform.forward * movement.y);

		//
		// Rotational movement.
		//

		// @TODO@ Change this to use forces?
		float rotation_amount = Gamepad.current.rightStick.ReadValue().x;
		if (rotation_amount == 0.0f)
		{
			if (Keyboard.current[Key.Q].isPressed) { rotation_amount -= 1.0f; }
			if (Keyboard.current[Key.E].isPressed) { rotation_amount += 1.0f; }
		}
		rotation_amount *= torque * Time.deltaTime;

		robot_base.transform.RotateAround(pivot_indicator_position, Vector3.up, rotation_amount);

		//
		// Misc.
		//

		robot_pivot_indicator.transform.position = pivot_indicator_position;
		button_west                              = Gamepad.current.buttonWest.isPressed;
		button_east                              = Gamepad.current.buttonEast.isPressed;
		button_south                             = Gamepad.current.buttonSouth.isPressed;
		button_north                             = Gamepad.current.buttonNorth.isPressed;
	}
}
