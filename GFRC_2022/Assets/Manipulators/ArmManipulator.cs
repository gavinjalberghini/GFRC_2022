using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmManipulator : PrimaryManipulator
{
	public OmniArm omniarm;
	public Claw    claw;

	public override void free()
	{
		claw.try_releasing();
	}

	public void control(float yaw, float pitch, bool toggle)
	{
		omniarm.change_yaw  (yaw  );
		omniarm.change_pitch(pitch);
		if (toggle)
		{
			claw.toggle();
		}
	}
}
