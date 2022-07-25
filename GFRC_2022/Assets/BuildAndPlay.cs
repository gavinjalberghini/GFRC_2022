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
	public TMP_Dropdown drp_primary;
	public TMP_Text     txt_primary;
	public TMP_Dropdown drp_secondary;
	public TMP_Text     txt_secondary;

	void Start()
	{
		btn_play_test.onClick.AddListener(delegate {
			print("play test");
		});

		btn_play_simulation.onClick.AddListener(delegate {
			print("play simulation");
		});

		drp_drive.onValueChanged.AddListener(delegate {
			print("selected " + drp_drive.value);
		});

		drp_primary.onValueChanged.AddListener(delegate {
			switch (drp_primary.value)
			{
				case 0:
				{
					txt_primary.text = "Optionally pick a primary system to attach to the build.";
				} break;

				case 1:
				{
					txt_primary.text = "A ball launcher system that pivots and rotates.";
				} break;

				case 2:
				{
					txt_primary.text = "A ball launcher system that remains stationary relative to the robot frame.";
				} break;

				case 3:
				{
					txt_primary.text = "A single joint appendage that allows for collecting cargo from the game field and scoring with it.";
				} break;

				case 4:
				{
					txt_primary.text = "A two joint appendage that allows for collecting cargo from the game field and scoring with it.";
				} break;

				case 5:
				{
					txt_primary.text = "A two joint extendable appendage that allows for collecting cargo from the game field and scoring with it.";
				} break;

				case 6:
				{
					txt_primary.text = "A simple elevator that raises a game object high enough to be gravity fed into a goal.";
				} break;
			}
		});

		drp_secondary.onValueChanged.AddListener(delegate {
			switch (drp_secondary.value)
			{
				case 0:
				{
					txt_secondary.text = "Optionally pick a secondary system to attach to the build.";
				} break;

				case 1:
				{
					txt_secondary.text = "A climbing system that fires a projectile and ratchets the robot along the cord.";
				} break;

				case 2:
				{
					txt_secondary.text = "A climbing system that extends two hooks that latch onto the bar.";
				} break;
			}
		});
	}
}
