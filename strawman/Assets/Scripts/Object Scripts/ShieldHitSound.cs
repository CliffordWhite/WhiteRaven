using UnityEngine;
using System.Collections;

public class ShieldHitSound : MonoBehaviour {

    //Audio
    public AudioSource FXSource;
    public AudioClip ShieldDeflect;
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Projectile")
            FXSource.PlayOneShot(ShieldDeflect, 1.0f);
    }
}
