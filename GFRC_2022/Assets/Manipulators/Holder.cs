using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Holder : MonoBehaviour
{
	GameObject cargo      = null;
	bool       is_grabbed = false;

	Vector3 delta = new Vector3(0.0f, 0.0f);

	private void OnTriggerStay(Collider collider)
	{
		if ((collider.gameObject.CompareTag("BlueCargo") || collider.gameObject.CompareTag("RedCargo")) && cargo == null)
		{
			cargo = collider.gameObject;
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if ((collider.gameObject.CompareTag("BlueCargo") || collider.gameObject.CompareTag("RedCargo")) && cargo != null && !is_grabbed)
		{
			cargo = null;
		}
	}

	void Update()
	{
		if (cargo != null)
		{
			if (is_grabbed)
			{
				delta                    = dampen(delta, new Vector3(0.0f, 0.0f, 0.0f), 0.01f);
				cargo.transform.position = transform.position + new Vector3(0.0f, cargo.transform.localScale.y, 0.0f) / 2.0f + delta;
			}

			if (key_now_down(Key.Enter) || gamepad_buttons_now_down().x == -1.0f)
			{
				if (is_grabbed)
				{
					cargo.GetComponent<SphereCollider>().enabled = true;
					cargo.GetComponent<Rigidbody>().isKinematic  = false;
					cargo                                        = null;
				}
				else
				{
					delta                                        = cargo.transform.position - transform.position;
					cargo.GetComponent<SphereCollider>().enabled = false;
					cargo.GetComponent<Rigidbody>().isKinematic  = true;
				}

				is_grabbed = !is_grabbed;
			}
		}
	}

}
