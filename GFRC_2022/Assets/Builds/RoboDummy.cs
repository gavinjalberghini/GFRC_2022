using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboDummy : MonoBehaviour
{
	public float moveSpeed;
	public float rotSpeed;

	private bool isWandering;
	private bool isRotatingleft;
	private bool isRotatingright;
	private bool isWalking;

	void Start()
	{
		
	}

	void Update()
	{
		if (!isWandering) 
		{
			GetComponent<AudioSource>().Stop();
			StartCoroutine(Wander());
		}
		if (isRotatingleft)
		{
			transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
		}
		if (isRotatingright)
		{
			transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
		}
		if (isWalking)
		{
			transform.position += transform.forward * moveSpeed * Time.deltaTime;
		}
	}

	IEnumerator Wander() 
	{
		int rotTime = Random.Range(1, 3);
		int rotateWait = Random.Range(1, 2);
		int rotateLorR = Random.Range(1, 2);
		int walkWait = Random.Range(1, 5);
		int walkTime = Random.Range(1, 6);

		isWandering = true;

		yield return new WaitForSeconds(walkWait);
		isWalking = true;
		yield return new WaitForSeconds(walkTime);
		isWalking = false;
		yield return new WaitForSeconds(rotateWait);
		if (rotateLorR == 1) 
		{
			isRotatingright = true;
			yield return new WaitForSeconds(rotTime);
			isRotatingright = false;
		}
		if (rotateLorR == 2)
		{
			isRotatingleft = true;
			yield return new WaitForSeconds(rotTime);
			isRotatingleft = false;
		}

		isWandering = false;
	}

	void OnTriggerEnter(Collider col) 
	{
		if (col.gameObject.layer == 10) 
		{
			isWalking = false;
			isRotatingright = true;
		}
	}
	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.layer == 10)
		{
			isWalking = true;
			isRotatingright = false;
		}
	}
}
