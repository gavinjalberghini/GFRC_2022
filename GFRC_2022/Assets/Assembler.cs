using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
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
	[HideInInspector]        bool       is_red_alliance = true;

	public GameObject[] ordered_primaries;
	public GameObject[] ordered_secondaries;
	public GameObject   floor_intake;
	public Material     mat_red;
	public Material     mat_blue;

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
			GetComponent<RobotBrain>().primary = curr_primary_obj.GetComponent<PrimaryManipulator>();
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
			GetComponent<RobotBrain>().secondary = curr_secondary_obj.GetComponent<SecondaryManipulator>();
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

	public void set_alliance(bool is_red)
	{
		is_red_alliance = is_red;
		foreach (Transform transform in transform.Find("Body"))
		{
			transform.gameObject.GetComponent<MeshRenderer>().material = is_red ? mat_red : mat_blue;
		}
	}

	public void free()
	{
		if (curr_primary_obj  ) curr_primary_obj  .GetComponent<PrimaryManipulator  >().free();
		if (curr_secondary_obj) curr_secondary_obj.GetComponent<SecondaryManipulator>().free();
		foreach (var container in GetComponent<RobotBrain>().cargo_containers)
		{
			container.try_unloading(true);
		}
	}

	void Update()
	{
		if (RobotBrain.subtype<DualCaneManipulator>(GetComponent<RobotBrain>().secondary))
		{
			Action<Transform> set = null;
			set = (Transform t) => {
				t.gameObject.layer = is_red_alliance ? 8 : 9;
				foreach (Transform u in t)
				{
					set(u);
				}
			};

			set(GetComponent<RobotBrain>().secondary.transform);
		}
		else if (RobotBrain.subtype<GrapplingHookManipulator>(GetComponent<RobotBrain>().secondary))
		{
			GetComponent<RobotBrain>().secondary.transform.Find("Grapple").Find("Hook").tag = "Hook";
		}
	}
}
