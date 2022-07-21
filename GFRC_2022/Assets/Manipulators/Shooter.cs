using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : PrimaryManipulator
{
	public OmniArm omniarm;
	public float   force;

	public bool try_shooting(CargoContainer container)
	{
		GameObject cargo = container.try_unloading(true);
		if (cargo)
		{
			cargo.transform.position = transform.position + omniarm.arm().up * omniarm.length;
			cargo.GetComponent<Rigidbody>().AddForce(omniarm.arm().up * force, ForceMode.Impulse);
			return true;
		}
		return false;
	}
}
