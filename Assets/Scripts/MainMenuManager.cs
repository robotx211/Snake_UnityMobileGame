using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	//hides and shows the relevant UI objects in the main menu
	//also handles button functions

	bool inOpeningMenu;

	public GameObject openingMenu;
	public GameObject gameSelectMenu;

	// Use this for initialization
	void Start () {

		inOpeningMenu = true;

	}

	public void ShowGameSelect(){

		if (inOpeningMenu == true) {

			openingMenu.SetActive (false);
			gameSelectMenu.SetActive (true);

			inOpeningMenu = false;
		}

	}

	public void ShowMainMenu(){

		if (inOpeningMenu == false) {

			openingMenu.SetActive (true);
			gameSelectMenu.SetActive (false);

			inOpeningMenu = true;
		}
	}

	public void LoadGameMode1 (){

		//regular game

		SceneManager.LoadScene (1);

	}

	public void LoadGameMode2 (){

		//time trial game
		//not yet implemented

	}
}
