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
	public TMP_InputField fld_team_number;
	public TMP_InputField fld_team_name;

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

		btn_sign_up.onClick.AddListener(delegate {
			print("sign up | " + fld_username.text + " " + fld_pin.text + " " + fld_team_number.text + " " + fld_team_name.text);
		});

		btn_log_in.onClick.AddListener(delegate {
			print("log in");
		});
	}
}
