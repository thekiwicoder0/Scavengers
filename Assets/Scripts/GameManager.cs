using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public BoardManager boardScript;
	public float turnDelay = 0.1f;
	public float levelStartDelay = 1;
	private int level = 1;
	private List<Enemy> enemies = new List<Enemy>();
	private bool enemiesMoving = false;
	private Text levelText;
	private GameObject levelImage;
	private bool doingSetup = false;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this);
		}
		DontDestroyOnLoad (gameObject);
		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	void InitGame() {
		doingSetup = true;
		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text>();
		levelText.text = "Day " + level;
		levelImage.gameObject.SetActive (true);
		Invoke ("HideLevelImage", levelStartDelay);

		boardScript.SetupScene (level);
		enemies.Clear ();
	}

	private void HideLevelImage(){
		levelImage.SetActive (false);
		doingSetup = false;
	}

	private void OnLevelWasLoaded(int levelIndex){
		level++;
		InitGame ();
	}

	void Update() {
		if (playersTurn || enemiesMoving || doingSetup) {
			return;
		}

		StartCoroutine (MoveEnemies());
	}

	public void AddEnemyToList(Enemy script){
		enemies.Add (script);
	}

	IEnumerator MoveEnemies() {
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds (turnDelay);
		}

		for (int i = 0; i < enemies.Count; ++i) {
			enemies [i].MoveEnemy ();
			yield return new WaitForSeconds (enemies [i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}

	public void GameOver() 
	{
		levelText.text = "After " + level + " days, you starved";
		levelImage.gameObject.SetActive (true);
		enabled = false;
	}
}
