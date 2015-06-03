using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {
	public float speed;
	public GameObject playerObject;
	// Update is called once per frame
	void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shaman")
        {
			playerObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
			playerObject.GetComponent<Rigidbody> ().useGravity = false;
		}
	}

	void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shaman")
        {
			playerObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
			playerObject.GetComponent<Rigidbody> ().useGravity = true;
		}
	}

	void OnTriggerStay(Collider other){
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shaman")
        {
			if (Input.GetKey (KeyCode.S))
				playerObject.transform.position = new Vector3 (playerObject.transform.position.x,
			                                               playerObject.transform.position.y - speed * Time.deltaTime,
			                                               playerObject.transform.position.z);
			// added for changing jump mechanic
			else if(Input.GetKey (KeyCode.W))
			{
				playerObject.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 0);

				playerObject.transform.position = new Vector3 (playerObject.transform.position.x,
				                                               playerObject.transform.position.y + speed * Time.deltaTime,
				                                               playerObject.transform.position.z);
			}
			else if(!Input.GetKey (KeyCode.A)&&!Input.GetKey (KeyCode.D)/*&&!Input.GetKey (KeyCode.W)*/)
				playerObject.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 0);
		}
	}

}
