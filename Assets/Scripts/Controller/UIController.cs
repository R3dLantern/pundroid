using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controller Script for UI elements within the respective scenes.
/// </summary>
public class UIController : MonoBehaviour
{
	GameController gameController;
	SoundController soundController;
	HighScoreController highScoreController;

    GameObject player;
    GameObject erasePanel, gameOverPanel, newScorePanel, optionsPanel, pausePanel, quitPanel, startPanel;

    public Texture2D[] cursors;
	public CursorMode cMode = CursorMode.Auto;
	public Vector2 hotspot = Vector2.zero;

	public bool newScorePanelActive;

	public bool countdownFinished = false;

	void Awake() { CheckScene(SceneManager.GetActiveScene().buildIndex); }
	
	void Start()
    {	
		soundController = GameObject.FindWithTag ("SoundController").GetComponent<SoundController>();

        if (SceneManager.GetActiveScene().buildIndex == 1) { gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>(); }

		CheckPanels (SceneManager.GetActiveScene().buildIndex);

		if (SceneManager.GetActiveScene().buildIndex == 2)
        {
			highScoreController = GameObject.FindWithTag ("ScoreController").GetComponent<HighScoreController>();
		}
	}

	void Update()
    {
		if (SceneManager.GetActiveScene().buildIndex == 1)
        {
			if (gameController.gameOver)
            {
				gameOverPanel.SetActive(true);
                if (newScorePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Return)) { EnterScore(); }
			}
			if (!gameController.gameOver && optionsPanel.activeInHierarchy && Input.GetKeyDown (KeyCode.Escape)) { HideOptions(); }
            else if (!gameController.gameOver && Input.GetKeyDown (KeyCode.Escape))
            {
				if (!gameController.isPaused) { PauseGame(); }
                else { ResumeGame(); }
			}
		}	
	}

	/// <summary>
	/// Checks for the scene the controller is initialized in and assigns the necessary UI elements.
	/// </summary>
	/// <param name="level">build index of the scene</param>
	void CheckScene(int level)
    {
		switch (level)
        {
		    default:
		    case 0:
			    startPanel = GameObject.FindWithTag("panel_start");
			    optionsPanel = GameObject.FindWithTag("panel_options");
			    quitPanel = GameObject.FindWithTag("panel_quit");
                if (PlayerPrefs.HasKey("newScore")) { PlayerPrefs.DeleteKey("newScore"); }
			    break;
		    case 1:
			    player = GameObject.FindWithTag("Player");
			    pausePanel = GameObject.FindWithTag("panel_pause");
			    optionsPanel = GameObject.FindWithTag("panel_options");
			    gameOverPanel = GameObject.FindWithTag("panel_gameover");
			    quitPanel = GameObject.FindWithTag("panel_quit");
			    newScorePanel = GameObject.FindWithTag("panel_newScore");
			    break;
		    case 2:
			    erasePanel = GameObject.FindWithTag("panel_erase");
			    break;
		}
	}

	void CheckPanels(int level)
    {
		switch (level)
        {
		    default:
		    case 0:
			    optionsPanel.SetActive(false);
			    quitPanel.SetActive(false);
			    break;
		    case 1:
			    pausePanel.SetActive(false);
			    optionsPanel.SetActive(false);
			    gameOverPanel.SetActive(false);
			    quitPanel.SetActive(false);
			    newScorePanel.SetActive(false);
			    break;
		    case 2:
			    erasePanel.SetActive(false);
			    break;
		}
	}

	void OnMouseEnter() { Cursor.SetCursor(cursors[SceneManager.GetActiveScene().buildIndex % 2], hotspot, cMode); }

	void OnMouseExit() { Cursor.SetCursor(null, Vector2.zero, cMode); }

	public void PauseGame()
    {
		Time.timeScale = 0;
		player.SetActive(false);
		pausePanel.SetActive(true);
		gameController.isPaused = true;
	}

	//Button methods
	public void ResumeGame()
    {
		gameController.isPaused = false;
		pausePanel.SetActive(false);
		player.SetActive(true);
		Time.timeScale = 1;
	}

	public void ShowNewScorePanel()
    {
		newScorePanel.SetActive(true);
		newScorePanelActive = true;
	}

	public void HideNewScorePanel()
    {
		newScorePanel.SetActive(false);
		newScorePanelActive = false;
	}

	public GameObject GetNewScorePanel() { return newScorePanel; }

	public void ShowOptions() { optionsPanel.SetActive(true); }

	public void HideOptions() { optionsPanel.SetActive(false); }

	public void LoadMenu() { SceneManager.LoadScene(0); }
		
	public void LoadGame() { SceneManager.LoadScene(1); }

	public void LoadLeaderboard() { SceneManager.LoadScene(2); }

	public void QuitToDesktop()
    {
		quitPanel.SetActive(true);
        if (SceneManager.GetActiveScene().buildIndex == 0) { startPanel.SetActive(false); }
	}

	public void QuitYes() { Application.Quit(); }

	public void QuitNo()
    {
		quitPanel.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex == 0) { startPanel.SetActive(true); }
	}

	public void EnterScore()
    {
		string newName = newScorePanel.GetComponentInChildren<InputField>().text;
        if (newName.Length == 0) { newName = "<ohne Namen>"; }
		int newScore = gameController.GetScore();
		PlayerPrefs.SetString("newName", newName);
		PlayerPrefs.SetInt("newScore", newScore);
		newScorePanel.SetActive(false);
	}

	public void EraseData() { erasePanel.SetActive(true); }

	public void EraseYes()
    {
		highScoreController.deleteLeaderboard();
		erasePanel.SetActive(false);
	}

	public void EraseNo() { erasePanel.SetActive(false); }

	public void OnHover() { soundController.OnHover(); }

	public void OnRelease() { soundController.OnRelease(); }
}
