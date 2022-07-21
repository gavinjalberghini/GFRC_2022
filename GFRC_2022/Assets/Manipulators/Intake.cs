using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intake : TertiaryManipulator
{
	public CargoContainer[] cargo_containers;

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

