using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public abstract class DriveController : MonoBehaviour
{
	public abstract void control(Vector2 translation, float steering);
}

