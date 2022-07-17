using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Hook : MonoBehaviour
{
	public GameObject contact     = null;
	public Vector3    contact_pos = new Vector3(0.0f, 0.0f, 0.0f);

	void OnCollisionEnter(Collision collision)
	{
		if
		(
			collision.gameObject.CompareTag("BlueHangar0") ||
			collision.gameObject.CompareTag("BlueHangar1") ||
			collision.gameObject.CompareTag("BlueHangar2") ||
			collision.gameObject.CompareTag("BlueHangar3") ||
			collision.gameObject.CompareTag( "RedHangar0") ||
			collision.gameObject.CompareTag( "RedHangar1") ||
			collision.gameObject.CompareTag( "RedHangar2") ||
			collision.gameObject.CompareTag( "RedHangar3")
		)
		{
			contact     = collision.gameObject;
			contact_pos = collision.contacts[0].point;
		}
	}
}
