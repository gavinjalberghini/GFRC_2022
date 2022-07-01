using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class SwerveDrive : MonoBehaviour
{
	Wheel[]    wheels           = new Wheel[4];
	Vector2[]  wheel_directions = new Vector2[4]; // @TODO@ Use angles for better animation.
	Vector2    movement         = new Vector2(0.0f, 0.0f);
	float      steering         = 0.0f;

	void Start()
	{
		wheels[0] = transform.Find("Wheel BL").gameObject.GetComponent<Wheel>(); // @TODO@ Some Unity engineering to make it where adjusting the size of the base will also adjust the positions of the wheel.
		wheels[1] = transform.Find("Wheel BR").gameObject.GetComponent<Wheel>();
		wheels[2] = transform.Find("Wheel FL").gameObject.GetComponent<Wheel>();
		wheels[3] = transform.Find("Wheel FR").gameObject.GetComponent<Wheel>();
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
			steering = dampen(steering, target_steering * 4.0f, GREASE); // @TODO@ Remove magic number?
		}

		//
		// Misc.
		//

		for (int i = 0; i < 4; i += 1)
		{
			Vector3 wheel_to_rotation = transform.position - wheels[i].transform.position;
			Vector3 perpendicular     = Quaternion.Inverse(transform.rotation) * Vector3.Cross(wheel_to_rotation, transform.up);

			wheels[i].drive_activation = movement.magnitude + wheel_to_rotation.magnitude * Mathf.Abs(steering);
			wheel_directions[i]        = movement + new Vector2(perpendicular.x, perpendicular.z) * steering;

			if (wheel_directions[i].magnitude > 0.001f)
			{
				wheels[i].transform.rotation = transform.rotation;
				wheels[i].transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f - argument(wheel_directions[i]) * 180.0f / Mathf.PI);
			}
		}

		apply_wheel_physics(GetComponent<Rigidbody>(), wheels);
	}
}
