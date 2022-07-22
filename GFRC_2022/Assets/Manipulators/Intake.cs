using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intake : SecondaryManipulator
{
	[HideInInspector] public GameObject cargo;

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.CompareTag("BlueCargo") || collider.gameObject.CompareTag("RedCargo"))
		{
			cargo = collider.gameObject;
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject == cargo)
		{
			cargo = null;
		}
	}
}

