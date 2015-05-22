using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {

	public Vector3 tarPosition;
    Vector3 OldPosition;
    public bool Hasmoved;
    public GameObject LeverObject;
	bool movingUp,movingDown;
	public float speed;
	// Use this for initialization
	void Start () {
		movingUp = false;
		movingDown = false;
		Hasmoved = false;
		OldPosition = LeverObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (movingUp) {
			LeverObject.transform.position = Vector3.MoveTowards(LeverObject.transform.position,tarPosition,speed*Time.deltaTime);
			if (Vector3.Distance(LeverObject.transform.position,tarPosition) <=.01f) {
				LeverObject.transform.position = tarPosition;
				movingUp = false;
			}
		}
		else if (movingDown) {
			LeverObject.transform.position = Vector3.MoveTowards(LeverObject.transform.position,OldPosition,speed*Time.deltaTime);
			if (Vector3.Distance(LeverObject.transform.position,OldPosition) <=.01f) {
				LeverObject.transform.position = OldPosition;
				movingDown = false;
			}
		}
	}
    void FixedUpdate()
    {
        
    }


    public void HasMoved()
    {        
		if (!Hasmoved) {
			Hasmoved = !Hasmoved;
			movingUp = true;
			movingDown = false;
		}
		else {
			Hasmoved = !Hasmoved;
			movingUp = false;
			movingDown = true;
		}
    }
}
