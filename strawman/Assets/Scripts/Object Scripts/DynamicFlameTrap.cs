using UnityEngine;
using System.Collections;

public class DynamicFlameTrap : MonoBehaviour {

	public GameObject fire;
	public float startDelay;
	public float burnTime;
	public float pauseTime;
	float currTimer;
	bool burn,start;

	// Use this for initialization
	void Start () {
		currTimer = 0.0f;
		burn = start = false;
	}
	
	// Update is called once per frame
	void Update () {
		//check if trap has waited for start delay
		if (start) {
			//if delay reached and burn is active then increment and check
			if (burn) {
				if (!fire.activeSelf) {
					fire.SetActive(true);
				}
				currTimer+=Time.deltaTime;
				if (currTimer >= burnTime) {
					//if trap has burned for total burn time reset timer and switch to pause
					burn = false;
					currTimer = 0.0f;
					fire.SetActive(false);
				}
			}
			//check and increment against pause time turning on burn if pause time up
			else {
				currTimer+=Time.deltaTime;
				if (currTimer >= pauseTime) {
					burn = true;
					currTimer = 0.0f;
				}
			}
		}
		//if delay not yet reached increment and check
		else {
			currTimer+=Time.deltaTime;
			if (currTimer >= startDelay) {
				start = true;
				currTimer = 0.0f;
			}
		}
	}
}
