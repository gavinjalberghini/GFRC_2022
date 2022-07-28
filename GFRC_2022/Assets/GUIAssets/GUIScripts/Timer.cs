using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Global;

public class Timer : MonoBehaviour
{
    public float gameTime = 150f; //amount of time in seconds
    public bool isTimerStarted = false;

    public bool timerFinished;
    private bool prompt;

    public float promptTime = 10f; //also in seconds
    public Text promptCountDown;
    private float i; //prompttimer

    public GameObject StartScreen;
    public GameObject PromptScreen;
    public GameObject DisplayTime;

    [HideInInspector]
    public string m, s;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Sound("Null");
        Welcome();
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
                FindObjectOfType<AudioManager>().Sound("Shoot");

            }
            else if (key_now_down(Key.Backspace))
            {
                Welcome();
            }
        }
        else if (isTimerStarted)
            Countdown();

        if (key_now_down(Key.Enter) && !isTimerStarted)
        {
            Prompt();
            FindObjectOfType<AudioManager>().Sound("Beep");
        }

        //float mm, ss;
        if (gameTime >= 60)
        {
            m = Mathf.Floor(gameTime / 60).ToString();
            //mm = float.Parse(m);
            s = Mathf.Floor(gameTime % 60).ToString();
            //ss = float.Parse(s);
        }
        else
        {
            m = "0";
            s = Mathf.Floor(gameTime).ToString();
        }
        timerFinished = gameTime <= 0f;
    }

    void Countdown()
    {
        gameTime -= Time.deltaTime;
        isTimerStarted = true;
    }

    void Welcome()
    {
        i = 10f;
        prompt = false;
        StartScreen.SetActive(true);
        PromptScreen.SetActive(false);
    }

    void Prompt() 
    {
        StartScreen.SetActive(false);
        PromptScreen.SetActive(true);
        prompt = true;
        
        i -= Time.deltaTime;
        promptCountDown.text = i.ToString("0");
        if(i < 0f)
            Begin();
    }

    void Begin()
    {
        PromptScreen.SetActive(false);
        prompt = false;
        Countdown();
    }
}
