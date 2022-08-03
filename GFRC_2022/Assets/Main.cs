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
using static Global;

public class Main : MonoBehaviour
{
	public Hub             hub_top;
	public Hub             hub_bot;
	public Hangar          hangar_blue;
	public Hangar          hangar_red;
	public TextMeshProUGUI debug;
	public Text            redScore;
	public Text            blueScore;
	public GameObject[]    ordered_bases;
	public PlayCamera      play_camera;
	public Timer           timer;

	public static bool                randomized_robot_spawn;
	public static int                 assmebler_base_index;
	public static Assembler.Primary   assmebler_curr_primary;
	public static Assembler.Secondary assmebler_curr_secondary;
	public static bool                assmebler_using_floor_intake;
	public static bool                assembler_red_alliance;

	GameObject[] RobotReds;
	GameObject[] RobotBlues;

	void Start()
	{
		Wheel.show_indicator = false;

		//
		// Spawn robots.
		//

		{
			GameObject[] xs = new GameObject[1];
			xs[0] = Instantiate(ordered_bases[assmebler_base_index]);
			xs[0].GetComponent<Assembler>().pick(assmebler_curr_primary);
			xs[0].GetComponent<Assembler>().pick(assmebler_curr_secondary);
			xs[0].GetComponent<Assembler>().set_floor_intake(assmebler_using_floor_intake);
			xs[0].GetComponent<Assembler>().set_alliance(assembler_red_alliance);

			{
				GameObject focused_robot = xs[0];
				play_camera.robot_subject = focused_robot.GetComponent<Transform>();
				focused_robot.GetComponent<RobotBrain>().enabled = true;
			}

			if (assembler_red_alliance)
			{
				RobotReds  = xs;
				RobotBlues = new GameObject[0];
			}
			else
			{
				RobotReds  = new GameObject[0];
				RobotBlues = xs;
			}
		}

		{
			Action<GameObject[], List<Transform>> spawn_robots =
				(robots, spawn_points) =>
				{
					for (int i = 0; i < robots.Length; i += 1)
					{
						int spawn_index = randomized_robot_spawn ? UnityEngine.Random.Range(0, spawn_points.Count) : 0;
						robots[i].transform.position = spawn_points[spawn_index].position;
						robots[i].transform.rotation = spawn_points[spawn_index].rotation;
						//robots[i].GetComponent<RobotBrain>().enabled = false;
						spawn_points.RemoveAt(spawn_index);
					}
				};

			if (randomized_robot_spawn)
			{
				var spawn_points = transform.Find("RandomSpawn").Cast<Transform>().ToList();
				spawn_robots(RobotReds , spawn_points);
				spawn_robots(RobotBlues, spawn_points);
			}
			else
			{
				spawn_robots(RobotReds , transform.Find("RedSpawn" ).Cast<Transform>().ToList());
				spawn_robots(RobotBlues, transform.Find("BlueSpawn").Cast<Transform>().ToList());
			}
		}
	}

	void Update()
	{
		if (timer.justFinished && db_currently_signed_in)
		{
			var entries = db_get_entries();
			for (int i = 0; i < entries.Count; i += 1)
			{
				if (entries[i].username == db_curr_username)
				{
					if (assembler_red_alliance)
					{
						entries[i].points = hub_top.redScore + hub_bot.redScore + hangar_red.calc_score();
					}
					else
					{
						entries[i].points = hub_top.blueScore + hub_bot.blueScore + hangar_blue.calc_score();
					}
					break;
				}
			}
			db_set_entries(entries);
		}

		debug.text =
			"score_hub_top_red  : " + hub_top.redScore + "\n" +
			"score_hub_bot_red  : " + hub_bot.redScore + "\n" +
			"score_hangar_red   : " + hangar_red.calc_score() + "\n" +
			"score_hub_top_blue : " + hub_top.blueScore + "\n" +
			"score_hub_bot_blue : " + hub_bot.blueScore + "\n" +
			"score_hangar_blue  : " + hangar_blue.calc_score() + "\n";

		redScore.text = "Red: " + (hub_top.redScore + hub_bot.redScore + hangar_red.calc_score());
		blueScore.text = "Blue: " + (hub_top.blueScore + hub_bot.blueScore + hangar_blue.calc_score());
	}
}
