using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeHead : MonoBehaviour {

	//moves per pixel (ie teleports to next grid cell

	public float cellWidth = 0.56f;
	//grid is 19x29

	public Vector2 lastPos;
	public Vector2 targetPos;
	private Vector2 moveDirection;
	private Vector2 lastMoveDirection;

	private bool waitForInput;
	private bool waitForTransition;
	private bool died;

	public GameObject childSegment;

	public GameObject segmentPrefab; //set in editor

	//lerp stuff
	public float lerpSpeed = 5f;
	public float lerpStartTime;

	public float lerpDistanceCovered;
	public float lerpFracJourney;

	public int length;
	private float startTime;
	public float timeAlive;


	public GameObject SpawnManager;

	public Text lengthLabel;
	public Text timeLabel;

	public Text finalLengthLabel;
	public Text finalTimeLabel;

	public GameObject UIManager;

	// Use this for initialization
	void Start () {
		//defaults to looking right, so starting move must be up, down or right
		moveDirection = new Vector2 (1, 0);
		waitForInput = true;
		waitForTransition = false;
		died = false;

		length = 6;
		timeAlive = 0f;


	}
	
	// Update is called once per frame
	void Update () {

		if (died == false) {

			UserInput ();

		}

	}

	void FixedUpdate (){

		//if not dead
		if (died == false) {
			//then allow movement

			//if initial input has been received
			if (waitForInput == false) {
				//then allow movement to begin

				//if previous transition has finished
				if (waitForTransition == false) {
					//then set new transition parameters
					lastPos = this.transform.position;
					lastMoveDirection = moveDirection;
					targetPos = lastPos + (moveDirection * cellWidth);
					lerpStartTime = Time.time;

					waitForTransition = true;
				}

				//continue current transition
				lerpDistanceCovered = (Time.time - lerpStartTime) * lerpSpeed;
				lerpFracJourney = lerpDistanceCovered / cellWidth;

				this.transform.position = Vector2.Lerp (lastPos, targetPos, lerpFracJourney);

				//if reached target position
				if (this.transform.position == new Vector3(targetPos.x, targetPos.y, 0)) {
					//flag for new transition parameters
					waitForTransition = false;
				}

				timeAlive = Time.time - startTime;
				lengthLabel.text = length.ToString () + "m";
				timeLabel.text = timeAlive.ToString("0.0") + "s"; //rounds to 1dp

			}

		}
	}

	void UserInput() {

		#if UNITY_STANDALONE

		//keyboard controls
		if ( Input.GetKey ("up") && lastMoveDirection != new Vector2(0, -1)) {
			moveDirection.Set (0, 1);
			if (waitForInput == true) {
				FirstInput ();
			}
		}
		if (Input.GetKey ("right") && lastMoveDirection != new Vector2(-1, 0)) {
			moveDirection.Set (1, 0);
			if (waitForInput == true) {
				FirstInput ();
			}
		}
		if (Input.GetKey ("down") && lastMoveDirection != new Vector2(0, 1)) {
			moveDirection.Set (0, -1);
			if (waitForInput == true) {
				FirstInput ();
			}
		}
		if (Input.GetKey ("left") && lastMoveDirection != new Vector2(1, 0)) {
			moveDirection.Set (-1, 0);
			if (waitForInput == true) {
				FirstInput ();
			}
		}

		//debug code
		if (Input.GetKeyDown ("space")) {
			SpawnNewSegment ();
		}

		#else

		if (Input.touchCount > 0) {

		float touchX = Input.GetTouch (0).position.x - (Screen.width / 2);
		float touchY = Input.GetTouch (0).position.y - (Screen.height / 2);

			//up
			if ( touchY >= touchX && touchY >= -touchX
				&& lastMoveDirection != new Vector2(0, -1)) {
				moveDirection.Set (0, 1);
				if (waitForInput == true) {
					FirstInput ();
				}
			}
			//right
			if ( touchX > touchY && touchX > -touchY
				&& lastMoveDirection != new Vector2(-1, 0)) {
				moveDirection.Set (1, 0);
				if (waitForInput == true) {
					FirstInput ();
				}
			}
			//down
			if ( touchY <= touchX && touchY <= -touchX
				&& lastMoveDirection != new Vector2(0, 1)) {
				moveDirection.Set (0, -1);
				if (waitForInput == true) {
					FirstInput ();
				}
			}
			//left
			if ( touchX < touchY && touchX < -touchY
				&& lastMoveDirection != new Vector2(1, 0)) {
				moveDirection.Set (-1, 0);
				if (waitForInput == true) {
					FirstInput ();
				}
			}
		}

		#endif

	}

	void FirstInput (){
		waitForInput = false;
		childSegment.GetComponent<SnakeSegment> ().InputReceived ();
		SpawnManager.GetComponent<SpawnManager> ().StartSpawning ();
		startTime = Time.time;
	}

	public void SpawnNewSegment() {

		if (childSegment != null) {
			//if has child, pass spawn command down
			childSegment.GetComponent<SnakeSegment> ().SpawnNewSegment ();
		} else {
			//if not, spawn it's own child

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

	void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.tag == "Wall") {
			Died ();
		}
		if (other.gameObject.tag == "Enemy") {

			Died ();
		}
		if (other.gameObject.tag == "SnakeSegment") {
			Died ();
		}
		if (other.gameObject.tag == "Food") {

			other.GetComponent<Food> ().Pickup (this.gameObject);
			length++;

		}
		if (other.gameObject.tag == "Star") {

			other.GetComponent<Star> ().Pickup (this.gameObject);

		}


	}

	public void Died() {
		//handles everything that happens when the player dies
		died = true;

		if (childSegment != null) {
			childSegment.GetComponent<SnakeSegment> ().Died ();
		}

		finalLengthLabel.text = "Length: " + length.ToString () + "m!";
		finalTimeLabel.text = "Time: " + timeAlive.ToString("0.0") + "s!";

		SpawnManager.GetComponent<SpawnManager> ().Died ();
		UIManager.GetComponent<UIManager> ().Died ();
	}
}

	
