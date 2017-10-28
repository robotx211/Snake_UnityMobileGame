using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	//spawns enemies at set intervals, or at random intervals within set parameters

	public int gridSizex = 19;
	public int gridSizey = 29;

	//(0, 0) is the centre, so the enemies can be spawned at (-9 -> +9, -14 -> +14)
	//(+ve, +ve) is top right

	public GameObject enemyPrefab;

	public float spawnTimeFixedDelay;

	public bool randomSpawnTime = false;
	public float spawnTimeRandDelayMin;
	public float spawnTimeRandDelayMax;

	private float nextSpawnTime;
	private float spawnTimeRandDelay;

	// Use this for initialization
	void Start () {

		nextSpawnTime = spawnTimeFixedDelay;

	}

	// Update is called once per frame
	public void SpawnUpdate () {

		if (randomSpawnTime == false) {

			if (Time.time >= nextSpawnTime) {
				//spawn enemy
				SpawnEnemy ();

				nextSpawnTime = Time.time + spawnTimeFixedDelay;
			}

		} else {

			if (Time.time >= nextSpawnTime) {
				//spawn enemy
				SpawnEnemy ();

				nextSpawnTime = Time.time + Random.Range(spawnTimeRandDelayMin, spawnTimeRandDelayMax);
			}

		}


	}

	public void SpawnEnemy () {

		//works if the grid sizes are odd, so (0,0) is in the centre
		int xPos = Random.Range (0, (gridSizex + 1) / 2) * RandomSign();
		int yPos = Random.Range (0, (gridSizey + 1) / 2) * RandomSign();

		bool valid;

		//send send rays at and around the location to check if a collider is there, or in the area
		//could also set a zone around the player head where suff cannot spawn

		//has some bugs, need more work/ reworking
		//could probably move this code into it's own class, so it's not repeated on every spawner

		do {
			valid = true;

			//if any of the 5 rays hits something with any of the game componen tags, check another cell

			for (int x = -1; x <= 1; x++) {

				for (int y = -1; y <= 1; y++) {

					if (Physics.Raycast (new Vector3 ((xPos * 0.56f) + (0.15f * x), (yPos * 0.56f) + (0.15f * y), -1f), new Vector3 (0, 0, 1), 2f, 8) == true){
						//hit something in game layer, so don't spawn

						valid = false;
					}

				}

			}

		} while (valid == false);

		//quick extra check
		if (valid == true) {
			Instantiate(enemyPrefab, new Vector2 ((xPos * 0.56f), (yPos * 0.56f)), Quaternion.identity);
		}
	}

	int RandomSign () {

		int sign = Random.Range (0,2);

		if (sign == 0) {
			return -1;
		} else {
			return 1;
		}

	}

	public void ActivateSpawner(){
		nextSpawnTime = Time.time + spawnTimeFixedDelay;
	}
}
