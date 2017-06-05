using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Holds the instances for the SoundController and the GameController classes.
/// </summary>
public class Loader : MonoBehaviour
{
	public GameObject optionsController;
	public GameObject gameController;
    public GameObject highscoreController;

    void Awake() {
        if (SoundController.instance == null) { Instantiate(optionsController); }
        if (SceneManager.GetActiveScene().buildIndex == 1 && GameController.instance == null) { Instantiate(gameController); }
        if (SceneManager.GetActiveScene().buildIndex > 0 && HighScoreController.instance == null) { Instantiate(highscoreController); }
	}
}