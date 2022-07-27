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
		dual_canes,
		human_feed_intake
	};

	[HideInInspector] public GameObject curr_primary_obj;
	[HideInInspector] public GameObject curr_secondary_obj;
	[HideInInspector] public Primary    curr_primary;
	[HideInInspector] public Secondary  curr_secondary;
	[HideInInspector] public bool       using_floor_intake;

	public GameObject[] ordered_primaries;
	public GameObject[] ordered_secondaries;
	public GameObject   floor_intake;

	public void pick(Primary new_primary)
	{
		if (curr_primary_obj)
		{
			Destroy(curr_primary_obj);
		}

		curr_primary = new_primary;
		if (new_primary != Primary.none)
		{
			curr_primary_obj = Instantiate(ordered_primaries[(int) new_primary - 1], transform);
			curr_primary_obj.SetActive(true);
			GetComponent<RobotBrain>().primary = ordered_primaries[(int) new_primary - 1].GetComponent<PrimaryManipulator>();
		}
		else
		{
			GetComponent<RobotBrain>().primary = null;
		}
	}

	public void pick(Secondary new_secondary)
	{
		if (curr_secondary_obj)
		{
			Destroy(curr_secondary_obj);
		}

		curr_secondary = new_secondary;
		if (new_secondary != Secondary.none)
		{
			curr_secondary_obj = Instantiate(ordered_secondaries[(int) new_secondary - 1], transform);
			curr_secondary_obj.SetActive(true);
			GetComponent<RobotBrain>().secondary = ordered_secondaries[(int) new_secondary - 1].GetComponent<SecondaryManipulator>();
		}
		else
		{
			GetComponent<RobotBrain>().secondary = null;
		}
	}

	public void set_floor_intake(bool state)
	{
		using_floor_intake = state;
		floor_intake.SetActive(state);
		GetComponent<RobotBrain>().floor_intake = state ? floor_intake.GetComponent<Intake>() : null;
	}
}
