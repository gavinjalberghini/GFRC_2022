using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class DualCane : MonoBehaviour
{
	public float     width        = 0.25f;
	public float     height       = 0.15f;
	public float     min_height   = 0.05f;
	public float     max_height   = 1.0f;
	public float     height_speed = 1.0f;
	public Transform cane_left    = null;
	public Transform cane_right   = null;
	public Transform hook_left    = null;
	public Transform hook_right   = null;

	float dampen_height = 0.0f;

	void adjust_canes()
	{
		cane_left .localPosition = new Vector3(-width / 2.0f, dampen_height / 2.0f, 0.0f);
		cane_right.localPosition = new Vector3( width / 2.0f, dampen_height / 2.0f, 0.0f);
		cane_left .localScale    = new Vector3(cane_left .localScale.x, dampen_height / 2.0f, cane_left .localScale.z);
		cane_right.localScale    = new Vector3(cane_right.localScale.x, dampen_height / 2.0f, cane_right.localScale.z);
		hook_left .localPosition = new Vector3(-width / 2.0f, dampen_height, 0.0f);
		hook_right.localPosition = new Vector3( width / 2.0f, dampen_height, 0.0f);
	}

	void OnValidate()
	{
		width         = Mathf.Clamp(width, 0.01f, 0.5f);
		height        = Mathf.Clamp(height, min_height, max_height);
		dampen_height = height;
		adjust_canes();
	}

	void Update()
	{
		height -= key_down(Key.J) ? height_speed * Time.deltaTime : 0.0f;
		height += key_down(Key.K) ? height_speed * Time.deltaTime : 0.0f;
		height  = Mathf.Clamp(height, min_height, max_height);
		dampen_height = dampen(dampen_height, height, 0.0001f);
		adjust_canes();
	}
}
