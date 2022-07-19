using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	public Hub    hub_top;
	public Hub    hub_bot;
	public Hangar hangar_blue;
	public Hangar hangar_red;

	void Update()
	{
		Debug.Log
		(
			"\nRed  hangar  : " + hangar_red.calc_score()
		);
	}
}
