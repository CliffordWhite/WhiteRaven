using UnityEngine;
using System.Collections;

public class ShamanController : MonoBehaviour
{

//    bool FacingRight = true;//Which way he is facing.
    //Audio
    public AudioSource FXSource;
    public AudioClip EvilLaugh;
    //Sprite flipping
//    GameObject Image = null;

	// animations
	public Animator animate;
    //Player Detection
    RaycastHit Connected;
    Vector3 PlayerDirection;
    //Ladder Detection
    //RaycastHit LadderFound;
    //Vector3 LadderDirection;
    //public bool OnLadder;
    //Player
    GameObject Player;
    //Distance check
    public float distancecheck;
    //Bools
    public bool Awake;
    public bool Moving;
    //Falling vars.
    RaycastHit Grounded;
    public bool Falling;
    //Shaman Speed;
    public float speed = 1.0f;





    // Use this for initialization
    void Start()
    {
//        Image = transform.FindChild("ShamanSprite").gameObject;
		if (animate == null)
			animate = gameObject.GetComponent<Animator>();
		animate.Play("Idle");
        Player = GameObject.FindWithTag("Player");
        Awake = false;
        Moving = false;
        //OnLadder = false;
        //Falling = true;
        //  startPos = transform.position = waypoints[wpIndexStart].position;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void FixedUpdate()
    {
        PlayerDirection = Player.transform.position - transform.position;
        PlayerDirection.Normalize();
        Ray LookingForPlayer = new Ray(transform.position, PlayerDirection);
        //Debug.DrawRay(transform.position, PlayerDirection * distancecheck);
        if (!Awake) // sleep mode
        {
            if (Physics.Raycast(LookingForPlayer, out Connected, distancecheck))
            {
                if (Connected.collider.tag == "Player")
                {
                    Awake = true;
                    FXSource.PlayOneShot(EvilLaugh, 1.0f);
					animate.Play("Awake");
                }
            }
        }
       // else if (!OnLadder && !(Player.transform.position.y - transform.position.y < 0.25))
       // {
       //    FindLadder();
       // }
        else
        {

			if (Mathf.Abs(Player.transform.position.y - transform.position.y) >= distancecheck) {
				Awake = false;
				animate.Play("Idle");
				GetComponent<Rigidbody>().velocity = Vector2.zero;
			}
			else
			{
            	if (PlayerDirection.x > 0 /*&& !FacingRight*/)
					animate.Play("MoveRight");
            	else if (PlayerDirection.x < 0 /*&& FacingRight*/)
					animate.Play("MoveLeft");
				GetComponent<Rigidbody>().velocity = new Vector2(PlayerDirection.x*speed,0);
			}
		}
    }

    /*void Chase()
    {
        Ray LookingForGround = new Ray(transform.position, Vector3.down);
        if (Falling && Physics.Raycast(LookingForGround, out Grounded, 100.0f))//raycast down to see if on floor or not.
        {
            if (!(Grounded.collider.tag == "Floor") && !((transform.position - Grounded.transform.position).magnitude < 1.0f))
            {
                GetComponent<Rigidbody>().MovePosition(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z));
            }
        }
        Vector2 velocity = new Vector2((transform.position.x - Player.transform.position.x) * speed, (0.0f) * speed);
        
	   GetComponent<Rigidbody>().velocity = -velocity;
       if(Moving)
        {

            velocity = new Vector2(0.0f, (transform.position.y - Player.transform.position.y + 1) * speed);
            GetComponent<Rigidbody>().velocity = -velocity;
           if(Player.transform.position.y - transform.position.y > 0.5)
           {
               GetComponent<Rigidbody>().MovePosition(new Vector3(transform.position.x, transform.position.y + 0.09f, transform.position.z));
              
           }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!(Player.transform.position.y - transform.position.y < 0.25f))
        {
          Falling = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ladder" && !(Player.transform.position.y - transform.position.y < 0.25) )
        {
            if (transform.position.x != other.transform.position.x)
            {
                GetComponent<Rigidbody>().MovePosition(new Vector3(other.transform.position.x, transform.position.y, transform.position.z));
            }
           GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
           GetComponent<Rigidbody>().useGravity = false;
           Moving = true;
           OnLadder = true;
           Falling = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ladder" && Player.transform.position.y - transform.position.y >= 0.25)
        {
         
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().useGravity = true;
            Moving = false;
            if (!(Player.transform.position.y - transform.position.y < 0.25f))
            {
                Falling = true;
            }
            //Falling = true;
        }

    }

    void FindLadder()
    {
        LadderDirection = Player.transform.position - transform.position;
        LadderDirection.Normalize();
        Ray LookingForLadder = new Ray(transform.position, LadderDirection);
            if (Physics.Raycast(LookingForLadder, out LadderFound, 100.0f)) // LadderFound is the object.
            {
                if (Connected.collider.tag == "Ladder")
                {
                    Vector2 velocity = new Vector2((transform.position.x - LadderFound.transform.position.x) * speed, (0.0f) * speed);
                    GetComponent<Rigidbody>().velocity = -velocity;
                    OnLadder = true;
                    Falling = false;
                }
            }
    }*/

//    void Flip()
//    {
//        Quaternion newRotation = Quaternion.identity;
//        if (FacingRight)
//        {
//            newRotation.y = 180.0f;
//            FacingRight = !FacingRight;
//        }
//        else
//        {
//            FacingRight = !FacingRight;
//        }
//        Image.transform.localRotation = newRotation;
//    }
}
