using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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
	public TMP_InputField  input;
	public GameObject[]    test_images;

	string output_string;

	void Start()
	{
		input.onEndEdit.AddListener(delegate {
			output_string = "";

			if (input.text == "wipe")
			{
				db_wipe();
			}
			else if (input.text == "print")
			{
				output_string = "Points printed.";
				var xs = db_get_table();
				if (xs.Count == 0)
				{
					print("No data.");
				}
				else
				{
					foreach (var x in xs)
					{
						print("alliance : " + x.alliance + " | team #" + x.team + " | scored : " + x.points);
					}
				}
			}
			else if (input.text == "ralph")
			{
				output_string = "Pick your Ralphs";
				foreach (var img in test_images)
				{
					img.SetActive(true);
				}
			}
			else if (input.text.StartsWith("set"))
			{
				string[] components = input.text.Split(" ")[1..];
				int      team;
				int      points;
				if (components.Length == 3 && int.TryParse(components[1], out team) && int.TryParse(components[2], out points))
				{
					output_string = "Points set : " + input.text;
					db_set_points(components[0], team, points);
				}
				else
				{
					output_string = "Failed to set points.";
				}
			}
			else
			{
				string[] components = input.text.Split(" ");
				int      team;
				int      points;
				if (components.Length == 2 && int.TryParse(components[1], out team) && db_try_get_points(components[0], team, out points))
				{
					output_string = input.text + " : " + points;
				}
				else
				{
					output_string = "Invalid search up  : \"" + input.text + "\"";
				}
			}
		});

		foreach (var img in test_images)
		{
			img.SetActive(false);

			img.GetComponent<ClickDetector>().click = delegate
				{
					img.GetComponent<Outline>().effectColor = new Color(0.0f, 0.0f, 0.0f, 1.0f - img.GetComponent<Outline>().effectColor.a);
				};
		}
	}

	void Update()
	{
		debug.text =
			"score_hub_top_red  : " + hub_top.redScore + "\n" +
			"score_hub_bot_red  : " + hub_bot.redScore + "\n" +
			"score_hangar_red   : " + hangar_red.calc_score() + "\n" +
			"score_hub_top_blue : " + hub_top.blueScore + "\n" +
			"score_hub_bot_blue : " + hub_bot.blueScore + "\n" +
			"score_hangar_blue  : " + hangar_blue.calc_score() + "\n" +
			output_string;
	}

	const string DATABASE_URI_NAME = "URI=file:scores.db";

	public void db_wipe()
	{
		using (var connection = new SqliteConnection(DATABASE_URI_NAME))
		{
			connection.Open();
			using (var command = connection.CreateCommand())
			{
				command.CommandText =
					"DROP TABLE IF EXISTS scores;\n" +
					"CREATE TABLE scores (id INTEGER PRIMARY KEY AUTOINCREMENT, alliance VARCHAR(16), team INT, points INT);";
				command.ExecuteNonQuery();
			}
			connection.Close();
		}
	}

	public void db_set_points(string alliance, int team, int points)
	{
		using (var connection = new SqliteConnection(DATABASE_URI_NAME))
		{
			connection.Open();
			using (var command = connection.CreateCommand())
			{
				command.CommandText =
					"INSERT OR REPLACE INTO scores (id, alliance, team, points) VALUES\n" +
						"((SELECT id FROM scores WHERE alliance = \"" + alliance + "\" AND team = " + team + "), \"" + alliance + "\", \"" + team + "\", \"" + points + "\");\n";
				command.ExecuteNonQuery();
			}
			connection.Close();
		}
	}

	public bool db_try_get_points(string alliance, int team, out int points)
	{
		using (var connection = new SqliteConnection(DATABASE_URI_NAME))
		{
			connection.Open();
			using (var command = connection.CreateCommand())
			{
				command.CommandText = "SELECT alliance, team, points FROM scores WHERE alliance = '" + alliance + "' AND team = '" + team + "';";
				using (var reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						points = int.Parse(reader["points"].ToString());
						return true; // @TODO@ Memory leak? Meh...
					}
					reader.Close();
				}
			}
			connection.Close();
		}

		points = 0;
		return false;
	}

	public void db_add_points(string alliance, int team, int addend)
	{
		int points;
		if (db_try_get_points(alliance, team, out points))
		{
			db_set_points(alliance, team, points + addend);
		}
		else
		{
			db_set_points(alliance, team, addend);
		}
	}

	public List<(string alliance, int team, int points)> db_get_table()
	{
		var list = new List<(string, int, int)>();
		using (var connection = new SqliteConnection(DATABASE_URI_NAME))
		{
			connection.Open();
			using (var command = connection.CreateCommand())
			{
				command.CommandText = "SELECT * FROM scores;";
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add((reader["alliance"].ToString(), int.Parse(reader["team"].ToString()), int.Parse(reader["points"].ToString())));
					}
					reader.Close();
				}
			}
			connection.Close();
		}
		return list;
	}
}
