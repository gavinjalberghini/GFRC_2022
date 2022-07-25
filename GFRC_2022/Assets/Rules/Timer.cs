using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Timer : MonoBehaviour
{
    public float gameTime = 150f; //amount of time in seconds
    private bool isTimerStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            Countdown();
        else if (isTimerStarted)
            Countdown();

        string m, s;
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

        print(m + " minutes");
        print(s + " seconds");
    }

    void Countdown()
    {
        gameTime -= Time.deltaTime;
        isTimerStarted = true;
    }
}
