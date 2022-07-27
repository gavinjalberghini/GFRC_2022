using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPointShooterManipulator : PrimaryManipulator
{
	public Shooter shooter;

	public override void free()
	{
	}

	public void control(bool shoot, CargoContainer[] cargo_containers)
	{
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
