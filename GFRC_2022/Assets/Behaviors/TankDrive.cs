using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class TankDrive : MonoBehaviour
{
	Wheel[] wheels = new Wheel[6];

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

		foreach (var wheel in wheels)
		{
			wheel.activation = dampen(wheel.activation, movement.y + Vector3.Dot(wheel.transform.forward, Vector3.Cross(transform.position - wheel.transform.position, transform.up)) * steering, GREASE);
		}
	}
}
