using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriveController : DriveController
{
	public Wheel[] driving_wheels;
	public Wheel[] steering_wheels;
	public float   steering_angle;

	public override void control(Vector2 translation, float steering)
	{
		translation.y += Mathf.Abs(steering);
		translation    = translation.magnitude > 1.0f ? translation.normalized : translation;
		steering       = Mathf.Clamp(translation.x + steering, -1.0f, 1.0f);

		foreach (var wheel in driving_wheels)
		{
			wheel.power = translation.y;
		}

		foreach (var wheel in steering_wheels)
		{
			wheel.target_angle = steering * steering_angle;
			wheel.power        = translation.y;
		}
	}
}
