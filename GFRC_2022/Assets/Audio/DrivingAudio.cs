using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingAudio : MonoBehaviour
{
    public bool IsDriving;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IsDriving = gameObject.GetComponent<TankDrive>().IsDriving;
        if (!IsDriving) { GetComponent<AudioSource>().Stop(); }
        if (IsDriving && GetComponent<AudioSource>().isPlaying == false)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
