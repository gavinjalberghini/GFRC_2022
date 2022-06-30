using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

// @TODO@ Check the wheels work in weird orientations.
public class HDrive : MonoBehaviour
{
	public float max_movement_speed = 8.0f;
	public float max_steering_speed = 8.0f;

	Rigidbody  rigid_body;
	Wheel[]    wheels   = new Wheel[5];
	Vector2    movement = new Vector2(0.0f, 0.0f);
	float      steering = 0.0f;

	void Start()
	{
		wheels[0]  = transform.Find("Wheel BL"    ).gameObject.GetComponent<Wheel>(); // @TODO@ Some Unity engineering to make it where adjusting the size of the base will also adjust the positions of the wheel.
		wheels[1]  = transform.Find("Wheel BR"    ).gameObject.GetComponent<Wheel>();
		wheels[2]  = transform.Find("Wheel FL"    ).gameObject.GetComponent<Wheel>();
		wheels[3]  = transform.Find("Wheel FR"    ).gameObject.GetComponent<Wheel>();
		wheels[4]  = transform.Find("Wheel Center").gameObject.GetComponent<Wheel>();
		rigid_body = GetComponent<Rigidbody>();
	}

	void Update()
	{
		//
		// Cardinal movement.
		//

		{
			Vector2 target_movement = left_stick();
			if (target_movement == new Vector2(0.0f, 0.0f))
			{
				target_movement = wasd_normalized();
			}

			target_movement *= (max_movement_speed - rigid_body.velocity.magnitude) * 0.2f;
			movement         = dampen(movement, target_movement, 0.00001f);
		}

		wheels[0].drive_activation =
		wheels[1].drive_activation =
		wheels[2].drive_activation =
		wheels[3].drive_activation = movement.y;
		wheels[4].drive_activation = movement.x;

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
			target_steering *= (max_steering_speed - rigid_body.angularVelocity.magnitude) * 0.1f;
			steering         = dampen(steering, target_steering, 0.000001f);
		}

		wheels[0].drive_activation +=  steering;
		wheels[1].drive_activation += -steering;
		wheels[2].drive_activation +=  steering;
		wheels[3].drive_activation += -steering;

		//
		// Apply corresponding forces.
		//

		foreach (Wheel wheel in wheels)
		{
			wheel.drive_activation = Mathf.Clamp(wheel.drive_activation, -1.0f, 1.0f);
			Vector3 force = (wheel.transform.forward + wheel.transform.right * wheel.strafe_k) * wheel.drive_activation * wheel.drive_force;
			rigid_body.AddForce (force                                                               / wheels.Length); // @NOTE@ Division by amount of wheels.
			rigid_body.AddTorque(Vector3.Cross(wheel.transform.position - transform.position, force) / wheels.Length); // @NOTE@ Division by amount of wheels.
		}
	}
}
