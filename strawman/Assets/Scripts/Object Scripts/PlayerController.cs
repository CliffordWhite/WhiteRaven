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
    //Whip
    public float distance;
    public float distancecheck = 7.0f;
    public float MinDistance = 1.0f;
    public bool isGrappled;
    Vector3 _dir;
    //Audio
    public AudioSource FXSource;
    public AudioClip DeathSound;
    public AudioClip WhipMissSound;
    public AudioClip WhipConnectSound;
    public AudioClip ShieldDeflect;
    //DrawLine (Placeholder for animation)
    // Line start width
    public float startWidth = 0.05f;
    // Line end width
    public float endWidth = 0.05f;
    LineRenderer line;


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
        //Line drawing settings
        line = gameObject.AddComponent<LineRenderer>();
        line.SetWidth(startWidth, endWidth);
        line.SetVertexCount(2);
        line.material.color = Color.red;
        line.enabled = false;
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
        {
            shieldAnchor.transform.eulerAngles = new Vector3(shieldAnchor.transform.rotation.eulerAngles.x, shieldAnchor.transform.rotation.eulerAngles.y, Mathf.Atan2((screenPos.y - shieldAnchor.transform.position.y), (screenPos.x - shieldAnchor.transform.position.x)) * Mathf.Rad2Deg);
        }
        else
        {
            shieldAnchor.transform.eulerAngles = new Vector3(shieldAnchor.transform.rotation.eulerAngles.x, shieldAnchor.transform.rotation.eulerAngles.y, Mathf.Atan2((screenPos.y - shieldAnchor.transform.position.y), -(screenPos.x - shieldAnchor.transform.position.x)) * Mathf.Rad2Deg);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isGrappled)
        {
            RaycastHit Connected;
            Ray WhipThrown;
            _dir = (screenPos - transform.position).normalized;
            WhipThrown = new Ray(transform.position, _dir);
            Debug.DrawRay(transform.position, _dir * distancecheck);

            WhipMissed();
            if (Physics.Raycast(WhipThrown, out Connected, distancecheck))
            {
                
                if (Connected.collider.tag == "Hookable")
                {
                    WhipConnect();
                }
               
                
            }
        }
	}
    void FixedUpdate() 
    {
        if (line.enabled)
            line.enabled = false;
        mousePos = Input.mousePosition;
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z));
        _dir = (screenPos - transform.position).normalized;
        if(!grounded)
        { return; }
        GetComponent<Rigidbody>().velocity = new Vector3(HeMoved * maxSpeed, GetComponent<Rigidbody>().velocity.y, 0.0f);

        if (HeMoved > 0 && !FacingRight)
            Flip();
        else if (HeMoved < 0 && FacingRight)
            Flip();

        if (Input.GetKey(KeyCode.Space) && grounded || Input.GetKey(KeyCode.W) && grounded)
        {
            GetComponent<Rigidbody>().AddForce(0.0f, 250.0f, 0.0f);
            grounded = false;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isGrappled = false;
        }

       
    }
    void Flip()
    {

        Quaternion Scale = transform.localRotation;
        if (FacingRight)
        {
            Scale.y = 180;
            FacingRight = !FacingRight;
        }
        else
        {
            Scale.y = 0;
            FacingRight = !FacingRight;
        }
        transform.localRotation = Scale;
        HeMoved = 0;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fatal")
        {
            FXSource.PlayOneShot(DeathSound, 1.0f);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "Projectile")
        {
            FXSource.PlayOneShot(DeathSound, 1.0f);
        }
    }

    void WhipConnect()
    {
        isGrappled = true;
        FXSource.PlayOneShot(WhipConnectSound, 1.0f);
    }

    void WhipMissed()
    {
        distance = Vector3.Distance(screenPos, transform.position);
        _dir = (screenPos - transform.position).normalized;

        //sets the line positions start and end points
        //and enables the line to be drawn

            line.enabled = true;
            line.SetPosition(0, GetComponent<Rigidbody>().transform.position);
            if(distance < distancecheck)
            line.SetPosition(1, screenPos);
            else
            {
                line.SetPosition(1, _dir * distancecheck + transform.position);
            }
        FXSource.PlayOneShot(WhipMissSound, 1.0f);
    }

}
