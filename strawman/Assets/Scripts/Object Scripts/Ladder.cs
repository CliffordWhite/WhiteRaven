﻿using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {
	public float speed;
	public GameObject playerObject;
	// Update is called once per frame
	void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Player")
        {
			playerObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
			playerObject.GetComponent<Rigidbody> ().useGravity = false;
		}
	}

	void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "Player")
        {
			playerObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
            if(!playerObject.GetComponent<PlayerController>().FlyModeOn)
			    playerObject.GetComponent<Rigidbody> ().useGravity = true;
            playerObject.GetComponent<PlayerController>().OnLadder = false;

		}
	}

	void OnTriggerStay(Collider other){
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.S))
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

			//Bug Fix #7//////////////////////
			//Check if attached to hookable/// 
			//object before setting climb/////
			//animation///////////////////////
			//////////////////////////////////
			if (!playerObject.GetComponent<PlayerController>().isGrappled) {
            	playerObject.GetComponent<PlayerController>().anim.Play("Climb");
			}
			//End Bug Fix #7//////////////////
            playerObject.GetComponent<PlayerController>().OnLadder = true;
		}
	}

}
