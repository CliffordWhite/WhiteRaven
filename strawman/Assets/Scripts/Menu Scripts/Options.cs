using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Options : MonoBehaviour {

	int selected;
	public GameObject[] _button;
	public AudioSource _SFXsource;
	public AudioSource _Musicsource;
	public AudioClip _accept;
	public AudioClip _changeSelection;
	public GameObject MusicSelector;
	public GameObject SFXSelector;
	public GameObject fullscreenCheck;
	bool transition;
	// Use this for initialization
	void Start () {
		transition = false;
		selected = 0;
		//position selectors to volumes currently selected
		MusicSelector.GetComponent<Slider> ().value = GameManager.manager.MusicVolume * 0.1f;
		SFXSelector.GetComponent<Slider> ().value = GameManager.manager.SFXVolume * 0.1f;
	
		_SFXsource.volume = GameManager.manager.SFXVolume * 0.1f;
		_Musicsource.volume = GameManager.manager.MusicVolume * 0.1f;
		fullscreenCheck.SetActive (GameManager.manager.isFullscreen);
		
	}
	
	// Update is called once per frame
	void Update () {
		//if not changing volumes allow moving through menu
		if (Input.anyKeyDown) {
			if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && selected != 0) {
				if (selected == _button.Length){
					transition = true;
					_SFXsource.PlayOneShot(_accept, 1.0f);
					float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
					Invoke("PauseLoad",fadetime);
				}
				else if (selected == 3) {
					_SFXsource.PlayOneShot(_changeSelection,1.0f);
					GameManager.manager.isFullscreen = !GameManager.manager.isFullscreen;
					fullscreenCheck.SetActive(GameManager.manager.isFullscreen);
					Screen.fullScreen = GameManager.manager.isFullscreen;
				}
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
		//if in volume change disable movement through menu and allow change of selected volume

	}
	//move selection based on mouse position and disable highlight from keyboard traversal
	public void MouseOver(GameObject _obj)
	{
		if (!transition) {
			_SFXsource.PlayOneShot (_changeSelection, 1.0f);
			for (int i = 1; i <= _button.Length; i++) {
				if (_obj == _button [i - 1]) {
					selected = i;
					break;
				}
			}
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

	public void ChangeMusicVolume(){
		GameManager.manager.MusicVolume = MusicSelector.GetComponent<Slider> ().value * 10.0f;
		_Musicsource.volume = MusicSelector.GetComponent<Slider> ().value;
	}	
	public void ChangeSFXVolume(){
		_SFXsource.PlayOneShot (_changeSelection, 1.0f);
		GameManager.manager.SFXVolume = SFXSelector.GetComponent<Slider> ().value * 10.0f;
		_SFXsource.volume = SFXSelector.GetComponent<Slider> ().value;
	}

	void PauseLoad()
	{
		Application.LoadLevel (0);
	}
}
