using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class Database : MonoBehaviour
{
	const string NAME = "URI=file:scores.db";

	public enum Alliance
	{
		red,
		blue
	};

	public void reset()
	{
		using (var connection = new SqliteConnection(NAME))
		{
			connection.Open();
			using (var command = connection.CreateCommand())
			{
				command.CommandText = "CREATE TABLE IF NOT EXISTS scores (id INTEGER PRIMARY KEY AUTOINCREMENT, alliance VARCHAR(4), team INT, points INT);";
				command.ExecuteNonQuery();
			}
			connection.Close();
		}
	}

	public void set_points(Alliance alliance, int team, int points)
	{
		using (var connection = new SqliteConnection(NAME))
		{
			connection.Open();
			using (var command = connection.CreateCommand())
			{
				command.CommandText =
					"INSERT OR REPLACE INTO scores (id, alliance, team, points) VALUES\n" +
						"((SELECT id FROM scores WHERE alliance = \"" + alliance.ToString() + "\" AND team = " + team + "), \"" + alliance.ToString() + "\", \"" + team + "\", \"" + points + "\");\n";
				command.ExecuteNonQuery();
			}
			connection.Close();
		}
	}

	public void print_points()
	{
		using (var connection = new SqliteConnection(NAME))
		{
			connection.Open();
			using (var command = connection.CreateCommand())
			{
				command.CommandText = "SELECT * FROM scores";
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

	void Start()
	{
		reset();

		set_points(Alliance.red, 2, 6);
		set_points(Alliance.red, 2, 3);
		set_points(Alliance.blue, 2, 3);

		print_points();
	}

	void Update()
	{

	}
}
