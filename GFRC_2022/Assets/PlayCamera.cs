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
		birds_eye,
		third_person,
		robo_cam
	};

	public Mode      mode              = Mode.outside_view;
	public Transform robot_subject     = null;
	public Transform birds_eye_subject = null;

	public GameObject vantage_points_group;
	public Vector3    third_person_delta_pos = new Vector3(0.0f, 4.0f, -1.0f);
	public Vector3    birds_eye_pos          = new Vector3(0.0f, 8.0f, 0.0f);
	public Transform  center;

	Vector3 vantage_pos;
	Vector3 tele_pos;

	const float HEIGHT = 1.15f;

	void Start()
	{
		vantage_pos        = vantage_points_group.transform.GetChild(0).Find("VantagePoint").position + new Vector3(0.0f, HEIGHT, 0.0f);
		tele_pos           = vantage_points_group.transform.GetChild(0).Find("Tele").position;
		transform.position = vantage_pos;
	}

	void Update()
	{
		const float GREASE = 0.00001f;
		if (key_now_down(Key.Tab))
		{
			mode = Mode.birds_eye;
		}

		switch (mode)
		{
			case Mode.outside_view:
			{
				if (mouse_now_down())
				{
					mode = Mode.robo_cam;
				}
				else
				{
					transform.position = dampen(transform.position, vantage_pos, GREASE);
					transform.rotation = dampen(transform.rotation, Quaternion.LookRotation((robot_subject.position + center.position) / 2 - vantage_pos, new Vector3(0.0f, 1.0f, 0.0f)), GREASE);
				}
			} break;

			case Mode.third_person:
			{
				transform.position = dampen(transform.position, robot_subject.position + third_person_delta_pos, GREASE);
				transform.rotation = dampen(transform.rotation, Quaternion.LookRotation(-third_person_delta_pos, new Vector3(0.0f, 1.0f, 0.0f)), GREASE);
			} break;

			case Mode.birds_eye:
			{
				transform.position = dampen(transform.position, birds_eye_pos, GREASE);
				transform.rotation = dampen(transform.rotation, Quaternion.LookRotation(birds_eye_subject.position - birds_eye_pos, new Vector3(0.0f, 1.0f, 0.0f)), GREASE);

				if (mouse_now_down())
				{
					RaycastHit hit;
					if (Physics.Raycast(GetComponent<Camera>().ScreenPointToRay(mouse_pos()), out hit))
					{
						Transform vantage_point = null;
						foreach (Transform t in vantage_points_group.transform)
						{
							if (!vantage_point || Vector3.Distance(vantage_point.position, hit.point) > Vector3.Distance(t.position, hit.point))
							{
								vantage_point = t;
								tele_pos      = t.Find("Tele").position;
							}
						}
						if (Vector3.Distance(vantage_point.position, hit.point) < Vector3.Distance(robot_subject.position, hit.point))
						{
							vantage_pos = vantage_point.position + new Vector3(0.0f, HEIGHT, 0.0f);
							mode = Mode.outside_view;
						}
						else
						{
							mode = Mode.third_person;
						}
					}
				}
			} break;

			case Mode.robo_cam:
			{
				if (mouse_now_down())
				{
					mode = Mode.outside_view;
				}
				else
				{
					transform.position = dampen(transform.position, vantage_pos, GREASE);
					transform.rotation = dampen(transform.rotation, Quaternion.LookRotation(tele_pos - vantage_pos, new Vector3(0.0f, 1.0f, 0.0f)), GREASE);
				}
			} break;
		}
	}
}


