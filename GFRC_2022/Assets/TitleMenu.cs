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
	public GameObject     note_pop_up;
	public ClickDetector  note_pop_up_outside;
	public Button         btn_sign_in;
	public Button         btn_continue;
	public TMP_InputField fld_username;
	public TMP_InputField fld_pin;
	public TMP_InputField fld_teamnumber;
	public TMP_Text       txt_username;
	public TMP_Text       txt_report;

	void Start()
	{
		build_and_play.onClick.AddListener(delegate {
			if (db_currently_signed_in)
			{
				SceneManager.LoadScene("Scenes/BuildAndPlay");
			}
			else
			{
				note_pop_up.SetActive(true);
			}
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

		note_pop_up_outside.click_down = delegate {
			note_pop_up.SetActive(false);
			txt_report.gameObject.SetActive(false);
		};

		btn_sign_in.onClick.AddListener(delegate {
			note_pop_up.SetActive(false);
			user_pop_up.SetActive(true);
		});

		btn_continue.onClick.AddListener(delegate {
			SceneManager.LoadScene("Scenes/BuildAndPlay");
		});

		{ // @TEMP@ DEBUG!
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
						" | scored     : " + x.points
					);
				}
			}
		} // @TEMP@ DEBUG!

		fld_username.onValidateInput += delegate(string str, int i, char c) {
			if (c == ' ')
			{
				return '_';
			}
			else if ('a' <= c && c <= 'z' || 'A' <= c && c <= 'Z' || '0' <= c && c <= '9' || c == '$')
			{
				return c;
			}
			else
			{
				return '\0';
			}
		};

		btn_sign_up.onClick.AddListener(delegate {
			db_currently_signed_in = false;
			if (fld_username.text == "")
			{
				report_red("Username required!");
			}
			else if (fld_pin.text.Length != 4)
			{
				report_red("4-Digit pin required!");
			}
			else
			{
				bool found_username = false;
				var  entries        = db_get_entries();
				foreach (var entry in entries)
				{
					if (entry.username == fld_username.text)
					{
						found_username = true;
						report_red("Username taken!");
						break;
					}
				}
				if (!found_username)
				{
					entries.Add
					(
						new DB_Entry
							{
								username   = fld_username.text,
								pin        = fld_pin.text,
								teamnumber = fld_teamnumber.text,
								points     = -1,
								unixtime   = 0
							}
					);
					db_set_entries(entries);

					db_currently_signed_in = true;
					db_curr_username       = fld_username.text;
					txt_report.gameObject.SetActive(true);
					txt_report.text        = "Logged as " + fld_username.text;
					txt_report.color       = new Color(0.05043304f, 0.4785869f, 0.8018868f);
				}
			}
		});

		btn_log_in.onClick.AddListener(delegate {
			db_currently_signed_in = false;
			if (fld_username.text == "")
			{
				report_red("Username required!");
			}
			else if (fld_pin.text.Length != 4)
			{
				report_red("4-Digit pin required!");
			}
			else
			{
				bool found_username = false;
				foreach (var entry in db_get_entries())
				{
					if (entry.username == fld_username.text)
					{
						found_username = true;
						if (entry.pin == fld_pin.text)
						{
							db_currently_signed_in = true;
							db_curr_username       = entry.username;
							txt_report.gameObject.SetActive(true);
							txt_report.text        = "Logged as " + entry.username;
							txt_report.color       = new Color(0.05043304f, 0.4785869f, 0.8018868f);
							fld_teamnumber.text    = entry.teamnumber;
						}
						else
						{
							report_red("Invalid pin!");
						}
						break;
					}
				}
				if (!found_username)
				{
					report_red("Unknown username!");
				}
			}
		});
	}

	void report_red(string message)
	{
		txt_report.gameObject.SetActive(true);
		txt_report.text  = message;
		txt_report.color = new Color(0.9150943f, 0.2129464f, 0.24059f);
	}

	void Update()
	{
		txt_username.gameObject.SetActive(db_currently_signed_in);
		txt_username.text = db_curr_username;
	}
}
