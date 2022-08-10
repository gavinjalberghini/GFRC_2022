using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Global;

public class Timer : MonoBehaviour
{
	private float gameTime = 150f; //amount of time in seconds
	public float gameStartTime = 150f;
	public bool isTimerStarted = false;

	public bool timerFinished;
	public bool justFinished;
	private bool prompt = false;
	private bool end = false;

	public float promptTime = 10f; //also in seconds
	public Text promptCountDown;
	public Text endCountDown;
	private float i; //prompttimer
	private float e; //endtimer
	private float j = 0f; //warntimer

	public GameObject StartScreen;
	public GameObject PromptScreen;
	public GameObject DisplayTime;
	public GameObject EndScreen;

	[HideInInspector]
	public float m, s;

	// Start is called before the first frame update
	void Start()
	{
		gameTime = gameStartTime;
		FindObjectOfType<AudioManager>().Sound("Music");
		EndScreen.SetActive(false);
		OpenScreen(StartScreen, PromptScreen, false, false);
	}

	// Update is called once per frame
	void Update()
	{
		if (prompt)
		{
			Prompt();
			if (key_now_down(Key.Enter))
			{
				Begin();

			}
			else if (key_now_down(Key.Backspace))
			{
				OpenScreen(StartScreen, PromptScreen, false, false);
			}
		}
		else if (isTimerStarted && !timerFinished)
			Countdown();

		if (key_now_down(Key.Enter) && !isTimerStarted)
		{
			OpenScreen(PromptScreen, StartScreen, true, false);
			FindObjectOfType<AudioManager>().Sound("Beep");
		}

		m = Mathf.Floor(gameTime / 60.0f);
		s = Mathf.Max(Mathf.Floor(gameTime % 60.0f), 0.0f);

		if (end)
		{
			Return();
			if (key_now_down(Key.Enter))
			{
				FindObjectOfType<AudioManager>().Sound("Shoot");
				SceneManager.LoadScene("Scenes/Title Menu Scene");
			}
			else if (key_now_down(Key.Backspace))
			{
				SceneManager.LoadScene("Scenes/BuildAndPlay");
			}
		}

		bool oldTimerFinished = timerFinished;
		timerFinished = gameTime <= 0f;
		justFinished = !oldTimerFinished && timerFinished;
		if (justFinished)
		{
			OpenScreen(EndScreen, StartScreen, false, true);
			FindObjectOfType<AudioManager>().Sound("Air Horn");
		}
	}

	void Countdown()
	{
		gameTime -= Time.deltaTime;
		isTimerStarted = true;
		if (gameTime < 6f && gameTime >= 0f)
		{
			j += Time.deltaTime;
			if (j >= 1)
			{
				j = 0;
				FindObjectOfType<AudioManager>().Sound("Time");
			}
		}
	}

	void OpenScreen(GameObject Screen, GameObject ScreenToClose, bool isprompt, bool isend)
	{
		if(isprompt)
			i = 10f;

		if (isend)
			e = 10f;

		prompt = isprompt;
		end = isend;
		Screen.SetActive(true);
		ScreenToClose.SetActive(false);
	}

	void Prompt()
	{
		i -= Time.deltaTime;
		promptCountDown.text = i.ToString("0");
		if(i < 0f)
			Begin();
	}

	void Return()
	{
		e -= Time.deltaTime;
		endCountDown.text = e.ToString("0");
		if (e < 0f)
			SceneManager.LoadScene("Scenes/Title Menu Scene");
	}

	void Begin()
	{
		PromptScreen.SetActive(false);
		prompt = false;
		FindObjectOfType<AudioManager>().Sound("Shoot");
		Countdown();
	}
}
