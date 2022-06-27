using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mechanum_drive : MonoBehaviour
{
	public float movement_force = 64.0f;
	public float torque         = 64.0f;
	public float max_speed      = 1.0f;

	GameObject robot_base;
	GameObject robot_pivot_indicator;
	Rigidbody  robot_base_rigid_body;
	Vector2    pivot_offset;

	void Start()
	{
		robot_base            = transform.Find("Base").gameObject;
		robot_pivot_indicator = transform.Find("Pivot Indicator").gameObject;
		robot_base_rigid_body = robot_base.GetComponent<Rigidbody>();
	}

	void Update()
	{
		//
		// Pivot change.
		//

		if (Input.GetKeyDown(KeyCode.LeftArrow )) { pivot_offset.x -= 1.0f; }
		if (Input.GetKeyDown(KeyCode.RightArrow)) { pivot_offset.x += 1.0f; }
		if (Input.GetKeyDown(KeyCode.DownArrow )) { pivot_offset.y -= 1.0f; }
		if (Input.GetKeyDown(KeyCode.UpArrow   )) { pivot_offset.y += 1.0f; }
		pivot_offset.x = Mathf.Clamp(pivot_offset.x, -1.0f, 1.0f);
		pivot_offset.y = Mathf.Clamp(pivot_offset.y, -1.0f, 1.0f);

		Vector3 pivot_indicator_position =
			robot_base.transform.position
				+ robot_base.transform.right   * robot_base.transform.localScale.x * 0.5f * pivot_offset.x
				+ robot_base.transform.forward * robot_base.transform.localScale.z * 0.5f * pivot_offset.y;

		//
		// Cardinal movement.
		//

		Vector2 movement = new Vector2(0.0f, 0.0f);
		if (Input.GetKey(KeyCode.A)) { movement.x -= 1.0f; }
		if (Input.GetKey(KeyCode.D)) { movement.x += 1.0f; }
		if (Input.GetKey(KeyCode.S)) { movement.y -= 1.0f; }
		if (Input.GetKey(KeyCode.W)) { movement.y += 1.0f; }
		movement *= movement_force;
		movement *= Mathf.Max(max_speed - Vector3.Magnitude(robot_base_rigid_body.velocity), 0.0f); // @NOTE@ Stops from forever accelerating.
		robot_base_rigid_body.AddForce(robot_base.transform.right * movement.x + robot_base.transform.forward * movement.y);

		//
		// Rotational movement.
		//

		// @TODO@ Change this to use forces?
		float rotation_amount = 0.0f;
		if (Input.GetKey(KeyCode.Q)) { rotation_amount -= 1.0f; }
		if (Input.GetKey(KeyCode.E)) { rotation_amount += 1.0f; }
		rotation_amount *= torque * Time.deltaTime;

		robot_base.transform.RotateAround(pivot_indicator_position, Vector3.up, rotation_amount);

		robot_pivot_indicator.transform.position = pivot_indicator_position;
	}
}
