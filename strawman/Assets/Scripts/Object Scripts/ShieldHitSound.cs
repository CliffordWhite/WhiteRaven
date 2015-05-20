using UnityEngine;
using System.Collections;

public class ShieldHitSound : MonoBehaviour {

    //Audio
    public AudioSource FXSource;
    public AudioClip ShieldDeflectSound;
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Projectile")
            FXSource.PlayOneShot(ShieldDeflectSound, 1.0f);
    }
}
