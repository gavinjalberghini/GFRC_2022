using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class ForkliftDriveController : DriveController // @TODO@ Does not feel good.
{
	public Wheel wheel_back_left;
	public Wheel wheel_back_right;
	public Wheel wheel_front_left;
	public Wheel wheel_front_right;

	public override void control(Vector2 translation, float steering)
	{
		translation = translation.magnitude > 1.0f ? translation.normalized : translation;
		steering    = Mathf.Clamp(translation.x + steering, -1.0f, 1.0f);

		wheel_back_left  .power = translation.y - translation.x + steering;
		wheel_back_right .power = translation.x + translation.y - steering;
		wheel_front_left .power = translation.x + translation.y           ;
		wheel_front_right.power = translation.y - translation.x           ;
	}
}

