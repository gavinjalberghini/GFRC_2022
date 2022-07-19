using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class KiwiDriveController : DriveController
{
	public Wheel[] wheels;

	public override void control(Vector2 translation, float steering)
	{
		translation = translation.magnitude > 1.0f ? translation.normalized : translation;
		steering    = Mathf.Clamp(steering, -1.0f, 1.0f);

		// @TODO@ Make strafing better.
		foreach (var wheel in wheels)
		{
			wheel.power =
				Mathf.Clamp
				(
					Vector3.Dot(wheel.direction(), transform.position - wheel.transform.position + v2_on_plane(transform.right, transform.forward, translation)) * translation.magnitude - steering,
					-1.0f,
					1.0f
				);
		}
	}
}
