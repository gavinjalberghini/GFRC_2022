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
	public Button build_and_play;
	public Button show_leaderboard;
	public Button exit;

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
	}
}
