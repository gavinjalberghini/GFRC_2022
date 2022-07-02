using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class KiwiDrive : MonoBehaviour
{
	Wheel[] wheels = new Wheel[3];

	void Start()
	{
		wheels[0] = transform.Find("Wheel 0").gameObject.GetComponent<Wheel>(); // @TODO@ Some Unity engineering to make it where adjusting the size of the base will also adjust the positions of the wheel.
		wheels[1] = transform.Find("Wheel 1").gameObject.GetComponent<Wheel>();
		wheels[2] = transform.Find("Wheel 2").gameObject.GetComponent<Wheel>();
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

		float[] MAGIC_SCALARS = new float[] { 1.0f, 1.7f, 1.7f }; // @TODO@ My spidey sense are tingling that we can do some linear algebra to find the right activations to move in a specific direction.
		for (int i = 0; i < wheels.Length; i += 1)
		{
			wheels[i].activation =
				dampen
				(
					wheels[i].activation,
					Mathf.Clamp
					(
						Vector3.Dot(wheels[i].transform.forward, transform.position - wheels[i].transform.position + v2_on_plane(transform.right, transform.forward, movement))
							* MAGIC_SCALARS[i]
							- steering,
						-1.0f,
						1.0f
					),
					GREASE
				);
		}
	}
}
