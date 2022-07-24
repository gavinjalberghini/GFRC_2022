using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class OmniArm : MonoBehaviour
{
	[Header("Length")]
	public float length;
	public float length_min;
	public float length_max;
	public float length_speed;

	[Header("Pitch")]
	public float pitch;
	public float pitch_min;
	public float pitch_max;
	public float pitch_speed;

	[Header("Yaw")]
	public float yaw;
	public float yaw_min;
	public float yaw_max;
	public float yaw_speed;

	[Header("Hand")]
	public Transform  hand;
	public bool       hand_rel_orientation;
	[ConditionalHide("hand_rel_orientation", true)] public bool       hand_leveled;
	[ConditionalHide("hand_rel_orientation", true)] public Quaternion hand_additional_rotation = Quaternion.identity;
	[HideInInspector]                               public Quaternion target_hand_additional_rotation;

	float target_length;
	float target_pitch;
	float target_yaw;

	public Transform arm()     => transform.Find("Arm");
	public Vector3   arm_end() => transform.position + arm().up * length;

	public float change_pitch (float amount) => target_pitch   = Mathf.Clamp(target_pitch  + Mathf.Clamp(amount, -1.0f, 1.0f) * pitch_speed  * Time.deltaTime, pitch_min , pitch_max );
	public float change_length(float amount) => target_length  = Mathf.Clamp(target_length + Mathf.Clamp(amount, -1.0f, 1.0f) * length_speed * Time.deltaTime, length_min, length_max);
	public float change_yaw   (float amount) => target_yaw    +=                             Mathf.Clamp(amount, -1.0f, 1.0f) * yaw_speed    * Time.deltaTime                         ;


	void OnValidate()
	{
		target_length                   = length;
		target_pitch                    = pitch;
		target_yaw                      = yaw;
		target_hand_additional_rotation = hand_additional_rotation;
		Update();
	}

	void Update()
	{
		length_min    = Mathf.Clamp(length_min, 0.0f, length_max);
		length_max    = Mathf.Clamp(length_max, length_min, 1.0f);
		length        = Mathf.Clamp(length       , length_min, length_max);
		target_length = Mathf.Clamp(target_length, length_min, length_max);
		length        = dampen(length, target_length, 0.00001f);

		pitch_min    = Mathf.Clamp(pitch_min, -90.0f, pitch_max);
		pitch_max    = Mathf.Clamp(pitch_max, pitch_min, 90.0f);
		pitch        = Mathf.Clamp(pitch, pitch_min, pitch_max);
		target_pitch = Mathf.Clamp(target_pitch, pitch_min, pitch_max);
		pitch        = dampen(pitch, target_pitch, 0.0001f);

		yaw_min = Mathf.Clamp(yaw_min, -180.0f, yaw_max);
		yaw_max = Mathf.Clamp(yaw_max, yaw_min, 180.0f);
		if (yaw_min == -180.0f && yaw_max == 180.0f)
		{
			yaw        = mod(yaw       , -180.0f, 180.0f);
			target_yaw = mod(target_yaw, -180.0f, 180.0f);
		}
		yaw        = Mathf.Clamp(yaw       , yaw_min, yaw_max);
		target_yaw = Mathf.Clamp(target_yaw, yaw_min, yaw_max);
		yaw        = dampen_angle(yaw, target_yaw, 0.0001f);

		arm().localRotation = Quaternion.Euler(90.0f - pitch, yaw, 0.0f);
		set_local_scale_y(arm(), length / 2.0f);
		arm().localPosition = arm().localRotation * new Vector3(0.0f, length / 2.0f, 0.0f);

		hand_additional_rotation = dampen(hand_additional_rotation, target_hand_additional_rotation, 0.0001f);

		if (hand)
		{
			hand.position = transform.position + arm().up * length;
			if (hand_rel_orientation)
			{
				hand.localRotation  = Quaternion.Euler(hand_leveled ? 0.0f : -pitch, yaw, 0.0f);
				hand.localRotation *= hand_additional_rotation;
			}
		}
	}
}
