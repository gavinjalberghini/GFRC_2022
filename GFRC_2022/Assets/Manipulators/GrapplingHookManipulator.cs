using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookManipulator : SecondaryManipulator
{
	public OmniArm omniarm;
	public Grapple grapple;

	public override void free()
	{
	}

	public void control(bool shoot)
	{
		transform.Find("Grapple").GetComponent<Grapple>().anchor = transform.parent.GetComponent<Rigidbody>();
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

