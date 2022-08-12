using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;
using Mono.Data.Sqlite;
using UnityEngine.EventSystems;
using static Global;

public class Main : MonoBehaviour
{
	public enum State
	{
		start,
		counting_down,
		playing,
		end
	};

	public float game_time;
	[HideInInspector] public State state;
	float countdown;
	float end_t;
	public TMP_Text   txt_countdown;
	public TMP_Text   txt_timer;
	public TMP_Text   txt_score;
	public GameObject start_display;
	public GameObject end_display;

	public Hub          hub_top;
	public Hub          hub_bot;
	public Hangar       hangar_blue;
	public Hangar       hangar_red;
	public GameObject[] ordered_bases;
	public PlayCamera   play_camera;
	public GameObject   blue_cargo_source;
	public GameObject   red_cargo_source;
	public GameObject   blue_zone;
	public GameObject   red_zone;
	public GameObject   forfeit_display;
	public Button       forfeit_yes;
	public Button       forfeit_no;

	public static bool           randomized_robot_spawn = true;
	public static bool           use_dummy_robots = true;
	public static int            assembler_base_index;
	public static Assembler.Data assembler_data = new Assembler.Data();

	List<GameObject> RobotReds  = new List<GameObject>();
	List<GameObject> RobotBlues = new List<GameObject>();
	GameObject       player;
	bool             got_highscore;

	int final_points;
	int calc_points() =>
		assembler_data.is_red_alliance
			? hub_top.redScore  + hub_bot.redScore  + (player.GetComponent<RobotBrain>().touching_ground ? 0 : hangar_red .calc_score ())
			: hub_top.blueScore + hub_bot.blueScore + (player.GetComponent<RobotBrain>().touching_ground ? 0 : hangar_blue.calc_score());

	void Start()
	{
		Wheel.show_indicator = false;

		forfeit_no.onClick.AddListener(delegate {
			forfeit_display.SetActive(false);
		});

		forfeit_yes.onClick.AddListener(delegate {
			SceneManager.LoadScene("Scenes/Title Menu Scene");
		});

		//
		// Spawn robots.
		//

		// assembler_data.curr_primary = Assembler.Primary.fixed_point_shooter;
		// assembler_data.curr_secondary = Assembler.Secondary.human_feed_intake;
		// assembler_data.using_floor_intake = true;
		// randomized_robot_spawn = false;
		// use_dummy_robots = false;

		{
			player = Instantiate(ordered_bases[assembler_base_index]);
			player.GetComponent<Assembler>().use_data(assembler_data);

			GameObject focused_robot = player;
			play_camera.robot_subject = focused_robot.GetComponent<Transform>();
			focused_robot.GetComponent<RobotBrain>().enabled      = true;
			focused_robot.GetComponent<RobotBrain>().cargo_source = assembler_data.is_red_alliance ? red_cargo_source : blue_cargo_source;
			red_zone .SetActive(assembler_data.curr_secondary == Assembler.Secondary.human_feed_intake &&  assembler_data.is_red_alliance);
			blue_zone.SetActive(assembler_data.curr_secondary == Assembler.Secondary.human_feed_intake && !assembler_data.is_red_alliance);

			hub_top.sfxForRed = assembler_data.is_red_alliance;
			hub_bot.sfxForRed = assembler_data.is_red_alliance;
			if (assembler_data.is_red_alliance)
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

			if (use_dummy_robots)
			{
				while (RobotReds.Count < 4)
				{
					make_random_dummy(RobotReds);
				}
				while (RobotBlues.Count < 4)
				{
					make_random_dummy(RobotBlues);
				}
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

		GetComponent<AudioManager>().Sound("Music");
	}

	void Update()
	{
		switch (state)
		{
			case State.start:
			{
				if (any_key_now_down() && !key_now_down(Key.Tab))
				{
					state     = State.counting_down;
					countdown = 3.0f;
					txt_countdown.gameObject.SetActive(true);
					start_display.SetActive(false);
				}
			} break;

			case State.counting_down:
			{
				float old_countdown = countdown;
				countdown = Mathf.Max(countdown - Time.deltaTime, 0.0f);

				if (Mathf.Floor(old_countdown) != Mathf.Floor(countdown))
				{
					GetComponent<AudioManager>().Sound("Time");
				}

				txt_countdown.text = Mathf.Ceil(countdown).ToString();
				txt_countdown.fontSize = Mathf.Lerp(0.0f, 256.0f, 1.0f - Mathf.Pow(1.0f - countdown % 1.0f, 2.0f));

				if (countdown == 0.0f)
				{
					state = State.playing;
					txt_countdown.gameObject.SetActive(false);
					txt_timer.gameObject.SetActive(true);
					txt_score.gameObject.SetActive(true);
					GetComponent<AudioManager>().Sound("Air Horn");

					if (assembler_data.is_red_alliance)
					{
						txt_score.color = RED;
					}
					else
					{
						txt_score.color = BLUE;
					}
				}
			} break;

			case State.playing:
			{
				if (key_now_down(Key.Escape))
				{
					forfeit_display.SetActive(!forfeit_display.activeInHierarchy);
				}

				float old_game_time = game_time;

				game_time = Mathf.Max(game_time - Time.deltaTime, 0.0f);
				txt_timer.text =
					game_time <= 10.0f
						? game_time.ToString("n2") + "s"
						: Mathf.Floor(game_time / 60.0f).ToString() + ":" + Mathf.Floor(game_time % 60.0f).ToString().PadLeft(2, '0');
				txt_score.text = calc_points().ToString();

				if (game_time <= 10.0f && Mathf.Floor(old_game_time) != Mathf.Floor(game_time))
				{
					txt_timer.color = new Color(1.0f, 0.0f, 0.0f);
					GetComponent<AudioManager>().Sound("Time");
				}
				else
				{
					txt_timer.color = dampen(txt_timer.color, new Color(1.0f, 1.0f, 1.0f), 0.1f);
				}

				if (game_time == 0.0f)
				{
					forfeit_display.SetActive(false);
					state = State.end;
					txt_timer.gameObject.SetActive(false);
					txt_score.gameObject.SetActive(false);
					GetComponent<AudioManager>().Sound("Air Horn");

					final_points = calc_points();

					if (db_currently_signed_in && randomized_robot_spawn && use_dummy_robots)
					{
						var entries = db_get_entries();
						for (int i = 0; i < entries.Count; i += 1)
						{
							if (entries[i].username == db_curr_username)
							{
								if (entries[i].points < final_points)
								{
									got_highscore = entries[i].points != -1;
									entries[i].points = final_points;

									entries[i].build = "";
									string[] base_names = { "Mecanum", "Swerve", "Tank", "Car", "Kiwi", "Forklift", "H" };
									entries[i].build += base_names[assembler_base_index];
									if (assembler_data.curr_primary != Assembler.Primary.none)
									{
										string[] primary_names = { "_", "Turret Mounted Shooter", "Fixed Point Shooter", "Simple Arm", "Jointed Arm", "Telescopic Arm", "Bucket" };
										entries[i].build += "\n" + primary_names[(int) assembler_data.curr_primary];
									}
									if (assembler_data.curr_secondary != Assembler.Secondary.none)
									{
										string[] secondary_names = { "_", "Grappling Hook", "Dual Canes", "Human Feed Intake" };
										entries[i].build += "\n" + secondary_names[(int) assembler_data.curr_secondary];
									}
									if (assembler_data.using_floor_intake)
									{
										entries[i].build += "\nFoor Intake";
									}

									entries[i].unixtime = DateTimeOffset.Now.ToUnixTimeSeconds();
								}

								break;
							}
						}
						db_set_entries(entries);
					}
				}
			} break;

			case State.end:
			{
				end_t = Mathf.Min(end_t + Time.deltaTime / 6.0f, 1.0f);

				if (end_t == 1.0f)
				{
					end_display.transform.Find("Subtitle").GetComponent<TMP_Text>().color = dampen(end_display.transform.Find("Subtitle").GetComponent<TMP_Text>().color, new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.1f);
					if (any_key_now_down())
					{
						SceneManager.LoadScene("Scenes/Title Menu Scene");
					}
				}

				in_window(end_t, 0.25f, 1.0f, display_t => {
					end_display.SetActive(true);

					in_window(display_t, 0.0f, 0.05f, fade_in_t => end_display.GetComponent<CanvasGroup>().alpha = Mathf.Pow(fade_in_t, 3.0f));
					if (display_t >= 0.05f)
					{
						end_display.GetComponent<CanvasGroup>().alpha = 1.0f;
					}

					end_display.transform.Find("Title").GetComponent<TMP_Text>().text = got_highscore ? "New High Score!" : "You Scored";

					in_window(display_t, 0.25f, 1.0f, score_t => {
						if (got_highscore)
						{
							end_display.transform.Find("Title").GetComponent<TMP_Text>().color =
								dampen
								(
									end_display.transform.Find("Title").GetComponent<TMP_Text>().color,
									score_t == 1.0f
										? new Color(1.0f, 1.0f, 1.0f)
										: Color.HSVToRGB(score_t, 0.8f, 1.0f),
									0.1f
								);
						}
						end_display.transform.Find("Score").GetComponent<TMP_Text>().text  = Mathf.Round(final_points * (1.0f - Mathf.Pow(1.0f - score_t, 4.0f))).ToString();

						if (any_key_now_down())
						{
							end_t = 1.0f;
						}
					});
				});
			} break;
		}
	}
}
