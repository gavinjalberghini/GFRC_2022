using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMountedShooterManipulator : PrimaryManipulator
{
	public Shooter shooter;

	public override void free()
	{
	}

	public void control(float yaw, float pitch, bool shoot, CargoContainer[] cargo_containers)
	{
		shooter.omniarm.change_yaw  (yaw);
		shooter.omniarm.change_pitch(pitch);
		if (shoot)
		{
			foreach (var container in cargo_containers)
			{
				if (shooter.try_shooting(container))
				{
					break;
				}
			}
		}
	}
}
