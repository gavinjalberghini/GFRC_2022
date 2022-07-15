using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Arm : MonoBehaviour
{
	public bool             fix_length       = false;
	public float            length           = 0.75f;
	public float            length_min       = 0.1f;
	public float            length_max       = 1.0f;
	public float            length_speed     = 1.0f;
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
	public Transform        hand             = null;

	float dampen_length = 0.0f;
	float dampen_pitch  = 0.0f;
	float dampen_yaw    = 0.0f;

	void readjust_arm()
	{
		transform.Find("Neck").localRotation              = Quaternion.Euler(-dampen_pitch, dampen_yaw, 0.0f);
		transform.Find("Neck").Find("Cube").localScale    = new Vector3(0.05f, 0.05f, dampen_length);
		transform.Find("Neck").Find("Cube").localPosition = new Vector3(0.0f, 0.0f, dampen_length / 2.0f);

		if (hand != null)
		{
			hand.position = transform.position + transform.Find("Neck").forward * dampen_length;
			hand.rotation = transform.Find("Neck").rotation;
		}
	}

	void OnValidate()
	{
		length_min    = Mathf.Clamp(length_min, 0.0f, length_max);
		length_max    = Mathf.Clamp(length_max, length_min, 1.0f);
		length        = Mathf.Clamp(length, length_min, length_max);
		dampen_length = length;
		pitch_min     = Mathf.Min(pitch_min, pitch_max);
		pitch_max     = Mathf.Max(pitch_max, pitch_min);
		pitch         = Mathf.Clamp(pitch, pitch_min, pitch_max);
		yaw           = Mathf.Clamp(mod(yaw + 180.0f, 360.0f) - 180.0f, -yaw_range / 2.0f, yaw_range / 2.0f);
		yaw_range     = Mathf.Clamp(yaw_range, 0.0f, 360.0f);
		dampen_pitch  = pitch;
		dampen_yaw    = yaw;
		readjust_arm();
	}

	void OnStart()
	{
		dampen_length = length;
		dampen_pitch  = pitch;
		dampen_yaw    = yaw;
	}

	void Update()
	{
		if (!fix_length)
		{
			if (shoulder_left() || key_down(Key.LeftBracket))
			{
				length -= length_speed * Time.deltaTime;
			}
			if (shoulder_right() || key_down(Key.RightBracket))
			{
				length += length_speed * Time.deltaTime;
			}
		}
		length        = Mathf.Clamp(length, length_min, length_max);
		dampen_length = dampen(dampen_length, length, 0.001f);
		readjust_arm();

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

		dampen_pitch       = dampen(dampen_pitch, pitch, pitch_dampening);
		dampen_yaw         = dampen_angle(dampen_yaw, yaw, yaw_dampening);
		transform.Find("Neck").localRotation = Quaternion.Euler(-dampen_pitch, dampen_yaw, 0.0f);
	}
}

