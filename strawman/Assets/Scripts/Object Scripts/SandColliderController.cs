using UnityEngine;
using System.Collections;

public class SandColliderController : MonoBehaviour {

	public bool IsBox = false;
	public bool BoxFill = false;
	public float BoxFillGrowth = 0.2f;
	public float MaxFillSize = 0.0f;
	public bool RightSide = true;
	GameObject Parent = null;
	bool Done = false;

	void Start()
	{
		if (!BoxFill)
			Parent = transform.parent.gameObject;
	}

	void Update()
	{
		if (!Done) {
			if (!BoxFill && Parent.activeSelf) {
				float scaler;
				if (Parent.transform.localScale.y != 0)
					scaler = (1 - Parent.transform.localScale.x / Parent.transform.localScale.y) * 2;
				else
					scaler = 0;
			
				if (IsBox) {
					Vector3 CenterFix = transform.GetComponent<BoxCollider> ().center;
					if (RightSide) {
						if (scaler < 0) {
							transform.eulerAngles = new Vector3 (0, 0, 329.0f - 7.0f * scaler);
							CenterFix.y = 0.275f + 0.0275f * scaler;
							CenterFix.x = 0.2f - 0.025f * scaler;
						} else {
							transform.eulerAngles = new Vector3 (0, 0, 329.0f - 20.0f * scaler);
							CenterFix.y = 0.275f + 0.065f * scaler;
							CenterFix.x = 0.2f - 0.1f * scaler;
						}
					} else {
						if (scaler < 0) {
							transform.eulerAngles = new Vector3 (0, 0, 31.0f + 7.0f * scaler);
							CenterFix.y = 0.275f + 0.0275f * scaler;
							CenterFix.x = -0.2f + 0.025f * scaler;
						} else {
							transform.eulerAngles = new Vector3 (0, 0, 31.0f + 20.0f * scaler);
							CenterFix.y = 0.275f + 0.065f * scaler;
							CenterFix.x = -0.2f + 0.1f * scaler;
						}
					}
					transform.GetComponent<BoxCollider> ().center = CenterFix;
				} else {
					Vector3 CenterFix = transform.GetComponent<SphereCollider> ().center;
					if (scaler < 0) {
						CenterFix.y = 0.1f + 0.025f * scaler;
						transform.GetComponent<SphereCollider> ().radius = 0.2f + 0.0375f * scaler;
					} else {
						CenterFix.y = 0.1f + 0.1f * scaler;
						transform.GetComponent<SphereCollider> ().radius = 0.2f - 0.1f * scaler;
					}
					transform.GetComponent<SphereCollider> ().center = CenterFix;
				}
			} else if (BoxFill) {
				transform.localScale += new Vector3 (0, BoxFillGrowth * Time.deltaTime, 0);
				transform.position += new Vector3 (0, BoxFillGrowth * Time.deltaTime * 0.5f, 0);
				if (transform.localScale.y >= MaxFillSize)
					Done = true;
			}
		}
	}

	void OnTriggerEnter(Collider _obj)
	{
		if( _obj.tag == "Player" )
		{
			_obj.GetComponent<PlayerController>().IsInSand = 1;
		}

		if( _obj.tag == "SandDepth" )
		{
			_obj.transform.parent.SendMessage("KillPlayer");
		}
	}
	
	void OnTriggerExit(Collider _obj)
	{
		if( _obj.tag == "Player" )
		{
			_obj.GetComponent<PlayerController>().IsInSand = -1;
		}
	}
}
