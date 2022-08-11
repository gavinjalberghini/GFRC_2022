using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class Grapple : MonoBehaviour
{
	public enum GrappleState
	{
		ready,
		thrown,
		hooked,
		resetting
	};

	public GameObject hook;
	public Rigidbody  anchor;
	public float      length_max;
	public float      strength;
	[HideInInspector] public GrappleState state;

	float        thrown_time;
	Vector3      reset_delta_pos;
	SpringJoint  joint;

	public bool shoot()
	{
		if (state == GrappleState.ready)
		{
			GetComponent<AudioManager>().Sound("Shoot");
			state = GrappleState.thrown;
			hook.GetComponent<SphereCollider>().enabled = true;
			hook.GetComponent<Rigidbody>().isKinematic  = false;
			hook.GetComponent<Rigidbody>().velocity     = new Vector3(0.0f, 0.0f, 0.0f);
			hook.GetComponent<Rigidbody>().AddForce(transform.forward * 8.0f, ForceMode.Impulse);
			thrown_time = 0.0f;
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool release()
	{
		if (state == GrappleState.hooked)
		{
			reset();
			return true;
		}
		else
		{
			return false;
		}
	}

	public void toggle()
	{
		if (state == GrappleState.ready)
		{
			shoot();
		}
		else if (state == GrappleState.hooked)
		{
			release();
		}
	}

	void reset()
	{
		if (state == GrappleState.hooked)
		{
			hook.GetComponent<Hook>().contact.GetComponent<Railing>().isHooked = false;
			hook.GetComponent<Hook>().contact = null;
		}
		state                                       = GrappleState.resetting;
		hook.GetComponent<SphereCollider>().enabled = false;
		hook.GetComponent<Rigidbody>().isKinematic  = true;
		reset_delta_pos = hook.transform.position - transform.position;

		if (joint)
		{
			Destroy(joint);
		}
	}

	void OnValidate()
	{
		Update();
	}

	void Awake()
	{
		OnValidate();
	}

	void Update()
	{
		switch (state)
		{
			case GrappleState.ready:
			{
				hook.transform.position = transform.position;
			} break;

			case GrappleState.thrown:
			{
				thrown_time += Time.deltaTime;

				if (thrown_time < 1.0f && Vector3.Distance(transform.position, hook.transform.position) < 3.0f)
				{
					if
					(
						hook.GetComponent<Hook>().contact &&
						(
							hook.GetComponent<Hook>().contact.CompareTag("BlueHangar0") ||
							hook.GetComponent<Hook>().contact.CompareTag("BlueHangar1") ||
							hook.GetComponent<Hook>().contact.CompareTag("BlueHangar2") ||
							hook.GetComponent<Hook>().contact.CompareTag("BlueHangar3") ||
							hook.GetComponent<Hook>().contact.CompareTag( "RedHangar0") ||
							hook.GetComponent<Hook>().contact.CompareTag( "RedHangar1") ||
							hook.GetComponent<Hook>().contact.CompareTag( "RedHangar2") ||
							hook.GetComponent<Hook>().contact.CompareTag( "RedHangar3")
						)
					)
					{
						hook.GetComponent<Hook>().contact.GetComponent<Railing>().isHooked = true;
						GetComponent<AudioManager>().Sound("Click");
						state                                       = GrappleState.hooked;
						hook.GetComponent<SphereCollider>().enabled = false;
						hook.GetComponent<Rigidbody>().isKinematic  = true;
						joint                                       = anchor.gameObject.AddComponent<SpringJoint>();
						joint.autoConfigureConnectedAnchor          = false;
						joint.connectedAnchor                       = hook.GetComponent<Hook>().contact_pos;
						joint.spring                                = strength;
						joint.damper                                = 7.5f;
						joint.massScale                             = 4.5f;
						joint.minDistance                           = 0.0f;
						joint.maxDistance                           =
						length_max                                  = Mathf.Clamp(Vector3.Distance(transform.position, hook.transform.position), 0.0f, 3.0f);
					}
				}
				else
				{
					reset();
				}
			} break;

			case GrappleState.hooked:
			{
				joint.maxDistance =
				length_max        = Mathf.Clamp(length_max, 0.0f, 3.0f);
				hook.transform.position = hook.GetComponent<Hook>().contact_pos;
			} break;

			case GrappleState.resetting:
			{
				reset_delta_pos = dampen(reset_delta_pos, new Vector3(0.0f, 0.0f, 0.0f), 0.0001f);

				if (reset_delta_pos.magnitude > 0.001f)
				{
					hook.transform.position = transform.position + reset_delta_pos;
				}
				else
				{
					reset_delta_pos = new Vector3(0.0f, 0.0f, 0.0f);
					hook.transform.position = transform.position;
					state = GrappleState.ready;
				}
			} break;
		}

		GetComponent<LineRenderer>().SetPosition(0, transform.position);
		GetComponent<LineRenderer>().SetPosition(1, hook.transform.position);
	}
}
