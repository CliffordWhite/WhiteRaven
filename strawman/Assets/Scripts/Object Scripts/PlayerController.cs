using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float maxSpeed;
    bool grounded;
    bool FacingRight;
    float HeMoved;
    //shield anchor
    public GameObject shieldAnchor;
    private Vector2 mousePos;
    private Vector3 screenPos;

	// Use this for initialization
	void Start () {
        if (maxSpeed == 0)
            maxSpeed = 6.0f;
        grounded = true;
        FacingRight = true;
        //Shield Anchor
        if (shieldAnchor == null)
            shieldAnchor = GameObject.FindWithTag("ShieldAnchor");
        shieldAnchor.SetActive(false);


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

        //for shield to be active/inactive
        mousePos = Input.mousePosition;
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z));

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            shieldAnchor.SetActive(true);
      
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            shieldAnchor.SetActive(false);
        }
        if (FacingRight)
            shieldAnchor.transform.eulerAngles = new Vector3(shieldAnchor.transform.rotation.eulerAngles.x, shieldAnchor.transform.rotation.eulerAngles.y, Mathf.Atan2((screenPos.y - shieldAnchor.transform.position.y), (screenPos.x - shieldAnchor.transform.position.x)) * Mathf.Rad2Deg);
        else
            shieldAnchor.transform.eulerAngles = new Vector3(shieldAnchor.transform.rotation.eulerAngles.x, shieldAnchor.transform.rotation.eulerAngles.y, Mathf.Atan2((screenPos.y - shieldAnchor.transform.position.y), -(screenPos.x - shieldAnchor.transform.position.x)) * Mathf.Rad2Deg);

	}
    void FixedUpdate() 
    {
        mousePos = Input.mousePosition;
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z));

        

        if(!grounded)
        { return; }
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
