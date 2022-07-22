using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HDriveController : DriveController
{
	public Wheel wheel_back_left;
	public Wheel wheel_back_right;
	public Wheel wheel_front_left;
	public Wheel wheel_front_right;
	public Wheel wheel_center;

	public override void control(Vector2 translation, float steering)
	{
		translation = translation.magnitude > 1.0f ? translation.normalized : translation;
		steering    = Mathf.Clamp(steering, -1.0f, 1.0f);

		wheel_back_right .power =
		wheel_front_right.power = translation.y - steering;
		wheel_back_left  .power =
		wheel_front_left .power = translation.y + steering;
		wheel_center     .power = translation.x;
	}
}

