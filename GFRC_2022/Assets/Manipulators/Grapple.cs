using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Grapple : MonoBehaviour
{
	public GameObject body             = null;
	public float      length           = 0.25f;
	public bool       fix_pitch        = false;
	public float      pitch            = 45.0f;
	public float      pitch_min        = 10.0f;
	public float      pitch_max        = 80.0f;
	public float      pitch_speed      = 90.0f;
	public float      pitch_dampening  = 0.01f;
	public bool       fix_yaw          = false;
	public float      yaw              = 0.0f;
	public float      yaw_range        = 360.0f;
	public float      yaw_speed        = 180.0f;
	public float      yaw_dampening    = 0.001f;
	public float      shoot_force      = 512.0f;

	enum HookState
	{
		ready,
		thrown,
		hooked,
		resetting
	};

	float       dampen_pitch = 0.0f;
	float       dampen_yaw   = 0.0f;
	HookState   hook_state   = HookState.ready;
	float       hook_time    = 0.0f;
	Vector3     hook_offset  = new Vector3(0.0f, 0.0f, 0.0f);
	SpringJoint joint        = null;

	public Vector3        cannon_direction() => transform.Find("Cannon").forward;
	public Vector3        cannon_end      () => transform.position + transform.Find("Cannon").forward * length;
	public Transform      hook            () => transform.Find("Hook");
	public Rigidbody      hook_rigidbody  () => transform.Find("Hook").GetComponent<Rigidbody>();
	public SphereCollider hook_collider   () => transform.Find("Hook").GetComponent<SphereCollider>();

	void OnValidate()
	{
		length                                                  = Mathf.Clamp(length, 0.1f, 1.0f);
		pitch_min                                               = Mathf.Min(pitch_min, pitch_max);
		pitch_max                                               = Mathf.Max(pitch_max, pitch_min);
		pitch                                                   = Mathf.Clamp(pitch, pitch_min, pitch_max);
		yaw                                                     = Mathf.Clamp(mod(yaw + 180.0f, 360.0f) - 180.0f, -yaw_range / 2.0f, yaw_range / 2.0f);
		yaw_range                                               = Mathf.Clamp(yaw_range, 0.0f, 360.0f);
		dampen_pitch                                            = pitch;
		dampen_yaw                                              = yaw;
		transform.Find("Cannon").localRotation                  = Quaternion.Euler(-pitch, yaw, 0.0f);
		transform.Find("Cannon").Find("Cylinder").localScale    = new Vector3(transform.Find("Cannon").Find("Cylinder").localScale.x, length / 2.0f, transform.Find("Cannon").Find("Cylinder").localScale.z);
		transform.Find("Cannon").Find("Cylinder").localPosition = new Vector3(0.0f, 0.0f, length / 2.0f);
		hook().position                                         = cannon_end();
	}

	void Update()
	{
		Vector2 angle_delta = dpad();
		if (angle_delta == new Vector2(0.0f, 0.0f))
		{
			angle_delta = arrow_keys();
		}

		if (!fix_pitch)
		{
			pitch = Mathf.Clamp(pitch + angle_delta.y * pitch_speed * Time.deltaTime, pitch_min, pitch_max);
		}
		if (!fix_yaw)
		{
			yaw = Mathf.Clamp(mod(yaw + angle_delta.x * yaw_speed * Time.deltaTime + 180.0f, 360.0f) - 180.0f, -yaw_range / 2.0f, yaw_range / 2.0f);
		}

		switch (hook_state)
		{
			case HookState.ready:
			{
				if (key_now_down(Key.Space) || gamepad_buttons_now_down().y == -1.0f)
				{
					hook_state                   = HookState.thrown;
					hook_rigidbody().isKinematic = false;
					hook_collider().enabled      = true;
					hook_rigidbody().AddForce(cannon_direction() * shoot_force);
				}
				else
				{
					hook_rigidbody().isKinematic = true;
					hook_collider().enabled      = false;
					hook_offset                  = dampen(hook_offset, new Vector3(0.0f, 0.0f, 0.0f), 0.000001f);
					hook().position              = cannon_end() + hook_offset;
				}
			} break;

			case HookState.thrown:
			{
				if (hook().GetComponent<Hook>().contact)
				{
					hook_state = HookState.hooked;
					hook_time  = 0.0f;

					joint                              = body.AddComponent<SpringJoint>();
					joint.autoConfigureConnectedAnchor = false;
					joint.connectedAnchor              = hook().GetComponent<Hook>().contact_pos;

					joint.spring    = 400.0f;
					joint.damper    = 7.5f;
					joint.massScale = 4.5f;

					joint.maxDistance = 3.0f;
					joint.minDistance = 0.1f;
				}
				else
				{
					hook_time += Time.deltaTime;
					if (Vector3.Distance(hook().position, cannon_end()) > 3.0f || hook_time > 1.0f)
					{
						hook_state  = HookState.resetting;
						hook_offset = hook().position - cannon_end();
					}
					else
					{
						hook_rigidbody().isKinematic = false;
						hook_collider().enabled      = true;
					}
				}
			} break;

			case HookState.hooked:
			{
				if (key_down(Key.Comma))
				{
					joint.maxDistance -= 1.0f * Time.deltaTime;
				}
				if (key_down(Key.Period))
				{
					joint.maxDistance += 1.0f * Time.deltaTime;
				}
				joint.maxDistance = Mathf.Clamp(joint.maxDistance, joint.minDistance, 3.0f);

				if (key_now_down(Key.Enter))
				{
					hook_state  = HookState.resetting;
					hook_offset = hook().position - cannon_end();
					hook().GetComponent<Hook>().contact     = null;
					hook().GetComponent<Hook>().contact_pos = new Vector3(0.0f, 0.0f, 0.0f);
					Destroy(joint);
				}
				else
				{
					hook_rigidbody().isKinematic = true;
					hook_collider().enabled      = false;
					hook().position              = hook().GetComponent<Hook>().contact_pos;
				}
			} break;

			case HookState.resetting:
			{
				hook_time += Time.deltaTime;

				if (Vector3.Distance(hook().position, cannon_end()) < 0.01f || hook_time > 1.0f)
				{
					hook_state = HookState.ready;
					hook_time  = 0.0f;
				}
				else
				{
					hook_offset     = dampen(hook_offset, new Vector3(0.0f, 0.0f, 0.0f), 0.001f);
					hook().position = cannon_end() + hook_offset;
				}
			} break;
		}

		dampen_pitch                           = dampen(dampen_pitch, pitch, pitch_dampening);
		dampen_yaw                             = dampen_angle(dampen_yaw, yaw, yaw_dampening);
		transform.Find("Cannon").localRotation = Quaternion.Euler(-dampen_pitch, dampen_yaw, 0.0f);

		GetComponent<LineRenderer>().SetPosition(0, cannon_end());
		GetComponent<LineRenderer>().SetPosition(1, hook().position);
	}
}
