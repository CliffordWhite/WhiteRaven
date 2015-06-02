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
	bool MusicSel;
	bool SFXSel;
	bool transition;
	// Use this for initialization
	void Start () {
		transition = false;
		selected = 1;
		_button [0].GetComponent<Image> ().color = Color.yellow;
		//position selectors to volumes currently selected
		MusicSelector.GetComponent<RectTransform>().position = new Vector3(MusicSelector.GetComponent<RectTransform>().position.x +(10*(GameManager.manager.MusicVolume-5)),
		                                                                   MusicSelector.GetComponent<RectTransform>().position.y,
		                                                                   MusicSelector.GetComponent<RectTransform>().position.z);
		SFXSelector.GetComponent<RectTransform>().position = new Vector3(SFXSelector.GetComponent<RectTransform>().position.x +(10*(GameManager.manager.SFXVolume-5)),
		                                                                   SFXSelector.GetComponent<RectTransform>().position.y,
		                                                                   SFXSelector.GetComponent<RectTransform>().position.z);
		MusicSel = false;
		SFXSel = false;
		_SFXsource.volume = GameManager.manager.SFXVolume * 0.1f;
		_Musicsource.volume = GameManager.manager.MusicVolume * 0.1f;
		fullscreenCheck.SetActive (GameManager.manager.isFullscreen);
		
	}
	
	// Update is called once per frame
	void Update () {
		//if not changing volumes allow moving through menu
		if (Input.anyKeyDown && !MusicSel && !SFXSel) {
			if ((Input.GetKeyDown(KeyCode.Return)|| Input.GetMouseButtonDown(0))) {
				if (selected == _button.Length){
					transition = true;
					_SFXsource.PlayOneShot(_accept, 1.0f);
					float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
					Invoke("PauseLoad",fadetime);
				}
				else if (selected == 1) {
					_SFXsource.PlayOneShot(_changeSelection,1.0f);
					MusicSel = true;
					_Musicsource.Play();
				}
				else if (selected == 2) {
					_SFXsource.PlayOneShot(_changeSelection,1.0f);
					SFXSel = true;
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
		//if in volume change disable movement through menu and allow change of selected volume
		else if (Input.anyKeyDown && MusicSel) {
			if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.Escape)) {
				_SFXsource.PlayOneShot(_changeSelection,1.0f);
				MusicSel = false;
				_Musicsource.Stop();
			}
			if ((Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow)) && _Musicsource.volume < 1.0f) {
				MusicSelector.GetComponent<RectTransform>().position = new Vector3(MusicSelector.GetComponent<RectTransform>().position.x+10,
				                                                                   MusicSelector.GetComponent<RectTransform>().position.y,
				                                                                   MusicSelector.GetComponent<RectTransform>().position.z);
				GameManager.manager.MusicVolume +=1;
				_Musicsource.volume =GameManager.manager.MusicVolume*0.1f;

			}
			if ((Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow)) &&_Musicsource.volume > 0.0f) {
				MusicSelector.GetComponent<RectTransform>().position = new Vector3(MusicSelector.GetComponent<RectTransform>().position.x-10,
				                                                                   MusicSelector.GetComponent<RectTransform>().position.y,
				                                                                   MusicSelector.GetComponent<RectTransform>().position.z);
				GameManager.manager.MusicVolume -=1;
				_Musicsource.volume =GameManager.manager.MusicVolume*0.1f;
			}
		}
		else if (Input.anyKeyDown && SFXSel) {
			if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.Escape)) {
				_SFXsource.PlayOneShot(_changeSelection,1.0f);
				SFXSel = false;
			}
			if ((Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow)) &&_SFXsource.volume < 1.0f) {
				SFXSelector.GetComponent<RectTransform>().position = new Vector3(SFXSelector.GetComponent<RectTransform>().position.x+10,
				                                                                   SFXSelector.GetComponent<RectTransform>().position.y,
				                                                                   SFXSelector.GetComponent<RectTransform>().position.z);
				GameManager.manager.SFXVolume +=1;
				_SFXsource.volume =GameManager.manager.SFXVolume*0.1f;
				_SFXsource.PlayOneShot(_changeSelection,1.0f);
			}
			if ((Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow))&&_SFXsource.volume > 0.0f) {
				SFXSelector.GetComponent<RectTransform>().position = new Vector3(SFXSelector.GetComponent<RectTransform>().position.x-10,
				                                                                 SFXSelector.GetComponent<RectTransform>().position.y,
				                                                                 SFXSelector.GetComponent<RectTransform>().position.z);
				GameManager.manager.SFXVolume -=1;
				_SFXsource.volume =GameManager.manager.SFXVolume*0.1f;
				_SFXsource.PlayOneShot(_changeSelection,1.0f);
			}
		}
	}
	//move selection based on mouse position and disable highlight from keyboard traversal
	public void MouseOver(GameObject _obj)
	{
		if (!MusicSel && !SFXSel && !transition) {
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
		Application.LoadLevel (0);
	}
}
