using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Camera : MonoBehaviour
{
	public Quaternion initial_orientation = Quaternion.identity;
	public float      sensitivity         = 0.2f;
	public float      dampening           = 0.1f;
	public bool       is_focused          = true;
	public Transform  focused_object      = null;

	Vector2 target_yaw_pitch = new Vector2(0.0f, 0.0f);
	Vector2 yaw_pitch        = new Vector2(0.0f, 0.0f);

	void OnValidate()
	{
		sensitivity         = Mathf.Clamp(sensitivity, 0.01f, 1.0f);
		dampening           = Mathf.Clamp(dampening, 0.0f, 1.0f);
		transform.rotation  = initial_orientation;
	}

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		if (key_now_down(Key.Tab))
		{
			is_focused       = !is_focused;
			yaw_pitch.x      = mod(yaw_pitch.x, 360.0f);
			yaw_pitch.y      = mod(yaw_pitch.y, 360.0f);
			target_yaw_pitch = yaw_pitch;
		}

		transform.localRotation = initial_orientation;

		if (is_focused && focused_object != null)
		{
			Quaternion look = Quaternion.LookRotation(Quaternion.Inverse(initial_orientation) * (focused_object.position - transform.position), transform.up);
			target_yaw_pitch = new Vector2(look.eulerAngles.y, look.eulerAngles.x);
		}
		else
		{
			target_yaw_pitch   += new Vector2(mouse_delta().x * sensitivity, -mouse_delta().y * sensitivity);
			target_yaw_pitch.y  = Mathf.Clamp(target_yaw_pitch.y, -80.0f, 80.0f);
		}

		yaw_pitch.x += dampen(0.0f, min_degree_arc(yaw_pitch.x, target_yaw_pitch.x), dampening / (is_focused ? 10.0f : 1000.0f));
		yaw_pitch.y += dampen(0.0f, min_degree_arc(yaw_pitch.y, target_yaw_pitch.y), dampening / (is_focused ? 10.0f : 1000.0f));

		transform.localRotation = Quaternion.AngleAxis(yaw_pitch.x, transform.up) * Quaternion.AngleAxis(yaw_pitch.y, transform.right) * initial_orientation;
	}
}

