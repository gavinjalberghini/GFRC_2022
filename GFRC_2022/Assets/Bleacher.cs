using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleacher : MonoBehaviour
{
	public Material[] mats;

	void Start()
	{
		foreach (Transform t in transform.Find("Audience"))
		{
			Material mat = mats[Random.Range(0, mats.Length)];
			foreach (Transform u in t)
			{
				u.GetComponent<MeshRenderer>().material = mat;
			}
		}
	}
}
