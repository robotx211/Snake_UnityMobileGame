using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour {

	//follows the player around, gets added when the player eats food

	public float cellWidth = 0.56f; //auto updates if changed in the snake head script

	public Vector2 lastPos;
	public Vector2 targetPos;

	public bool waitForInput = false;
	private bool waitForTransition;
	private bool died;
	private bool waitForParentMovement = true; //used to make new segments wait for their parent to move before starting to move themselves

	//will be assigned when new segemnts are spawned
	//set in editor for starting segments
	public GameObject childSegment;
	public GameObject parentSegment;

	public bool isStartingSegment = false; //assigned in editor
	private bool isParentSnakeHead = false;

	public GameObject segmentPrefab; //set in editor

	//lerp stuff
	public float lerpSpeed = 5f;
	public float lerpStartTime;

	public float lerpDistanceCovered;
	public float lerpFracJourney;

	// Use this for initialization
	void Start () {

		if (isStartingSegment == true) {
			waitForInput = true;

			lastPos = this.transform.position;
		}
		waitForTransition = false;
		died = false;

		if (parentSegment.tag == "SnakeHead") {
			isParentSnakeHead = true;
		}
	}

	void FixedUpdate (){
		//if not dead
		if (died == false) {
			//then allow movement

			//if initial input has been received
			if (waitForInput == false) {
				//then allow movement to begin

				if (isParentSnakeHead == true) {
					if (lastPos != parentSegment.GetComponent<SnakeHead> ().lastPos) {
						waitForParentMovement = false;
					}
				} else {
					if (lastPos != parentSegment.GetComponent<SnakeSegment> ().lastPos) {
						waitForParentMovement = false;
					}
				}

				if (waitForParentMovement == false) {

					//if previous transition has finished
					if (waitForTransition == false) {
						//then set new transition parameters

						lastPos = this.transform.position;

						if (isParentSnakeHead == true) {
							targetPos = parentSegment.GetComponent<SnakeHead> ().lastPos;
						} else {
							targetPos = parentSegment.GetComponent<SnakeSegment> ().lastPos;
						}

						lerpStartTime = Time.time;

						waitForTransition = true;

					}

					//continue current transition
					lerpDistanceCovered = (Time.time - lerpStartTime) * lerpSpeed;
					lerpFracJourney = lerpDistanceCovered / cellWidth;

					this.transform.position = Vector2.Lerp (lastPos, targetPos, lerpFracJourney);

					//if reached target position
					if (this.transform.position == new Vector3 (targetPos.x, targetPos.y, 0)) {
						//flag for new transition parameters
						waitForTransition = false;
					}

				} 


			}

		}
	}

	public void SpawnNewSegment (){

		//if segment already has a child, pass on segment spawn command
		if (childSegment != null) {
			childSegment.GetComponent<SnakeSegment> ().SpawnNewSegment ();
		} else /* if segment has no child, spawn a new sewgment */ {

			GameObject NewSegment = Instantiate (segmentPrefab);

			childSegment = NewSegment;

			SnakeSegment NewSegmentScript = NewSegment.GetComponent<SnakeSegment>();

			NewSegmentScript.parentSegment = this.gameObject;

			//workaround 
			if (NewSegmentScript.isStartingSegment == true) {
				NewSegmentScript.isStartingSegment = false;
			}

		}

	}

	//once input is recived on snake head, recursively calls it on all starting segments
	public void InputReceived (){
		waitForInput = false;

		if (childSegment != null) {
			childSegment.GetComponent<SnakeSegment> ().InputReceived ();
		}
	}

	public void Died() {
		died = true;

		if (childSegment != null) {
			childSegment.GetComponent<SnakeSegment> ().Died ();
		}
	}
}
