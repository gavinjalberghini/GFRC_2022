using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class SwerveDrive : MonoBehaviour
{
	Transform pivot;
	Transform robot_base;
	Wheel[]   wheels       = new Wheel[4];
	Vector2   pivot_offset = new Vector2(0.0f, 0.0f);

	void Start()
	{
		pivot      = transform.Find("Pivot Indicator");
		robot_base = transform.Find("Base"           );
		wheels[0]  = transform.Find("Wheel BL"       ).gameObject.GetComponent<Wheel>(); // @TODO@ Some Unity engineering to make it where adjusting the size of the base will also adjust the positions of the wheel.
		wheels[1]  = transform.Find("Wheel BR"       ).gameObject.GetComponent<Wheel>();
		wheels[2]  = transform.Find("Wheel FL"       ).gameObject.GetComponent<Wheel>();
		wheels[3]  = transform.Find("Wheel FR"       ).gameObject.GetComponent<Wheel>();
	}

	void Update()
	{
		const float GREASE = 0.000001f; // @NOTE@ How quickly the movement and steering changes.

		//
		// Pivot change.
		//

		// @TODO@ Change this to presses?
		pivot_offset   += (arrow_keys() + gamepad_buttons()).normalized * Time.deltaTime;
		pivot_offset.x  = Mathf.Clamp(pivot_offset.x, -0.5f, 0.5f);
		pivot_offset.y  = Mathf.Clamp(pivot_offset.y, -0.5f, 0.5f);
		pivot.position  = transform.position + v2_on_plane(transform.right * robot_base.localScale.x, transform.forward * robot_base.localScale.z, pivot_offset);

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

		for (int i = 0; i < wheels.Length; i += 1)
		{
			Vector3 to_pivot        = pivot.position - wheels[i].transform.position;
			Vector2 pivot_direction = new Vector2(Vector3.Dot(to_pivot, transform.right), Vector3.Dot(to_pivot, transform.forward)) * steering;
			if (movement != new Vector2(0.0f, 0.0f) || steering != 0.0f)
			{
				wheels[i].angle +=
					dampen
					(
						0.0f,
						min_degree_arc
						(
							wheels[i].angle,
							-360.0f / TAU * ((pivot_direction.magnitude > 0.0001f ? argument(pivot_direction) : 0.0f) + argument(rotate(movement, -90.0f)))
						),
						GREASE
					);
			}
			wheels[i].angle       = mod(wheels[i].angle, 360.0f);
			wheels[i].activation  = dampen(wheels[i].activation, Mathf.Clamp(movement.magnitude + Mathf.Abs(steering) * to_pivot.magnitude, -1.0f, 1.0f), GREASE);
		}
	}
}
