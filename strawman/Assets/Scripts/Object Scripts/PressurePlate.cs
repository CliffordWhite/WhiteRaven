using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

	public GameObject toEffect;
	public Vector3 tarPosition;
	bool isTriggered;
	bool isPressured;

	void start()
	{
		isTriggered = false;
		isPressured = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (!isTriggered) 
			toEffect.transform.position = tarPosition;
		if (!isPressured)
			transform.position.Set (transform.position.x, transform.position.y - .5f, transform.position.z);
		isPressured = true;
		isTriggered = false;
	}

	void OnTriggerExit(Collider other)
	{
		isPressured = false;
		transform.position.Set (transform.position.x, transform.position.y + .5f, transform.position.z);
	}
}
