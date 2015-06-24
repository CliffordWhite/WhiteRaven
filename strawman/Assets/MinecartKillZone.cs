using UnityEngine;
using System.Collections;

public class MinecartKillZone : MonoBehaviour 
{
	public GameObject player;	//attach player to make sending kill message easy

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "MineCart")
			player.SendMessage("KillAlways");
	}
}
 