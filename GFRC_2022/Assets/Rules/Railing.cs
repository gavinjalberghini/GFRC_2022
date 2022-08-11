using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railing : MonoBehaviour
{
	public bool robotHangingRed;
	public bool robotHangingBlue;
	public bool isRed;
	public bool isHooked;

	List<GameObject> hanging_objs = new List<GameObject>(0);

	void Start()
	{
		isRed = gameObject.GetComponentInParent<Hangar>().isRed;
	}

	void OnTriggerEnter(Collider robot)
	{
		if (robot.gameObject.CompareTag("RedRobot") && isRed)
		{
			robotHangingRed = true;
			hanging_objs.Add(robot.gameObject);
		}
		else if (robot.gameObject.CompareTag("BlueRobot") && !isRed)
		{
			robotHangingBlue = true;
			hanging_objs.Add(robot.gameObject);
		}
	}

	void OnTriggerExit(Collider robot)
	{
		for (int i = 0; i < hanging_objs.Count; i += 1)
		{
			if (hanging_objs[i] == robot.gameObject)
			{
				hanging_objs.RemoveAt(i);
				if (isRed)
				{
					robotHangingRed = hanging_objs.Count != 0;
				}
				else
				{
					robotHangingBlue = hanging_objs.Count != 0;
				}
				break;
			}
		}
	}
}
