using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class SwerveDrive : MonoBehaviour
{
	public float max_movement_speed = 8.0f;
	public float max_steering_speed = 8.0f;

	GameObject robot_base;
	Rigidbody  rigid_body;
	GameObject pivot_indicator;
	Vector2    pivot_offset;
	Wheel[]    wheels           = new Wheel[4];
	Vector2[]  wheel_directions = new Vector2[4]; // @TODO@ Use angles for better animation.
	Vector2    movement         = new Vector2(0.0f, 0.0f);
	float      steering         = 0.0f;

	void Start()
	{
		robot_base      = transform.Find("Base"           ).gameObject;
		pivot_indicator = transform.Find("Pivot Indicator").gameObject;
		wheels[0]       = transform.Find("Wheel BL"       ).gameObject.GetComponent<Wheel>(); // @TODO@ Some Unity engineering to make it where adjusting the size of the base will also adjust the positions of the wheel.
		wheels[1]       = transform.Find("Wheel BR"       ).gameObject.GetComponent<Wheel>();
		wheels[2]       = transform.Find("Wheel FL"       ).gameObject.GetComponent<Wheel>();
		wheels[3]       = transform.Find("Wheel FR"       ).gameObject.GetComponent<Wheel>();
		rigid_body      = GetComponent<Rigidbody>();
	}

	void Update()
	{
		//
		// Pivot change.
		//

		// @TODO@ Make the pivot change depending on the orientation of the camera (e.g. pressing "up" would make the pivot go up, even when it is going lower on the base).
		const float PIVOT_MOVE_SPEED = 4.0f;
		if (Keyboard.current[Key.LeftArrow ].isPressed || Gamepad.current.buttonWest .isPressed) { pivot_offset.x -= PIVOT_MOVE_SPEED * Time.deltaTime; }
		if (Keyboard.current[Key.RightArrow].isPressed || Gamepad.current.buttonEast .isPressed) { pivot_offset.x += PIVOT_MOVE_SPEED * Time.deltaTime; }
		if (Keyboard.current[Key.DownArrow ].isPressed || Gamepad.current.buttonSouth.isPressed) { pivot_offset.y -= PIVOT_MOVE_SPEED * Time.deltaTime; }
		if (Keyboard.current[Key.UpArrow   ].isPressed || Gamepad.current.buttonNorth.isPressed) { pivot_offset.y += PIVOT_MOVE_SPEED * Time.deltaTime; }
		pivot_offset.x = Mathf.Clamp(pivot_offset.x, -1.0f, 1.0f);
		pivot_offset.y = Mathf.Clamp(pivot_offset.y, -1.0f, 1.0f);

		Vector3 pivot_indicator_position =
			robot_base.transform.position
				+ robot_base.transform.right   * robot_base.transform.localScale.x * 0.5f * pivot_offset.x
				+ robot_base.transform.forward * robot_base.transform.localScale.z * 0.5f * pivot_offset.y;

		//
		// Cardinal movement.
		//

		{
			Vector2 target_movement = Gamepad.current == null ? new Vector2(0.0f, 0.0f) : Gamepad.current.leftStick.ReadValue();
			if (target_movement == new Vector2(0.0f, 0.0f))
			{
				if (Keyboard.current[Key.A].isPressed) { target_movement.x -= 1.0f; }
				if (Keyboard.current[Key.D].isPressed) { target_movement.x += 1.0f; }
				if (Keyboard.current[Key.S].isPressed) { target_movement.y -= 1.0f; }
				if (Keyboard.current[Key.W].isPressed) { target_movement.y += 1.0f; }
				if (target_movement != new Vector2(0.0f, 0.0f))
				{
					target_movement = Vector3.Normalize(target_movement);
				}
			}
			target_movement *= (max_movement_speed - rigid_body.velocity.magnitude) * 0.2f;
			movement = dampen(movement, target_movement, 0.00001f);
		}

		wheels[0].drive_activation = wheels[3].drive_activation = movement.magnitude;
		wheels[1].drive_activation = wheels[2].drive_activation = movement.magnitude;

		for (int i = 0; i < 4; i += 1)
		{
			wheel_directions[i] = dampen(wheel_directions[i], movement, 0.001f);
		}

		//
		// Rotational movement.
		//

		{
			float target_steering = Gamepad.current == null ? 0.0f : Gamepad.current.rightStick.ReadValue().x;
			if (target_steering == 0.0f)
			{
				if (Keyboard.current[Key.Q].isPressed) { target_steering -= 1.0f; }
				if (Keyboard.current[Key.E].isPressed) { target_steering += 1.0f; }
			}
			target_steering *= (max_steering_speed - rigid_body.angularVelocity.magnitude) * 0.1f;
			steering         = dampen(steering, target_steering, 0.0001f);
		}

		for (int i = 0; i < 4; i += 1)
		{
			Vector3 wheel_to_rotation = pivot_indicator.transform.position - wheels[i].transform.position;
			Vector3 perpendicular     = Quaternion.Inverse(robot_base.transform.rotation) * Vector3.Cross(wheel_to_rotation, robot_base.transform.up);

			wheels[i].drive_activation += wheel_to_rotation.magnitude * Mathf.Abs(steering); // @TODO@ Remove magic number!
			wheel_directions[i]         = dampen(wheel_directions[i], new Vector2(perpendicular.x, perpendicular.z) * steering, 0.0001f);
		}

		//
		// Apply corresponding forces.
		//

		for (int i = 0; i < 4; i += 1)
		{
			if (wheel_directions[i].magnitude > 0.001f)
			{
				wheels[i].transform.rotation = robot_base.transform.rotation;
				wheels[i].transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f - Mathf.Atan2(wheel_directions[i].y, wheel_directions[i].x) * 180.0f / Mathf.PI);
			}

			wheels[i].drive_activation = Mathf.Clamp(wheels[i].drive_activation, -1.0f, 1.0f);
			Vector3 force = (wheels[i].transform.forward + wheels[i].transform.right * wheels[i].strafe_k) * wheels[i].drive_activation * wheels[i].drive_force;
			rigid_body.AddForce (force                                                                              / 4.0f); // @NOTE@ Division by amount of wheels.
			rigid_body.AddTorque(Vector3.Cross(wheels[i].transform.position - robot_base.transform.position, force) / 4.0f); // @NOTE@ Division by amount of wheels.
		}

		//
		// Misc.
		//

		pivot_indicator.transform.position = pivot_indicator_position;
	}
}
