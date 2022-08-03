using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangar : MonoBehaviour
{
	public Railing[] railings_from_lowest_to_highest;
	public bool isRed;

	public int calc_score()
	{
		if (isRed)
		{
			if (railings_from_lowest_to_highest[0].robotHangingRed)
			{
				return 4;
			}
			else if (railings_from_lowest_to_highest[1].robotHangingRed)
			{
				return 6;
			}
			else if (railings_from_lowest_to_highest[2].robotHangingRed)
			{
				return 10;
			}
			else if (railings_from_lowest_to_highest[3].robotHangingRed)
			{
				return 15;
			}
			else
			{
				return 0;
			}
		}
		else
		{
			if (railings_from_lowest_to_highest[0].robotHangingBlue)
			{
				return 4;
			}
			else if (railings_from_lowest_to_highest[1].robotHangingBlue)
			{
				return 6;
			}
			else if (railings_from_lowest_to_highest[2].robotHangingBlue)
			{
				return 10;
			}
			else if (railings_from_lowest_to_highest[3].robotHangingBlue)
			{
				return 15;
			}
			else
			{
				return 0;
			}
		}
	}
}
