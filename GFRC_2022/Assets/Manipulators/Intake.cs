using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intake : MonoBehaviour
{
	public Vector3          dims             = new Vector3(0.3f, 0.15f, 0.05f);
	public CargoContainer[] cargo_containers = null;

	void OnValidate()
	{
		dims.x = Mathf.Max(dims.x, 0.01f);
		dims.y = Mathf.Max(dims.y, 0.01f);
		dims.z = Mathf.Max(dims.z, 0.01f);
		transform.Find("Cube").localScale = dims;
		GetComponent<BoxCollider>().size  = dims;
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.CompareTag("BlueCargo") || collider.gameObject.CompareTag("RedCargo"))
		{
			foreach (var container in cargo_containers)
			{
				if (container.try_loading(collider.gameObject))
				{
					break;
				}
			}
		}
	}
}
