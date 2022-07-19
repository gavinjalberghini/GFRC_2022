using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Mono.Data.Sqlite;
using static Global;

public class Main : MonoBehaviour
{
	public Hub             hub_top;
	public Hub             hub_bot;
	public Hangar          hangar_blue;
	public Hangar          hangar_red;
	public TextMeshProUGUI debug;

	void Update()
	{
		debug.text =
			"score_hub_top_red  : " + hub_top.redScore + "\n" +
			"score_hub_bot_red  : " + hub_bot.redScore + "\n" +
			"score_hangar_red   : " + hangar_red.calc_score() + "\n" +
			"score_hub_top_blue : " + hub_top.blueScore + "\n" +
			"score_hub_bot_blue : " + hub_bot.blueScore + "\n" +
			"score_hangar_blue  : " + hangar_blue.calc_score() + "\n";
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

	public (bool, int) db_try_get_points(string alliance, int team)
	{
		bool success = false;
		int  points  = 0;

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
						success = true;
						points  = int.Parse(reader["points"].ToString());
					}
					reader.Close();
				}
			}
			connection.Close();
		}

		return (success, points);
	}

	public void db_add_points(string alliance, int team, int addend)
	{
		(bool success, int points) = db_try_get_points(alliance, team);
		if (success)
		{
			db_set_points(alliance, team, points + addend);
		}
		else
		{
			db_set_points(alliance, team, addend);
		}
	}

	public void db_print_points()
	{
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
						print(":: " + reader["alliance"] + " " + reader["team"] + " " + reader["points"]);
					}
					reader.Close();
				}
			}
			connection.Close();
		}
	}
}
