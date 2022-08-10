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
	public Hub              hub_top;
	public Hub              hub_bot;
	public Hangar           hangar_blue;
	public Hangar           hangar_red;
	public TextMeshProUGUI  debug;
	public Text             redScore;
	public Text             blueScore;
	public GameObject[]     ordered_bases;
	public PlayCamera       play_camera;
	public Timer            timer;
	//public GameObject[]     dummies;

	public static bool                randomized_robot_spawn;
	public static int                 assembler_base_index;
	public static Assembler.Primary   assembler_curr_primary;
	public static Assembler.Secondary assembler_curr_secondary;
	public static bool                assembler_using_floor_intake;
	public static bool                assembler_red_alliance;

	List<GameObject> RobotReds  = new List<GameObject>();
	List<GameObject> RobotBlues = new List<GameObject>();

	void Start()
	{
		Wheel.show_indicator = false;

		//
		// Spawn robots.
		//

		{
			GameObject player = Instantiate(ordered_bases[assembler_base_index]);

			player.GetComponent<Assembler>().pick(assembler_curr_primary);
			player.GetComponent<Assembler>().pick(assembler_curr_secondary);
			player.GetComponent<Assembler>().set_floor_intake(assembler_using_floor_intake);
			player.GetComponent<Assembler>().set_alliance(assembler_red_alliance);

			GameObject focused_robot = player;
			play_camera.robot_subject = focused_robot.GetComponent<Transform>();
			focused_robot.GetComponent<RobotBrain>().enabled = true;

			if (assembler_red_alliance)
			{
				RobotReds.Add(player);
			}
			else
			{
				RobotBlues.Add(player);
			}

			Action<List<GameObject>> make_random_dummy =
				(xs) =>
				{
					GameObject dummy = Instantiate(ordered_bases[UnityEngine.Random.Range(0, ordered_bases.Length)]);
					dummy.GetComponent<RobotBrain>().is_playing = false;
					dummy.GetComponent<Assembler>().pick((Assembler.Primary)   UnityEngine.Random.Range(0, Enum.GetNames(typeof(Assembler.Primary)).Length));
					dummy.GetComponent<Assembler>().pick((Assembler.Secondary) UnityEngine.Random.Range(0, Enum.GetNames(typeof(Assembler.Secondary)).Length));
					dummy.GetComponent<Assembler>().set_floor_intake(UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f);
					dummy.GetComponent<Assembler>().set_alliance(xs == RobotReds);
					xs.Add(dummy);
				};

			while (RobotReds.Count < 4)
			{
				make_random_dummy(RobotReds);
			}
			while (RobotBlues.Count < 4)
			{
				make_random_dummy(RobotBlues);
			}
		}

		{
			Action<List<GameObject>, List<Transform>> spawn_robots =
				(robots, spawn_points) =>
				{
					for (int i = 0; i < robots.Count; i += 1)
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
						entries[i].points = Math.Max(entries[i].points, hub_top.redScore + hub_bot.redScore + hangar_red.calc_score());
					}
					else
					{
						entries[i].points = Math.Max(entries[i].points, hub_top.blueScore + hub_bot.blueScore + hangar_blue.calc_score());
					}

					entries[i].build = "";

					string[] base_names = { "Mecanum", "Swerve", "Tank", "Car", "Kiwi", "Forklift", "H" };
					entries[i].build += base_names[assembler_base_index];

					if (assembler_curr_primary != Assembler.Primary.none)
					{
						string[] primary_names = { "_", "Turret Mounted Shooter", "Fixed Point Shooter", "Simple Arm", "Jointed Arm", "Telescopic Arm", "Bucket" };
						entries[i].build += "\n" + primary_names[(int) assembler_curr_primary];
					}

					if (assembler_curr_secondary != Assembler.Secondary.none)
					{
						string[] secondary_names = { "_", "Grappling Hook", "Dual Canes", "Human Feed Intake" };
						entries[i].build += "\n" + secondary_names[(int) assembler_curr_secondary];
					}

					if (assembler_using_floor_intake)
					{
						entries[i].build += "\nFoor Intake";
					}

					entries[i].unixtime = DateTimeOffset.Now.ToUnixTimeSeconds();
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
