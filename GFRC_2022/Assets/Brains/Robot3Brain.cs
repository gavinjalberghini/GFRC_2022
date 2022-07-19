using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Robot3Brain : MonoBehaviour
{
	public DriveController drive_controller;
	public Intake          intake;
	public Bucket          bucket;

	void Update()
	{
		{
			float qe = 0.0f;
			if (key_down(Key.Q)) { qe -= 1.0f; }
			if (key_down(Key.E)) { qe += 1.0f; }
			drive_controller.control(wasd(), qe);
		}

		bucket.try_loading(intake);
		bucket.target_pitch  += arrow_keys().x * 90.0f * Time.deltaTime;
		bucket.target_height += arrow_keys().y *         Time.deltaTime;
	}
}

