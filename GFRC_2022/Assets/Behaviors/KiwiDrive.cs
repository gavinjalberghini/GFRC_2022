using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class KiwiDrive : MonoBehaviour
{
	Wheel_DEPRECATED[] wheels   = new Wheel_DEPRECATED[3];
	Vector2 movement = new Vector2(0.0f, 0.0f);
	float   steering = 0.0f;

	void Start()
	{
		wheels[0]  = transform.Find("Wheel Back").gameObject.GetComponent<Wheel_DEPRECATED>(); // @TODO@ Some Unity engineering to make it where adjusting the size of the base will also adjust the positions of the wheel.
		wheels[1]  = transform.Find("Wheel FL"  ).gameObject.GetComponent<Wheel_DEPRECATED>();
		wheels[2]  = transform.Find("Wheel FR"  ).gameObject.GetComponent<Wheel_DEPRECATED>();
	}

	void Update()
	{
		const float GREASE = 0.000001f; // @NOTE@ How quickly the movement and steering changes.

		//
		// Cardinal movement.
		//

		{
			Vector2 target_movement = left_stick();
			if (target_movement == new Vector2(0.0f, 0.0f))
			{
				target_movement = wasd_normalized();
			}
			movement = dampen(movement, target_movement, GREASE);
		}

		foreach (Wheel_DEPRECATED wheel in wheels)
		{
			wheel.drive_activation = Vector3.Dot(wheel.transform.forward, transform.right * movement.x + transform.forward * movement.y);
		}

		//
		// Rotational movement.
		//

		{
			float target_steering = right_stick().x;
			if (target_steering == 0.0f)
			{
				if (Keyboard.current[Key.Q].isPressed) { target_steering -= 1.0f; }
				if (Keyboard.current[Key.E].isPressed) { target_steering += 1.0f; }
			}
			steering = dampen(steering, target_steering * 0.9f, GREASE);
		}

		foreach (Wheel_DEPRECATED wheel in wheels)
		{
			wheel.drive_activation += steering;
		}

		//
		// Misc.
		//

		apply_wheel_physics(GetComponent<Rigidbody>(), wheels);
	}
}

