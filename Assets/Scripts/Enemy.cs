using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	//when touched by the player, kills the player

	public bool died;
	public float lifeTime; //if lifetime is set to less than 0, the lifetime becomes infinite

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
}
