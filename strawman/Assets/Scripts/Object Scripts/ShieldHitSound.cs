using UnityEngine;
using System.Collections;

public class ShieldHitSound : MonoBehaviour {

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Projectile")
            GetComponent<AudioSource>().Play();
    }
}
