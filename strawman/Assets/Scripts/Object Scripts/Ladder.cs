using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {
	public float climbSpeed;
	public GameObject target;
	// Update is called once per frame
	void OnTriggerStay(Collider other){
		if (Input.GetKeyDown(KeyCode.W)) {
			target.transform.position = new Vector3(target.transform.position.x,
			                                        target.transform.position.y + (climbSpeed*Time.deltaTime));
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			target.transform.position = new Vector3(target.transform.position.x,
			                                        target.transform.position.y - (climbSpeed*Time.deltaTime));
		}
	}
}
