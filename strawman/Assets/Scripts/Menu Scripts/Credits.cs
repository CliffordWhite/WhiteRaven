using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Credits : MonoBehaviour {

	int selected;
	public GameObject[] _button;
	public AudioSource _SFXsource;
	public AudioSource _Musicsource;
	public AudioClip _accept;
	public AudioClip _changeSelection;
	bool transition;
	public GameObject creditText;
	// Use this for initialization
	void Start () {
		transition = false;
		selected = 0;

		_SFXsource.volume = GameManager.manager.SFXVolume * .1f;
		_Musicsource.volume = GameManager.manager.MusicVolume * .1f;
	}
	
	// Update is called once per frame
	void Update () {
		creditText.transform.localPosition = Vector3.MoveTowards (creditText.transform.localPosition, new Vector3 (-245, 480, 0), 1.0f);
		if (Input.anyKeyDown) {
			//if back is pressed return to main
			if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && selected != 0) {
				transition = true;
				_SFXsource.PlayOneShot (_accept, 1.0f);
				float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
				Invoke ("PauseLoad", fadetime);
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
				if (selected > _button.Length || selected == 0)
					selected = 1;
			}
		}
		for (int i = 1; i <= _button.Length; i++) {
			if (i == selected)
				_button [i-1].GetComponent<Image> ().color = Color.yellow;
			else
				_button [i-1].GetComponent<Image> ().color = Color.white;
			
		}
	}
	
	public void MouseOver(GameObject _obj)
	{
		if (!transition) {
			_SFXsource.PlayOneShot (_changeSelection, 1.0f);
			selected = 1;
			_obj.GetComponent<Image> ().color = Color.yellow;
		}
	}

	public void MouseLeave(GameObject _obj)
	{
		if (!transition) {
			_obj.GetComponent<Image>().color = Color.white;
			selected = 0;
		}
	}

	
	void PauseLoad()
	{
		//loads main and allows pause for SFX
		if (GameManager.manager.endGameTransition) {
			GameManager.manager.endGameTransition = false;
			Application.LoadLevel(1);
		}
		else
			Application.LoadLevel (0);
	}
}
