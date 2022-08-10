using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class RobotBrain : MonoBehaviour
{
	public bool is_playing = true;
	[ConditionalHide("is_playing", true)] public bool                 using_assistant;
	[ConditionalHide("is_playing", true)] public DriveController      drive_controller;
	[ConditionalHide("is_playing", true)] public PrimaryManipulator   primary;
	[ConditionalHide("is_playing", true)] public SecondaryManipulator secondary;
	[ConditionalHide("is_playing", true)] public Intake               floor_intake;
	[ConditionalHide("is_playing", true)] public CargoContainer[]     cargo_containers;

	int selected_cargo_container_index;

	float       random_state_countdown;
	RandomState random_state;
	enum RandomState
	{
		idle,
		driving,
		reversing,
		strafing_left,
		strafing_right,
		turning_left,
		turning_right
	};

	public static bool subtype<T>(PrimaryManipulator   x) { return x && typeof(T).IsAssignableFrom(x.GetType()); }
	public static bool subtype<T>(SecondaryManipulator x) { return x && typeof(T).IsAssignableFrom(x.GetType()); }

	void Start()
	{
		GetComponent<Rigidbody>().centerOfMass = new Vector3(0.0f, 0.0f, 0.0f);
		transform.Find("RoboCam").gameObject.SetActive(is_playing);
	}

	void Update()
	{
		bool in_control = !FindObjectOfType<Timer>() || FindObjectOfType<Timer>().GetComponent<Timer>().isTimerStarted && !FindObjectOfType<Timer>().GetComponent<Timer>().timerFinished;
		if (is_playing)
		{
			//
			// Sends control inputs to drive controller, which will handle how wheels will be turned or driven.
			//

			{
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

			if (in_control)
			{
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
		else
		{
			random_state_countdown -= Time.deltaTime;
			if (random_state_countdown <= 0.0f)
			{
				random_state = (RandomState) UnityEngine.Random.Range(0, Enum.GetNames(typeof(RandomState)).Length);

				if (random_state == RandomState.idle)
				{
					random_state_countdown = UnityEngine.Random.Range(0.0f, 1.0f);
				}
				else if (random_state == RandomState.turning_left || random_state == RandomState.turning_right)
				{
					random_state_countdown = UnityEngine.Random.Range(1.0f, 2.0f);
				}
				else
				{
					random_state_countdown = UnityEngine.Random.Range(1.0f, 4.0f);
				}
			}

			Vector2 translation = new Vector2(0.0f, 0.0f);
			float   steering    = 0.0f;

			if (in_control)
			{
				switch (random_state)
				{
					case RandomState.idle:
					{
					} break;

					case RandomState.driving:
					{
						translation.y = 1.0f;
					} break;

					case RandomState.reversing:
					{
						translation.y = -1.0f;
					} break;

					case RandomState.strafing_left:
					{
						translation.x = -1.0f;
					} break;

					case RandomState.strafing_right:
					{
						translation.x = 1.0f;
					} break;

					case RandomState.turning_left:
					{
						steering = -1.0f;
					} break;

					case RandomState.turning_right:
					{
						steering = 1.0f;
					} break;
				}
			}

			drive_controller.control(translation, steering);

			if (translation == new Vector2(0.0f, 0.0f) && steering == 0.0f)
			{
				GetComponent<AudioSource>().Stop();
			}
			else if (!GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().Play();
			}
		}
	}
}
