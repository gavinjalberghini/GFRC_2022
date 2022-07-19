using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Hook : MonoBehaviour
{
	[HideInInspector] public GameObject contact     = null;
	[HideInInspector] public Vector3    contact_pos = new Vector3(0.0f, 0.0f, 0.0f);

	void OnCollisionEnter(Collision collision)
	{
		{
			contact     = collision.gameObject;
			contact_pos = collision.contacts[0].point;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject == contact)
		{
			contact     = null;
			contact_pos = new Vector3(0.0f, 0.0f, 0.0f);
		}
	}
}
