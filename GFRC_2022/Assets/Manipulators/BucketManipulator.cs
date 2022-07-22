using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class BucketManipulator : PrimaryManipulator
{
	[Header("Height")]
	public float height     = 1.0f;
	public float height_min = 0.1f;
	public float height_max = 0.75f;

	[Header("Pitch")]
	public float pitch      = 0.0f;
	public float pitch_min  = 0.0f;
	public float pitch_max  = 0.0f;

	[HideInInspector] public float target_height;
	[HideInInspector] public float target_pitch;

	public bool try_loading(Intake intake)
	{
		if (intake.cargo)
		{
			load(intake.cargo);
			intake.cargo = null;
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool try_loading(CargoContainer container)
	{
		GameObject obj = container.try_unloading(true);
		if (obj)
		{
			load(obj);
			return true;
		}
		else
		{
			return false;
		}
	}

	void load(GameObject cargo)
	{
		cargo.transform.position = transform.position + transform.up * (height + 0.2f);
	}

	void OnValidate()
	{
		target_height = height;
		target_pitch  = pitch;
		Update();
	}

	void Update()
	{
		height_min    = Mathf.Clamp(height_min, 0.0f, height_max);
		height_max    = Mathf.Clamp(height_max, height_min, 4.0f);
		height        = Mathf.Clamp(       height, height_min, height_max);
		target_height = Mathf.Clamp(target_height, height_min, height_max);
		height        = dampen(height, target_height, 0.0001f);

		pitch_min     = Mathf.Clamp(pitch_min, -90, pitch_max);
		pitch_max     = Mathf.Clamp(pitch_max, pitch_min, 90);
		pitch         = Mathf.Clamp(       pitch, pitch_min, pitch_max);
		target_pitch  = Mathf.Clamp(target_pitch, pitch_min, pitch_max);
		pitch         = dampen(pitch, target_pitch, 0.0001f);

		set_local_pos_y     (transform.Find("Arm"   ), height / 2.0f);
		set_local_scale_y   (transform.Find("Arm"   ), height       );
		set_local_pos_y     (transform.Find("Basket"), height       );
		set_local_rotation_x(transform.Find("Basket"), pitch        );
	}
}
