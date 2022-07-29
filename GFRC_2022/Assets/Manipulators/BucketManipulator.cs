using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class BucketManipulator : PrimaryManipulator
{
	public Transform spawn_point;

	[Header("Height")]
	public float height       = 1.0f;
	public float height_min   = 0.1f;
	public float height_max   = 0.75f;
	public float height_speed = 1.0f;

	[Header("Pitch")]
	public float pitch       = 0.0f;
	public float pitch_min   = 0.0f;
	public float pitch_max   = 0.0f;
	public float pitch_speed = 90.0f;

	float target_height;
	float target_pitch;

	public float change_height(float amount) => target_height = Mathf.Clamp(target_height + Mathf.Clamp(amount, -1.0f, 1.0f) * height_speed * Time.deltaTime, height_min, height_max);
	public float change_pitch (float amount) => target_pitch  = Mathf.Clamp(target_pitch  + Mathf.Clamp(amount, -1.0f, 1.0f) * pitch_speed  * Time.deltaTime, pitch_min , pitch_max );

	public override void free()
	{
	}

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
		cargo.transform.position = spawn_point.position;
	}

	public void control(float pitch, float length, bool store, CargoContainer cargo_container)
	{
		if (store)
		{
			try_loading(cargo_container);
		}
		change_height(length);
		change_pitch (pitch);
	}

	void OnValidate()
	{
		target_height = height;
		target_pitch  = pitch;
		Update();
	}

	void Awake()
	{
		OnValidate();
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
