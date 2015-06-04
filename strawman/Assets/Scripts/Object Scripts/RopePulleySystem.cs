using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class RopePulleySystem : MonoBehaviour {

	[Header ("Try Using Small Numbers Like 0.05f")]
	public float MoveSpeed = 0.01f;
	[Header ("Object One Settings. - Primary Object")]
	public GameObject ObjectOne = null;
	Vector3 StartPosOne = Vector3.zero;
	public Vector3 EndPosOne = Vector3.zero;
	
	[Header ("Object Two Settings. - Secondary Object")]
	public GameObject ObjectTwo = null;
	Vector3 StartPosTwo = Vector3.zero;
	public Vector3 EndPosTwo = Vector3.zero;
	float ObjTwoMoveSpeed = 0.0f;
	
	[Header ("Starting Positions Will Be Auto Set AT Start")]
	public bool Moving = false;
	bool MovingForward = true;
	
	[Header ("Place Cogs In Order From Primary Object to Secondary")]
	public GameObject[] Cogs;
	[Header ("Set to Same Size: Turn On If Cog Rotates In Wrong Direction")]
	public bool[] CogRotationFix;
	float[] CogFix;
	
	[Header ("Place Rope Prefab Here.")]
	public GameObject RopeObject = null;
	public int NumberOfRopes = 0;
	GameObject[] RopeTo;
	GameObject[] Ropes;
	
	void Start ()
	{
		if (ObjectOne == null || ObjectTwo == null)
		{
			Debug.LogError("Rope Pulley System Scrip Requires both objects to have a refrence!");
			Destroy(this);
		} else
		{
			StartPosOne = ObjectOne.transform.position;	
			StartPosTwo = ObjectTwo.transform.position;	
			ObjTwoMoveSpeed = ((StartPosTwo - EndPosTwo).magnitude / (StartPosOne - EndPosOne).magnitude) * MoveSpeed;
			
			RopeTo = new GameObject[Cogs.Length + 2];
			for (int i = 0; i < RopeTo.Length; i++)
			{
				if (i == 0)
					RopeTo[i] = ObjectOne;
				else if (i == RopeTo.Length - 1)
					RopeTo[i] = ObjectTwo;
				else
					RopeTo[i] = Cogs[i - 1];
			}
			
			CogFix = new float[Cogs.Length];
			for (int i = 0; i < Cogs.Length; i++)
			{
				if (CogRotationFix[i])
					CogFix[i] = -1.0f;
				else
					CogFix[i] = 1.0f;
			}
			Ropes = new GameObject[NumberOfRopes];
			for (int i = 0; i < NumberOfRopes; i++)
			{
				Ropes[i] = Instantiate(RopeObject);
				Ropes[i].transform.parent = transform;
				Ropes[i].transform.position = Vector3.zero;
			}
		}
	}
	
	void FixedUpdate ()
	{
		if (Moving)
		{
			if (MovingForward)
			{
				for (int i = 0; i < Cogs.Length; i++)
					Cogs[i].transform.RotateAround(Cogs[i].transform.position, new Vector3(0,0,1.0f),  -CogFix[i] * 135.0f * MoveSpeed);
				ObjectOne.transform.position = Vector3.MoveTowards(ObjectOne.transform.position, EndPosOne, MoveSpeed);
				ObjectTwo.transform.position = Vector3.MoveTowards(ObjectTwo.transform.position, EndPosTwo, ObjTwoMoveSpeed);
				if((ObjectOne.transform.position - EndPosOne).magnitude <= 0.001f)
				{
					//Moving = false;
					MovingForward = false;
				}
			}
			else
			{
				for (int i = 0; i < Cogs.Length; i++)
					Cogs[i].transform.RotateAround(Cogs[i].transform.position, new Vector3(0,0,1.0f),  CogFix[i] * 135.0f * MoveSpeed);
				ObjectOne.transform.position = Vector3.MoveTowards(ObjectOne.transform.position, StartPosOne, MoveSpeed);
				ObjectTwo.transform.position = Vector3.MoveTowards(ObjectTwo.transform.position, StartPosTwo, ObjTwoMoveSpeed);
				if((ObjectOne.transform.position - StartPosOne).magnitude <= 0.001f)
				{
					//Moving = false;
					MovingForward = true;
				}
			}
			for (int RopeNum = 0; RopeNum < NumberOfRopes; RopeNum++)
			{
				Vector3	Startpos = RopeTo[RopeNum].transform.position;
				Startpos.z += 0.1f;
				if (RopeNum != 0)
				{
					if (RopeTo[RopeNum - 1].transform.position.x < RopeTo[RopeNum + 1].transform.position.x)
						Startpos.x -= 0.15f;
					else
						Startpos.x += 0.15f;
					
					if (RopeTo[RopeNum].transform.position.y < RopeTo[RopeNum + 1].transform.position.y)
						Startpos.y -= 0.15f;
					else
						Startpos.y += 0.15f;
				}
				Ropes[RopeNum].GetComponent<LineRenderer>().SetPosition(0, Startpos);
					
				Vector3	Endpos = RopeTo[RopeNum + 1].transform.position;
				Endpos.z += 0.1f;
				if (RopeNum != NumberOfRopes - 1)
				{
					if (RopeTo[RopeNum - 1].transform.position.x < RopeTo[RopeNum + 1].transform.position.x)
						Endpos.x -= 0.15f;
					else
						Endpos.x += 0.15f;
					
					if (RopeTo[RopeNum].transform.position.y < RopeTo[RopeNum + 1].transform.position.y)
						Endpos.y -= 0.15f;
					else
						Endpos.y += 0.15f;
				}
				Ropes[RopeNum].GetComponent<LineRenderer>().SetPosition(1, Endpos);
			}
		}
	}
}
