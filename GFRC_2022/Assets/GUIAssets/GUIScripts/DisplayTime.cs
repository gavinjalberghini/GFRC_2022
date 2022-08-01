using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour
{
    public Text Time;
    private string time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Timer>().s >= 10f)
            time = FindObjectOfType<Timer>().m + ":" + FindObjectOfType<Timer>().s;
        else
            time = FindObjectOfType<Timer>().m + ":0" + FindObjectOfType<Timer>().s;

        if (((FindObjectOfType<Timer>().m * 60f) + FindObjectOfType<Timer>().s) > 135f)
            Time.text = "Auto: " + time;
        else
            Time.text = "TeleOp: " + time;
    }
}
