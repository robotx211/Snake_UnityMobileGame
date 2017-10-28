using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	//handles all spawners 

	public bool startSpawning;
	public bool died;

	// Use this for initialization
	void Start () {
		startSpawning = false;
		died = false;

	}
	
	// Update is called once per frame
	void Update () {

		if (startSpawning == true) {
			if (died == false) {
				this.GetComponent<FoodSpawner> ().SpawnUpdate ();
				this.GetComponent<EnemySpawner> ().SpawnUpdate ();
				this.GetComponent<StarSpawner> ().SpawnUpdate ();

			}
		}
			
	}

	public void Died () {

		this.died = true;

	}

	public void StartSpawning () {

		//stops spawn counters starting before first player input

		this.startSpawning = true;

		this.GetComponent<FoodSpawner> ().ActivateSpawner ();
		this.GetComponent<EnemySpawner> ().ActivateSpawner ();
		this.GetComponent<StarSpawner> ().ActivateSpawner ();
	}
}
