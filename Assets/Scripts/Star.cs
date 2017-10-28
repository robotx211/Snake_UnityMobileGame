using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

	//when picked up by the player, destroy every enemy

	public bool died;
	public float lifeTime; // -1 used for infinite life

	public GameObject[] enemies;

	// Use this for initialization
	void Start () {
		if (lifeTime > 0) {
			Destroy (this.gameObject, lifeTime);
		}

		died = false;
	}

	// Update is called once per frame
	void Update () {

		if (died == true) {
			Destroy (this.gameObject);
		}

	}

	public void Pickup(GameObject trigger) {

		if (trigger.tag == "SnakeHead") {
			enemies = GameObject.FindGameObjectsWithTag("Enemy");

			foreach (GameObject enemy in enemies) {
				enemy.GetComponent<Enemy> ().died = true;
			}

			died = true;
		}
	}
}
