using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class mechanum_drive_wheeled : MonoBehaviour
{
	public float max_movement_speed = 4.0f;
	public float max_steering_speed = 6.0f;

	GameObject robot_base;
	Rigidbody  rigid_body;
	Wheel[]    wheels = new Wheel[4];
	Vector2    pivot_offset;

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
		float[] wheel_target_activations = new float[4];

		//
		// Cardinal movement.
		//


		{
			Vector2 movement = Gamepad.current == null ? new Vector2(0.0f, 0.0f) : Gamepad.current.leftStick.ReadValue();
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
			movement *= Mathf.Max(max_movement_speed - rigid_body.velocity.magnitude, 0.0f); // @TODO@ Stops from forever accelerating. Could be better...

			wheel_target_activations[0] = wheel_target_activations[3] = movement.y - movement.x;
			wheel_target_activations[1] = wheel_target_activations[2] = movement.x + movement.y;
		}

		//
		// Rotational movement.
		//

		{
			float steering = Gamepad.current == null ? 0.0f : Gamepad.current.rightStick.ReadValue().x;
			if (steering == 0.0f)
			{
				if (Keyboard.current[Key.Q].isPressed) { steering -= 1.0f; }
				if (Keyboard.current[Key.E].isPressed) { steering += 1.0f; }
			}
			steering *= Mathf.Max(max_steering_speed - rigid_body.angularVelocity.magnitude, 0.0f); // @TODO@ Stops from forever accelerating. Could be better...

			wheel_target_activations[0] +=  steering;
			wheel_target_activations[1] += -steering;
			wheel_target_activations[2] +=  steering;
			wheel_target_activations[3] += -steering;
		}

		//
		// Apply corresponding forces.
		//

		for (int i = 0; i < 4; i += 1)
		{
			const float DAMPENING_CONSTANT = 0.0001f; // @TODO@ Make this adjustable?
			wheel_target_activations[i] = Mathf.Clamp(wheel_target_activations[i], -1.0f, 1.0f);
			wheels[i].drive_activation  = wheel_target_activations[i] + (wheels[i].drive_activation - wheel_target_activations[i]) * Mathf.Pow(DAMPENING_CONSTANT, Time.deltaTime); // @TODO@ Extract framerate independent dampening into a global function.

			Vector3 force = (wheels[i].transform.forward + wheels[i].transform.right * wheels[i].strafe_k) * wheels[i].drive_activation * wheels[i].drive_force;
			rigid_body.AddForce (force                                                                   / 4.0f); // @NOTE@ Division by amount of wheels.
			rigid_body.AddTorque(Vector3.Cross(wheels[i].transform.position - transform.position, force) / 4.0f); // @NOTE@ Division by amount of wheels.
		}
	}
}
