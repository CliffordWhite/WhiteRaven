using UnityEngine;
using System.Collections;

public class ShamanController : MonoBehaviour {

    bool FacingRight = true;//Which way he is facing.
    //Audio
    public AudioSource FXSource;
    public AudioClip EvilLaugh;
    //Sprite flipping
    GameObject Image = null;
    //Player Detection
    RaycastHit Connected;
    Vector3 PlayerDirection;
    //Player
    GameObject Player;
    //Distance check
    public float distancecheck;
    //Bools
    public bool Awake;
    //Shaman start and end position
   // private Vector3 startPos, endPos;
    //Shaman start time
    private float startTime;
    //Shaman Speed;
    public float speed = 1.0f;
    //Waypoints to move between (Astar)
    //public Transform[] waypoints;
    //private int wpIndexStart = 0;
    //private int wpIndexUP = 1;
    //private int wpIndexDown = 2;




	// Use this for initialization
	void Start () {
        Image = transform.FindChild("ShamanSprite").gameObject;
        Player = GameObject.FindWithTag("Player");
        Awake = false;
      //  startPos = transform.position = waypoints[wpIndexStart].position;
	}
	
	// Update is called once per frame
	void Update () {

	}
    void FixedUpdate()
    {
        PlayerDirection = Player.transform.position - transform.position;
        PlayerDirection.Normalize();
        Ray LookingForPlayer = new Ray(transform.position, PlayerDirection);
        //Debug.DrawRay(transform.position, PlayerDirection * distancecheck);
        if (!Awake)
        {
            if (Physics.Raycast(LookingForPlayer, out Connected, distancecheck))
            {
                if (Connected.collider.tag == "Player")
                {
                    Awake = true;
                    FXSource.PlayOneShot(EvilLaugh, 1.0f);
                }
            }
        }
        else
        {
            startTime = Time.time;
            if (PlayerDirection.x > 0 && !FacingRight)
                Flip();
            else if (PlayerDirection.x < 0 && FacingRight)
                Flip();

            if (Physics.Raycast(LookingForPlayer, out Connected, distancecheck))
            {
                if (Connected.collider.tag == "Player")
                {
                    Chase();//chase the player down.
                }
            }
        }
    }

    void Chase() 
    {
        //float i = (Time.time - startTime) / speed;

        //if(PlayerDirection.y > 0)//player is above
        //{
        //    endPos = waypoints[wpIndexUP].position;
            
        //} 
        //else if (PlayerDirection.y < 0)//player is below
        //{
        //    endPos = waypoints[wpIndexDown].position;
        //}
        //else if (PlayerDirection.x > 0 && !FacingRight) //left
        //{
        //    //transform.GetComponent<Rigidbody>().AddForce(-125.0f, 0.0f, 0.0f, ForceMode.Acceleration);
        //    endPos = Player.transform.position;
        //}
        //else if (PlayerDirection.x < 0 && FacingRight)//Right
        //{
        //    //transform.GetComponent<Rigidbody>().AddForce(125.0f, 0.0f, 0.0f, ForceMode.Acceleration);
        //    endPos = Player.transform.position;
        //}
        //transform.position = Vector3.Lerp(startPos, endPos, i);
        //startPos = endPos;
    }

    void Flip()
    {
        Quaternion newRotation = Quaternion.identity;
        if (FacingRight)
        {
            newRotation.y = 180.0f;
            FacingRight = !FacingRight;
        }
        else
        {
            FacingRight = !FacingRight;
        }
        Image.transform.localRotation = newRotation;
    }
}
