using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Mono.Data.Sqlite;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static Global;

public class BuildAndPlay : MonoBehaviour
{
	public Canvas       canvas;
	public Button       btn_play_simulation;
	public Button       btn_view_bindings;
	public Button       btn_zoom;
	public GameObject   bindings_pop_up;
	public TMP_Dropdown drp_drive;
	public TMP_Text     txt_drive;
	public TMP_Dropdown drp_primary;
	public TMP_Text     txt_primary;
	public TMP_Dropdown drp_secondary;
	public TMP_Text     txt_secondary;
	public Toggle       tgl_floor_intake;
	public Toggle       tgl_assistant;
	public Toggle       tgl_dummy_robots;
	public Toggle       tgl_randomized_spawn;
	public TMP_Dropdown drp_alliance;
	public GameObject   preview_camera;
	public Transform    reset_point;
	public GameObject   slider_power;
	public GameObject   slider_yaw;
	public GameObject   slider_pitch;
	public GameObject[] ordered_bases;

	GameObject curr_build;
	bool       zoomed = true;
	float      zoom_t = 1.0f;

	void Start()
	{
		btn_play_simulation.onClick.AddListener(delegate {
			Main.randomized_robot_spawn = false;
			Main.assembler_base_index   = drp_drive.value;
			Main.assembler_data         = curr_build.GetComponent<Assembler>().data;
			SceneManager.LoadScene("Scenes/Main Scene");
		});

		canvas.GetComponent<ClickDetector>().click_down = delegate {
			bindings_pop_up.SetActive(false);
		};

		btn_view_bindings.onClick.AddListener(delegate {
			bindings_pop_up.SetActive(true);
		});

		btn_zoom.onClick.AddListener(delegate {
			zoomed = !zoomed;
		});

		drp_drive.onValueChanged.AddListener(delegate {
			switch (drp_drive.value)
			{
				case 0:
				{
					txt_drive.text = "The mecanum drive uses specialized wheels that incurs a sideways force when driving. Allows for direct strafing.";
				} break;

				case 1:
				{
					txt_drive.text = "The swerve drive has wheels that can be turned independently of each other. Allows easy strafing and steering.";
				} break;

				case 2:
				{
					txt_drive.text = "The tank drive has six traction wheels with the middle wheel slightly lower to allow better turning.";
				} break;

				case 3:
				{
					txt_drive.text = "The car drive uses the front wheels to steer like an actual car. Poor turning angles however.";
				} break;

				case 4:
				{
					txt_drive.text = "The kiwi drive uses a triangular frame with each side having its own wheel. Generally offensive due to movement capabilities.";
				} break;

				case 5:
				{
					txt_drive.text = "Similar to a Tank Drive, except has four total wheels with two front omni wheels in the front.";
				} break;

				case 6:
				{
					txt_drive.text = "The H-drive uses a wheel perpendicular to the four main wheels to allow direct strafing.";
				} break;
			}
			build(ordered_bases[drp_drive.value]);
		});

		drp_primary.onValueChanged.AddListener(delegate {
			slider_power.SetActive(false);
			slider_yaw  .SetActive(false);
			slider_pitch.SetActive(false);

			switch (drp_primary.value)
			{
				case 0:
				{
					txt_primary.text = "Optionally pick a primary system to attach to the build.";
					curr_build.GetComponent<Assembler>().pick(Assembler.Primary.none);
				} break;

				case 1:
				{
					txt_primary.text = "A ball launcher system that pivots and rotates.";
					curr_build.GetComponent<Assembler>().pick(Assembler.Primary.turret_mounted_shooter);
					slider_power.SetActive(true);
				} break;

				case 2:
				{
					txt_primary.text = "A ball launcher system that remains stationary relative to the robot frame.";
					curr_build.GetComponent<Assembler>().pick(Assembler.Primary.fixed_point_shooter);
					slider_power.SetActive(true);
					slider_yaw  .SetActive(true);
					slider_pitch.SetActive(true);
				} break;

				case 3:
				{
					txt_primary.text = "A single joint appendage that allows for collecting cargo from the game field and scoring with it.";
					curr_build.GetComponent<Assembler>().pick(Assembler.Primary.simple_arm);
				} break;

				case 4:
				{
					txt_primary.text = "A two joint appendage that allows for collecting cargo from the game field and scoring with it.";
					curr_build.GetComponent<Assembler>().pick(Assembler.Primary.jointed_arm);
				} break;

				case 5:
				{
					txt_primary.text = "A two joint extendable appendage that allows for collecting cargo from the game field and scoring with it.";
					curr_build.GetComponent<Assembler>().pick(Assembler.Primary.telescopic_arm);
				} break;

				case 6:
				{
					txt_primary.text = "A simple elevator that raises a game object high enough to be gravity fed into a goal.";
					curr_build.GetComponent<Assembler>().pick(Assembler.Primary.bucket);
				} break;
			}
		});

		drp_secondary.onValueChanged.AddListener(delegate {
			switch (drp_secondary.value)
			{
				case 0:
				{
					txt_secondary.text = "Optionally pick a secondary system to attach to the build.";
					curr_build.GetComponent<Assembler>().pick(Assembler.Secondary.none);
				} break;

				case 1:
				{
					txt_secondary.text = "A climbing system that fires a projectile and ratchets the robot along the cord.";
					curr_build.GetComponent<Assembler>().pick(Assembler.Secondary.grappling_hook);
				} break;

				case 2:
				{
					txt_secondary.text = "A climbing system that extends two hooks that latch onto the bar.";
					curr_build.GetComponent<Assembler>().pick(Assembler.Secondary.dual_canes);
				} break;

				case 3:
				{
					txt_secondary.text = "A secondary intake where cargo can be collected from atop.";
					curr_build.GetComponent<Assembler>().pick(Assembler.Secondary.human_feed_intake);
				} break;
			}
		});

		build(ordered_bases[0]);

		Wheel.show_indicator = true;
		bindings_pop_up.SetActive(false);
	}

	void Update()
	{
		if (key_now_down(Key.Tab))
		{
			zoomed = !zoomed;
		}

		zoom_t = dampen(zoom_t, zoomed ? 1.0f : 0.0f, 0.1f);
		btn_zoom.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Lerp(32.0f, 64.0f, zoom_t), Mathf.Lerp(32.0f, 64.0f, zoom_t));

		curr_build.GetComponent<Assembler>().data.is_using_assistant = tgl_assistant.isOn;
		curr_build.GetComponent<Assembler>().data.shooter_power_t    = slider_power.transform.Find("Slider").GetComponent<Slider>().value;
		curr_build.GetComponent<Assembler>().data.shooter_yaw_t      = slider_yaw.  transform.Find("Slider").GetComponent<Slider>().value;
		curr_build.GetComponent<Assembler>().data.shooter_pitch_t    = slider_pitch.transform.Find("Slider").GetComponent<Slider>().value;
		curr_build.GetComponent<Assembler>().set_alliance(drp_alliance.value == 0);
		curr_build.GetComponent<Assembler>().set_floor_intake(tgl_floor_intake.isOn);

		preview_camera.transform.position = curr_build.transform.position + new Vector3(0.8f, 1.1f, 1.3f) * (2.0f - zoom_t);
		preview_camera.transform.rotation = Quaternion.LookRotation(curr_build.transform.position - preview_camera.transform.position, new Vector3(0.0f, 1.0f, 0.0f));

		if (bindings_pop_up.activeInHierarchy)
		{
			String gamepad_1 = "";
			String gamepad_2 = "";

			if (tgl_assistant.isOn)
			{
				gamepad_1 +=
					"Drive/Strafe : (L)\n" +
					"Steer : (R)\n";
				gamepad_2 +=
					"Cycle cargo containers : (Left) / (Right)\n" +
					"Dismount current cargo container : (L3)\n";
			}
			else
			{
				gamepad_1 +=
					"Drive/Steer : (L)\n" +
					"Cycle cargo containers : (Left) / (Right)\n" +
					"Dismount current cargo container : (L3)\n";
			}

			if (RobotBrain.subtype<TurretMountedShooterManipulator>(curr_build.GetComponent<RobotBrain>().primary))
			{
				var str =
					"Shooter yaw and pitch : (R)\n" +
					"Shoot shooter : (R2)\n";
				if (tgl_assistant.isOn) { gamepad_2 += str; }
				else                    { gamepad_1 += str; }
			}
			else if (RobotBrain.subtype<FixedPointShooterManipulator>(curr_build.GetComponent<RobotBrain>().primary))
			{
				var str =
					"Shoot shooter : (R2)\n";
				if (tgl_assistant.isOn) { gamepad_2 += str; }
				else                    { gamepad_1 += str; }
			}
			else if (RobotBrain.subtype<ArmManipulator>(curr_build.GetComponent<RobotBrain>().primary))
			{
				var str =
					"Arm yaw and pitch : (R)\n" +
					"Arm toggle grab : (R2)\n";
				if (tgl_assistant.isOn) { gamepad_2 += str; }
				else                    { gamepad_1 += str; }
			}
			else if (RobotBrain.subtype<WristAndArmManipulator>(curr_build.GetComponent<RobotBrain>().primary))
			{
				var str =
					"Arm yaw and pitch : (R)\n" +
					"Arm toggle grab : (R2)\n" +
					"Arm toggle joint : (R3)\n";
				if (tgl_assistant.isOn) { gamepad_2 += str; }
				else                    { gamepad_1 += str; }
			}
			else if (RobotBrain.subtype<TelescopicArmManipulator>(curr_build.GetComponent<RobotBrain>().primary))
			{
				var str =
					"Arm yaw and pitch : (R)\n" +
					"Arm toggle grab : (R2)\n" +
					"Arm toggle joint : (R3)\n" +
					"Extend/retract arm : (Down) / (Up)\n";
				if (tgl_assistant.isOn) { gamepad_2 += str + "Arm extend/retract : (Ly)\n"; }
				else                    { gamepad_1 += str; }
			}
			else if (RobotBrain.subtype<BucketManipulator>(curr_build.GetComponent<RobotBrain>().primary))
			{
				var str =
					"Tilt bucket : (R2)\n" +
					"Load bucket : (R3)\n" +
					"Extend/Retract bucket : (Down) / (Up)\n";
				if (tgl_assistant.isOn) { gamepad_2 += str + "Bucket extend/retract : (Ly)\n"; }
				else                    { gamepad_1 += str; }
			}

			if (RobotBrain.subtype<GrapplingHookManipulator>(curr_build.GetComponent<RobotBrain>().secondary))
			{
				var str =
					"Shoot grapple : (L2)\n";
				if (tgl_assistant.isOn) { gamepad_2 += str; }
				else                    { gamepad_1 += str; }
			}
			else if (RobotBrain.subtype<DualCaneManipulator>(curr_build.GetComponent<RobotBrain>().secondary))
			{
				var str =
					"Extend dual canes : (L2)\n";
				if (tgl_assistant.isOn) { gamepad_2 += str; }
				else                    { gamepad_1 += str; }
			}

			bindings_pop_up.transform.Find("Gamepad 1").Find("Description").GetComponent<TMP_Text>().text = gamepad_1;

			if (tgl_assistant.isOn)
			{
				bindings_pop_up.transform.Find("Gamepad 2").gameObject.SetActive(true);
				bindings_pop_up.transform.Find("Gamepad 2").Find("Description").GetComponent<TMP_Text>().text = gamepad_2;
			}
			else
			{
				bindings_pop_up.transform.Find("Gamepad 2").gameObject.SetActive(false);
			}
		}

		Main.use_dummy_robots       = tgl_dummy_robots.isOn;
		Main.randomized_robot_spawn = tgl_randomized_spawn.isOn;
	}

	void build(GameObject robot_base)
	{
		Vector3        pos  = curr_build ? curr_build.transform.position : reset_point.position;
		Quaternion     rot  = curr_build ? curr_build.transform.rotation : reset_point.rotation;
		Assembler.Data data = curr_build ? curr_build.GetComponent<Assembler>().data : new Assembler.Data();
		curr_build?.GetComponent<Assembler>().free();
		Destroy(curr_build);
		curr_build                    = Instantiate(robot_base);
		curr_build.transform.position = pos;
		curr_build.transform.rotation = rot;
		curr_build.GetComponent<Assembler>().use_data(data);
	}
}
