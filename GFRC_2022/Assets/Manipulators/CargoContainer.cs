using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class CargoContainer : MonoBehaviour
{
	public Material mat;
	public Material mat_highlighted;

	[HideInInspector] public GameObject cargo;

	Vector3 cargo_delta_pos;

	public void set_highlight(bool condition)
	{
		if (condition)
		{
			GetComponent<MeshRenderer>().material = mat_highlighted;
		}
		else
		{
			GetComponent<MeshRenderer>().material = mat;
		}
	}

	public bool try_loading(Intake intake)
	{
		if (cargo || !intake.cargo)
		{
			return false;
		}
		else
		{
			cargo                                        = intake.cargo;
			cargo_delta_pos                              = cargo.transform.position - transform.position;
			cargo.GetComponent<SphereCollider>().enabled = false;
			cargo.GetComponent<Rigidbody>().isKinematic  = true;
			intake.cargo                                 = null;
			return true;
		}
	}

	public bool try_loading(GameObject new_cargo)
	{
		if (cargo)
		{
			return false;
		}
		else
		{
			cargo                                        = new_cargo;
			cargo_delta_pos                              = cargo.transform.position - transform.position;
			cargo.GetComponent<SphereCollider>().enabled = false;
			cargo.GetComponent<Rigidbody>().isKinematic  = true;
			return true;
		}
	}

	public GameObject try_unloading(bool reenable_physics)
	{
		if (cargo && reenable_physics)
		{
			cargo.GetComponent<SphereCollider>().enabled = true;
			cargo.GetComponent<Rigidbody>().isKinematic  = false;
		}
		GameObject obj = cargo;
		cargo = null;
		return obj;
	}

	void Update()
	{
		if (cargo)
		{
			cargo_delta_pos          = dampen(cargo_delta_pos, new Vector3(0.0f, 0.0f, 0.0f), 0.01f);
			cargo.transform.position = transform.position + transform.up * cargo.transform.localScale.y / 2.0f + cargo_delta_pos;
		}
	}
}
