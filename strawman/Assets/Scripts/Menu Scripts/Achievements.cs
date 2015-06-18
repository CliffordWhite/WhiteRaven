using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Achievements : MonoBehaviour {

	int selected;
	public GameObject[] _button;
	public AudioSource _SFXsource;
	public AudioSource _Musicsource;
	public AudioClip _accept;
	public GameObject[] achieves;
	public AudioClip _changeSelection;
	public GameObject BeatText;
	public GameObject BeatHardText;
	public GameObject AllTreasureText;
	bool transition;
	int levelBeat, treasureCol, hardLevelBeat;
	// Use this for initialization
	void Start () {
		transition = false;
		selected = 0;
		//check manager for volume and set to this value
		_SFXsource.volume = GameManager.manager.SFXVolume * .1f;
		_Musicsource.volume = GameManager.manager.MusicVolume * .1f;
		for (int i = 0; i < achieves.Length-1; i++) {
			if (GameManager.manager.achieveList[i])
				achieves[i].SetActive(true);
		}

		levelBeat = 0;
		for (int c = 0; c < GameManager.manager.levelCompleted.Length; c++) {
			if (GameManager.manager.levelCompleted[c]) 
				levelBeat++;
		}

		hardLevelBeat = 0;
		if (GameManager.manager.hardModeOn) {
			for (int c = 0; c < GameManager.manager.levelCompleted.Length; c++) {
				if (GameManager.manager.levelCompleted [c]) 
					hardLevelBeat++;
			}
		}

		treasureCol = 0;
		for (int t = 0; t < GameManager.manager.treasureCollected.Length; t++) {
			if (GameManager.manager.treasureCollected[t])
				treasureCol++;
		}
		for (int st = 0; st < GameManager.manager.secrettreasureCollected.Length; st++) {
			if (GameManager.manager.secrettreasureCollected[st])
				treasureCol++;
		}
		BeatText.GetComponent<Text>().text = "Beat All Levels \n\t("+levelBeat+"/15)";
		BeatHardText.GetComponent<Text>().text = "Beat All Levels (Hard) \n\t("+hardLevelBeat+"/15)";
		AllTreasureText.GetComponent<Text>().text = "Collect All Treasures \n\t("+treasureCol+"/20)";
	}
	
	// Update is called once per frame
	void Update () {

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
		Application.LoadLevel (0);
	}
}
