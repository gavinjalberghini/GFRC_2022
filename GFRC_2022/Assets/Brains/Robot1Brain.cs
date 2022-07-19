using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public class Robot1Brain : MonoBehaviour
{
	public HDriveController drive_controller;
	public DualCane         dual_cane;

	void Update()
	{
		{
			float qe = 0.0f;
			if (key_down(Key.Q)) { qe -= 1.0f; }
			if (key_down(Key.E)) { qe += 1.0f; }
			drive_controller.control(wasd(), qe);
		}

		dual_cane.target_height += (key_down(Key.UpArrow) ? 1.5f : -1.0f) * Time.deltaTime;
	}
}
