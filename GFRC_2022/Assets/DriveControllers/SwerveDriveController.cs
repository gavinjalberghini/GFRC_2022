using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class SwerveDriveController : DriveController
{
	public Wheel[] wheels;

	public override void control(Vector2 translation, float steering)
	{
		translation = translation.magnitude > 1.0f ? translation.normalized : translation;
		steering    = Mathf.Clamp(steering, -1.0f, 1.0f);

		foreach (var wheel in wheels)
		{
			Vector2 way = translation + new Vector2(wheel.transform.localPosition.z, -wheel.transform.localPosition.x).normalized * steering;
			if (way.magnitude > 0.0001f)
			{
				wheel.target_angle = (90.0f - argument(way) / TAU * 360.0f);
				wheel.power        = 1.0f;
			}
			else
			{
				wheel.power = 0.0f;
			}
		}
	}
}
