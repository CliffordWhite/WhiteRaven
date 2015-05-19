using UnityEngine;
using System.Collections;

public class PlayerShield : MonoBehaviour
{

    //Audio
    public AudioSource FXSource;
    public AudioClip ShieldDeflect;


    public GameObject shield;

    private Vector2 mousePos;
    private Vector3 screenPos;
    void Start()
    {
        shield = GameObject.FindWithTag("Shield");
        shield.SetActive(false);
    }

    void Update()
    {

        mousePos = Input.mousePosition;
        screenPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z));

        shield.transform.eulerAngles = new Vector3(shield.transform.rotation.eulerAngles.x, shield.transform.rotation.eulerAngles.y, Mathf.Atan2((screenPos.y - shield.transform.position.y), (screenPos.x - shield.transform.position.x)) * Mathf.Rad2Deg);
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            shield.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            shield.SetActive(false);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Projectile")
        {
            FXSource.PlayOneShot(ShieldDeflect, 1.0f);
        }
    }
}