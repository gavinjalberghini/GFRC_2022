using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class DemoCamera : MonoBehaviour
{
	public enum Mode
	{
		birds_eye,
		third_person
	};

	public Mode      mode              = Mode.birds_eye;
	public Transform robot_subject     = null;

	public Vector3 birds_eye_delta_pos    = new Vector3(0.0f, 4.0f, 0.0f);
	public Vector3 third_person_delta_pos = new Vector3(0.0f, 1.0f, -1.0f);

	void Start()
	{
		Wheel.show_indicator = true;
	}

	void Update()
	{
		if (key_now_down(Key.O))
		{
			Wheel.show_indicator = !Wheel.show_indicator;
		}

		if (key_now_down(Key.Tab))
		{
			mode = (Mode)(((int) mode + 1) % System.Enum.GetNames(typeof(Mode)).Length);
		}

		switch (mode)
		{
			case Mode.birds_eye:
			{
				transform.position = dampen(transform.position, robot_subject.position + birds_eye_delta_pos, 0.01f);
				transform.rotation = dampen(transform.rotation, Quaternion.LookRotation(new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f)), 0.01f);
			} break;

			case Mode.third_person:
			{
				transform.position = dampen(transform.position, robot_subject.position + third_person_delta_pos, 0.001f);
				transform.rotation = dampen(transform.rotation, Quaternion.LookRotation(-third_person_delta_pos, new Vector3(0.0f, 1.0f, 0.0f)), 0.01f);
			} break;
		}
	}
}

