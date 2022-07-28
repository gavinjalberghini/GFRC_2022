using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMountedShooterManipulator : PrimaryManipulator
{
	public Shooter shooter;

	public override void free()
	{
	}

	public void control(float yaw, float pitch, bool shoot, CargoContainer cargo_container)
	{
		shooter.omniarm.change_yaw  (yaw);
		shooter.omniarm.change_pitch(pitch);
		if (shoot)
		{
			shooter.try_shooting(cargo_container);
		}
	}
}
