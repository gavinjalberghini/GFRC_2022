using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Bucket : MonoBehaviour
{
	public float            height           = 1.0f;
	public float            min_height       = 0.1f;
	public float            max_height       = 0.75f;
	public float            pitch            = 0.0f;
	public CargoContainer[] cargo_containers = null;

	public Transform arm()    => transform.Find("Arm");
	public Transform basket() => transform.Find("Basket");

	float dampen_height = 0.0f;
	float dampen_pitch  = 0.0f;

	void readjust()
	{
		arm().localScale       = new Vector3(arm().localScale.x, dampen_height, arm().localScale.z);
		arm().localPosition    = new Vector3(0.0f, dampen_height / 2.0f, 0.0f);
		basket().localPosition = new Vector3(0.0f, dampen_height, 0.0f);
		basket().localRotation = Quaternion.Euler(dampen_pitch, 0.0f, 0.0f);
	}

	void OnValidate()
	{
		height = Mathf.Clamp(height, min_height, max_height);
		pitch  = Mathf.Clamp(pitch , 0.0f, 90.0f);
		dampen_height = height;
		dampen_pitch  = pitch;
		readjust();
	}

	void Update()
	{
		if (key_down(Key.DownArrow))
		{
			height -= 1.0f * Time.deltaTime;
		}
		if (key_down(Key.UpArrow))
		{
			height += 1.0f * Time.deltaTime;
		}
		height        = Mathf.Clamp(height, min_height, max_height);
		dampen_height = dampen(dampen_height, height, 0.0001f);

		if (key_down(Key.Comma))
		{
			pitch -= 90.0f * Time.deltaTime;
		}
		if (key_down(Key.Period))
		{
			pitch += 90.0f * Time.deltaTime;
		}
		pitch        = Mathf.Clamp(pitch , 0.0f, 90.0f);
		dampen_pitch = dampen(dampen_pitch, pitch, 0.0001f);

		if (key_now_down(Key.Enter))
		{
			foreach (var container in cargo_containers)
			{
				GameObject cargo = container.try_unloading();
				if (cargo)
				{
					cargo.transform.position = basket().position + basket().up * 0.3f;
					break;
				}
			}
		}

		readjust();
	}
}
