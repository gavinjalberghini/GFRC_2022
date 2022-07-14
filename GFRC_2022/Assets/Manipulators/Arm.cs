using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Arm : MonoBehaviour
{
	public float            length           = 0.75f;
	public bool             fix_pitch        = false;
	public float            pitch            = 45.0f;
	public float            pitch_min        = 10.0f;
	public float            pitch_max        = 80.0f;
	public float            pitch_speed      = 90.0f;
	public float            pitch_dampening  = 0.01f;
	public bool             fix_yaw          = false;
	public float            yaw              = 0.0f;
	public float            yaw_range        = 360.0f;
	public float            yaw_speed        = 180.0f;
	public float            yaw_dampening    = 0.001f;
	public CargoContainer[] cargo_containers = null;
	public Transform        neck             = null;
	public Transform        holder           = null;

	float dampen_pitch = 0.0f;
	float dampen_yaw   = 0.0f;

	void OnValidate()
	{
		length                          = Mathf.Clamp(length, 0.1f, 1.0f);
		pitch                           = Mathf.Clamp(pitch, pitch_min, pitch_max);
		pitch_min                       = Mathf.Clamp(pitch_min, 0.0f, pitch_max);
		pitch_max                       = Mathf.Clamp(pitch_max, pitch_min, 80.0f);
		yaw                             = Mathf.Clamp(mod(yaw + 180.0f, 360.0f) - 180.0f, -yaw_range / 2.0f, yaw_range / 2.0f);
		yaw_range                       = Mathf.Clamp(yaw_range, 0.0f, 360.0f);
		dampen_pitch                    = pitch;
		dampen_yaw                      = yaw;
		neck.localRotation              = Quaternion.Euler(-pitch, yaw, 0.0f);
		neck.Find("Cube").localScale    = new Vector3(0.05f, 0.05f, length);
		neck.Find("Cube").localPosition = new Vector3(0.0f, 0.0f, length / 2.0f);
		holder.position                 = transform.position + Quaternion.AngleAxis(dampen_yaw, transform.up) * transform.forward * holder.localScale.z / 2.0f + neck.forward * neck.Find("Cube").localScale.z;
	}

	void OnStart()
	{
		dampen_pitch = pitch;
		dampen_yaw   = yaw;
	}

	void Update()
	{
		Vector2 angle_delta = dpad();
		if (angle_delta == new Vector2(0.0f, 0.0f))
		{
			angle_delta = arrow_keys();
		}

		if (!fix_pitch)
		{
			pitch = Mathf.Clamp(pitch + angle_delta.y * pitch_speed * Time.deltaTime, pitch_min, pitch_max);
		}
		if (!fix_yaw)
		{
			yaw = Mathf.Clamp(mod(yaw + angle_delta.x * yaw_speed * Time.deltaTime + 180.0f, 360.0f) - 180.0f, -yaw_range / 2.0f, yaw_range / 2.0f);
		}

		if (key_now_down(Key.Space) || gamepad_buttons_now_down().y == -1.0f)
		{
			foreach (var container in cargo_containers)
			{
				GameObject cargo = container.try_unloading();
				if (cargo != null)
				{
					cargo.transform.position = neck.position + neck.forward * neck.localScale.z / 2.0f;
					break;
				}
			}
		}

		dampen_pitch       = dampen(dampen_pitch, pitch, pitch_dampening);
		dampen_yaw         = dampen_angle(dampen_yaw, yaw, yaw_dampening);
		neck.localRotation = Quaternion.Euler(-dampen_pitch, dampen_yaw, 0.0f);
		holder.position    = transform.position + Quaternion.AngleAxis(dampen_yaw, transform.up) * transform.forward * holder.localScale.z / 2.0f + neck.forward * neck.Find("Cube").localScale.z;
	}
}
