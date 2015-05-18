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
	bool MusicSel;
	bool SFXSel;
	// Use this for initialization
	void Start () {
		selected = 1;
		_button [0].GetComponent<Image> ().color = Color.yellow;
		MusicSelector.GetComponent<RectTransform>().position = new Vector3(MusicSelector.GetComponent<RectTransform>().position.x +(10*(GameManager.manager.MusicVolume-5)),
		                                                                   MusicSelector.GetComponent<RectTransform>().position.y,
		                                                                   MusicSelector.GetComponent<RectTransform>().position.z);
		SFXSelector.GetComponent<RectTransform>().position = new Vector3(SFXSelector.GetComponent<RectTransform>().position.x +(10*(GameManager.manager.SFXVolume-5)),
		                                                                   SFXSelector.GetComponent<RectTransform>().position.y,
		                                                                   SFXSelector.GetComponent<RectTransform>().position.z);
		MusicSel = false;
		SFXSel = false;
		_SFXsource.volume = GameManager.manager.SFXVolume * .1f;
		_Musicsource.volume = GameManager.manager.MusicVolume * .1f;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.anyKeyDown && !MusicSel && !SFXSel) {
			if ((Input.GetKeyDown(KeyCode.Return)|| Input.GetMouseButtonDown(0))) {
				if (selected == _button.Length){
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
		else if (Input.anyKeyDown && MusicSel) {
			if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.Escape)) {
				_SFXsource.PlayOneShot(_changeSelection,1.0f);
				MusicSel = false;
				_Musicsource.Stop();
			}
			if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow)) {
				MusicSelector.GetComponent<RectTransform>().position = new Vector3(MusicSelector.GetComponent<RectTransform>().position.x+10,
				                                                                   MusicSelector.GetComponent<RectTransform>().position.y,
				                                                                   MusicSelector.GetComponent<RectTransform>().position.z);
				GameManager.manager.MusicVolume +=1;
				_Musicsource.volume += .1f;

			}
			if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow)) {
				MusicSelector.GetComponent<RectTransform>().position = new Vector3(MusicSelector.GetComponent<RectTransform>().position.x-10,
				                                                                   MusicSelector.GetComponent<RectTransform>().position.y,
				                                                                   MusicSelector.GetComponent<RectTransform>().position.z);
				GameManager.manager.MusicVolume -=1;
				_Musicsource.volume -= .1f;
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
				_SFXsource.volume += .1f;
				_SFXsource.PlayOneShot(_changeSelection,1.0f);
			}
			if ((Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow))&&_SFXsource.volume > .0f) {
				SFXSelector.GetComponent<RectTransform>().position = new Vector3(SFXSelector.GetComponent<RectTransform>().position.x-10,
				                                                                 SFXSelector.GetComponent<RectTransform>().position.y,
				                                                                 SFXSelector.GetComponent<RectTransform>().position.z);
				GameManager.manager.SFXVolume -=1;
				_SFXsource.volume -= .1f;
				_SFXsource.PlayOneShot(_changeSelection,1.0f);
			}
		}
	}
	
	public void MouseOver(GameObject _obj)
	{
		if (!MusicSel && !SFXSel) {
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
