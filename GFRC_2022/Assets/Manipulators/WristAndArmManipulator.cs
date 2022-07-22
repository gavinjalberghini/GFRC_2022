using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristAndArmManipulator : PrimaryManipulator
{
	public OmniArm omniarm_lower;
	public OmniArm omniarm_upper;
	public Claw    claw;
	[HideInInspector] public bool using_upper;
}

