using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangar : MonoBehaviour
{
	public Railing[] railings_from_lowest_to_highest;

	public int calc_score()
	{
		if (railings_from_lowest_to_highest[0].robotHanging)
		{
			return 4;
		}
		else if (railings_from_lowest_to_highest[1].robotHanging)
		{
			return 6;
		}
		else if (railings_from_lowest_to_highest[2].robotHanging)
		{
			return 10;
		}
		else if (railings_from_lowest_to_highest[3].robotHanging)
		{
			return 15;
		}
		else
		{
			return 0;
		}
	}
}
