using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour
{
	public int autoScoreRange;
	public int scoreAddend;

	[HideInInspector] public int blueScore;
	[HideInInspector] public int redScore;

	void Start()
	{
		blueScore = Random.Range(0, autoScoreRange);
		redScore  = Random.Range(0, autoScoreRange);
	}

	void OnTriggerEnter(Collider cargo)
	{
		if (cargo.gameObject.layer == 6)
		{
			blueScore += scoreAddend;
		}
		else if (cargo.gameObject.layer == 7)
		{
			redScore += scoreAddend;
		}
	}
}
