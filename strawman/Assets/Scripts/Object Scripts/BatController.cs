using UnityEngine;
using System.Collections;

public class BatController : MonoBehaviour {
    //BoxCollider's for sprites
    public BoxCollider WingsOut;
    public BoxCollider WingsIn;
    //sprites
    public Sprite WingsOutSprite;
    public Sprite WingsInSprite;
    //Waypoints to move between
    public Transform[] waypoints;
    private int wpIndex = 0;
    private Vector3 startPos, endPos;
    //Values
    public float stayTime = 1.0f;
    public float idleTimer = 0.0f;
    public float speed = 2.0f;
    private float startTime;
    public float FlapWings;
    public bool WingsAreIn;



	// Use this for initialization
	void Start () {
        startPos = transform.position = waypoints[wpIndex].position;
        GetComponent<SpriteRenderer>().sprite = WingsOutSprite;
        WingsAreIn = false;
        FlapWings = 0.0f;
        WingsOut.GetComponent<BoxCollider>().enabled = true;
        WingsIn.GetComponent<BoxCollider>().enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
        if (idleTimer <= stayTime)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > stayTime)
            {
                Flap();
                startTime = Time.time;
                wpIndex++;
                if (wpIndex == waypoints.Length)
                    wpIndex = 0;
                endPos = waypoints[wpIndex].position;
            }
        }
        else
        {
            float i = (Time.time - startTime) / speed;
            transform.position = Vector3.Lerp(startPos, endPos, i);
            if (i >= 1)
            {
                Flap();
                idleTimer = 0.0f;
                startPos = endPos;
            }
        }
	}
    void Flap()
    {

        if (WingsAreIn)
        {
            WingsAreIn = false;
            WingsOut.GetComponent<BoxCollider>().enabled = true;
            WingsIn.GetComponent<BoxCollider>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = WingsOutSprite;

        }
        else
        {
            WingsAreIn = true;
            WingsOut.GetComponent<BoxCollider>().enabled = false;
            WingsIn.GetComponent<BoxCollider>().enabled = true;
            GetComponent<SpriteRenderer>().sprite = WingsInSprite;

        }
    }
}
