using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class TankDrive : MonoBehaviour
{
	Wheel[] wheels   = new Wheel[6];
	Vector2 movement = new Vector2(0.0f, 0.0f);
	float   steering = 0.0f;

	void Start()
	{
		wheels[0] = transform.Find("Wheel L0").gameObject.GetComponent<Wheel>(); // @TODO@ Some Unity engineering to make it where adjusting the size of the base will also adjust the positions of the wheel.
		wheels[1] = transform.Find("Wheel L1").gameObject.GetComponent<Wheel>();
		wheels[2] = transform.Find("Wheel L2").gameObject.GetComponent<Wheel>();
		wheels[3] = transform.Find("Wheel R0").gameObject.GetComponent<Wheel>();
		wheels[4] = transform.Find("Wheel R1").gameObject.GetComponent<Wheel>();
		wheels[5] = transform.Find("Wheel R2").gameObject.GetComponent<Wheel>();
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

		foreach (var wheel in wheels)
		{
			wheel.drive_activation = movement.y;
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
			steering = dampen(steering, target_steering, GREASE);
		}

		foreach (var wheel in wheels)
		{
			wheel.drive_activation +=
				Vector3.Dot(wheel.transform.position - transform.position, transform.right) < 0.0f
					?  steering
					: -steering;
		}

		//
		// Misc.
		//

		apply_wheel_physics(GetComponent<Rigidbody>(), wheels);
	}
}
