using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Robot2Brain : MonoBehaviour
{
	public KiwiDriveController drive_controller;
	public OmniArm             lower_arm;
	public OmniArm             upper_arm;
	public Claw                claw;

	void Update()
	{
		{
			float qe = 0.0f;
			if (key_down(Key.Q)) { qe -= 1.0f; }
			if (key_down(Key.E)) { qe += 1.0f; }
			drive_controller.control(wasd(), qe);
		}

		lower_arm.target_yaw   += arrow_keys().x * 80.0f * Time.deltaTime;
		lower_arm.target_pitch += arrow_keys().y * 80.0f * Time.deltaTime;
		upper_arm.target_pitch += arrow_keys().y * 80.0f * Time.deltaTime;

		if (key_now_down(Key.Enter))
		{
			claw.toggle();
		}
	}
}
