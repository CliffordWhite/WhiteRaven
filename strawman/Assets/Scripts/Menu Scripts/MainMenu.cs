using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

	int selected;
	public GameObject[] _button;
	public AudioSource _SFXsource;
	public AudioSource _Musicsource;
	public AudioClip _accept;
	public AudioClip _changeSelection;
	bool transition;
	// Use this for initialization
	void Start () {
		transition = false;
		selected = 1;
		_button [0].GetComponent<Image> ().color = Color.yellow;

		_SFXsource.volume = GameManager.manager.SFXVolume * .1f;
		_Musicsource.volume = GameManager.manager.MusicVolume * .1f;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.anyKeyDown && !transition) {
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) {
				transition = true;
				_SFXsource.PlayOneShot(_accept, 1.0f);
				float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
				Invoke("PauseLoad",fadetime);
			}
			if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
				_SFXsource.PlayOneShot(_changeSelection,1.0f);
				selected--;
				if (selected <= 0)
					selected = _button.Length;
			}
			if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
				_SFXsource.PlayOneShot(_changeSelection);
				selected++;
				if (selected > _button.Length)
					selected = 1;
			}
			for (int i = 1; i <= _button.Length; i++) {
				if (i == selected)
					_button [i-1].GetComponent<Image> ().color = Color.yellow;
				else
					_button [i-1].GetComponent<Image> ().color = Color.white;
				
			}
		}
	}

	public void MouseOver(GameObject _obj)
	{
		if (!transition) {
			_SFXsource.PlayOneShot (_changeSelection, 1.0f);
			for (int i = 1; i <= _button.Length; i++) {
				if (_obj == _button [i - 1]) {
					selected = i;
				}
				_button [i - 1].GetComponent<Image> ().color = Color.white;			
			}
			_obj.GetComponent<Image> ().color = Color.yellow;
		}
	}

	void PauseLoad()
	{
		// Start level 1 if play is selected
		// once levelSelect is created, remove if statement
		//if (selected == 1)
		//	Application.LoadLevel (6);
		//else

		// quit the game if exit was selected
        if (selected == 6)
            Application.Quit();
        else if (selected != 1)
            Application.LoadLevel(selected);
        else if (selected == 1)
            Application.LoadLevel(21);
	}

}
