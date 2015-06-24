using UnityEngine;
using System.Collections;

public class GodModeFatal : MonoBehaviour 
{
	public GameObject player;

	void Start()
	{
		if (player == null)
			player = GameObject.FindWithTag("Player");
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
			player.SendMessage("KillAlways");
	}
}
