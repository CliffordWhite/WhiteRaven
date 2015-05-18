﻿using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {
	public GameObject target;
	public float camDist;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (target.transform.position.x,
		                                 target.transform.position.y,
		                                 target.transform.position.z-camDist);

	}
}
