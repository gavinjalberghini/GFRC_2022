using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class RobotBrain : MonoBehaviour
{
	public bool is_playing = true;
	public DriveController      drive_controller;
	public PrimaryManipulator   primary;
	public SecondaryManipulator secondary;
	public Intake               floor_intake;
	public CargoContainer[]     cargo_containers;
	[HideInInspector] public bool touching_ground;

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

	public GameObject cargo_source;

	bool  trigger_triggered_this_frame;
	float human_feed_countdown;

	void OnTriggerStay(Collider collider)
	{
		if (collider.CompareTag(GetComponent<Assembler>().data.is_red_alliance ? "RedZone" : "BlueZone") && !trigger_triggered_this_frame && is_playing && subtype<Intake>(secondary) && (key_now_down(Key.Enter) || trigger_left_now_down(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0)))
		{
			if (human_feed_countdown == 0.0f)
			{
				human_feed_countdown = 1.0f;

				bool available_slot = false;
				foreach (var cargo_container in cargo_containers)
				{
					if (!cargo_container.cargo)
					{
						available_slot = true;
						break;
					}
				}
				if (available_slot)
				{
					Transform farthest_cargo = null;

					foreach (Transform cargo in cargo_source.transform)
					{
						bool taken = false;
						foreach (var cargo_container in cargo_containers)
						{
							if (cargo_container.cargo == cargo.gameObject)
							{
								taken = true;
								break;
							}
						}
						if (!taken && (!farthest_cargo || Vector3.Distance(cargo.position, transform.position) > Vector3.Distance(farthest_cargo.position, transform.position)))
						{
							farthest_cargo = cargo;
						}
					}
					if (farthest_cargo)
					{
						trigger_triggered_this_frame = true;
						farthest_cargo.position = secondary.transform.position;
						foreach (var container in cargo_containers)
						{
							if (container.try_loading(farthest_cargo.gameObject))
							{
								GetComponent<AudioManager>().Sound("Shoot");
								break;
							}
						}
					}
				}
			}
			else
			{
				GetComponent<AudioManager>().Sound("Harsh Beep");
			}
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag("ArenaFloor"))
		{
			touching_ground = true;
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.CompareTag("ArenaFloor"))
		{
			touching_ground = false;
		}
	}

	void Start()
	{
		GetComponent<Rigidbody>().centerOfMass = new Vector3(0.0f, 0.0f, 0.0f);
		transform.Find("RoboCam").gameObject.SetActive(is_playing);

		if (is_playing)
		{
			if (primary)
			{
				foreach (Transform trans in primary.gameObject.GetComponentsInChildren<Transform>(true))
				{
					if (trans.gameObject.layer != 16)
					{
						trans.gameObject.layer = 15;
					}
				}
			}
			if (secondary)
			{
				foreach (Transform trans in secondary.gameObject.GetComponentsInChildren<Transform>(true))
				{
					if (trans.gameObject.layer != 16)
					{
						trans.gameObject.layer = 15;
					}
				}
			}
		}
	}

	void Update()
	{
		human_feed_countdown = Mathf.Max(human_feed_countdown - Time.deltaTime, 0.0f);
		bool in_control = !FindObjectOfType<Main>() || FindObjectOfType<Main>().state == Main.State.playing;
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
						? left_stick(0) + wasd()
						: new Vector2(0.0f, 0.0f);

				float steering =
					in_control
						?
							(shoulder_left_down(0) ? -1.0f : 0.0f) + (shoulder_right_down(0) ? 1.0f : 0.0f) +
							(GetComponent<Assembler>().data.is_using_assistant ? right_stick(0).x : 0.0f)
								+ qe
						: 0.0f;

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

			if (in_control)
			{
				//
				// Intakes and cargo containers.
				//

				foreach (var container in cargo_containers)
				{
					if (floor_intake && container.try_loading(floor_intake))
					{
						GetComponent<AudioManager>().Sound("Shoot");
						break;
					}
				}

				if (key_now_down(Key.LeftArrow ) || dpad_left_now_down (GetComponent<Assembler>().data.is_using_assistant ? 1 : 0)) { selected_cargo_container_index -= 1; }
				if (key_now_down(Key.RightArrow) || dpad_right_now_down(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0)) { selected_cargo_container_index -= 1; }
				selected_cargo_container_index = mod(selected_cargo_container_index, cargo_containers.Length);

				if (key_now_down(Key.Z) || left_stick_now_down(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0))
				{
					if (cargo_containers[selected_cargo_container_index].try_unloading(true))
					{
						GetComponent<AudioManager>().Sound("Pop");
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
							yaw             : arrow_keys().x          +  right_stick           (GetComponent<Assembler>().data.is_using_assistant ? 1 : 0).x,
							pitch           : arrow_keys().y          +  right_stick           (GetComponent<Assembler>().data.is_using_assistant ? 1 : 0).y,
							shoot           : key_now_down(Key.Space) || trigger_right_now_down(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0),
							cargo_container : cargo_containers[selected_cargo_container_index]
						);
				}
				else if (subtype<FixedPointShooterManipulator>(primary))
				{
					(primary as FixedPointShooterManipulator).control
						(
							shoot           : key_now_down(Key.Space) || trigger_right_now_down(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0),
							cargo_container : cargo_containers[selected_cargo_container_index]
						);
				}
				else if (subtype<ArmManipulator>(primary))
				{
					(primary as ArmManipulator).control
						(
							yaw    : arrow_keys().x          +  right_stick           (GetComponent<Assembler>().data.is_using_assistant ? 1 : 0).x,
							pitch  : arrow_keys().y          +  right_stick           (GetComponent<Assembler>().data.is_using_assistant ? 1 : 0).y,
							toggle : key_now_down(Key.Space) || trigger_right_now_down(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0)
						);
				}
				else if (subtype<WristAndArmManipulator>(primary))
				{
					(primary as WristAndArmManipulator).control
						(
							yaw          : arrow_keys().x          + right_stick(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0).x,
							pitch        : arrow_keys().y          + right_stick(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0).y,
							joint_toggle : key_now_down(Key.X)     || right_stick_now_down  (GetComponent<Assembler>().data.is_using_assistant ? 1 : 0),
							grab_toggle  : key_now_down(Key.Space) || trigger_right_now_down(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0)
						);
				}
				else if (subtype<TelescopicArmManipulator>(primary))
				{
					(primary as TelescopicArmManipulator).control
						(
							yaw          :                             arrow_keys().x         + right_stick(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0).x,
							pitch        : (!key_down(Key.LeftShift) ? arrow_keys().y : 0.0f) + right_stick(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0).y,
							length       : ( key_down(Key.LeftShift) ? arrow_keys().y : 0.0f) +            (GetComponent<Assembler>().data.is_using_assistant ? left_stick(1).y + dpad(1).y : dpad(0).y),
							joint_toggle : key_now_down(Key.X)     || right_stick_now_down  (GetComponent<Assembler>().data.is_using_assistant ? 1 : 0),
							grab_toggle  : key_now_down(Key.Space) || trigger_right_now_down(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0)
						);

				}
				else if (subtype<BucketManipulator>(primary))
				{
					(primary as BucketManipulator).control
						(
							pitch           : key_down(Key.X) ? 1.0f : (trigger_right(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0) > 0.0f ?  1.0f : -1.0f),
							length          : arrow_keys().y + (GetComponent<Assembler>().data.is_using_assistant ? left_stick(1).y + dpad(1).y : dpad(0).y),
							store           : key_now_down(Key.Space) || right_stick_now_down(0),
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
							shoot : key_now_down(Key.Enter) || trigger_left_now_down(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0)
						);
				}
				else if (subtype<DualCaneManipulator>(secondary))
				{
					(secondary as DualCaneManipulator).control
						(
							extend : key_down(Key.Enter) || trigger_left(GetComponent<Assembler>().data.is_using_assistant ? 1 : 0) > 0.0f
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
					random_state_countdown = UnityEngine.Random.Range(0.5f, 1.0f);
				}
				else
				{
					random_state_countdown = UnityEngine.Random.Range(0.5f, 2.0f);
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

		trigger_triggered_this_frame = false;
	}
}
