using UnityEngine;
using System.Collections;

public class BatController : MonoBehaviour 
{
	// culling unnecessary code for animations

	//BoxCollider's for sprites
//    public BoxCollider WingsOut;
//    public BoxCollider WingsIn;
    //sprites
//    public Sprite WingsOutSprite;
//    public Sprite WingsInSprite;
    //Waypoints to move between
	public Animator animate;
    public Transform[] waypoints;
    private int wpIndex = 0;
    private Vector3 startPos, endPos;
    //Values
    public float stayTime = 1.0f;
    public float idleTimer = 0.0f;
    public float speed = 2.0f;
//    public float FlapWings;
//    public bool WingsAreIn;

	float startTime;


	// Use this for initialization
	void Start () 
	{
        startPos = transform.position = waypoints[wpIndex].position;
//        GetComponent<SpriteRenderer>().sprite = WingsOutSprite;
//        WingsAreIn = false;
//        FlapWings = 0.0f;
//        WingsOut.GetComponent<BoxCollider>().enabled = true;
//        WingsIn.GetComponent<BoxCollider>().enabled = false;
		if (animate == null)
			animate = transform.GetComponentInChildren<Animator>();
		if (gameObject.tag == "HM Fatal" && !GameManager.manager.hardModeOn)
			gameObject.SetActive(false);
		

	}
	
	// Update is called once per frame
	void Update () 
	{
        if (idleTimer <= stayTime)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > stayTime)
            {
                startTime = Time.time;
                wpIndex++;
                if (wpIndex == waypoints.Length)
                    wpIndex = 0;
                endPos = waypoints[wpIndex].position;
				DirectionCheck();
            }
        }
        else
        {
            float i = (Time.time - startTime) / speed;
            transform.position = Vector3.Lerp(startPos, endPos, i);
            if (i >= 1)
            {
                idleTimer = 0.0f;
                startPos = endPos;
            }
        }
	}
//    void Flap()
//    {
//
//        if (WingsAreIn)
//        {
//            WingsAreIn = false;
//            WingsOut.GetComponent<BoxCollider>().enabled = true;
//            WingsIn.GetComponent<BoxCollider>().enabled = false;
//            GetComponent<SpriteRenderer>().sprite = WingsOutSprite;
//
//        }
//        else
//        {
//            WingsAreIn = true;
//            WingsOut.GetComponent<BoxCollider>().enabled = false;
//            WingsIn.GetComponent<BoxCollider>().enabled = true;
//            GetComponent<SpriteRenderer>().sprite = WingsInSprite;
//
//        }
//    }

	void DirectionCheck()
	{
		// get a direction vector to know which way bat is flying
		Vector2 direction = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);
		
		// play the appropriate animation for which way the bat is flying
		if (direction.x > 0.0f && direction.y < 0.01f)
			animate.Play("FlyRight");
		else if (direction.x < 0.0f && direction.y < 0.01f)
			animate.Play("FlyLeft");
		else if (direction.y > 0.0f && direction.x < 0.01f)
			animate.Play("FlyUp");
		else if (direction.y < 0.0f && direction.x < 0.01f)
			animate.Play("FlyDown");
	}

}
