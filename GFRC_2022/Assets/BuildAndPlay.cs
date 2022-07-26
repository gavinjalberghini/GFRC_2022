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
	public Button       btn_play_test;
	public Button       btn_play_simulation;
	public TMP_Dropdown drp_drive;
	public TMP_Text     txt_drive;
	public TMP_Dropdown drp_primary;
	public TMP_Text     txt_primary;
	public TMP_Dropdown drp_secondary;
	public TMP_Text     txt_secondary;

	public GameObject   base_mecanum;
	public GameObject   curr_build;

	void Start()
	{
		btn_play_test.onClick.AddListener(delegate {
			print("play test");
		});

		btn_play_simulation.onClick.AddListener(delegate {
			print("play simulation");
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
		});

		drp_primary.onValueChanged.AddListener(delegate {
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
				} break;

				case 2:
				{
					txt_primary.text = "A ball launcher system that remains stationary relative to the robot frame.";
					curr_build.GetComponent<Assembler>().pick(Assembler.Primary.fixed_point_shooter);
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
			}
		});

		build();

		Wheel.show_indicator = true;
	}

	void build()
	{
		Vector3 pos = curr_build.transform.position;
		Destroy(curr_build);
		curr_build                    = Instantiate(base_mecanum);
		curr_build.transform.position = pos;
	}
}
