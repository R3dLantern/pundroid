using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// The GameController handles all backend mechanisms happening in the game scene.
/// </summary>
public class GameController : MonoBehaviour {

	//Hazard variables
	public GameObject[] hazards;
	public int hazardCount; 
	public int hazardMax; 
	public int hazardStart;

	public float boundaryX = 3f;
	public float boundaryY = 5.5f;
	
	Text scoreText;
	HealthBarManager hb;

	UIController ui;

	public int playerHealth;
	public bool playerIsHit;

	public static GameController instance = null;

	public bool gameOver;
	public bool isPaused;
	public int pUpCount = 0;
	int score;
	int wavecount = 0;

	public bool shipAppeared = false;

	/// <summary>
	/// Awake this instance and find the UI texts.
	/// </summary>
	void Awake(){
		// Only one GameController instance at a time
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();
		hb = GameObject.FindWithTag ("HealthBar").GetComponent<HealthBarManager> ();
		ui = GameObject.Find ("Canvas").GetComponent<UIController> ();
	}
	/// <summary>
	/// Sets all parameters and spawns a number of hazards around the player.
	/// </summary>
	void Start (){
		gameOver = false;
		score = 0;
		scoreText.text = "Score: " + score;
		wavecount++;
		UpdateScore ();
		playerHealth = 2;
		for (int j = 0; j < hazardStart; j++) {
			GameObject hazard = hazards [Random.Range (0, hazards.Length)];
			SafeInstantiate (hazard);
		}
		hazardCount = hazardStart;
		hazardMax = hazardCount;
		if(isPaused)
			isPaused = false;
		if(Time.timeScale != 1)
			Time.timeScale = 1;
	}
	/// <summary>
	/// The Update checks for the Game Over-boolean,
	/// the current count of hazards in the game and allows to pause.
	/// </summary>
	void Update (){

		if (hazardCount < 6) {
			for (int j = hazardCount; j < hazardMax; j++) {
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				SafeInstantiate (hazard);
			}
			hazardCount = hazardMax;
			hazardMax++;
			wavecount++;
		}
	}

	/// <summary>
	/// Adds the score of a destroyed asteroid to the total score.
	/// </summary>
	/// <param name="newScoreValue">New score value.</param>
	public void AddScore (int newScoreValue){
		score += newScoreValue;
		UpdateScore ();
	}

	/// <summary>
	/// Updates the score.
	/// </summary>
	void UpdateScore (){
		scoreText.text = "Score: " + score;
	}

	/// <summary>
	/// Signals the end of the game.
	/// </summary>
	public void GameOver (){
		gameOver = true;
		CheckPrefs ();
	}

	/// <summary>
	/// Gets the score.
	/// </summary>
	/// <returns>The score.</returns>
	public int getScore(){
		return score;
	}

	/// <summary>
	/// Decreases the player's health.
	/// Is called in response to a collision of the player with an asteroid.
	/// PLANNED: Interaction with the Health bar; Player animation  (CRITICAL CONDITION)
	/// </summary>
	public void PlayerHit(){
		hb.DecreaseHealth();
	}

	/// <summary>
	/// Checks the PlayerPrefs for unneeded keys.
	/// </summary>
	void CheckPrefs(){
		string scorekey = "key";
		int[] scores = new int[5];
		for (int i = 0; i < scores.Length; i++) {
			scores [i] = PlayerPrefs.GetInt (scorekey + i);
		}
		if (score >= scores [4])
			ui.showNewScorePanel ();
	}
		
	/// <summary>
	/// Safely instantiates an object outside of a possible collision radius of the player.
	/// </summary>
	/// <param name="toInstance">The object to instantiate.</param>
	void SafeInstantiate(GameObject toInstance){
		Vector3 rndmPos;
		Vector2 toCheck;
		bool notSafe = true;
		// Pick a random position on the world map.
		rndmPos = new Vector3 (Random.Range (-boundaryX - 1f, boundaryX + 1f),
			Random.Range (-boundaryY - 1f, boundaryY + 1f),
			0);
		toCheck = new Vector2 (rndmPos.x, rndmPos.y);

		while (notSafe) {
			notSafe = false;
			RaycastHit2D[] hit = Physics2D.CircleCastAll (new Vector2 (toCheck.x, toCheck.y + 3f), 3f, new Vector2 (toCheck.x, toCheck.y - 3f));
			for (int i = 0; i < hit.Length; i++) {
				if (hit [i].collider.name == "Player") {
					notSafe = true;
					rndmPos = new Vector3 (Random.Range (-boundaryX - 1f, boundaryX + 1f),
						Random.Range (-boundaryY - 1f, boundaryY + 1f),
						0);
					toCheck = new Vector2 (rndmPos.x, rndmPos.y);
				}
			}
		}
		Instantiate (toInstance, rndmPos, Quaternion.identity);
	}
}