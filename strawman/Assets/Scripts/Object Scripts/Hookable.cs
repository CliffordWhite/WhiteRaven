using UnityEngine;
using System.Collections;

public class Hookable : MonoBehaviour {

	//game objects
	private GameObject player;

	// Use this for initialization
	void Start () {
		//sets the game objects
		player = GameObject.FindWithTag("Player");

	}
	
	// Update is called once per frame
	void Update () 
	{
        //If the Left click button is released during the swing cancel the joint
        if (Input.GetMouseButtonUp(0))
        {
            GetComponent<HingeJoint>().connectedBody = null;
            player.GetComponent<Rigidbody>().freezeRotation = true;
        }
	}

}
