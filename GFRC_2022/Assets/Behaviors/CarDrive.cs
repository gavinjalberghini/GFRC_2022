using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class CarDrive : MonoBehaviour
{
	Transform robot_base;
	Wheel[]   wheels      = new Wheel[4];
	float     steer_angle = 0.0f;

	void Start()
	{
		robot_base = transform.Find("Base"    );
		wheels[0]  = transform.Find("Wheel BL").gameObject.GetComponent<Wheel>(); // @TODO@ Some Unity engineering to make it where adjusting the size of the base will also adjust the positions of the wheel.
		wheels[1]  = transform.Find("Wheel BR").gameObject.GetComponent<Wheel>();
		wheels[2]  = transform.Find("Wheel FL").gameObject.GetComponent<Wheel>();
		wheels[3]  = transform.Find("Wheel FR").gameObject.GetComponent<Wheel>();
	}

	void Update()
	{
		const float GREASE = 0.000001f; // @NOTE@ How quickly the movement and steering changes.

		//
		// Movement.
		//

		Vector2 movement = left_stick();
		if (movement == new Vector2(0.0f, 0.0f))
		{
			movement = wasd_normalized();
		}

		float steering = right_stick().x;
		if (steering == 0.0f)
		{
			if (Keyboard.current[Key.Q].isPressed) { steering += -1.0f; }
			if (Keyboard.current[Key.E].isPressed) { steering +=  1.0f; }
		}
		steer_angle = dampen(steer_angle, steering * 60.0f, GREASE);

		for (int i = 0; i < 2; i += 1)
		{
			wheels[i].activation = dampen(wheels[i].activation, Mathf.Clamp(movement.y, -1.0f, 1.0f), GREASE);
		}
		for (int i = 2; i < 4; i += 1)
		{
			wheels[i].angle      = steer_angle;
			wheels[i].activation = dampen(wheels[i].activation, Mathf.Clamp(movement.y, -1.0f, 1.0f), GREASE);
		}
	}
}

