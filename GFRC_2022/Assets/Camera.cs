using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Camera : MonoBehaviour
{
	public float      sensitivity     = 0.2f;
	public float      dampening_rot   = 0.1f;
	public float      dampening_pos   = 0.01f;
	public bool       is_focused      = true;
	public bool       is_relative_pos = false;
	public Transform  subject         = null;

	Vector2    target_yaw_pitch     = new Vector2(0.0f, 0.0f);
	Vector2    yaw_pitch            = new Vector2(0.0f, 0.0f);
	Quaternion initial_orientation  = Quaternion.identity;
	Vector3    initial_relative_pos = new Vector3(0.0f, 0.0f, 0.0f);

	void OnValidate()
	{
		sensitivity         = Mathf.Clamp(sensitivity, 0.01f, 1.0f);
		dampening_rot       = Mathf.Clamp(dampening_rot, 0.0f, 1.0f);
		initial_orientation = transform.rotation;

		if (subject != null)
		{
			initial_relative_pos = transform.position - subject.position;
		}
	}

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		if (is_relative_pos && subject != null)
		{
			transform.position = dampen(transform.position, subject.position + initial_relative_pos, dampening_pos);
		}

		target_yaw_pitch.x = mod(target_yaw_pitch.x, 360.0f);
		target_yaw_pitch.y = mod(target_yaw_pitch.y, 360.0f);
		yaw_pitch.x        = mod(       yaw_pitch.x, 360.0f);
		yaw_pitch.y        = mod(       yaw_pitch.y, 360.0f);

		if (key_now_down(Key.Tab))
		{
			is_focused       = !is_focused;
			target_yaw_pitch = yaw_pitch;
		}

		transform.localRotation = initial_orientation;

		if (is_focused && subject != null)
		{
			Quaternion look  = Quaternion.LookRotation(Quaternion.Inverse(initial_orientation) * (subject.position - transform.position), transform.up);
			target_yaw_pitch = new Vector2(look.eulerAngles.y, look.eulerAngles.x);
		}
		else
		{
			target_yaw_pitch   += new Vector2(mouse_delta().x * sensitivity, -mouse_delta().y * sensitivity);
			target_yaw_pitch.y  = Mathf.Clamp(min_degree_arc(0.0f, target_yaw_pitch.y), -80.0f, 80.0f);
		}

		yaw_pitch.x += dampen(0.0f, min_degree_arc(yaw_pitch.x, target_yaw_pitch.x), dampening_rot / (is_focused ? 10.0f : 1000.0f));
		yaw_pitch.y += dampen(0.0f, min_degree_arc(yaw_pitch.y, target_yaw_pitch.y), dampening_rot / (is_focused ? 10.0f : 1000.0f));

		transform.localRotation = Quaternion.AngleAxis(yaw_pitch.x, transform.up) * Quaternion.AngleAxis(yaw_pitch.y, transform.right) * initial_orientation;
	}
}

