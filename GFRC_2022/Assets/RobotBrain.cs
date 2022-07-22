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
			drive_controller.control(left_stick() + wasd(), left_stick().x + qe);
		}

		//
		// Handles primary manipulators case by case.
		//

		if (primary == null)
		{
			print("NO PRIMARY MANIPULATOR");
		}
		else if (subtype<TurretMountedShooterManipulator>(primary))
		{
			var   turret = primary as TurretMountedShooterManipulator;
			float pitch  = using_assistant ? left_stick(1).y : right_stick().y;
			float yaw    = using_assistant ? left_stick(1).x : right_stick().x;
			bool  shoot  = trigger_right_now_down();

			turret.shooter.omniarm.target_yaw   += yaw   * 90.0f * Time.deltaTime;
			turret.shooter.omniarm.target_pitch += pitch * 90.0f * Time.deltaTime;
			if (shoot)
			{
				foreach (var container in cargo_containers)
				{
					if (turret.shooter.try_shooting(container))
					{
						break;
					}
				}
			}
		}
		else if (subtype<FixedPointShooterManipulator>(primary))
		{
			var  turret = primary as FixedPointShooterManipulator;
			bool shoot  = trigger_right_now_down();

			if (shoot)
			{
				foreach (var container in cargo_containers)
				{
					if (turret.shooter.try_shooting(container))
					{
						break;
					}
				}
			}
		}
		else if (subtype<ArmManipulator>(primary))
		{
			var     arm       = primary as ArmManipulator;
			Vector2 yaw_pitch = using_assistant ? left_stick(1) : right_stick();
			bool    toggle    = btn_south_now_down();

			arm.omniarm.target_yaw   += yaw_pitch.x * 90.0f * Time.deltaTime;
			arm.omniarm.target_pitch += yaw_pitch.y * 90.0f * Time.deltaTime;
			if (toggle)
			{
				arm.claw.toggle();
			}
		}
		else if (subtype<WristAndArmManipulator>(primary))
		{
			var     wrist_and_arm = primary as WristAndArmManipulator;
			Vector2 yaw_pitch     = using_assistant ? left_stick(1) : right_stick();
			bool    toggle_joint  = using_assistant ? shoulder_left_now_down(1) : shoulder_right_now_down();
			bool    toggle_grab   = btn_south_now_down();

			if (toggle_joint)
			{
				wrist_and_arm.using_upper = !wrist_and_arm.using_upper;
			}
			if (wrist_and_arm.using_upper)
			{
				wrist_and_arm.omniarm_upper.target_yaw   += yaw_pitch.x * 90.0f * Time.deltaTime;
				wrist_and_arm.omniarm_upper.target_pitch += yaw_pitch.y * 90.0f * Time.deltaTime;
			}
			else
			{
				wrist_and_arm.omniarm_lower.target_yaw   += yaw_pitch.x * 90.0f * Time.deltaTime;
				wrist_and_arm.omniarm_lower.target_pitch += yaw_pitch.y * 90.0f * Time.deltaTime;
			}
			if (toggle_grab)
			{
				wrist_and_arm.claw.toggle();
			}
		}
		else if (subtype<TelescopicArmManipulator>(primary))
		{
			var     telescopic   = primary as TelescopicArmManipulator;
			Vector2 yaw_pitch    = using_assistant ? left_stick(1) : right_stick();
			bool    toggle_joint = using_assistant ? shoulder_left_now_down(1) : shoulder_right_now_down();
			bool    toggle_grab  = right_stick_now_down();
			float   extension    = (btn_south_down() ? 1.0f : 0.0f) + (btn_north_down() ? -1.0f : 0.0f);

			if (toggle_joint)
			{
				telescopic.using_upper = !telescopic.using_upper;
			}
			if (telescopic.using_upper)
			{
				telescopic.omniarm_upper.target_yaw   += yaw_pitch.x * 90.0f * Time.deltaTime;
				telescopic.omniarm_upper.target_pitch += yaw_pitch.y * 90.0f * Time.deltaTime;
			}
			else
			{
				telescopic.omniarm_lower.target_yaw   += yaw_pitch.x * 90.0f * Time.deltaTime;
				telescopic.omniarm_lower.target_pitch += yaw_pitch.y * 90.0f * Time.deltaTime;
			}
			telescopic.omniarm_lower.target_length += extension * Time.deltaTime;
			if (toggle_grab)
			{
				telescopic.claw.toggle();
			}
		}
		else if (subtype<BucketManipulator>(primary))
		{
			var   bucket    = primary as BucketManipulator;
			bool  grab      = right_stick_now_down();
			float pitch     = -right_stick().y;
			float extension = (btn_south_down() ? 1.0f : 0.0f) + (btn_north_down() ? -1.0f : 0.0f);

			if (grab)
			{
				foreach (var container in cargo_containers)
				{
					if (bucket.try_loading(container))
					{
						break;
					}
				}
			}
			bucket.target_height += extension * Time.deltaTime;
			bucket.target_pitch  += pitch * 90.0f * Time.deltaTime;
		}
		else
		{
			print("UNSUPPORTED PRIMARY MANIPULATOR");
		}

		//
		// Handles secondary manipulators case by case.
		//

		if (secondary == null)
		{
			print("NO SECONDARY MANIPULATOR");
		}
		else if (subtype<GrapplingHookManipulator>(secondary))
		{
			var   grappling_hook = secondary as GrapplingHookManipulator;
			float pitch   = right_stick().y;
			float yaw     = right_stick().x;
			bool  toggle  = using_assistant ? trigger_left_now_down(1) : trigger_right_now_down();
			bool  retract = shoulder_right_down();

			grappling_hook.omniarm.target_yaw   += yaw   * 90.0f * Time.deltaTime;
			grappling_hook.omniarm.target_pitch += pitch * 90.0f * Time.deltaTime;
			if (toggle)
			{
				grappling_hook.grapple.toggle();
			}
			if (grappling_hook.grapple.state == Grapple.GrappleState.hooked && retract)
			{
				grappling_hook.grapple.length_max -= 1.0f * Time.deltaTime;
			}
		}
		else if (subtype<DualCaneManipulator>(secondary))
		{
			var dual_cane = secondary as DualCaneManipulator;

			if (shoulder_right_down())
			{
				dual_cane.target_height += 1.0f * Time.deltaTime;
			}
			else
			{
				dual_cane.target_height -= 1.0f * Time.deltaTime;
			}
		}
		else if (subtype<Intake>(secondary))
		{
		}
		else
		{
			print("UNSUPPORTED SECONDARY MANIPULATOR");
		}

		//
		//
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
