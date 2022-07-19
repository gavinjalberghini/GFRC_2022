using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class DualCane : MonoBehaviour
{
	public float spacing;
	public float height;
	public float height_min;
	public float height_max;

	[HideInInspector] public float target_height;

	void OnValidate()
	{
		target_height = height;
		Update();
	}

	void Update()
	{
		spacing       = Mathf.Max(spacing, 0.0f);
		height_min    = Mathf.Clamp(height_min, 0.0f, height_max);
		height_max    = Mathf.Clamp(height_max, height_min, 4.0f);
		height        = Mathf.Clamp(       height, height_min, height_max);
		target_height = Mathf.Clamp(target_height, height_min, height_max);
		height        = dampen(height, target_height, 0.0001f);

		set_local_scale_y(transform.Find("Cylinder L"), height / 2.0f);
		set_local_scale_y(transform.Find("Cylinder R"), height / 2.0f);
		transform.Find("Cylinder L").localPosition = new Vector3(-spacing / 2.0f, height / 2.0f, 0.0f);
		transform.Find("Cylinder R").localPosition = new Vector3( spacing / 2.0f, height / 2.0f, 0.0f);
		transform.Find("Hook L")    .localPosition = new Vector3(-spacing / 2.0f, height       , 0.0f);
		transform.Find("Hook R")    .localPosition = new Vector3( spacing / 2.0f, height       , 0.0f);
	}
}
