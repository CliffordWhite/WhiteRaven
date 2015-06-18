using UnityEngine;
using System.Collections;

public class specialwall : MonoBehaviour {

	public GameObject[] levers;
	public int limit;
	public Vector3 tarPos;
	// Update is called once per frame
	void Update () {
		if (limit < 5) {
			limit = 0;
			for (int i = 0; i < levers.Length; i++) {
				limit += levers [i].GetComponent<Lever> ().hastriggered;
			}
		} else
			transform.position = Vector3.MoveTowards (transform.position, tarPos, 0.25f);
	}
}
