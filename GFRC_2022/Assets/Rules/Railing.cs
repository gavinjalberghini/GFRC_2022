using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railing : MonoBehaviour
{
	[HideInInspector] public bool robotHangingRed; // @TODO@ This doesn't say what robot is hanging, or how many. Could lead to ambigious cases.
	[HideInInspector] public bool robotHangingBlue;
	[HideInInspector] public bool isRed;

	void Start() 
	{
		isRed = gameObject.GetComponentInParent<Hangar>().isRed;
	}

	void OnTriggerEnter(Collider robot)
	{
		if (robot.gameObject.layer == 8 && isRed)
		{
			robotHangingRed = true;
		}

		if (robot.gameObject.layer == 9 && isRed)
		{
			robotHangingBlue = true;
		}
	}

	void OnTriggerExit(Collider robot)
	{
		if (robot.gameObject.layer == 8 && isRed)
		{
			robotHangingRed = false;
		}

		if (robot.gameObject.layer == 9 && isRed)
		{
			robotHangingBlue = false;
		}
	}
}
