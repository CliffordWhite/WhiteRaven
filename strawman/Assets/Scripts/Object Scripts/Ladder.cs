using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {
	public float speed;
	public GameObject playerObject;
	// Update is called once per frame
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player")
			playerObject.GetComponent<Rigidbody> ().useGravity = false;
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player")
			playerObject.GetComponent<Rigidbody> ().useGravity = true;
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.tag == "Player") {
			if (Input.GetKeyDown (KeyCode.A))
				playerObject.transform.position = new Vector3 (playerObject.transform.position.x - speed * Time.deltaTime,
			                                               playerObject.transform.position.y,
			                                               playerObject.transform.position.z);
			else if (Input.GetKeyDown (KeyCode.D))
				playerObject.transform.position = new Vector3 (playerObject.transform.position.x + speed * Time.deltaTime,
			                                               playerObject.transform.position.y,
			                                               playerObject.transform.position.z);
			else if (Input.GetKeyDown (KeyCode.W))
				playerObject.transform.position = new Vector3 (playerObject.transform.position.x,
			                                               playerObject.transform.position.y + speed * Time.deltaTime,
			                                               playerObject.transform.position.z);
			else if (Input.GetKeyDown (KeyCode.S))
				playerObject.transform.position = new Vector3 (playerObject.transform.position.x,
			                                               playerObject.transform.position.y - (speed * Time.deltaTime),
			                                               playerObject.transform.position.z);
			else
				playerObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		}
	}

}
