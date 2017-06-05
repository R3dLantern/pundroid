using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the scoreboard and player-based scores made in the game
/// </summary>
public class HighScoreController : MonoBehaviour
{
	public Text[] names;
	public Text[] scores;

    public static HighScoreController instance = null;

    int rankingPosition;

	GameController gameController;

	string nameKey = "name";
	string scoreKey = "score";

	int[] scoresData;
	string[] namesData;

    /// <summary>
    /// Check for singleton status
    /// </summary>
    private void Awake()
    {
        // Only one HighscoreController instance at a time
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    /// <summary>
    /// Initialize data arrays and get the current values from the PlayerPrefs
    /// </summary>
	void Start()
    {
		scoresData = new int[scores.Length];
		namesData = new string[names.Length];
		updateScoreTable();
	}

    /// <summary>
    /// Checks whether a new highscore has been set.
    /// </summary>
    /// <param name="newScore">the new score to be checked against the current leaderboard</param>
    /// <returns>true, if new highscore has been set, else false</returns>
    public bool IsNewHighscore(int newScore)
    {
        // Score Zero requires no calculation
        if (newScore == 0) { return false; }
        else
        {
            int[] currentScoreBoard = this.getCurrentScoreboard();
            if (currentScoreBoard[currentScoreBoard.Length - 1] == 0 || newScore > currentScoreBoard[currentScoreBoard.Length - 1]) { return true; }
            return false;
        }
    }

    /// <summary>
    /// Get the current scoreboard data.
    /// </summary>
    /// <returns>array of integers representing the scores from the leaderboard</returns>
    public int[] getCurrentScoreboard()
    {
        int[] currentScoreboard = new int[5];
        for (int i = 0; i < currentScoreboard.Length; i++) { currentScoreboard[i] = PlayerPrefs.GetInt(scoreKey + i); }
        return currentScoreboard;
    }

	/// <summary>
	/// Submits the new score.
	/// </summary>
	/// <param name="newName">New Name.</param>
	/// <param name="newScore">New score.</param>
	void SubmitScore(string newName, int newScore)
    {
		int r = 0;
		while (newScore < scoresData [r] && r < scoresData.Length) { rankingPosition = ++r; }
		for (int k = scoresData.Length - 1; k > rankingPosition; k--)
        {
			scoresData [k] = scoresData [k - 1];
			namesData [k] = namesData [k - 1];
		}
		scoresData [rankingPosition] = newScore;
		namesData [rankingPosition] = newName;
		for (int k = 0; k < scoresData.Length; k++)
        {
			PlayerPrefs.SetString(nameKey + k, namesData [k]);
			PlayerPrefs.SetInt(scoreKey + k, scoresData [k]);
		}
		PlayerPrefs.DeleteKey("newScore");
		PlayerPrefs.DeleteKey("newName");
		updateScoreTable();
	}

    /// <summary>
    /// Update the Score Table to the newest values.
    /// </summary>
	void updateScoreTable()
    {
		for (int i = 0; i < scoresData.Length; i++)
        {
			namesData [i] = PlayerPrefs.GetString(nameKey + i);
			scoresData [i] = PlayerPrefs.GetInt(scoreKey + i);
			names [i].text = namesData [i];
			scores [i].text = scoresData [i].ToString ();
		}
	}

    /// <summary>
    /// Delete the current leaderboard and refresh the score table.
    /// </summary>
	public void deleteLeaderboard()
    {
		for (int j = 0; j < scoresData.Length; j++)
        {
			PlayerPrefs.DeleteKey(nameKey + j);
			PlayerPrefs.DeleteKey(scoreKey + j);
		}
		updateScoreTable();
	}
}