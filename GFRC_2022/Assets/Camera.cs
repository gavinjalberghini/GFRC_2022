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

	Vector2 target_yaw_pitch = new Vector2(0.0f, 0.0f);
	Vector2 yaw_pitch        = new Vector2(0.0f, 0.0f);

	void OnValidate()
	{
		sensitivity         = Mathf.Clamp(sensitivity, 0.01f, 1.0f);
		dampening           = Mathf.Clamp(dampening, 0.0f, 1.0f);
		transform.rotation  = initial_orientation;
	}

	void Update()
	{
		Cursor.lockState = CursorLockMode.Locked;

		// @TODO@ Is there a way to make the camera only turn when the game itself is focused on in the editor?
		if (Application.isFocused)
		{
			target_yaw_pitch   += new Vector2(mouse_delta().x * sensitivity, -mouse_delta().y * sensitivity);
			target_yaw_pitch.y  = Mathf.Clamp(target_yaw_pitch.y, -80.0f, 80.0f);
			yaw_pitch           = dampen(yaw_pitch, target_yaw_pitch, dampening / 10000.0f);

			transform.localRotation = initial_orientation;
			transform.localRotation = Quaternion.AngleAxis(yaw_pitch.x, transform.up) * Quaternion.AngleAxis(yaw_pitch.y, transform.right) * initial_orientation;
		}
	}
}

