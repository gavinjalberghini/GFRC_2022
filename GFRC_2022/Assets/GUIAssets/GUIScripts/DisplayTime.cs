using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour
{
    public Text Time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Timer>().s >= 10f)
            Time.text = FindObjectOfType<Timer>().m + ":" + FindObjectOfType<Timer>().s;
        else
            Time.text = FindObjectOfType<Timer>().m + ":0" + FindObjectOfType<Timer>().s;
    }
}
