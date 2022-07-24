using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class RobotBrain : MonoBehaviour
{
	public bool                 using_assistant;
	public DriveController      drive_controller;
	public PrimaryManipulator   primary;
	public SecondaryManipulator secondary;
	public Intake               floor_intake;
	public CargoContainer[]     cargo_containers;

	bool subtype<T>(PrimaryManipulator   x) { return x && typeof(T).IsAssignableFrom(x.GetType()); }
	bool subtype<T>(SecondaryManipulator x) { return x && typeof(T).IsAssignableFrom(x.GetType()); }

	void Update()
	{
		//
		// Sends control inputs to drive controller, which will handle how wheels will be turned or driven.
		//

		{
			float qe = 0.0f;
			if (key_down(Key.Q)) { qe -= 1.0f; }
			if (key_down(Key.E)) { qe += 1.0f; }

			Vector2 translation = (using_assistant ? new Vector2(0.0f, left_stick(0).y) : left_stick(0)) + wasd();

			drive_controller.control
				(
					translation : translation,
					steering    : (using_assistant ? right_stick(0).x : left_stick(0).x) + qe
				);

			if (translation == new Vector2(0.0f, 0.0f))
			{
				GetComponent<AudioSource>().Stop();
			}
			else if (!GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().Play();
			}
		}

		//
		// Handles primary manipulators case by case.
		//

		if (subtype<TurretMountedShooterManipulator>(primary))
		{
			(primary as TurretMountedShooterManipulator).control
				(
					yaw              : right_stick           (using_assistant ? 1 : 0).x,
					pitch            : right_stick           (using_assistant ? 1 : 0).y,
					shoot            : trigger_right_now_down(using_assistant ? 1 : 0),
					cargo_containers : cargo_containers
				);
		}
		else if (subtype<FixedPointShooterManipulator>(primary))
		{
			(primary as FixedPointShooterManipulator).control
				(
					shoot            : trigger_right_now_down(using_assistant ? 1 : 0),
					cargo_containers : cargo_containers
				);
		}
		else if (subtype<ArmManipulator>(primary))
		{
			(primary as ArmManipulator).control
				(
					yaw    : right_stick         (using_assistant ? 1 : 0).x,
					pitch  : right_stick         (using_assistant ? 1 : 0).y,
					toggle : right_stick_now_down(using_assistant ? 1 : 0)
				);
		}
		else if (subtype<WristAndArmManipulator>(primary))
		{
			(primary as WristAndArmManipulator).control
				(
					yaw          : right_stick(using_assistant ? 1 : 0).x,
					pitch        : right_stick(using_assistant ? 1 : 0).y,
					joint_toggle : shoulder_right_now_down(using_assistant ? 1 : 0),
					grab_toggle  : right_stick_now_down   (using_assistant ? 1 : 0)
				);
		}
		else if (subtype<TelescopicArmManipulator>(primary))
		{
			(primary as TelescopicArmManipulator).control
				(
					yaw          : right_stick(using_assistant ? 1 : 0).x,
					pitch        : right_stick(using_assistant ? 1 : 0).y,
					length       : using_assistant ? left_stick(1).y + dpad(1).y : dpad(0).y,
					joint_toggle : shoulder_right_now_down(using_assistant ? 1 : 0),
					grab_toggle  : right_stick_now_down   (using_assistant ? 1 : 0)
				);

		}
		else if (subtype<BucketManipulator>(primary))
		{
			(primary as BucketManipulator).control
				(
					pitch            : -right_stick(using_assistant ? 1 : 0).y,
					length           : using_assistant ? left_stick(1).y + dpad(1).y : dpad(0).y,
					store            : right_stick_now_down(0),
					cargo_containers : cargo_containers
				);
		}

		//
		// Handles secondary manipulators case by case.
		//

		if (subtype<GrapplingHookManipulator>(secondary))
		{
			(secondary as GrapplingHookManipulator).control
				(
					shoot : shoulder_left_now_down(using_assistant ? 1 : 0)
				);
		}
		else if (subtype<DualCaneManipulator>(secondary))
		{
			(secondary as DualCaneManipulator).control
				(
					extend : shoulder_left_down(using_assistant ? 1 : 0)
				);
		}

		//
		// Automatic handling of intakes.
		//

		if (floor_intake || subtype<Intake>(secondary))
		{
			foreach (var container in cargo_containers)
			{
				if (container.try_loading(floor_intake) || (subtype<Intake>(secondary) && container.try_loading(secondary as Intake)))
				{
					break;
				}
			}
		}
	}
}
