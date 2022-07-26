using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assembler : MonoBehaviour
{
	public enum Primary
	{
		none,
		turret_mounted_shooter,
		fixed_point_shooter,
		simple_arm,
		jointed_arm,
		telescopic_arm,
		bucket
	};

	public enum Secondary
	{
		none,
		grappling_hook,
		dual_canes
	};

	[HideInInspector] public GameObject curr_primary;
	[HideInInspector] public GameObject curr_secondary;

	public GameObject[] ordered_primaries;
	public GameObject[] ordered_secondaries;

	public void pick(Primary new_primary)
	{
		if (curr_primary)
		{
			Destroy(curr_primary);
		}

		if (new_primary != Primary.none)
		{
			curr_primary = Instantiate(ordered_primaries[(int) new_primary - 1], transform);
			curr_primary.SetActive(true);
			GetComponent<RobotBrain>().primary = ordered_primaries[(int) new_primary - 1].GetComponent<PrimaryManipulator>();
		}
		else
		{
			GetComponent<RobotBrain>().primary = null;
		}
	}

	public void pick(Secondary new_secondary)
	{
		if (curr_secondary)
		{
			Destroy(curr_secondary);
		}

		if (new_secondary != Secondary.none)
		{
			curr_secondary = Instantiate(ordered_secondaries[(int) new_secondary - 1], transform);
			curr_secondary.SetActive(true);
			GetComponent<RobotBrain>().secondary = ordered_primaries[(int) new_secondary - 1].GetComponent<SecondaryManipulator>();
		}
		else
		{
			GetComponent<RobotBrain>().secondary = null;
		}
	}
}
