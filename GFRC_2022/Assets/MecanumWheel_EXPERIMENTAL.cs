using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class MecanumWheel_EXPERIMENTAL : MonoBehaviour
{
	public Transform drive_indicator     = null;
	public Transform strafe_indicator    = null;
	public Transform net_force_indicator = null;
	public float     radius              = 0.25f;
	public float     max_torque          = 2.0f;
	public float     strafe_k            = 1.0f;
	public float     activation          = 0.0f;

	public float get_width() => transform.Find("Tire").Find("Cylinder").localScale.y;

	void update_indicators()
	{
		drive_indicator    .localScale = new Vector3(drive_indicator.localScale.x, drive_indicator.localScale.y, activation                              );
		strafe_indicator   .localScale = new Vector3(activation * strafe_k       , drive_indicator.localScale.y, drive_indicator.localScale.z            );
		net_force_indicator.localScale = new Vector3(drive_indicator.localScale.x, drive_indicator.localScale.y, hypot(activation * strafe_k, activation));

		if (Mathf.Abs(activation) > 0.001f)
		{
			net_force_indicator.rotation = transform.rotation * Quaternion.LookRotation(new Vector3(activation * strafe_k, 0.0f, activation), transform.up);
		}
	}

	void OnValidate()
	{
		radius     = Mathf.Clamp(radius    ,  0.1f, 0.15f);
		strafe_k   = Mathf.Clamp(strafe_k  , -1.0f, 1.00f);
		activation = Mathf.Clamp(activation, -1.0f, 1.00f);
		GetComponent<WheelCollider>().radius = radius;
		transform.Find("Tire").Find("Cylinder").localScale = new Vector3(radius * 2.0f, 0.01f, radius * 2.0f);
		transform.Find("Tire").Find("Collider").localScale = transform.Find("Tire").Find("Cylinder").localScale * 0.9f;
		update_indicators();
	}

	void Update()
	{
		if (GetComponent<WheelCollider>().isGrounded)
		{
			GetComponent<Rigidbody>().AddForce(transform.right * activation * strafe_k);
		}
		GetComponent<WheelCollider>().motorTorque = max_torque * activation;

		update_indicators();
	}
}

