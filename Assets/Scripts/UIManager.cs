using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	//hides and shows the relevant UI objects during the game
	//also handles button functions

	bool paused;

	public GameObject playingMenu;
	public GameObject pausedMenu;
	public GameObject diedMenu;

	// Use this for initialization
	void Start () {

		paused = false;
		playingMenu.SetActive (true);
		pausedMenu.SetActive (false);
		diedMenu.SetActive (false);

	}

	public void Pause(){

		if (paused == false) {

			playingMenu.SetActive (false);
			pausedMenu.SetActive (true);

			Time.timeScale = 0.0f;

			paused = true;
		}

	}

	public void Play(){

		if (paused == true) {

			playingMenu.SetActive (true);
			pausedMenu.SetActive (false);

			Time.timeScale = 1.0f;

			paused = false;
		}
	}

	public void Died() {

		Debug.Log ("UIManagerDied");

		playingMenu.SetActive (false);
		pausedMenu.SetActive (false);
		diedMenu.SetActive (true);
	}

	public void Retry() {

		SceneManager.LoadScene (1);
	}

	public void ToMainMenu () {

		SceneManager.LoadScene (0);
	}

}
