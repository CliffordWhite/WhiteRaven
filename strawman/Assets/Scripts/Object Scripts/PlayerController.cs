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
    public float distanceFromHook;
    public float distance;
    public float distancecheck = 7.0f;
    public float MinDistance = 1.0f;
    public float HookDistanceMin = 3.0f;
    public float HookDistanceMax = 7.0f;
    public bool isGrappled;
    Vector3 _dir;
    RaycastHit Connected;
    GameObject Hookable;
    //Lever
    GameObject Lever;
    public bool LeverFacingRight;
    //Armor
    GameObject Armor;
    public Sprite NormalSprite;
    public Sprite ArmorSprite;
    GameObject SpriteSwitch;
    public bool HasArmor;
    // Player Rotation after whip
    Quaternion Origrotation;
    Transform NewTransform;

    //Audio
    public AudioSource FXSource;
    public AudioClip DeathSound;
    public AudioClip WhipMissSound;
    public AudioClip WhipConnectSound;
    public AudioClip ShieldDeflectSound;
    public AudioClip ArmorPickUpSound;

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
        //Whip swing rotation fix
        NewTransform = transform;
        Origrotation = NewTransform.rotation;
        //Lever
        LeverFacingRight = false;
        if (Lever == null)
            Lever = GameObject.FindWithTag("Lever");
        //Armor
        HasArmor = false;
        if (SpriteSwitch == null)
            SpriteSwitch = GameObject.FindWithTag("Sprite");
        //Line drawing settings
        line = gameObject.AddComponent<LineRenderer>();
        line.SetWidth(startWidth, endWidth);
        line.SetVertexCount(2);
        line.material.color = Color.red;
        line.enabled = false;
        SpriteSwitch.GetComponent<SpriteRenderer>().sprite = NormalSprite;
	}
	
	// Update is called once per frame
	void Update () {

		// ignore this entire call if game is paused
		if (GameManager.paused)
			return;
		
        if (GetComponent<Rigidbody>().velocity.y == 0.0f)
        {
            grounded = true;
        }
		//removed this because it was causing player movement issues
       // else
       // {
       //     grounded = false;
       // }
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
            Ray WhipThrown;
            _dir = (screenPos - transform.position).normalized;
            WhipThrown = new Ray(transform.position, _dir);
            Debug.DrawRay(transform.position, _dir * distancecheck);
            
            if (Physics.Raycast(WhipThrown, out Connected, distancecheck))
            {
                if (Connected.collider.tag == "Hookable")
                {
                    grounded = false;
                    isGrappled = true;
                    Hookable = Connected.collider.gameObject;
                    WhipConnect();
                }

                if(Connected.collider.tag == "Lever")
                {
                    Lever = Connected.collider.gameObject;
                    LeverConnect();
                }
            }
            if(!isGrappled)
                WhipMissed();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isGrappled = false;
            NewTransform.rotation = Origrotation;
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        if (isGrappled)
            HookOnAdjust();
	}
    void FixedUpdate() 
    {
        if (line.enabled && !isGrappled)
            line.enabled = false;
        else if (line.enabled && isGrappled)
        {
            DrawLine();
        }
        mousePos = Input.mousePosition;
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z));
        _dir = (screenPos - transform.position).normalized;
       
        
        if(!grounded && !isGrappled)
        { return; }

        else if(grounded && !isGrappled)
            GetComponent<Rigidbody>().velocity = new Vector3(HeMoved * maxSpeed, GetComponent<Rigidbody>().velocity.y, 0.0f);
        else if (!grounded && isGrappled)
        {
            HookedOn();
            return;
        }


        if (HeMoved > 0 && !FacingRight)
            Flip();
        else if (HeMoved < 0 && FacingRight)
            Flip();

        if ((Input.GetKey(KeyCode.Space) && grounded) || (Input.GetKey(KeyCode.W) && grounded))
        {
            GetComponent<Rigidbody>().AddForce(0.0f, 250.0f, 0.0f);
            grounded = false;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isGrappled = false;
            GetComponent<Rigidbody>().freezeRotation = true;
            NewTransform.rotation = Origrotation;
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
			float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
			Invoke ("RestartLevel", fadetime);
        }
        else if (other.tag == "ArmorUp")
        {
            Armor = other.gameObject;
            Armor.SetActive(false);
            FXSource.PlayOneShot(ArmorPickUpSound, 1.0f);         
            SpriteSwitch.GetComponent<SpriteRenderer>().sprite = ArmorSprite;
            HasArmor = true;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "Projectile" && !HasArmor)
        {
            FXSource.PlayOneShot(DeathSound, 1.0f);
			float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
			Invoke ("RestartLevel", fadetime);
		}
        

    }
	void RestartLevel()
	{
		// reloads the current level from the start
		Application.LoadLevel (Application.loadedLevel);
	}
    void WhipConnect()
    {
        DrawLine();
        isGrappled = true;
        Hookable.GetComponent<HingeJoint>().connectedBody = GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().freezeRotation = false;
        FXSource.PlayOneShot(WhipConnectSound, 1.0f);
    }
    void WhipMissed()
    {
        DrawLine();
        FXSource.PlayOneShot(WhipMissSound, 1.0f);
    }
    void DrawLine()
    {
        distance = Vector3.Distance(screenPos, transform.position);
        _dir = (screenPos - transform.position).normalized;
        //sets the line positions start and end points
        //and enables the line to be drawn
        line.enabled = true;
        line.SetPosition(0, GetComponent<Rigidbody>().transform.position);
        if (isGrappled)
        {
            line.SetPosition(1, Connected.transform.position);
        }
        else if (distance < distancecheck)
            line.SetPosition(1, screenPos);
        else
        {
            line.SetPosition(1, _dir * distancecheck + transform.position);
        }

    }
    void HookedOn() 
    {
        if (Input.GetKey(KeyCode.W) && isGrappled && GetComponent<Rigidbody>().velocity.magnitude < 5.0f
                && GetComponent<Rigidbody>().position.y < Hookable.GetComponent<Rigidbody>().position.y
                && distanceFromHook > HookDistanceMin && distanceFromHook <= HookDistanceMax)
        {
            Hookable.GetComponent<HingeJoint>().connectedBody = null;
            GetComponent<Rigidbody>().transform.position += new Vector3(0.0f, 0.2f, 0.0f);
            Hookable.GetComponent<HingeJoint>().connectedBody = GetComponent<Rigidbody>();

        }
        else if (Input.GetKey(KeyCode.S) && isGrappled && GetComponent<Rigidbody>().velocity.magnitude < 5.0f
                 && GetComponent<Rigidbody>().position.y < Hookable.GetComponent<Rigidbody>().position.y
                 && distanceFromHook > HookDistanceMin && distanceFromHook <= HookDistanceMax)
        {
            Hookable.GetComponent<HingeJoint>().connectedBody = null;
            GetComponent<Rigidbody>().transform.position += new Vector3(0.0f, -0.2f, 0.0f);
            Hookable.GetComponent<HingeJoint>().connectedBody = GetComponent<Rigidbody>();
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)  && isGrappled && GetComponent<Rigidbody>().position.y < Hookable.GetComponent<Rigidbody>().position.y
                 && distanceFromHook > HookDistanceMin && distanceFromHook <= HookDistanceMax)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(HeMoved * maxSpeed, GetComponent<Rigidbody>().velocity.y, 0.0f);
        }
    }
    void HookOnAdjust()
    {
        distanceFromHook = Vector3.Distance(transform.position, Hookable.transform.position);
        if (isGrappled && distanceFromHook > HookDistanceMin && distanceFromHook >= HookDistanceMax)
        {
            Hookable.GetComponent<HingeJoint>().connectedBody = null;
            GetComponent<Rigidbody>().transform.position += new Vector3(0.0f, 0.1f, 0.0f);
            Hookable.GetComponent<HingeJoint>().connectedBody = GetComponent<Rigidbody>();
        }
        else if (isGrappled && distanceFromHook < HookDistanceMin && distanceFromHook <= HookDistanceMax)
        {
            Hookable.GetComponent<HingeJoint>().connectedBody = null;
            GetComponent<Rigidbody>().transform.position -= new Vector3(0.0f, 0.1f, 0.0f);
            Hookable.GetComponent<HingeJoint>().connectedBody = GetComponent<Rigidbody>();
        }
    }
    void LeverConnect()
    {
        Vector3 Scale = Lever.transform.localScale;
        if (LeverFacingRight)
        {
            Scale.x = -1;
            LeverFacingRight = !LeverFacingRight;
        }
        else
        {
            Scale.x = 1;
            LeverFacingRight = !LeverFacingRight;
        }
        Lever.transform.localScale = Scale;
        FXSource.PlayOneShot(WhipConnectSound, 1.0f);
        Lever.GetComponent<Lever>().HasMoved();
    }
    public void HitWithArmor()
    {
        SpriteSwitch.GetComponent<SpriteRenderer>().sprite = NormalSprite;
        HasArmor = false;
    }
}
