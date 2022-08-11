using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class Cargo : MonoBehaviour
{
	public Transform reset_point;
	bool resetting;

	void OnTriggerEnter(Collider floor)
	{
		if (floor.CompareTag("Floor"))
		{
			resetting                              = true;
			GetComponent<SphereCollider>().enabled = false;
			GetComponent<Rigidbody>().isKinematic  = true;
		}
	}

	void Update()
	{
		if (resetting)
		{
			transform.position = dampen(transform.position, reset_point.position, 0.01f);
			if (Vector3.Distance(transform.position, reset_point.position) < 0.1f)
			{
				resetting                              = false;
				GetComponent<SphereCollider>().enabled = true;
				GetComponent<Rigidbody>().isKinematic  = false;
			}
		}
	}
}
