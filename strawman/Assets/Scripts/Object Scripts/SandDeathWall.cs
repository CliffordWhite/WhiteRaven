using UnityEngine;
using System.Collections;

public class SandDeathWall : MonoBehaviour
{
	[Header ("The further off the X and Y growth rate the less accurate the collision will be")]
	public float GrowthX = 0.1f;
	public float GrowthY = 0.1f;
	public Vector3 EndSize = Vector3.zero;
	public GameObject Grate = null;
	public AudioSource SFXSource = null;
	public AudioClip SandStart = null;
	public AudioClip SandEnd = null;
	public GameObject Finisher = null;
	bool Once = true;
	bool Growing = true;

	void Start ()
	{
		if (Grate != null)
		{
			Grate.GetComponent<ParticleSystem> ().Play ();
			if (SandStart != null)
				SFXSource.PlayOneShot(SandStart);
			if (SFXSource.clip != null)
				SFXSource.Play();

		}
	}
	
	void Update ()
	{
		if (Growing)
		{
			if (Once && !SFXSource.isPlaying)
			{
				SFXSource.Play();
				Once = false;
			}

			Growing = false;
			Vector3 ScaleFix = transform.localScale;
			
			if (transform.localScale.y >= EndSize.y)
				ScaleFix.y = EndSize.y;
			else
			{
				ScaleFix.y += GrowthY * Time.deltaTime;
				Growing = true;
			}
			
			if (transform.localScale.x >= EndSize.x)
				ScaleFix.x = EndSize.x;
			else
			{
				ScaleFix.x += GrowthX * Time.deltaTime;
				Growing = true;
			}

			transform.localScale = ScaleFix;
			if (!Growing)
			{
				if (Grate != null)
				{
					Grate.GetComponent<ParticleSystem> ().Stop ();
					if (SFXSource.clip != null)
						SFXSource.Stop();
					if (SandEnd != null)
						SFXSource.PlayOneShot(SandEnd);
				}

				if (Finisher != null)
					Finisher.SetActive(true);
			}
		}
	}
    void OnTriggerEnter(Collider _obj)
    {
        if (_obj.tag == "Player")
        {
            _obj.GetComponent<PlayerController>().IsInSand = 1;
        }

        if (_obj.tag == "SandDepth")
        {
            _obj.transform.parent.SendMessage("KillPlayer");
        }
    }

    void OnTriggerExit(Collider _obj)
    {
        if (_obj.tag == "Player")
        {
            _obj.GetComponent<PlayerController>().IsInSand = -1;
        }
    }
}
