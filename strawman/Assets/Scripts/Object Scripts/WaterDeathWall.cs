using UnityEngine;
using System.Collections;

public class WaterDeathWall : MonoBehaviour {

	float PosToScale = 2.9f;
	Vector3 posChange = Vector3.zero;
	int CurrentGrowth = 0;
	int ActualGrowth = 0;
	bool growing = false;
	
	public GameObject Parent = null;
	[Header ("For Each Growth, Set End Scale")]
	public float[] EndYScales;
	[Header ("For Each Growth, Set Order")]
	public int[] GrowthOrders;
	[Header ("Set Each Growth Speed")]
	public float[] GrowthSpeeds;
	Vector3[] YGrowths;
	
	void Start ()
	{
		YGrowths = new Vector3[GrowthSpeeds.Length];
		for( int i = 0; i < YGrowths.Length; i++ )
		{
			YGrowths[i] = new Vector3(0.0f, GrowthSpeeds[i], 0.0f);
		}
	}
	
	void Update ()
	{
		if( growing )
		{
			transform.localScale += YGrowths[ActualGrowth] * Time.deltaTime;
			if(transform.localScale.y >= EndYScales[ActualGrowth] )
			{
				Vector3 yclamp = transform.localScale;
				yclamp.y = EndYScales[ActualGrowth];
				transform.localScale =	yclamp;
				growing = false;
				Parent.SendMessage("DoneGrowing");
			}
			posChange.y = PosToScale * YGrowths[ActualGrowth].y * Time.deltaTime;
			transform.position += posChange;
		}
	}
	
	void OnTriggerEnter(Collider _obj)
	{
		if (_obj.tag == "PlayerBody")
		{
			_obj.transform.parent.GetComponent<PlayerController> ().WaterMode = 1;	
		}
		else if (_obj.tag == "PlayerHead")
		{
			_obj.transform.parent.GetComponent<PlayerController> ().isDrowning = true;
		}
	}
	
	void OnTriggerExit(Collider _obj)
	{
		if( _obj.tag == "PlayerBody" )
		{
			_obj.transform.parent.GetComponent<PlayerController>().WaterMode = -1;	
		}
		else if (_obj.tag == "PlayerHead")
		{
			_obj.transform.parent.GetComponent<PlayerController> ().isDrowning = false;
		}
	}
	
	void SetGrowth(int _value)
	{
		CurrentGrowth = _value;
		growing = false;
		for( int i = 0; i < GrowthOrders.Length; i++ )
		{
			if (GrowthOrders[i] == CurrentGrowth)
			{
				ActualGrowth = i;
				growing = true;
				Parent.SendMessage("Growing");
				break;
			}
		}
	}
}
