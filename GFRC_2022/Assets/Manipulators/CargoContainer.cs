using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class CargoContainer : MonoBehaviour
{
	public GameObject cargo = null;

	Vector3 delta = new Vector3(0.0f, 0.0f);

	public bool try_loading(GameObject obj)
	{
		if (cargo == null)
		{
			cargo                                        = obj;
			delta                                        = cargo.transform.position - transform.position;
			cargo.GetComponent<SphereCollider>().enabled = false;
			cargo.GetComponent<Rigidbody>().isKinematic  = true;
			return true;
		}
		else
		{
			return false;
		}
	}

	public GameObject try_unloading()
	{
		if (cargo == null)
		{
			return null;
		}
		else
		{
			cargo.GetComponent<SphereCollider>().enabled = true;
			cargo.GetComponent<Rigidbody>().isKinematic  = false;
			GameObject obj = cargo;
			cargo = null;
			return obj;
		}
	}

	void Update()
	{
		if (cargo != null)
		{
			delta                    = dampen(delta, new Vector3(0.0f, 0.0f, 0.0f), 0.01f);
			cargo.transform.position = transform.position + new Vector3(0.0f, cargo.transform.localScale.y, 0.0f) / 2.0f + delta;
		}
	}
}
