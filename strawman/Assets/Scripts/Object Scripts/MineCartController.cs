using UnityEngine;
using System.Collections;

public class MineCartController : MonoBehaviour {

	bool Enabled = false;
	bool OnRail = false;
	
	float MaxMoveSpeed = 0.15f;
	float MoveSpeed = 0.0f;
	
	float UpTime = 0.5f;
	float TimeUp = 0.0f;
	
	float MaxUpRate = 0.15f;
	float UpRate = 0.0f;
	bool MovingUp = false;
	
	public LayerMask MineRailMask;
	GameObject Player = null;
	
	void Start()
	{
		MoveSpeed = MaxMoveSpeed;
		UpRate = MaxUpRate;
	}
	
	void Update()
	{
		if (Enabled && OnRail && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) )
		{
			MovingUp = true;
			OnRail = false;
		}
	}
	
	void FixedUpdate()
	{
		RaycastHit PosCheck = new RaycastHit();
		Physics.Raycast( transform.position + Vector3.up, Vector3.down, out PosCheck, float.MaxValue, MineRailMask );
		if (PosCheck.distance > 1.2f || PosCheck.distance == 0.0f)
		{
			OnRail = false;
			transform.eulerAngles = new Vector3(0.0f, 0.0f, 25.0f);
			if (!MovingUp)
			{
				transform.position += Vector3.down * UpRate;
				UpRate += 0.01f;
				if (UpRate > MaxUpRate * 2.0f)
					UpRate = MaxUpRate * 2.0f;
			}
		}
		else if (!MovingUp)
		{
			OnRail = true;
			UpRate = MaxUpRate;
			TimeUp = 0.0f;
			if (PosCheck.collider.transform.tag != "MineRail")
			{
				MoveSpeed -= MaxMoveSpeed * 0.1f;
				if (MoveSpeed < 0.0f)
					MoveSpeed = 0.0f;
			}
			else
				MoveSpeed = MaxMoveSpeed;
			transform.rotation = PosCheck.collider.transform.rotation;
			Vector3 upFix = transform.position;
			upFix.y = PosCheck.point.y;
			transform.position = upFix;
				
		}
		
		if( Enabled )
		{
			transform.position += transform.right * MoveSpeed;
		}
		
		if (MovingUp)
		{
				
			transform.position += Vector3.up * UpRate;
			UpRate -= 0.01f;
			if (UpRate < MaxUpRate * 0.1f)
				UpRate = MaxUpRate * 0.1f;
			TimeUp += Time.deltaTime;
			if (TimeUp > UpTime)
			{
				MovingUp = false;
				UpRate = MaxUpRate;
				TimeUp = 0.0f;
			}
		}
	}
	
	void OnTriggerEnter(Collider _obj)
	{
		if( !Enabled && _obj.transform.tag == "Player" )
		{
			Enabled = true;
			Player = _obj.transform.gameObject;
			Player.SendMessage("MineCartMode", transform);
		}
	}
	
	void LeaveCart()
	{
		GetComponent<BoxCollider>().enabled = false;
	}
}