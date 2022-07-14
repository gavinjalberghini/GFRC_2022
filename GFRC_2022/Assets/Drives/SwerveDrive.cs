using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class SwerveDrive : MonoBehaviour
{
	public Transform drive_base;
	public Transform drive_head;
	public Transform pivot;
	public Wheel[]   wheels = new Wheel[4];
	public Vector2   dims   = new Vector2(0.5f, 0.5f);

	Vector2 pivot_offset = new Vector2(0.0f, 0.0f);

	void OnValidate()
	{
		dims.x                       = Mathf.Clamp(dims.x, 0.25f, 1.0f);
		dims.y                       = Mathf.Clamp(dims.y, 0.25f, 1.0f);
		drive_base.localScale        = new Vector3(dims.x, 0.05f, dims.y);
		drive_head.position          = drive_base.position + drive_base.forward * (dims.y + drive_head.localScale.z) * 0.5f;
		wheels[0].transform.position = drive_base.position + drive_base.right * dims.x * -0.5f + drive_base.forward * dims.y * -0.5f;
		wheels[1].transform.position = drive_base.position + drive_base.right * dims.x *  0.5f + drive_base.forward * dims.y * -0.5f;
		wheels[2].transform.position = drive_base.position + drive_base.right * dims.x * -0.5f + drive_base.forward * dims.y *  0.5f;
		wheels[3].transform.position = drive_base.position + drive_base.right * dims.x *  0.5f + drive_base.forward * dims.y *  0.5f;
	}

	void Update()
	{
		const float GREASE = 0.000001f; // @NOTE@ How quickly the movement and steering changes.

		//
		// Pivot change.
		//

		// @TODO@ Pivot to change depending on movement and direction.
		// pivot_offset   += (arrow_keys() + gamepad_buttons()).normalized * Time.deltaTime;
		// pivot_offset.x  = Mathf.Clamp(pivot_offset.x, -0.5f, 0.5f);
		// pivot_offset.y  = Mathf.Clamp(pivot_offset.y, -0.5f, 0.5f);
		// pivot.position  = transform.position + v2_on_plane(transform.right * drive_base.localScale.x, transform.forward * drive_base.localScale.z, pivot_offset);

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
