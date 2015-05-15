using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float maxSpeed;
    bool grounded;
    bool FacingRight;
    float HeMoved;
	// Use this for initialization
	void Start () {
        if (maxSpeed == 0)
            maxSpeed = 6.0f;
        grounded = true;
        FacingRight = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Rigidbody>().velocity.y == 0.0f)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        HeMoved = Input.GetAxis("Horizontal");
	}
    void FixedUpdate() 
    {
        if(grounded)
        GetComponent<Rigidbody>().velocity = new Vector3(HeMoved * maxSpeed, GetComponent<Rigidbody>().velocity.y, 0.0f);
       
        if (Input.GetKey(KeyCode.Space) && grounded || Input.GetKey(KeyCode.W) && grounded)
        {
            GetComponent<Rigidbody>().AddForce(0.0f, 250.0f, 0.0f);
            grounded = false;
        }

        if (HeMoved > 0 && !FacingRight)
            Flip();
        else if (HeMoved < 0 && FacingRight)
            Flip();
       
    }
    void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
        HeMoved = 0;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fatal")
        {
            GetComponent<AudioSource>().Play();
        }
    }

}
