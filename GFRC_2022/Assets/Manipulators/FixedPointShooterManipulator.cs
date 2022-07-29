using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPointShooterManipulator : PrimaryManipulator
{
	public Shooter shooter;

	public override void free()
	{
	}

	public void control(bool shoot, CargoContainer cargo_container)
	{
		if (shoot)
		{
			shooter.try_shooting(cargo_container);
		}
	}
}
