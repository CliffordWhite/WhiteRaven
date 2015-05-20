using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {

    public float Xposition;
    public float YPosition;
    public float Zposition;
    Vector3 OldPosition;
    public bool Hasmoved;
    public GameObject LeverObject;
	// Use this for initialization
	void Start () {
        Hasmoved = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void FixedUpdate()
    {
        
    }


        public void HasMoved()
    {
        if (!Hasmoved)
        {
            OldPosition = LeverObject.transform.position;
        }
        
            Hasmoved = !Hasmoved;
            if (Hasmoved)
        {
            LeverObject.transform.position = new Vector3(Xposition, YPosition, Zposition);
        }
        else
        {
            LeverObject.transform.position = OldPosition;
        }
    }
}
