using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class DemoBrain : MonoBehaviour
{
	public DriveController drive_controller;
	public Intake[]        intakes;
	public Bucket          bucket;

	void Update()
	{
		Vector2 translation =
			left_stick() == new Vector2(0.0f, 0.0f)
				? wasd_normalized()
				: left_stick();

		float steering = right_stick().x;
		if (steering == 0.0f)
		{
			if (key_down(Key.Q)) { steering -= 1.0f; }
			if (key_down(Key.E)) { steering += 1.0f; }
		}

		drive_controller.control(translation, steering);

		if (key_now_down(Key.Enter))
		{
			foreach (var intake in intakes)
			{
				if (bucket.try_loading(intake))
				{
					break;
				}

			}
		}

		bucket.target_height += arrow_keys().y * Time.deltaTime;
		bucket.target_pitch  += arrow_keys().x * 90.0f * Time.deltaTime;
	}
}
