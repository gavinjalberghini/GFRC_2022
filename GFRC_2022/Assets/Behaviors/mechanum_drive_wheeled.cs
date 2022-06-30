using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class mechanum_drive_wheeled : MonoBehaviour
{
	public float max_movement_speed = 4.0f;
	public float max_steering_speed = 6.0f;

	GameObject robot_base;
	Rigidbody  rigid_body;
	Wheel[]    wheels   = new Wheel[4];
	Vector2    movement = new Vector2(0.0f, 0.0f);
	float      steering = 0.0f;

	void Start()
	{
		robot_base = transform.Find("Base").gameObject;
		wheels[0]  = transform.Find("Wheel BL").gameObject.GetComponent<Wheel>();
		wheels[1]  = transform.Find("Wheel BR").gameObject.GetComponent<Wheel>();
		wheels[2]  = transform.Find("Wheel FL").gameObject.GetComponent<Wheel>();
		wheels[3]  = transform.Find("Wheel FR").gameObject.GetComponent<Wheel>();
		rigid_body = GetComponent<Rigidbody>();
	}

	void Update()
	{
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

		wheels[0].drive_activation = wheels[3].drive_activation = movement.y - movement.x;
		wheels[1].drive_activation = wheels[2].drive_activation = movement.x + movement.y;

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
			steering         = dampen(steering, target_steering, 0.000001f);
		}

		wheels[0].drive_activation +=  steering;
		wheels[1].drive_activation += -steering;
		wheels[2].drive_activation +=  steering;
		wheels[3].drive_activation += -steering;

		//
		// Apply corresponding forces.
		//

		for (int i = 0; i < 4; i += 1)
		{
			wheels[i].drive_activation = Mathf.Clamp(wheels[i].drive_activation, -1.0f, 1.0f);
			Vector3 force = (wheels[i].transform.forward + wheels[i].transform.right * wheels[i].strafe_k) * wheels[i].drive_activation * wheels[i].drive_force;
			rigid_body.AddForce (force                                                                   / 4.0f); // @NOTE@ Division by amount of wheels.
			rigid_body.AddTorque(Vector3.Cross(wheels[i].transform.position - transform.position, force) / 4.0f); // @NOTE@ Division by amount of wheels.
		}
	}
}
