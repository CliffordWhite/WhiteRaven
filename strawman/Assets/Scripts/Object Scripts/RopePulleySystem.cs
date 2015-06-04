using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class RopePulleySystem : MonoBehaviour {

	[Header ("Try Using Small Numbers Like 0.05f")]
	public float MoveSpeed = 0.05f;
	[Header ("Object One Settings. - Primary Object")]
	public GameObject ObjectOne = null;
	Vector3 StartPosOne = Vector3.zero;
	public Vector3 EndPosOne = Vector3.zero;
	
	[Header ("Object Two Settings. - Secondary Object")]
	public GameObject ObjectTwo = null;
	Vector3 StartPosTwo = Vector3.zero;
	public Vector3 EndPosTwo = Vector3.zero;
	float ObjTwoMoveSpeed = 0.0f;
	
	[Header ("Starting Positions Will Auto Set")]
	public bool Moving = false;
	bool MovingForward = true;
	
	[Header ("Place Cogs In Order From Primary Obj to Secondary")]
	public GameObject[] Cogs;
	[Header ("Set Size = to NumCogs: Turn On If Cog Rotates Wrong Way")]
	public bool[] CogRotationFix;
	float[] CogFix;
	
	[Header ("Place Rope Anchors In Order Starting From Primary Obj")]
	public GameObject[] Anchors;
	GameObject[] AnchorsList;
	
	[Header ("Place Rope Prefab Here.")]
	public GameObject RopeObject = null;
	GameObject[] Ropes;
	
	AudioSource SFXPlayer = null;
	void Start ()
	{
		if (ObjectOne == null || ObjectTwo == null)
		{
			Debug.LogError("Rope Pulley System Scrip Requires both objects to have a refrence!");
			Destroy(this);
		} else
		{
			StartPosOne = ObjectOne.transform.localPosition;	
			StartPosTwo = ObjectTwo.transform.localPosition;	
			ObjTwoMoveSpeed = ((StartPosTwo - EndPosTwo).magnitude / (StartPosOne - EndPosOne).magnitude) * MoveSpeed;
			
			AnchorsList = new GameObject[Cogs.Length + 2];
			for (int i = 0; i < AnchorsList.Length; i++)
			{
				if (i == 0)
					AnchorsList[i] = ObjectOne;
				else if (i == AnchorsList.Length - 1)
					AnchorsList[i] = ObjectTwo;
				else
					AnchorsList[i] = Anchors[i - 1];
			}
			
			CogFix = new float[Cogs.Length];
			for (int i = 0; i < Cogs.Length; i++)
			{
				if (CogRotationFix[i])
					CogFix[i] = -1.0f;
				else
					CogFix[i] = 1.0f;
			}
			Ropes = new GameObject[AnchorsList.Length - 1];
			for (int i = 0; i < Ropes.Length; i++)
			{
				Ropes[i] = Instantiate(RopeObject);
				Ropes[i].transform.parent = transform;
				Ropes[i].transform.position = Vector3.zero;
			}
			
			Vector3 LastPos = Vector3.zero;
			for (int RopeNum = 0; RopeNum < Ropes.Length; RopeNum++)
			{
				Vector3	Startpos = LastPos;
				if (RopeNum == 0)
				{
					Startpos = AnchorsList[RopeNum].transform.position;
					Startpos.z += 0.1f;
				}
				Ropes[RopeNum].GetComponent<LineRenderer>().SetPosition(0, Startpos);
				
				Vector3	Endpos;
				if (RopeNum == AnchorsList.Length - 1)
				{
					Endpos = ObjectTwo.transform.position;
					Endpos.z += 0.1f;
				}
				else
				{
					Endpos = AnchorsList[RopeNum + 1].transform.position;
					Endpos.z += 0.1f;	
				}
				LastPos = Endpos;
				Ropes[RopeNum].GetComponent<LineRenderer>().SetPosition(1, Endpos);
			}
			
			SFXPlayer = GetComponent<AudioSource>();
		}
	}
	
	void FixedUpdate ()
	{
		if (Moving)
		{
			if (!SFXPlayer.isPlaying)
					SFXPlayer.Play();
				
			if (MovingForward)
			{
				for (int i = 0; i < Cogs.Length; i++)
					Cogs[i].transform.RotateAround(Cogs[i].transform.position, new Vector3(0,0,1.0f),  -CogFix[i] * 135.0f * MoveSpeed);
				ObjectOne.transform.localPosition = Vector3.MoveTowards(ObjectOne.transform.localPosition, EndPosOne, MoveSpeed);
				ObjectTwo.transform.localPosition = Vector3.MoveTowards(ObjectTwo.transform.localPosition, EndPosTwo, ObjTwoMoveSpeed);
				if((ObjectOne.transform.localPosition - EndPosOne).magnitude <= 0.001f)
				{
					Moving = false;
					SFXPlayer.Stop();
					MovingForward = false;
				}
			}
			else
			{
				for (int i = 0; i < Cogs.Length; i++)
					Cogs[i].transform.RotateAround(Cogs[i].transform.position, new Vector3(0,0,1.0f),  CogFix[i] * 135.0f * MoveSpeed);
				ObjectOne.transform.localPosition = Vector3.MoveTowards(ObjectOne.transform.localPosition, StartPosOne, MoveSpeed);
				ObjectTwo.transform.localPosition = Vector3.MoveTowards(ObjectTwo.transform.localPosition, StartPosTwo, ObjTwoMoveSpeed);
				if((ObjectOne.transform.localPosition - StartPosOne).magnitude <= 0.001f)
				{
					Moving = false;
					SFXPlayer.Stop();
					MovingForward = true;
				}
			}
			Vector3 LastPos = Vector3.zero;
			for (int RopeNum = 0; RopeNum < Ropes.Length; RopeNum++)
			{
				Vector3	Startpos = LastPos;
				if (RopeNum == 0)
				{
					Startpos = AnchorsList[RopeNum].transform.position;
					Startpos.z += 0.1f;
				}
				Ropes[RopeNum].GetComponent<LineRenderer>().SetPosition(0, Startpos);
					
				Vector3	Endpos;
				if (RopeNum == AnchorsList.Length - 1)
				{
					Endpos = ObjectTwo.transform.position;
					Endpos.z += 0.1f;
				}
				else
				{
					Endpos = AnchorsList[RopeNum + 1].transform.position;
					Endpos.z += 0.1f;	
				}
				LastPos = Endpos;
				Ropes[RopeNum].GetComponent<LineRenderer>().SetPosition(1, Endpos);
			}
		}
		else if (SFXPlayer.isPlaying)
					SFXPlayer.Stop();
	}
	
	void GoForward(bool _Forward)
	{
		MovingForward = _Forward;
	}
}
