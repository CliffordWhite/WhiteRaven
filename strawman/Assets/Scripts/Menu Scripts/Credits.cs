using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Credits : MonoBehaviour {

	int selected;
	public GameObject[] _button;
	public AudioSource _source;
	public AudioClip _accept;
	// Use this for initialization
	void Start () {
		selected = 1;
		_button [0].GetComponent<Image> ().color = Color.yellow;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.anyKeyDown) {
			if ((Input.GetKeyDown (KeyCode.Return)|| Input.GetMouseButtonDown(0)) && selected == _button.Length) {
				_source.PlayOneShot (_accept, 1.0f);
				float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
				Invoke ("PauseLoad", fadetime);
			}
		}
	}
	
	public void MouseOver(GameObject _obj)
	{
		
	}
	
	void PauseLoad()
	{
		Application.LoadLevel (0);
	}
}
