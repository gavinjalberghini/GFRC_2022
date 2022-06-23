using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	if (Input.GetKey("left"))
		gameObject.transform.Translate(-1.0f * Time.deltaTime,  0.0f, 0.0f);
	if (Input.GetKey("right"))
		gameObject.transform.Translate( 1.0f * Time.deltaTime,  0.0f, 0.0f);
	if (Input.GetKey("down"))
		gameObject.transform.Translate( 0.0f, -4.0f * Time.deltaTime, 0.0f);
	if (Input.GetKey("up"))
		gameObject.transform.Translate( 0.0f,  4.0f * Time.deltaTime, 0.0f);
    }
}
