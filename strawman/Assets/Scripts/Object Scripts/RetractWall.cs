using UnityEngine;
using System.Collections;

public class RetractWall : MonoBehaviour {

	
	[Header ("Movement Speed, try small numebers like 0.01")]
	public float MoveSpeed = 0.0f;
	[Header ("Don't Forget to set and end position")]
	public Vector3 EndLocation = new Vector3(0, 0, 0);
	public bool Moving = false;
	private AudioSource SFXPlayer = null;
	
	void Start ()
	{
		SFXPlayer = GetComponent<AudioSource>();
		if( Moving )
			SFXPlayer.Play();
		EndLocation.z = transform.position.z;
	}
	
	void Update()
	{
		if( Moving && !SFXPlayer.isPlaying )
			SFXPlayer.Play();
		else if( !Moving && SFXPlayer.isPlaying )
				SFXPlayer.Stop();
	}
	
	void FixedUpdate ()
	{
		Vector3 oldPos = transform.position;
		if (Moving)
			transform.position = Vector3.MoveTowards( transform.position, EndLocation, MoveSpeed );
		if (oldPos == transform.position)
			Moving = false;
	}
	
	void Trigger()
	{
		Moving = true;
		SFXPlayer.Play();
	}
}