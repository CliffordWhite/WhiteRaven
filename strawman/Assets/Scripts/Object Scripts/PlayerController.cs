using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float Speed = 0.15f;
	bool FacingRight = true;
	float MoveDir = 0.0f;

    private Vector3 mousePos;
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

	Rigidbody MyRigidbody = new Rigidbody();
	GameObject RayLeftOrigin = null; // for jumping.
	GameObject RayRightOrigin = null;
	float RayMaxDist = 0.94f;
	public LayerMask RayMask;
	GameObject Image = null;
	bool InMineCart = false;

    // Use this for initialization
	void Start ()
	{
		MyRigidbody = transform.GetComponent<Rigidbody> ();
        FacingRight = true;

		RayLeftOrigin = transform.FindChild ("RayOrigin1").gameObject;
		RayRightOrigin = transform.FindChild ("RayOrigin2").gameObject;
		Image = transform.FindChild ("Sprite").gameObject;

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

	void Update ()
	{
		if (GameManager.paused)
			return;

		MoveDir = Input.GetAxisRaw("Horizontal");

		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z;



         if (Input.GetKeyDown(KeyCode.Mouse0) && !isGrappled)
        {
			Vector3 whipDirection = mousePos - transform.position;
			whipDirection.Normalize ();
			Ray WhipThrown = new Ray(transform.position, whipDirection);
			Debug.DrawRay(transform.position, mousePos * distancecheck);
			
			if (Physics.Raycast(WhipThrown, out Connected, distancecheck))
            {
                if (Connected.collider.tag == "Hookable")
                {
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
        if (isGrappled)
            HookOnAdjust();
	}

    void FixedUpdate() 
    {
        if (line.enabled)
		{
			if (!isGrappled)
				line.enabled = false;
			else
				DrawLine();
		}

		if( !InMineCart && MoveDir != 0 && !isGrappled )
			transform.position += new Vector3( MoveDir * Speed, 0.0f, 0.0f );
		else
		if( isGrappled )
		{
			HookedOn();
			return;
		}


		if (MoveDir > 0 && !FacingRight)
           Flip();
		else if (MoveDir < 0 && FacingRight)
           Flip();

		// Jump
		if (!InMineCart && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && MyRigidbody.velocity.y < 0.1f)
        {
			if (Physics.Raycast(RayLeftOrigin.transform.position, new Vector3(0, -1.0f, 0), RayMaxDist, RayMask) 
			    || Physics.Raycast(RayRightOrigin.transform.position, new Vector3(0, -1.0f, 0), RayMaxDist, RayMask))
				MyRigidbody.AddForce(0.0f, 250.0f, 0.0f, ForceMode.Acceleration);
        }
		else if (InMineCart && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && transform.parent.GetComponent<MineCartController>().OnTracks > 0)
		{
			transform.parent.transform.GetComponent<Rigidbody>().AddForce(0.0f, 250.0f, 0.0f, ForceMode.Acceleration);
		}

		// Detach Grapple
		if (isGrappled && Input.GetKeyUp(KeyCode.Mouse0))
        {
            isGrappled = false;
			MyRigidbody.freezeRotation = true;
            NewTransform.rotation = Origrotation;
        }
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
		MoveDir = 0;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fatal" || other.tag == "HM Fatal")
        {
			KillPlayer();
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
	
	void KillPlayer()
	{
		FXSource.PlayOneShot(DeathSound, 1.0f);
		float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
		if (GameManager.manager.hardModeOn)
			GameManager.manager.lives--;		// lose life if hard mode
		Invoke ("RestartLevel", fadetime);	
	}
	
    void OnCollisionEnter(Collision other)
    {
		if (!isGrappled)
			MyRigidbody.velocity = Vector3.zero;

        if((other.collider.tag == "Projectile" || other.collider.tag == "Shaman") && !HasArmor )
        {
            FXSource.PlayOneShot(DeathSound, 1.0f);
			float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
			if (GameManager.manager.hardModeOn)
				GameManager.manager.lives--;		// lose life if hard mode
			Invoke ("RestartLevel", fadetime);
		}
    }
	void RestartLevel()
	{
		if (GameManager.manager.hardModeOn && GameManager.manager.lives <= 0)
			Application.LoadLevel(0);		// load main menu if all lives lost

		// reloads the current level from the start
		else
			Application.LoadLevel (Application.loadedLevel);
	}
    void WhipConnect()
    {
        DrawLine();
		if( InMineCart )
		{
			transform.parent.transform.SendMessage("LeaveCart");
			MineCartMode( null );
		}
		Hookable.GetComponent<HingeJoint>().connectedBody = MyRigidbody;
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
		_dir = (mousePos - transform.position).normalized;
		distance = _dir.magnitude;
		_dir.Normalize ();
        //sets the line positions start and end points
        //and enables the line to be drawn
        line.enabled = true;
        line.SetPosition(0, transform.position);
        if (isGrappled)
        {
            line.SetPosition(1, Connected.transform.position);
        }
        else if (distance < distancecheck)
			line.SetPosition(1, mousePos);
		else
        {
            line.SetPosition(1, _dir * distancecheck + transform.position);
        }

    }
    void HookedOn() 
    {
		if (Input.GetKey(KeyCode.W) && isGrappled && MyRigidbody.velocity.magnitude < 5.0f
		    && MyRigidbody.position.y < Hookable.GetComponent<Rigidbody>().position.y
                && distanceFromHook > HookDistanceMin && distanceFromHook <= HookDistanceMax)
        {
            Hookable.GetComponent<HingeJoint>().connectedBody = null;
			MyRigidbody.transform.position += new Vector3(0.0f, 0.2f, 0.0f);
			Hookable.GetComponent<HingeJoint>().connectedBody = MyRigidbody;
			
		}
		else if (Input.GetKey(KeyCode.S) && isGrappled && MyRigidbody.velocity.magnitude < 5.0f
		         && MyRigidbody.position.y < Hookable.GetComponent<Rigidbody>().position.y
                 && distanceFromHook > HookDistanceMin && distanceFromHook <= HookDistanceMax)
        {
            Hookable.GetComponent<HingeJoint>().connectedBody = null;
			MyRigidbody.transform.position += new Vector3(0.0f, -0.2f, 0.0f);
			Hookable.GetComponent<HingeJoint>().connectedBody = MyRigidbody;
		}
		else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)  && isGrappled && MyRigidbody.position.y < Hookable.GetComponent<Rigidbody>().position.y
                 && distanceFromHook > HookDistanceMin && distanceFromHook <= HookDistanceMax)
        {
			MyRigidbody.AddForce(new Vector3(MoveDir * 6.0f, 0.0f, 0.0f));
        }
    }
    void HookOnAdjust()
    {
        distanceFromHook = Vector3.Distance(transform.position, Hookable.transform.position);
        if (isGrappled && distanceFromHook > HookDistanceMin && distanceFromHook >= HookDistanceMax)
        {
            Hookable.GetComponent<HingeJoint>().connectedBody = null;
			MyRigidbody.transform.position += new Vector3(0.0f, 0.1f, 0.0f);
			Hookable.GetComponent<HingeJoint>().connectedBody = MyRigidbody;
		}
		else if (isGrappled && distanceFromHook < HookDistanceMin && distanceFromHook <= HookDistanceMax)
        {
            Hookable.GetComponent<HingeJoint>().connectedBody = null;
			MyRigidbody.transform.position -= new Vector3(0.0f, 0.1f, 0.0f);
			Hookable.GetComponent<HingeJoint>().connectedBody = MyRigidbody;
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
	
	public void MineCartMode(Transform _inMineCart)
	{
		if( _inMineCart == null )
			InMineCart = false;
		else
			InMineCart = true;
		
		transform.parent = _inMineCart;
		if( InMineCart )
		{
			Vector3 center = transform.localPosition;
			center.x = 0.0f;
			center.y = 1.2f;
			transform.localPosition = center;
			MyRigidbody.isKinematic = true;
			MyRigidbody.useGravity = false;
			MyRigidbody.velocity = Vector3.zero;
		}
		else
		{
			MyRigidbody.isKinematic = false;
			MyRigidbody.useGravity = true;
		}
	}
}
