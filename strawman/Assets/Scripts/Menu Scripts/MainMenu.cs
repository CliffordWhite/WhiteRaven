using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

	int selected;
	public GameObject[] _button;
	public AudioSource _source;
	public AudioClip _accept;
	public AudioClip _changeSelection;
	// Use this for initialization
	void Start () {
		selected = 1;
		_button [0].GetComponent<Image> ().color = Color.yellow;

	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.anyKeyDown) {
			if (Input.GetKeyDown(KeyCode.Return)) {
				_source.PlayOneShot(_accept, 1.0f);
				Invoke("PauseLoad",.8f);
			}
			if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
				_source.PlayOneShot(_changeSelection,1.0f);
				selected--;
				if (selected <= 0)
					selected = _button.Length;
			}
			if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
				_source.PlayOneShot(_changeSelection);
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
		for (int i = 1; i <= _button.Length; i++) {
			if (_obj == _button[i-1]) {
				selected = i;
			}
			_button [i-1].GetComponent<Image> ().color = Color.white;			
		}
		_obj.GetComponent<Image> ().color = Color.yellow;
	}

	void PauseLoad()
	{
		Application.LoadLevel (selected);
	}

}
