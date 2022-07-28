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
        Time.text = FindObjectOfType<Timer>().m + ":" + FindObjectOfType<Timer>().s;
    }
}
