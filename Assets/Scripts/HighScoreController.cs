using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages the scoreboard and player-based scores made in the game
/// </summary>
public class HighScoreController : MonoBehaviour {

	public Text[] names;
	public Text[] scores;

	int rankingPosition;

	GameController gc;

	string nameKey = "name";
	string scoreKey = "score";

	int[] scoresData;
	string[] namesData;
	
	void Start () {
		scoresData = new int[scores.Length];
		namesData = new string[names.Length];
		updateScoreTable ();
		checkForNewHighScore ();
	}

	/// <summary>
	/// Checks for new high score.
	/// </summary>
	public void checkForNewHighScore(){
		int newScore = PlayerPrefs.GetInt ("newScore");
		if (newScore == 0)
			return;
		int t = scoresData.Length;
		if (newScore < scoresData[t-1]) {
			return;
		} else {
			string newName = PlayerPrefs.GetString ("newName");
			SubmitScore (newName, newScore);
		}
	}

	/// <summary>
	/// Submits the new score.
	/// </summary>
	/// <param name="newName">New Name.</param>
	/// <param name="newScore">New score.</param>
	void SubmitScore(string newName, int newScore){
		int r = 0;
		while (newScore < scoresData [r] && r < scoresData.Length) {
			rankingPosition = ++r;
		}

		for (int k = scoresData.Length - 1; k > rankingPosition; k--) {
			scoresData [k] = scoresData [k - 1];
			namesData [k] = namesData [k - 1];
		}

		scoresData [rankingPosition] = newScore;
		namesData [rankingPosition] = newName;

		for (int k = 0; k < scoresData.Length; k++) {
			PlayerPrefs.SetString (nameKey + k, namesData [k]);
			PlayerPrefs.SetInt (scoreKey + k, scoresData [k]);
		}

		PlayerPrefs.DeleteKey ("newScore");
		PlayerPrefs.DeleteKey ("newName");

		updateScoreTable ();
	}

	void updateScoreTable(){
		for (int i = 0; i < scoresData.Length; i++) {
			namesData [i] = PlayerPrefs.GetString (nameKey + i);
			scoresData [i] = PlayerPrefs.GetInt (scoreKey + i);
			names [i].text = namesData [i];
			scores [i].text = scoresData [i].ToString ();
		}
	}

	public void deleteLeaderboard(){
		for (int j = 0; j < scoresData.Length; j++) {
			PlayerPrefs.DeleteKey (nameKey + j);
			PlayerPrefs.DeleteKey (scoreKey + j);
		}
		updateScoreTable ();
	}

}
