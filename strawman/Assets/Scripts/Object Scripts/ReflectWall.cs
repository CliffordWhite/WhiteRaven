using UnityEngine;
using System.Collections;

public class ReflectWall : MonoBehaviour {

    public AudioSource FXSource;
    public AudioClip ReflectSound;

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Projectile")
        {
            FXSource.PlayOneShot(ReflectSound, 1.0f);
        }
    }
}
