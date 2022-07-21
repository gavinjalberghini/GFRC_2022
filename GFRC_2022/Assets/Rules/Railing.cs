using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railing : MonoBehaviour
{
	[HideInInspector] public bool robotHanging; // @TODO@ This doesn't say what robot is hanging, or how many. Could lead to ambigious cases.

	void OnTriggerEnter(Collider robot)
	{
		if (robot.gameObject.layer == 8)
		{
			robotHanging = true;
		}
	}

	void OnTriggerExit(Collider robot)
	{
		if (robot.gameObject.layer == 8)
		{
			robotHanging = false;
		}
	}
}
