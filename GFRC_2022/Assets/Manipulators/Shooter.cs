using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Shooter : MonoBehaviour
{
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
	public float            shoot_force      = 256.0f;
	public CargoContainer[] cargo_containers = null;
	public Transform        cannon           = null;

	float dampen_pitch = 0.0f;
	float dampen_yaw   = 0.0f;

	void OnValidate()
	{
		pitch                = Mathf.Clamp(pitch, pitch_min, pitch_max);
		pitch_min            = Mathf.Clamp(pitch_min, 0.0f, pitch_max);
		pitch_max            = Mathf.Clamp(pitch_max, pitch_min, 80.0f);
		yaw                  = Mathf.Clamp(mod(yaw + 180.0f, 360.0f) - 180.0f, -yaw_range / 2.0f, yaw_range / 2.0f);
		yaw_range            = Mathf.Clamp(yaw_range, 0.0f, 360.0f);
		dampen_pitch         = pitch;
		dampen_yaw           = yaw;
		cannon.localRotation = Quaternion.Euler(-pitch, yaw, 0.0f);
	}

	void OnStart()
	{
		dampen_pitch = pitch;
	}

	void Update()
	{
		if (!fix_pitch)
		{
			pitch = Mathf.Clamp(pitch + arrow_keys().y * pitch_speed * Time.deltaTime, pitch_min, pitch_max);
		}
		if (!fix_yaw)
		{
			yaw = Mathf.Clamp(mod(yaw + arrow_keys().x * yaw_speed * Time.deltaTime + 180.0f, 360.0f) - 180.0f, -yaw_range / 2.0f, yaw_range / 2.0f);
		}

		if (key_now_down(Key.Space))
		{
			foreach (var container in cargo_containers)
			{
				GameObject cargo = container.try_unloading();
				if (cargo != null)
				{
					cargo.transform.position = cannon.position + cannon.forward * cannon.localScale.z / 2.0f;
					cargo.GetComponent<Rigidbody>().AddForce(cannon.forward * shoot_force);
					break;
				}
			}
		}

		dampen_pitch         = dampen(dampen_pitch, pitch, pitch_dampening);
		dampen_yaw           = dampen_angle(dampen_yaw, yaw, yaw_dampening);
		cannon.localRotation = Quaternion.Euler(-dampen_pitch, dampen_yaw, 0.0f);
	}
}
