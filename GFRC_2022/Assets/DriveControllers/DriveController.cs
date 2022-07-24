using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global;

public abstract class DriveController : MonoBehaviour
{
	public abstract void control(Vector2 translation, float steering);
}

