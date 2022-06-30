using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
	public float drive_force      = 6.0f; // @NOTE@ Amount of force driving the wheel forward/backwards.
	public float drive_activation = 0.0f; // @NOTE@ Amount of "spinning" the wheel is doing.
	public float strafe_k         = 0.0f; // @NOTE@ Ratio of the strafe force to the drive force.

	Transform tire;
	Transform drive_force_indicator;
	Transform strafe_force_indicator;
	Transform net_force_indicator;

	void Start()
	{
		tire                   = transform.Find("Tire"                  ).gameObject.transform;
		drive_force_indicator  = transform.Find("Drive Force Indicator" ).gameObject.transform;
		strafe_force_indicator = transform.Find("Strafe Force Indicator").gameObject.transform;
		net_force_indicator    = transform.Find("Net Force Indicator"   ).gameObject.transform;
	}

	void Update()
	{
		const float FORCE_INDICATOR_SCALAR = 0.05f;

		//
		// Scale the drive force indicator.
		//

		drive_force_indicator.localScale = new Vector3(0.05f, drive_activation * drive_force * FORCE_INDICATOR_SCALAR, 0.05f);
		drive_force_indicator.position   = tire.position + transform.forward * drive_force_indicator.localScale.y;

		//
		// Scale the strafe force indicator.
		//

		strafe_force_indicator.localScale = new Vector3(0.05f, drive_activation * drive_force * strafe_k * FORCE_INDICATOR_SCALAR, 0.05f);
		strafe_force_indicator.position   = tire.position + transform.right * strafe_force_indicator.localScale.y;

		//
		// Scale and rotate the net force indicator.
		//

		if (strafe_k != 0.0f)
		{
			Vector2 net_force = new Vector2(drive_activation * drive_force * strafe_k, drive_activation * drive_force);
			net_force_indicator.localScale = new Vector3(0.05f, net_force.magnitude * FORCE_INDICATOR_SCALAR, 0.05f);
			net_force_indicator.position   = tire.position + transform.right * net_force_indicator.localScale.y;
			net_force_indicator.rotation   = transform.rotation * Quaternion.Euler(0.0f, 0.0f, 90.0f);
			if (net_force.magnitude > 0.001f)
			{
				net_force_indicator.RotateAround(tire.position, transform.up, -Mathf.Atan2(net_force.y, net_force.x) * 180.0f / Mathf.PI);
			}
		}
		else
		{
			net_force_indicator.localScale = new Vector3(0.0f, 0.0f, 0.0f);
		}
	}
}
