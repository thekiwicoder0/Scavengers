using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count {
		public int min;
		public int max;
		public Count(int min, int max){
			this.min = min;
			this.max = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5, 9);
	public Count foodCount = new Count(1, 5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outterWallTiles;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3>();

	void InitialiseList(){
		gridPositions.Clear ();
		for (int x = 1; x < columns-1; ++x) {
			for (int y = 1; y < rows-1; ++y) {
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	void BoardSetup(){
		boardHolder = new GameObject ("Board").transform;
		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				if (x == -1 || x == columns || y == -1 || y == rows)
					toInstantiate = outterWallTiles [Random.Range (0, outterWallTiles.Length)];
				
				var instance = GameObject.Instantiate (toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	Vector3 RandomPosition() {
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max) {
		int objectCount = Random.Range (min, max + 1);
		for (int i = 0; i < objectCount; ++i) {
			var randomPosition = RandomPosition ();
			GameObject tile = tileArray [Random.Range (0, tileArray.Length)];
			GameObject.Instantiate (tile, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene(int level) {
		BoardSetup ();
		InitialiseList ();
		LayoutObjectAtRandom (wallTiles, wallCount.min, wallCount.max);
		LayoutObjectAtRandom (foodTiles, foodCount.min, foodCount.max);
		int enemyCount = (int)Mathf.Log (level, 2);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		GameObject.Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0), Quaternion.identity);
	}

	void Start() {
				
	}

}
