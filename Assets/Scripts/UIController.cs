using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Controller Script for UI elements within the respective scenes.
/// </summary>
public class UIController : MonoBehaviour {

	GameController gc;
	OptionsController op;
	HighScoreController hs;

	public Texture2D[] cursors;
	public CursorMode cMode = CursorMode.Auto;
	public Vector2 hotspot = Vector2.zero;

	GameObject player;
	GameObject erasePanel, gameOverPanel, newscorePanel, optionsPanel, pausePanel, quitPanel, startPanel;
	public bool newScorePanelActive;

	public bool countdownFinished = false;

	void Awake () {
		CheckScene (SceneManager.GetActiveScene().buildIndex);
	}
	
	void Start(){
		
		op = GameObject.FindWithTag ("OptionsController").GetComponent<OptionsController> ();

		if (SceneManager.GetActiveScene ().buildIndex == 1)
			gc = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();

		CheckPanels (SceneManager.GetActiveScene().buildIndex);

		if (SceneManager.GetActiveScene ().buildIndex == 2) {
			hs = GameObject.FindWithTag ("ScoreController").GetComponent<HighScoreController> ();
			hs.checkForNewHighScore ();
		}
	}

	void Update(){
		if (SceneManager.GetActiveScene ().buildIndex == 1) {
			if (gc.gameOver) {
				gameOverPanel.SetActive (true);
				if(newscorePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Return))
					EnterScore();
			}

			if (!gc.gameOver && optionsPanel.activeInHierarchy && Input.GetKeyDown (KeyCode.Escape)) {
				hideOptions ();
			} else if (!gc.gameOver && Input.GetKeyDown (KeyCode.Escape)) {
				if (!gc.isPaused) {
					pauseGame ();
				} else {
					resumeGame ();
				}
			}
		}	
	}

	/// <summary>
	/// Checks for the scene the controller is initialized in and assigns the necessary UI elements.
	/// </summary>
	/// <param name="level">build index of the scene</param>
	void CheckScene(int level){
		switch (level) {
		default:
		case 0:
			startPanel = GameObject.FindWithTag ("panel_start");
			optionsPanel = GameObject.FindWithTag ("panel_options");
			quitPanel = GameObject.FindWithTag ("panel_quit");
			if (PlayerPrefs.HasKey ("newScore")) {
				PlayerPrefs.DeleteKey ("newScore");
			}
			break;
		case 1:
			player = GameObject.FindWithTag ("Player");
			pausePanel = GameObject.FindWithTag ("panel_pause");
			optionsPanel = GameObject.FindWithTag ("panel_options");
			gameOverPanel = GameObject.FindWithTag ("panel_gameover");
			quitPanel = GameObject.FindWithTag ("panel_quit");
			newscorePanel = GameObject.FindWithTag ("panel_newScore");
			break;
		case 2:
			erasePanel = GameObject.FindWithTag ("panel_erase");
			break;
		}
	}

	void CheckPanels(int level){
		switch (level) {
		default:
		case 0:
			optionsPanel.SetActive (false);
			quitPanel.SetActive (false);
			break;
		case 1:
			pausePanel.SetActive (false);
			optionsPanel.SetActive (false);
			gameOverPanel.SetActive (false);
			quitPanel.SetActive (false);
			newscorePanel.SetActive (false);
			break;
		case 2:
			erasePanel.SetActive (false);
			break;
		}
	}

	void OnMouseEnter(){
		Cursor.SetCursor(cursors[SceneManager.GetActiveScene().buildIndex % 2], hotspot, cMode);
	}

	void OnMouseExit() {
		Cursor.SetCursor(null, Vector2.zero, cMode);
	}

	public void pauseGame(){
		Time.timeScale = 0;
		player.SetActive (false);
		pausePanel.SetActive (true);
		gc.isPaused = true;
	}

	//Button methods
	public void resumeGame(){
		gc.isPaused = false;
		pausePanel.SetActive (false);
		player.SetActive (true);
		Time.timeScale = 1;
	}

	public void showNewScorePanel(){
		newscorePanel.SetActive (true);
		newScorePanelActive = true;

	}

	public void hideNewScorePanel(){
		newscorePanel.SetActive (false);
		newScorePanelActive = false;
	}

	public GameObject GetNewScorePanel(){
		return newscorePanel;
	}

	public void showOptions(){
		optionsPanel.SetActive (true);
	}

	public void hideOptions(){
		optionsPanel.SetActive (false);
	}

	public void LoadMenu(){
		SceneManager.LoadScene (0);
	}
		
	public void LoadGame(){
		SceneManager.LoadScene (1);
	}

	public void LoadLeaderboard(){
		SceneManager.LoadScene (2);
	}

	public void quitToDesktop(){
		quitPanel.SetActive(true);
		if (SceneManager.GetActiveScene ().buildIndex == 0)
			startPanel.SetActive (false);
	}

	public void QuitYes(){
		Application.Quit ();
	}

	public void QuitNo(){
		quitPanel.SetActive (false);
		if (SceneManager.GetActiveScene ().buildIndex == 0)
			startPanel.SetActive (true);
	}

	public void EnterScore(){
		string newName = newscorePanel.GetComponentInChildren<InputField> ().text;
		if (newName.Length == 0)
			newName = "<ohne Namen>";
		int newScore = gc.getScore ();
		PlayerPrefs.SetString ("newName", newName);
		PlayerPrefs.SetInt ("newScore", newScore);
		newscorePanel.SetActive (false);
	}

	public void EraseData(){
		erasePanel.SetActive (true);
	}

	public void EraseYes(){
		hs.deleteLeaderboard();
		erasePanel.SetActive (false);
	}

	public void EraseNo(){
		erasePanel.SetActive (false);
	}

	public void OnHover(){
		op.OnHover ();
	}

	public void OnRelease(){
		op.OnRelease ();
	}
}
