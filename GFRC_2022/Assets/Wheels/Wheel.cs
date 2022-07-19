using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class Wheel : MonoBehaviour
{
	public static bool show_indicator = false;

	public float radius;
	public float angle;
	public float drive_torque;
	public float stall_torque;
	public float strafe_force;

	[HideInInspector] public float power;
	[HideInInspector] public float target_angle;

	float dampen_indicator_scalar;

	Transform tire            () => transform.Find("Tire");
	Transform drive_indicator () => transform.Find("Drive Indicator");
	Transform strafe_indicator() => transform.Find("Strafe Indicator");

	public Vector3 direction() => tire().forward;

	void OnValidate()
	{
		target_angle = angle;
		Update();
	}

	void Update()
	{
		radius       = Mathf.Clamp(radius, 0.03f, 0.1f);
		angle        = mod(       angle, -180.0f, 180.0f);
		target_angle = mod(target_angle, -180.0f, 180.0f);
		angle        = dampen_angle(angle, target_angle, 0.0001f);
		power        = Mathf.Clamp(power, -1.0f, 1.0f);
		drive_torque = Mathf.Max(drive_torque, 0.0f);
		stall_torque = Mathf.Max(stall_torque, 0.0f);

		GetComponent<WheelCollider>().radius = radius;
		set_local_scale_x(tire(), radius * 2.0f);
		set_local_scale_z(tire(), radius * 2.0f);

		GetComponent<WheelCollider>().steerAngle = angle;
		set_local_rotation_y(tire(), angle);

		GetComponent<WheelCollider>().motorTorque = power * drive_torque;
		GetComponent<WheelCollider>().brakeTorque = power == 0.0f ? stall_torque : 0.0f;

		if (GetComponent<WheelCollider>().isGrounded)
		{
			GetComponent<WheelCollider>().attachedRigidbody?.AddForce(transform.right * strafe_force * power);
		}

		dampen_indicator_scalar = dampen(dampen_indicator_scalar, power * 0.85f, 0.00001f);
		drive_indicator ().gameObject.SetActive(show_indicator && Mathf.Abs(dampen_indicator_scalar) > 0.001f);
		strafe_indicator().gameObject.SetActive(show_indicator && Mathf.Abs(dampen_indicator_scalar) > 0.001f);
		set_local_rotation_y(drive_indicator (), angle);
		set_local_rotation_y(strafe_indicator(), angle + 90.0f);
		set_local_scale_z(drive_indicator (), dampen_indicator_scalar);
		set_local_scale_z(strafe_indicator(), dampen_indicator_scalar * strafe_force * 0.5f);
	}
}
