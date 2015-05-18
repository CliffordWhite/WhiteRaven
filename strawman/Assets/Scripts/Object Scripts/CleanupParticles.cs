using UnityEngine;
using System.Collections;

public class CleanupParticles : MonoBehaviour 
{
	void Update () 
	{
		if (gameObject.GetComponent<ParticleSystem> ().isStopped)
			Destroy (gameObject);
	}
}
