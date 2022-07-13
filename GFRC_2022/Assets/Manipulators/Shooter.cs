using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Shooter : MonoBehaviour
{
	public float      angle           = 45.0f;
	public bool       fix             = false;
	public float      angle_speed     = 90.0f;
	public float      angle_dampening = 0.01f;
	public float      shoot_force     = 256.0f;
	public GameObject ammo            = null;
	public Transform  cannon          = null;

	float dampen_angle = 0.0f;

	void OnValidate()
	{
		angle                = Mathf.Clamp(angle, 0.0f, 90.0f);
		dampen_angle         = angle;
		cannon.localRotation = Quaternion.Euler(-angle, 0.0f, 0.0f);
	}

	void OnStart()
	{
		dampen_angle = angle;
	}

	void Update()
	{
		if (!fix)
		{
			if (key_down(Key.J))
			{
				angle -= angle_speed * Time.deltaTime;
			}
			if (key_down(Key.K))
			{
				angle += angle_speed * Time.deltaTime;
			}
		}
		angle = Mathf.Clamp(angle, 0.0f, 90.0f);


		if (key_now_down(Key.Space))
		{
			GameObject bullet = Instantiate(ammo, cannon.position + cannon.forward * cannon.localScale.z / 2.0f, cannon.rotation);
			bullet.GetComponent<Rigidbody>().AddForce(cannon.forward * shoot_force);
		}

		dampen_angle = dampen(dampen_angle, angle, angle_dampening);
		cannon.localRotation = Quaternion.Euler(-dampen_angle, 0.0f, 0.0f);

	}
}
