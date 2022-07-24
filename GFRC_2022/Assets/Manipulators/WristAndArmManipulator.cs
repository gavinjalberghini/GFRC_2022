using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristAndArmManipulator : PrimaryManipulator
{
	public OmniArm omniarm_lower;
	public OmniArm omniarm_upper;
	public Claw    claw;
	[HideInInspector] public bool using_upper;

	public void control(float yaw, float pitch, bool joint_toggle, bool grab_toggle)
	{
		if (joint_toggle)
		{
			using_upper = !using_upper;
		}

		(using_upper ? omniarm_upper : omniarm_lower).change_yaw  (yaw);
		(using_upper ? omniarm_upper : omniarm_lower).change_pitch(pitch);

		if (grab_toggle)
		{
			claw.toggle();
		}
	}
}

