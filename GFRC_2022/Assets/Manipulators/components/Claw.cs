using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
	public bool try_grabbing()
	{
		return transform.Find("Cargo Container").GetComponent<CargoContainer>().try_loading(transform.Find("Intake").GetComponent<Intake>());
	}

	public bool try_releasing()
	{
		return transform.Find("Cargo Container").GetComponent<CargoContainer>().try_unloading(true);
	}

	public void toggle()
	{
		if (transform.Find("Cargo Container").GetComponent<CargoContainer>().cargo)
		{
			try_releasing();
		}
		else
		{
			try_grabbing();
		}
	}
}
