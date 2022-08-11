using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour
{
	public int  scoreAddend;
	public bool sfxForRed;

	[HideInInspector] public int blueScore;
	[HideInInspector] public int redScore;

	void OnTriggerEnter(Collider cargo)
	{
		if (cargo.gameObject.CompareTag("BlueCargo"))
		{
			if (!sfxForRed)
			{
				GetComponent<AudioManager>().Sound("Score");
			}
			blueScore += scoreAddend;
		}
		else if (cargo.gameObject.CompareTag("RedCargo"))
		{
			if (sfxForRed)
			{
				GetComponent<AudioManager>().Sound("Score");
			}
			redScore += scoreAddend;
		}
	}
}
