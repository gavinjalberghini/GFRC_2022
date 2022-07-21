using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class RobotBrain : MonoBehaviour
{
	public DriveController      drive_controller;
	public PrimaryManipulator   primary;
	public SecondaryManipulator secondary;
	public TertiaryManipulator  tertiary;
	public InternalManipulator  intern;

	public CargoContainer cargo_container; // @TODO@ This should be handled by internal manipulator class.

	bool subtype<T>(PrimaryManipulator   x) { return typeof(T).IsAssignableFrom(x.GetType()); }
	bool subtype<T>(SecondaryManipulator x) { return typeof(T).IsAssignableFrom(x.GetType()); }
	bool subtype<T>(TertiaryManipulator  x) { return typeof(T).IsAssignableFrom(x.GetType()); }
	bool subtype<T>(InternalManipulator  x) { return typeof(T).IsAssignableFrom(x.GetType()); }

	void Update()
	{
		//
		// Sends control inputs to drive controller, which will handle how wheels will be turned or driven.
		//

		{
			float qe = 0.0f;
			if (key_down(Key.Q)) { qe -= 1.0f; }
			if (key_down(Key.E)) { qe += 1.0f; }
			drive_controller.control(wasd(), qe);
		}

		//
		// Handles primary manipulators case by case.
		//

		if (primary == null)
		{
			print("NO PRIMARY MANIPULATOR");
		}
		else if (subtype<Shooter>(primary))
		{
			Shooter shooter = primary as Shooter;

			if (key_now_down(Key.Enter))
			{
				shooter.try_shooting(cargo_container);
			}

			shooter.omniarm.target_yaw   += arrow_keys().x * 90.0f * Time.deltaTime;
			shooter.omniarm.target_pitch += arrow_keys().y * 90.0f * Time.deltaTime;
		}
		else if (subtype<ArmClaw>(primary))
		{
			print("I got an arm");
		}
		else if (subtype<WristArmClaw>(primary))
		{
			print("I got an arm with a wrist");
		}
		else if (subtype<Bucket>(primary))
		{
			print("I got a bucket");
		}
		else
		{
			print("UNSUPPORTED PRIMARY MANIPULATOR");
		}

		//
		// Handles secondary manipulators case by case.
		//

		// @TODO@

		//
		// Handles tertiary manipulators case by case.
		//

		// @TODO@
	}
}

