using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

	//when picked up by the player, increase length by 1

	public bool died;
	public float lifeTime;

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
			trigger.GetComponent<SnakeHead> ().SpawnNewSegment();
			died = true;
		}

	}
}
