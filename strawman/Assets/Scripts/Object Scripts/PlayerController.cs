using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float Speed = 0.15f;
    public bool FacingRight = true;
    float MoveDir = 0.0f;
    float FlyDir = 0.0f;
    public bool OnLadder = false;

    private Vector3 mousePos;
    //Whip
    public float distanceFromHook;
    public float distance;
    public float distancecheck = 7.0f;
    public float MinDistance = 1.0f;			////////////////////////////////////
    public float HookDistanceMin = 3.0f;    	/// KNOWN BUG 10
	public float HookDistanceMax = 7.0f;		////////////////////////////////////											 
	public float missDuration = 0.1f;			// Variable for how long to render line when missed
	float missTimer = 0.0f;						// Used to count against the duration
	bool missedAndRender = false;				// Used to know when to use these timers
    public bool isGrappled;
    Vector3 _dir;
    RaycastHit Connected;
    GameObject Hookable;
    //Lever
    GameObject Lever;
    public bool LeverFacingRight;
    //Armor
    GameObject Armor;
    // public Sprite NormalSprite;
    //public Sprite ArmorSprite;
    //GameObject SpriteSwitch;
    public bool HasArmor;
    float invincibleFrames;
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
    GameObject PlayerHead;
    GameObject RayLeftOrigin = null; // for jumping.
    GameObject RayRightOrigin = null;
    float RayMaxDist = 0.94f;
    public LayerMask RayMask;
    //GameObject Image = null;
    bool InMineCart = false;

    // Water Stuff
    int UnderWater = 0;
    float BreathDuration = 15.0f;
    float TimeUnderWater = 0.0f;
    bool Drowning = false;
    public GameObject BreathBar = null;
    public Image BubblesImage = null;
    Vector3 gravityBase = new Vector3(0, -9.8f, 0);
    bool alive = true;

    // Sand Stuff
    GameObject RaySandDepthCheck = null;
    int InSand = 0;
    public LayerMask SandOnly;
    RaycastHit SandInfo = new RaycastHit();

    //animation stuff/animator
    public Animator anim;

    // Use this for initialization
    void Start()
    {
        gravityBase = Physics.gravity;
        BreathBar.SetActive(false);

        invincibleFrames = 0.0f;
        MyRigidbody = transform.GetComponent<Rigidbody>();
        
		FacingRight = true;

        RayLeftOrigin = transform.FindChild("RayOrigin1").gameObject;
        RayRightOrigin = transform.FindChild("RayOrigin2").gameObject;
        //Image = transform.FindChild ("Sprite").gameObject;

        RaySandDepthCheck = transform.FindChild("QuickSandDepth").gameObject;

        //Whip swing rotation fix
        NewTransform = transform;
        Origrotation = NewTransform.rotation;
        //Lever
        LeverFacingRight = false;
        if (Lever == null)
            Lever = GameObject.FindWithTag("Lever");
        //Armor
        HasArmor = false;
        //  if (SpriteSwitch == null)
        //     SpriteSwitch = GameObject.FindWithTag("Sprite");
        //Line drawing settings
        line = GetComponentInChildren<LineRenderer>();
        line.SetWidth(startWidth, endWidth);
        line.SetVertexCount(2);
        line.material.color = Color.black;
        line.enabled = false;
        line.sortingOrder = 0;
        // SpriteSwitch.GetComponent<SpriteRenderer>().sprite = NormalSprite;
        //Cheat Code bools
        flyModeOn = GameManager.manager.flyMode;
        addLives = 30;
        MoveDir = 0.0f;
        FlyDir = 0.0f;
        //animation
        anim = GetComponentInChildren<Animator>();
        anim.Play("idle");
        OnLadder = false;
        PlayerHead = GameObject.FindWithTag("PlayerHead");
    }

    void Update()
    {
        if (GameManager.paused)
            return;
        if (GameManager.manager.flyMode)
            FlyModeOn = true;

        MoveDir = Input.GetAxisRaw("Horizontal");
        if (FlyModeOn)
            FlyDir = Input.GetAxisRaw("Vertical");
		////////////////////////////////
		/// KNOWN BUG 10
		/// govern timer for how long 
		/// to render missed whip
		////////////////////////////////
		if (missedAndRender)
		{
			missTimer += Time.deltaTime;
			if (missTimer >= missDuration)
			{
				missTimer = 0.0f;
				missedAndRender = false;
			}
		}
		////////////////////////////////
		/// END KNOWN BUG 10
		////////////////////////////////
        anim.SetFloat("XSpeed", Mathf.Abs(MyRigidbody.velocity.x)); //swinging

        // anim.SetFloat("Speed", Mathf.Abs(MoveDir)); //Animator to switch from idle to run;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        if (invincibleFrames > 0.0f)
            invincibleFrames -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isGrappled)
        {
            anim.Play("WhipThrow");
            Vector3 whipDirection = mousePos - transform.position;
            whipDirection.Normalize();
            Ray WhipThrown = new Ray(transform.position, whipDirection);
            //Debug.DrawRay(transform.position, mousePos * distancecheck);
			if (missedAndRender)			// KNOWN BUG 10
				missedAndRender = false;	// Reset the missed render point or check for new connection
            if (Physics.Raycast(WhipThrown, out Connected, distancecheck, RayMask))
            {
                if (Connected.collider.tag == "Hookable")
                {
                    isGrappled = true;
                    Hookable = Connected.collider.gameObject;
                    WhipConnect();
                }

                if (Connected.collider.tag == "Lever")
                {
                    Lever = Connected.collider.gameObject;
                    LeverConnect();
                }
            }
            if (!isGrappled)
                WhipMissed();
        }
        else if (isGrappled && Input.GetKeyUp(KeyCode.Mouse0))
        {
            isGrappled = false;
            MyRigidbody.freezeRotation = true;
            NewTransform.rotation = Origrotation;
            MyRigidbody.drag = 0.0f;
            //anim.SetBool("WhipConnect", false);
        }

        if (isGrappled)
            HookOnAdjust();

        if (Drowning)
        {
            TimeUnderWater += Time.deltaTime;
            BubblesImage.fillAmount = 1.0f - TimeUnderWater / BreathDuration;
            if (TimeUnderWater >= BreathDuration && alive)
            {
                KillPlayer();
                BubblesImage.fillAmount = 0.0f;
                alive = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (MoveDir > 0 && !FacingRight)
            Flip();
        else if (MoveDir < 0 && FacingRight)
            Flip();

        if (MoveDir == 0 && (Physics.Raycast(RayLeftOrigin.transform.position, new Vector3(0, -1.0f, 0), RayMaxDist, RayMask)
                    || Physics.Raycast(RayRightOrigin.transform.position, new Vector3(0, -1.0f, 0), RayMaxDist, RayMask)
                    && MyRigidbody.velocity.y < 0.1f) && !OnLadder)
        {
            anim.Play("idle");
        }
        if (!(Physics.Raycast(RayLeftOrigin.transform.position, new Vector3(0, -1.0f, 0), RayMaxDist, RayMask)
                    || Physics.Raycast(RayRightOrigin.transform.position, new Vector3(0, -1.0f, 0), RayMaxDist, RayMask)
                    && MyRigidbody.velocity.y < 0.1f) && !isGrappled && !OnLadder)
            anim.Play("JumpAndFall");
        if (line.enabled)
        {
            if (!isGrappled && !missedAndRender)
                line.enabled = false;
            else
                DrawLine();
        }

        float speedReduction = 1.0f;
        float JumpForceMod = 1.0f;
        if (InSand > 0)
        {
            Physics.Raycast(RaySandDepthCheck.transform.position, Vector3.down, out SandInfo, 1.87f, SandOnly);
            float DistanceCheck = SandInfo.distance;
            if (SandInfo.collider == null)
                DistanceCheck = 1.87f;
            DistanceCheck = DistanceCheck / 1.87f;
            speedReduction = DistanceCheck * 0.25f;
            MyRigidbody.drag = 15.0f;
            JumpForceMod = 0.9f * (DistanceCheck / 1.87f);
        }
        else if (UnderWater > 0)
        {
            speedReduction = 0.5f;
            JumpForceMod = 0.6f;
        }
        else
        {
            Physics.gravity = new Vector3(0, -9.8f, 0);
            MyRigidbody.drag = 0.0f;
        }

        if (FlyDir != 0 && !isGrappled && !InMineCart)
        {
            transform.position += new Vector3(0.0f, FlyDir * Speed * speedReduction, 0.0f);
        }
        if (!InMineCart && MoveDir != 0 && !isGrappled)
        {
            transform.position += new Vector3(MoveDir * Speed * speedReduction, 0.0f, 0.0f);
            if ((Physics.Raycast(RayLeftOrigin.transform.position, new Vector3(0, -1.0f, 0), RayMaxDist, RayMask)
                || Physics.Raycast(RayRightOrigin.transform.position, new Vector3(0, -1.0f, 0), RayMaxDist, RayMask)
                && MyRigidbody.velocity.y < 0.1f))
                 anim.Play("Run");
        }
        else if (isGrappled)
        {
            HookedOn();
            anim.Play("Swing");
            return;
        }





        // Jump

		//////////////////////////////////////////////////////////////
		// FOUND BUG 32
		// player is able to move left and right with arrow keys since 
		// default keys for horizontal is A/D and Left/Right Arrows. 
		// Added Up Arrow for the jump to compliment the default values
		//////////////////////////////////////////////////////////////
		
		if (InSand > 0 && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !FlyModeOn)
        {
            MyRigidbody.AddForce(0.0f, 250.0f * JumpForceMod, 0.0f, ForceMode.Acceleration);
        }
		else if (!InMineCart && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && MyRigidbody.velocity.y < 0.1f)
		//////////////////////////////////////////////////////////////
        // END FOUND BUG 32
		//////////////////////////////////////////////////////////////
			
		{
            if (InSand > 0 || Physics.Raycast(RayLeftOrigin.transform.position, new Vector3(0, -1.0f, 0), RayMaxDist, RayMask)
                || Physics.Raycast(RayRightOrigin.transform.position, new Vector3(0, -1.0f, 0), RayMaxDist, RayMask))
            {
                if (InSand == 0)
                    MyRigidbody.AddForce(0.0f, 250.0f * JumpForceMod, 0.0f, ForceMode.Acceleration);
            }
        }

        if (Physics.Raycast(RayLeftOrigin.transform.position, new Vector3(0, -1.0f, 0), RayMaxDist, RayMask)
                || Physics.Raycast(RayRightOrigin.transform.position, new Vector3(0, -1.0f, 0), RayMaxDist, RayMask)
                && MyRigidbody.velocity.y < 0.1f)
        {
            anim.SetBool("Ground", true);
        }
        else
            anim.SetBool("Ground", false);

        anim.SetFloat("VSpeed", MyRigidbody.velocity.y); //Animator to switch to falling;

    }

    void Flip()
    {
        //Quaternion newRotation = Quaternion.identity;
        //if (FacingRight)
        //{
        //    newRotation.y = 180.0f;
        //    FacingRight = !FacingRight;
        //}
        //else
        //{
        //    FacingRight = !FacingRight;
        //}
        //transform.localRotation = newRotation;
        FacingRight = !FacingRight;
        Vector3 TheScale = anim.transform.localScale;
        TheScale.x *= -1;
        anim.transform.localScale = TheScale;
        //GetComponentInChildren<LineRenderer>().transform.localScale = TheScale;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (invincibleFrames <= 0.0f)
        {
            if ((other.tag == "Fatal" || other.tag == "HM Fatal") && !HasArmor)
                KillPlayer();
            else if ((other.tag == "Fatal" || other.tag == "HM Fatal") && HasArmor)
                HitWithArmor();
            else if (other.tag == "ArmorUp")
            {
                Armor = other.gameObject;
                Armor.SetActive(false);
                FXSource.PlayOneShot(ArmorPickUpSound, 1.0f);
                //SpriteSwitch.GetComponent<SpriteRenderer> ().sprite = ArmorSprite;
                HasArmor = true;
            }
        }
    }

    void KillPlayer()
    {
        if (!GameManager.manager.godMode)
        {
            Physics.gravity = gravityBase;
            FXSource.PlayOneShot(DeathSound, 1.0f);
            float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
            if (GameManager.manager.hardModeOn)
            {
                Physics.gravity = gravityBase;
                FXSource.PlayOneShot(DeathSound, 1.0f);
                if (GameManager.manager.hardModeOn)
                {
                    GameManager.manager.lives--;		// lose life if hard mode
                }
            }
                Invoke("RestartLevel", fadetime);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!isGrappled)
            MyRigidbody.velocity = Vector3.zero;

        if (!GameManager.manager.godMode)
        {
            if (invincibleFrames <= 0.0f)
            {
                if ((other.collider.tag == "Projectile" || other.collider.tag == "Shaman") && !HasArmor)
                {
                    FXSource.PlayOneShot(DeathSound, 1.0f);
                    float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
                    if (GameManager.manager.hardModeOn)
                    {
                        GameManager.manager.lives--;// lose life if hard mode
                    }
                    Invoke("RestartLevel", fadetime);
                }
                else if ((other.collider.tag == "Projectile" || other.collider.tag == "Shaman") && HasArmor)
                    HitWithArmor();
            }
        }
    }

    void RestartLevel()
    {
        if (GameManager.manager.hardModeOn && GameManager.manager.lives <= 0)
        {
            if (!GameManager.manager.webMode)
                GameManager.manager.EraseFile();
            else
                GameManager.manager.PlayerPrefsErase();
            Application.LoadLevel(0);		// load main menu if all lives lost
        }
        // reloads the current level from the start
        else
            Application.LoadLevel(Application.loadedLevel);
    }

    void WhipConnect()
    {
        DrawLine();
        // anim.SetBool("WhipConnect", true);
        if (InMineCart)
        {
            transform.parent.transform.SendMessage("LeaveCart");
            MineCartMode(null);
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
        _dir.Normalize();
        //sets the line positions start and end points
        //and enables the line to be drawn
        line.enabled = true;
        Vector3 ZFix = transform.position;
        ZFix.z = 0.0f;
        line.SetPosition(0, ZFix);
        Vector3 endPos = Vector3.zero;
        if (isGrappled)
        {
            endPos = Connected.transform.position;
            endPos.z = ZFix.z;
            line.SetPosition(1, endPos);
        }
		//////////////////////////////////
		/// KNOWN BUG 10
		/// setting up bool to allow whip 
		/// to flash for a manually set 
		/// variable amount of time
		//////////////////////////////////
        else if (distance < distancecheck && !missedAndRender)
        {
            endPos = mousePos;
            endPos.z = ZFix.z;
            line.SetPosition(1, endPos);
			missedAndRender = true;
        }
        else if (!missedAndRender)
        {
            endPos = _dir * distancecheck + transform.position;
            endPos.z = ZFix.z;
            line.SetPosition(1, endPos);
			missedAndRender = true;
        }
		///////////////////////////////
		/// END KNOWN BUG 10
		///////////////////////////////
    }

    void HookedOn()
    {
        if (UnderWater > 0)
            MyRigidbody.drag = 1.5f;
        else if (InSand == 0)
            MyRigidbody.drag = 0.0f;

        if (Input.GetKey(KeyCode.W) && isGrappled && MyRigidbody.velocity.magnitude < 5.0f
            && MyRigidbody.position.y < Hookable.GetComponent<Rigidbody>().position.y
                && distanceFromHook > HookDistanceMin && distanceFromHook <= HookDistanceMax)
        {
            Hookable.GetComponent<HingeJoint>().connectedBody = null;
            MyRigidbody.transform.position += new Vector3(0.0f, InSand > 0 ? 0.01f : 0.2f, 0.0f);
            Hookable.GetComponent<HingeJoint>().connectedBody = MyRigidbody;

        }
        else if (InSand == 0 && Input.GetKey(KeyCode.S) && isGrappled && MyRigidbody.velocity.magnitude < 5.0f
                 && MyRigidbody.position.y < Hookable.GetComponent<Rigidbody>().position.y
                 && distanceFromHook > HookDistanceMin && distanceFromHook <= HookDistanceMax)
        {
            Hookable.GetComponent<HingeJoint>().connectedBody = null;
            MyRigidbody.transform.position += new Vector3(0.0f, -0.2f, 0.0f);
            Hookable.GetComponent<HingeJoint>().connectedBody = MyRigidbody;
        }
        else if (InSand == 0 && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && isGrappled && MyRigidbody.position.y < Hookable.GetComponent<Rigidbody>().position.y
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
            Scale.x = -Scale.x;
            LeverFacingRight = !LeverFacingRight;
        }
        else
        {
            Scale.x = -Scale.x;
            LeverFacingRight = !LeverFacingRight;
        }
        Lever.transform.localScale = Scale;
        FXSource.PlayOneShot(WhipConnectSound, 1.0f);
        Lever.GetComponent<Lever>().HasMoved();
    }

    public void HitWithArmor()
    {
        if (!GameManager.manager.godMode)
        {
            //  SpriteSwitch.GetComponent<SpriteRenderer>().sprite = NormalSprite;
            HasArmor = false;
            invincibleFrames = 1.0f;
        }
    }

    public void MineCartMode(Transform _inMineCart)
    {
        if (_inMineCart == null)
            InMineCart = false;
        else
            InMineCart = true;

        transform.parent = _inMineCart;
        if (InMineCart)
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

    //Cheat codes
    public bool flyModeOn;
    int addLives;
    public bool FlyModeOn
    {
        get
        {
            return flyModeOn;
        }
        set
        {
            flyModeOn = value;
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().useGravity = !flyModeOn; // the ! is to turn off gravity.
        }
    }

    public int AddLives
    {
        get
        {
            return addLives;
        }
        set
        {
            addLives = value;
            GameManager.manager.lives += addLives;
        }
    }

    public int WaterMode
    {
        set
        {
            UnderWater += value;
            if (UnderWater > 0)
            {
                Physics.gravity = gravityBase * 0.2f;
            }
            else
            {
                Physics.gravity = gravityBase;
            }
        }
    }

    public bool isDrowning
    {
        set
        {
            Drowning = value;

            if (Drowning)
            {
                BreathBar.SetActive(true);
                BubblesImage.fillAmount = 1.0f;
                TimeUnderWater = 0.0f;
            }
            else
            {
                BreathBar.SetActive(false);
            }
        }
    }

    public int IsInSand
    {
        set
        {
            InSand += value;
        }
    }

}
