using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookManipulator : SecondaryManipulator
{
	public OmniArm omniarm;
	public Grapple grapple;

	public void control(bool shoot)
	{
		if (shoot)
		{
			grapple.toggle();
		}
		if (grapple.state == Grapple.GrappleState.hooked)
		{
			grapple.length_max -= 1.0f * Time.deltaTime;
		}
	}
}

