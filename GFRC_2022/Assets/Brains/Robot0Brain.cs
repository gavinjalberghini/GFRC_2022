using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Robot0Brain : MonoBehaviour
{
	public DriveController drive_controller;
	public Grapple         grapple;
	public Intake          intake;
	public CargoContainer  cargo_container;
	public Shooter         shooter;

	void Update()
	{
		{
			float qe = 0.0f;
			if (key_down(Key.Q)) { qe -= 1.0f; }
			if (key_down(Key.E)) { qe += 1.0f; }
			drive_controller.control(wasd(), qe);
		}

		if (key_now_down(Key.Space))
		{
			grapple.toggle();
		}

		cargo_container.try_loading(intake);

		if (key_now_down(Key.Enter))
		{
			shooter.try_shooting(cargo_container);
		}

		shooter.omniarm.target_yaw   += arrow_keys().x * 90.0f * Time.deltaTime;
		shooter.omniarm.target_pitch += arrow_keys().y * 90.0f * Time.deltaTime;
	}
}

