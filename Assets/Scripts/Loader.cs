using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Holds the instances for the OptionsController and the GameController classes.
/// </summary>
public class Loader : MonoBehaviour {

	public GameObject optionsController;
	public GameObject gameController;

	void Awake(){
		if (OptionsController.instance == null)
			Instantiate (optionsController);
		if (SceneManager.GetActiveScene ().buildIndex == 1 && GameController.instance == null)
			Instantiate (gameController);
	}
}
