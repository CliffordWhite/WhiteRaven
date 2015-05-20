using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Achievements : MonoBehaviour {

	int selected;
	public GameObject[] _button;
	public AudioSource _SFXsource;
	public AudioSource _Musicsource;
	public AudioClip _accept;
	// Use this for initialization
	void Start () {
		selected = 1;
		_button [0].GetComponent<Image> ().color = Color.yellow;
		//check manager for volume and set to this value
		_SFXsource.volume = GameManager.manager.SFXVolume * .1f;
		_Musicsource.volume = GameManager.manager.MusicVolume * .1f;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.anyKeyDown) {
			//if back is pressed return to main
			if ((Input.GetKeyDown (KeyCode.Return)|| Input.GetMouseButtonDown(0)) && selected == _button.Length) {
				_SFXsource.PlayOneShot (_accept, 1.0f);
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
		//loads main and allows pause for SFX
		Application.LoadLevel (0);
	}
}
