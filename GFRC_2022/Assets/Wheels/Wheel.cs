using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
	public Transform drive_indicator;
	public Transform strafe_indicator;
	public Transform net_force_indicator;
	public float     angle       = 0.0f;
	public float     max_torque  = 2.0f;
	public float     strafe_k    = 1.0f;
	public float     activation  = 0.0f;

	void update_indicators()
	{
		Transform vehicle = GetComponent<WheelCollider>()?.attachedRigidbody?.transform;

		if (vehicle != null)
		{
			transform.rotation = vehicle.rotation * Quaternion.Euler(0.0f, angle, 0.0f);

			drive_indicator.transform.rotation = vehicle.rotation * Quaternion.Euler(0.0f, angle, 0.0f);
			drive_indicator.localScale         = new Vector3(drive_indicator.localScale.x, drive_indicator.localScale.y, activation);

			if (strafe_indicator != null)
			{
				strafe_indicator.transform.rotation = vehicle.rotation * Quaternion.Euler(0.0f, angle, 0.0f);
				strafe_indicator.localScale         = new Vector3(strafe_k * activation, strafe_indicator.localScale.y, strafe_indicator.localScale.z);

				// @NOTE@ Wrong dimensions, but so what?
				Vector3 net_force = Quaternion.Euler(0.0f, angle, 0.0f) * new Vector3(strafe_k, 0.0f, 1.0f) * activation;
				if (net_force.magnitude > 0.0001f)
				{
					net_force_indicator.transform.rotation = vehicle.rotation * Quaternion.LookRotation(net_force, vehicle.up);
					net_force_indicator.localScale         = new Vector3(net_force_indicator.localScale.x, net_force_indicator.localScale.y, net_force.magnitude);
				}
				else
				{
					net_force_indicator.localScale         = new Vector3(net_force_indicator.localScale.x, net_force_indicator.localScale.y, 0.0f);
				}
			}
		}
	}

	void OnValidate()
	{
		strafe_k   = Mathf.Clamp(strafe_k  , -1.0f, 1.00f);
		activation = Mathf.Clamp(activation, -1.0f, 1.00f);
		update_indicators();
	}

	void Update()
	{
		GetComponent<WheelCollider>().steerAngle  = angle;
		GetComponent<WheelCollider>().motorTorque = max_torque * activation;
		update_indicators();
	}
}
