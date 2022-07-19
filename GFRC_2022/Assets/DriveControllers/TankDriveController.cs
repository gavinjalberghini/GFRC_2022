using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class TankDriveController : DriveController
{
	public Wheel[] wheels_left;
	public Wheel[] wheels_right;

	public override void control(Vector2 translation, float steering)
	{
		translation = translation.magnitude > 1.0f ? translation.normalized : translation;
		steering    = Mathf.Clamp(translation.x + steering, -1.0f, 1.0f);

		foreach (var wheel in wheels_left)
		{
			wheel.power = translation.y + steering;
		}
		foreach (var wheel in wheels_right)
		{
			wheel.power = translation.y - steering;
		}
	}
}

