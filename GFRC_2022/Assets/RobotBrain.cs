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

	int selected_cargo_container_index;

	public static bool subtype<T>(PrimaryManipulator   x) { return x && typeof(T).IsAssignableFrom(x.GetType()); }
	public static bool subtype<T>(SecondaryManipulator x) { return x && typeof(T).IsAssignableFrom(x.GetType()); }

	void Start()
	{
		GetComponent<Rigidbody>().centerOfMass = new Vector3(0.0f, 0.0f, 0.0f);
	}

	void Update()
	{
		//
		// Sends control inputs to drive controller, which will handle how wheels will be turned or driven.
		//

		{
			bool in_control = !FindObjectOfType<Timer>() || FindObjectOfType<Timer>().GetComponent<Timer>().isTimerStarted && !FindObjectOfType<Timer>().GetComponent<Timer>().timerFinished;
			float qe = 0.0f;
			if (in_control && key_down(Key.Q)) { qe -= 1.0f; }
			if (in_control && key_down(Key.E)) { qe += 1.0f; }
			Vector2 translation =
				in_control
					? (using_assistant ? left_stick(0) : new Vector2(0.0f, left_stick(0).y)) + wasd()
					: new Vector2(0.0f, 0.0f);

			drive_controller.control
				(
					translation : translation,
					steering    : (using_assistant ? right_stick(0).x : left_stick(0).x) + qe
				);

			if (translation == new Vector2(0.0f, 0.0f) && qe == 0.0f)
			{
				GetComponent<AudioSource>().Stop();
			}
			else if (!GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().Play();
			}
		}

		//
		// Intakes and cargo containers.
		//

		foreach (var container in cargo_containers)
		{
			if ((floor_intake && container.try_loading(floor_intake)) || (subtype<Intake>(secondary) && container.try_loading(secondary as Intake)))
			{
				FindObjectOfType<AudioManager>().Sound("Shoot");
				break;
			}
		}

		if (key_now_down(Key.LeftArrow ) || dpad_left_now_down (using_assistant ? 1 : 0)) { selected_cargo_container_index -= 1; }
		if (key_now_down(Key.RightArrow) || dpad_right_now_down(using_assistant ? 1 : 0)) { selected_cargo_container_index -= 1; }
		selected_cargo_container_index = mod(selected_cargo_container_index, cargo_containers.Length);

		if (left_stick_now_down(using_assistant ? 1 : 0))
		{
			if (cargo_containers[selected_cargo_container_index].try_unloading(true))
			{
				FindObjectOfType<AudioManager>().Sound("Pop");
			}
		}

		for (int i = 0; i < cargo_containers.Length; i += 1)
		{
			if (cargo_containers[(selected_cargo_container_index + i) % cargo_containers.Length].cargo)
			{
				selected_cargo_container_index = (selected_cargo_container_index + i) % cargo_containers.Length;
				break;
			}
		}

		for (int i = 0; i < cargo_containers.Length; i += 1)
		{
			cargo_containers[i].set_highlight(i == selected_cargo_container_index && cargo_containers[i].cargo);
		}

		//
		// Handles primary manipulators case by case.
		//

		if (subtype<TurretMountedShooterManipulator>(primary))
		{
			(primary as TurretMountedShooterManipulator).control
				(
					yaw             : right_stick           (using_assistant ? 1 : 0).x,
					pitch           : right_stick           (using_assistant ? 1 : 0).y,
					shoot           : trigger_right_now_down(using_assistant ? 1 : 0),
					cargo_container : cargo_containers[selected_cargo_container_index]
				);
		}
		else if (subtype<FixedPointShooterManipulator>(primary))
		{
			(primary as FixedPointShooterManipulator).control
				(
					shoot           : trigger_right_now_down(using_assistant ? 1 : 0),
					cargo_container : cargo_containers[selected_cargo_container_index]
				);
		}
		else if (subtype<ArmManipulator>(primary))
		{
			(primary as ArmManipulator).control
				(
					yaw    : right_stick           (using_assistant ? 1 : 0).x,
					pitch  : right_stick           (using_assistant ? 1 : 0).y,
					toggle : trigger_right_now_down(using_assistant ? 1 : 0)
				);
		}
		else if (subtype<WristAndArmManipulator>(primary))
		{
			(primary as WristAndArmManipulator).control
				(
					yaw          : right_stick(using_assistant ? 1 : 0).x,
					pitch        : right_stick(using_assistant ? 1 : 0).y,
					joint_toggle : right_stick_now_down  (using_assistant ? 1 : 0),
					grab_toggle  : trigger_right_now_down(using_assistant ? 1 : 0)
				);
		}
		else if (subtype<TelescopicArmManipulator>(primary))
		{
			(primary as TelescopicArmManipulator).control
				(
					yaw          : right_stick(using_assistant ? 1 : 0).x,
					pitch        : right_stick(using_assistant ? 1 : 0).y,
					length       : using_assistant ? left_stick(1).y + dpad(1).y : dpad(0).y,
					joint_toggle : right_stick_now_down  (using_assistant ? 1 : 0),
					grab_toggle  : trigger_right_now_down(using_assistant ? 1 : 0)
				);

		}
		else if (subtype<BucketManipulator>(primary))
		{
			(primary as BucketManipulator).control
				(
					pitch           : trigger_right(using_assistant ? 1 : 0) > 0.0f ?  1.0f : -1.0f,
					length          : using_assistant ? left_stick(1).y + dpad(1).y : dpad(0).y,
					store           : right_stick_now_down(0),
					cargo_container : cargo_containers[selected_cargo_container_index]
				);
		}

		//
		// Handles secondary manipulators case by case.
		//

		if (subtype<GrapplingHookManipulator>(secondary))
		{
			(secondary as GrapplingHookManipulator).control
				(
					shoot : trigger_left_now_down(using_assistant ? 1 : 0)
				);
		}
		else if (subtype<DualCaneManipulator>(secondary))
		{
			(secondary as DualCaneManipulator).control
				(
					extend : trigger_left(using_assistant ? 1 : 0) > 0.0f
				);
		}
	}
}
