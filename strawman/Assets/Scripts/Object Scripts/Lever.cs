using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {

	public Vector3 tarPosition;
    public Vector3 OldPosition;
    public GameObject LeverObject;
	bool movingUp,movingDown;
	public float speed;
	Lever[] m_Levers;
	public int hastriggered;
	// Use this for initialization
	void Start () {
		hastriggered = 0;
		m_Levers = GetComponents<Lever> ();
		movingUp = false;
		movingDown = false;
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Lever t_lever in m_Levers) {
			if (t_lever.movingUp) {
				t_lever.LeverObject.transform.position = Vector3.MoveTowards (t_lever.LeverObject.transform.position, t_lever.tarPosition, t_lever.speed * Time.deltaTime);
				if (Vector3.Distance (t_lever.LeverObject.transform.position, t_lever.tarPosition) <= .01f) {
					t_lever.LeverObject.transform.position = t_lever.tarPosition;
					t_lever.movingUp = false;
				}
			} else if (t_lever.movingDown) {
				t_lever.LeverObject.transform.position = Vector3.MoveTowards (t_lever.LeverObject.transform.position, t_lever.OldPosition, t_lever.speed * Time.deltaTime);
				if (Vector3.Distance (t_lever.LeverObject.transform.position, t_lever.OldPosition) <= .01f) {
					t_lever.LeverObject.transform.position = t_lever.OldPosition;
					t_lever.movingDown = false;
				}
			}
		}
	}
    void FixedUpdate()
    {
        
    }


    public void HasMoved()
    {   
		hastriggered++;
		foreach (Lever t_lever in m_Levers) {
			if (t_lever.LeverObject.transform.position == t_lever.OldPosition || t_lever.movingDown) {
				t_lever.movingUp = true;
				t_lever.movingDown = false;
			} else if (t_lever.LeverObject.transform.position == t_lever.tarPosition || t_lever.movingUp) {
				t_lever.movingUp = false;
				t_lever.movingDown = true;
			}
		}
    }
}
