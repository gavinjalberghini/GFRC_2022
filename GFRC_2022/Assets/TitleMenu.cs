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

public class TitleMenu : MonoBehaviour
{
	public Button         build_and_play;
	public Button         show_leaderboard;
	public Button         exit;
	public ClickDetector  user_icon;
	public GameObject     user_pop_up;
	public ClickDetector  user_pop_up_outside;
	public Button         btn_sign_up;
	public Button         btn_log_in;
	public TMP_InputField fld_username;
	public TMP_InputField fld_pin;
	public TMP_InputField fld_teamnumber;
	public TMP_InputField fld_teamname;

	void Start()
	{
		build_and_play.onClick.AddListener(delegate {
			SceneManager.LoadScene("Scenes/BuildAndPlay");
		});

		show_leaderboard.onClick.AddListener(delegate {
			SceneManager.LoadScene("Scenes/Leaderboard");
		});

		exit.onClick.AddListener(delegate {
			Application.Quit();
		});

		user_pop_up_outside.click_down = delegate {
			user_pop_up.SetActive(false);
		};

		user_icon.click_down = delegate {
			user_pop_up.SetActive(true);
		};

		{
			var xs = db_get_entries();
			if (xs.Count == 0)
			{
				print("No data.");
			}
			else
			{
				foreach (var x in xs)
				{
					print
					(
						" | username   : " + x.username   +
						" | pin        : " + x.pin        +
						" | teamnumber : " + x.teamnumber +
						" | teamname   : " + x.teamname   +
						" | scored     : " + x.points
					);
				}
			}
		}

		btn_sign_up.onClick.AddListener(delegate {
			bool valid   = true;
			var  entries = db_get_entries();

			if (fld_username.text == "")
			{
				print("Username required.");
				valid = false;
			}
			else if (fld_pin.text == "")
			{
				print("Pin required.");
				valid = false;
			}
			else
			{
				foreach (var entry in entries)
				{
					if (entry.username == fld_username.text)
					{
						print("Username taken.");
						valid = false;
						break;
					}
					else if (entry.teamname == fld_teamname.text)
					{
						print("Team name taken.");
						valid = false;
						break;
					}
					else if (entry.teamnumber == fld_teamnumber.text)
					{
						print("Team number taken.");
						valid = false;
						break;
					}
				}
			}


			if (valid)
			{
				entries.Add
				(
					new DB_Entry
						{
							username   = fld_username.text,
							pin        = fld_pin.text,
							teamnumber = fld_teamnumber.text,
							teamname   = fld_teamname.text
						}
				);
				db_set_entries(entries);
			}
		});

		btn_log_in.onClick.AddListener(delegate {
			print("log in");
		});

	}
}
