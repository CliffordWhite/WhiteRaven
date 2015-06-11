using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterDeathWallMain : MonoBehaviour {

	int CurrGrowth = 0;
	int CurrGrowing = 0;
	public int MaxGrowths = 0;
	[Header ("Assgin Each Water Death Wall Part here")]
	public GameObject[] Children;
	[Header ("Each Gameobject should contain each set of effects")]
	[Header ("Make sure to place them in the order you want them to activate")]
	[Header ("Null assignment means no particle effect during that growth")]
	public GameObject[] ParticleEffects;
	bool Done = false;
	
	void Start ()
	{
		
	}
	
	void Update ()
	{
		
		if(!Done && CurrGrowing == 0 )
		{
			if(CurrGrowth < MaxGrowths)
			{
				for (int i = 0; i < Children.Length; i++)
				{
					Children[i].SendMessage( "SetGrowth", CurrGrowth );
				}
				CurrGrowth++;
			}
			else
				Done = true;
		}
	}
	
	void Growing()
	{
		CurrGrowing++;
		if( CurrGrowing == 1 )
		{
			if (ParticleEffects[CurrGrowth] != null)
				ParticleEffects[CurrGrowth].GetComponent<ParticleSystem>().Play();
		}
	}
	
	void DoneGrowing()
	{
		CurrGrowing--;
		if( CurrGrowing == 0 )
		{
			if( ParticleEffects[ CurrGrowth - 1 ] != null )
				ParticleEffects[ CurrGrowth - 1 ].GetComponent<ParticleSystem>().Stop();;
		}
	}
}
