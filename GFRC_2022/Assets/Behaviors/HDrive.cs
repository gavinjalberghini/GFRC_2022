using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class HDrive : MonoBehaviour
{
	Wheel[] wheels = new Wheel[5];

	void Start()
	{
		wheels[0] = transform.Find("Wheel BL").gameObject.GetComponent<Wheel>(); // @TODO@ Some Unity engineering to make it where adjusting the size of the base will also adjust the positions of the wheel.
		wheels[1] = transform.Find("Wheel BR").gameObject.GetComponent<Wheel>();
		wheels[2] = transform.Find("Wheel FL").gameObject.GetComponent<Wheel>();
		wheels[3] = transform.Find("Wheel FR").gameObject.GetComponent<Wheel>();
		wheels[4] = transform.Find("Wheel C" ).gameObject.GetComponent<Wheel>();
	}

	void Update()
	{
		const float GREASE = 0.000001f; // @NOTE@ How quickly the movement and steering changes.

		Vector2 movement = left_stick();
		if (movement == new Vector2(0.0f, 0.0f))
		{
			movement = wasd_normalized();
		}

		float steering = right_stick().x;
		if (steering == 0.0f)
		{
			if (Keyboard.current[Key.Q].isPressed) { steering += -1.0f; }
			if (Keyboard.current[Key.E].isPressed) { steering +=  1.0f; }
		}

		wheels[0].activation = dampen(wheels[0].activation, Mathf.Clamp(movement.y + steering, -1.0f, 1.0f), GREASE);
		wheels[1].activation = dampen(wheels[1].activation, Mathf.Clamp(movement.y - steering, -1.0f, 1.0f), GREASE);
		wheels[2].activation = dampen(wheels[2].activation, Mathf.Clamp(movement.y + steering, -1.0f, 1.0f), GREASE);
		wheels[3].activation = dampen(wheels[3].activation, Mathf.Clamp(movement.y - steering, -1.0f, 1.0f), GREASE);
		wheels[4].activation = dampen(wheels[4].activation, Mathf.Clamp(movement.x           , -1.0f, 1.0f), GREASE);
	}
}
