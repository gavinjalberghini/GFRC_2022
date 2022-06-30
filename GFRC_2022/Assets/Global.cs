using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Global
{
	// @NOTE@ Framrate independent dampening. Use this to gradually move one value to other smoothly over each update tick.
	// `friction` of 0.0f will cause an instanteous transition from `start` to `end`.
	// `friction` of 0.5f will close the gap between `start` and `end` by half after one second.
	// `friction` of 1.0f will always make the value stay at `start`, e.g. infinite friction.
	static public float   dampen(float   start, float   end, float friction) { return end + (start - end) * Mathf.Pow(friction, Time.deltaTime); }
	static public Vector2 dampen(Vector2 start, Vector2 end, float friction) { return end + (start - end) * Mathf.Pow(friction, Time.deltaTime); }
}
