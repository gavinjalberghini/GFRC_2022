using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class KiwiDrive : MonoBehaviour
{
	public Transform[] sides  = new Transform[3];
	public Wheel[]     wheels = new Wheel[3];
	public float       radius = 0.5f;

	void OnValidate()
	{
		radius = Mathf.Clamp(radius, 0.2f, 1.0f);
		for (int i = 0; i < wheels.Length; i += 1)
		{
			sides [i].transform.localScale = new Vector3(Mathf.Sqrt(3.0f) * radius, sides[i].transform.localScale.y, sides[i].transform.localScale.z);
			sides [i].transform.rotation   = Quaternion.AngleAxis(-360.0f * i / wheels.Length, transform.up) * transform.rotation;
			sides [i].transform.position   =
			wheels[i].transform.position   = transform.position + v2_on_plane(-transform.forward, transform.right, polar(TAU * i / wheels.Length)) * radius / 2.0f;
			wheels[i].angle                = -360.0f * i / wheels.Length + 90.0f;
		}
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

		// @TODO@ Make strafing better.
		foreach (var wheel in wheels)
		{
			wheel.activation =
				dampen
				(
					wheel.activation,
					Mathf.Clamp
					(
						Vector3.Dot(wheel.transform.forward, transform.position - wheel.transform.position + v2_on_plane(transform.right, transform.forward, movement)) - steering,
						-1.0f,
						1.0f
					),
					GREASE
				);
		}
	}
}
