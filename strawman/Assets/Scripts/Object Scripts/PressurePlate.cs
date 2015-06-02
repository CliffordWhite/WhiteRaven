using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

	public GameObject toEffect;
	public Vector3 tarPosition;
	public float speed;
	public AudioClip clickSound;
	public AudioSource SFXSource;
	bool isTriggered;
	bool isPressured;
	bool objectMoving;

	void start()
	{
		isTriggered = false;
		isPressured = false;
		objectMoving = false;
	}

	void OnTriggerEnter()
	{
		SFXSource.PlayOneShot (clickSound, 1.0f);
		//if not triggered yet the start moving object
		if (!isTriggered) 
			objectMoving = true;
		//if not already pushed in then push in
		if (!isPressured)
			transform.position = new Vector3 (transform.position.x - (transform.up.x *.1f),
			                                  transform.position.y-(transform.up.y *.1f),
			                                  transform.position.z-(transform.up.z *.1f));

		isPressured = true;
		isTriggered = false;
	}

	void OnTriggerExit()
	{
		isPressured = false;
		//raise plate back up
		transform.position = new Vector3 (transform.position.x + (transform.up.x *.1f),
		                                  transform.position.y+(transform.up.y *.1f),
		                                  transform.position.z+(transform.up.z *.1f));
	}

	void Update()
	{


	}

	void FixedUpdate()
	{
		//if triggered start moving object to target position and when within distance of .01f set position and stop moving
		if (objectMoving) {
			toEffect.transform.position = Vector3.MoveTowards(toEffect.transform.position,tarPosition,speed*Time.deltaTime);
			if (Vector3.Distance(toEffect.transform.position,tarPosition) <=.01f) {
				toEffect.transform.position = tarPosition;
				objectMoving = false;
			}
		}
	}
}
