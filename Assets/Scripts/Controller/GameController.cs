using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The GameController handles all backend mechanisms happening in the game scene.
/// </summary>
public class GameController : MonoBehaviour
{
    HighScoreController highScoreController;
    UIController uiController;

    Text scoreText;

    //Hazard variables
    public GameObject[] hazards;
	public int hazardCount; 
	public int hazardMax; 
	public int hazardStart;

	public float boundaryX = 3f;
	public float boundaryY = 5.5f;

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
	void Awake()
    {
        // Only one GameController instance at a time
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
        highScoreController = GameObject.FindWithTag("ScoreController").GetComponent<HighScoreController>();
        uiController = GameObject.Find("Canvas").GetComponent<UIController>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
	}

	/// <summary>
	/// Sets all parameters and spawns a number of hazards around the player.
	/// </summary>
	void Start()
    {
		gameOver = false;
		score = 0;
		scoreText.text = "Score: " + score;
		wavecount++;
		UpdateScore();
		for (int j = 0; j < hazardStart; j++)
        {
			GameObject hazard = hazards[Random.Range (0, hazards.Length)];
			SafeInstantiate(hazard);
		}
		hazardCount = hazardStart;
		hazardMax = hazardCount;
        if (isPaused) { isPaused = false; }
        if (Time.timeScale != 1) { Time.timeScale = 1; }
	}

	/// <summary>
	/// The Update checks for the Game Over-boolean,
	/// the current count of hazards in the game and allows to pause.
	/// </summary>
	void Update()
    {
		if (hazardCount < 6)
        {
			for (int j = hazardCount; j < hazardMax; j++)
            {
				GameObject hazard = hazards[Random.Range(0, hazards.Length)];
				SafeInstantiate(hazard);
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
	public void AddScore(int newScoreValue)
    {
		score += newScoreValue;
		UpdateScore();
	}

	/// <summary>
	/// Updates the score.
	/// </summary>
	void UpdateScore() { scoreText.text = "Score: " + score; }

	/// <summary>
	/// Sets the game as over, and checks whether a new score has been set.
	/// </summary>
	public void GameOver()
    {
		gameOver = true;
        if(highScoreController.IsNewHighscore(score)) { uiController.ShowNewScorePanel(); }
	}

	/// <summary>
	/// Gets the score.
	/// </summary>
	/// <returns>The score.</returns>
	public int GetScore() { return score; }
		
	/// <summary>
	/// Safely instantiates an object outside of a possible collision radius of the player.
	/// </summary>
	/// <param name="toInstance">The object to instantiate.</param>
	void SafeInstantiate(GameObject toInstance)
    {
		Vector3 randomPosition;
		Vector2 positionToCheck;
		bool safeToSpawn = false;
		// Pick a random position on the world map.
		randomPosition = new Vector3(
            Random.Range (-boundaryX - 1f, boundaryX + 1f),
			Random.Range (-boundaryY - 1f, boundaryY + 1f),
			0);
		positionToCheck = new Vector2(randomPosition.x, randomPosition.y);

		while (!safeToSpawn)
        {
            safeToSpawn = true;
			RaycastHit2D[] hit = Physics2D.CircleCastAll(
                new Vector2(
                    positionToCheck.x,
                    positionToCheck.y + 3f
                    ),
                3f,
                new Vector2(
                    positionToCheck.x,
                    positionToCheck.y - 3f
                    )
            );
			for (int i = 0; i < hit.Length; i++)
            {
				if (hit[i].collider.name == "Player")
                {
					safeToSpawn = false;
					randomPosition = new Vector3(
                        Random.Range (-boundaryX - 1f, boundaryX + 1f),
						Random.Range (-boundaryY - 1f, boundaryY + 1f),
						0
                    );
					positionToCheck = new Vector2(randomPosition.x, randomPosition.y);
				}
			}
		}
		Instantiate(toInstance, randomPosition, Quaternion.identity);
	}
}