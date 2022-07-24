using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class PlayCamera : MonoBehaviour
{
	public enum Mode
	{
		outside_view,
		third_person,
		birds_eye
	};

	public Mode      mode              = Mode.outside_view;
	public Transform robot_subject     = null;
	public Transform birds_eye_subject = null;

	public Vector3 outside_pos            = new Vector3(-6.44f, 1.02f, 0.28f);
	public Vector3 third_person_delta_pos = new Vector3(0.0f, 4.0f, -1.0f);
	public Vector3 birds_eye_pos          = new Vector3(0.0f, 8.0f, 0.0f);

	void Update()
	{
		if (key_now_down(Key.Tab))
		{
			mode = (Mode)(((int) mode + 1) % System.Enum.GetNames(typeof(Mode)).Length);
		}

		switch (mode)
		{
			case Mode.outside_view:
			{
				transform.position = dampen(transform.position, outside_pos, 0.01f);
				transform.rotation = dampen(transform.rotation, Quaternion.LookRotation(robot_subject.position - outside_pos, new Vector3(0.0f, 1.0f, 0.0f)), 0.01f);
			} break;

			case Mode.third_person:
			{
				transform.position = dampen(transform.position, robot_subject.position + third_person_delta_pos, 0.01f);
				transform.rotation = dampen(transform.rotation, Quaternion.LookRotation(robot_subject.position - transform.position, new Vector3(0.0f, 1.0f, 0.0f)), 0.01f);
			} break;

			case Mode.birds_eye:
			{
				transform.position = dampen(transform.position, birds_eye_pos, 0.01f);
				transform.rotation = dampen(transform.rotation, Quaternion.LookRotation(birds_eye_subject.position - birds_eye_pos, new Vector3(0.0f, 1.0f, 0.0f)), 0.01f);
			} break;
		}
	}
}


