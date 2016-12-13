using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles ingame options, such as Volume and SoundFX-management.
/// </summary>
public class OptionsController : MonoBehaviour {

	public static OptionsController instance = null;

	int masterVolume;
	int sfxVolume;
	int menuVolume;
	int gameVolume;
	int scene;

	bool isMute;
	AudioSource[] sounds;
	AudioSource menuMusic, bgMusic, exp, laser, hover, release, powerUpPickup, powerUpWearDown, rapid_announce, crossfire_announce, heal_announce, shield_announce, countdownSfx, countdownSequence;

	Toggle muteToggle;
	Slider mSlider, sfxSlider, menuSlider, gameSlider;

	UIController ui;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
		sounds = GetComponents<AudioSource> ();
		if (SceneManager.GetActiveScene ().buildIndex < 2) {
			muteToggle = GameObject.FindWithTag ("volume_mute").GetComponent<Toggle> ();
			mSlider = GameObject.FindWithTag ("volume_master").GetComponent<Slider> ();
			sfxSlider = GameObject.FindWithTag ("volume_sfx").GetComponent<Slider> ();
			menuSlider = GameObject.FindWithTag ("volume_menu").GetComponent<Slider> ();
			gameSlider = GameObject.FindWithTag ("volume_game").GetComponent<Slider> ();
		}
		if (SceneManager.GetActiveScene ().buildIndex == 1) {
			ui = GameObject.FindWithTag ("OptionsController").GetComponent<UIController> ();
		}
	}

	void Start(){
		menuMusic = sounds [0];
		bgMusic = sounds [1];
		exp = sounds [2];
		laser = sounds [3];
		hover = sounds [4];
		release = sounds [5];
		powerUpPickup = sounds [6];
		powerUpWearDown = sounds [7];
		rapid_announce = sounds [8];
		crossfire_announce = sounds [9];
		heal_announce = sounds [10];
		shield_announce = sounds [11];
		countdownSfx = sounds [12];
		countdownSequence = sounds [13];

		checkMusic ();
		if (SceneManager.GetActiveScene ().buildIndex < 2) {
			muteToggle.isOn = PlayerPrefs.GetInt ("mute") == 1 ? true : false;
			mSlider.value = PlayerPrefs.GetInt ("master");
			sfxSlider.value = PlayerPrefs.GetInt ("sfx");
			menuSlider.value = PlayerPrefs.GetInt ("menu");
			gameSlider.value = PlayerPrefs.GetInt ("game");
		}

        SceneManager.activeSceneChanged += OnSceneChange;

	}

	void OnSceneChange(Scene last, Scene now)
    {
        checkMusic ();
		if (SceneManager.GetActiveScene().buildIndex < 2) {
			muteToggle = GameObject.FindWithTag ("volume_mute").GetComponent<Toggle> ();
			mSlider = GameObject.FindWithTag ("volume_master").GetComponent<Slider> ();
			sfxSlider = GameObject.FindWithTag ("volume_sfx").GetComponent<Slider> ();
			menuSlider = GameObject.FindWithTag ("volume_menu").GetComponent<Slider> ();
			gameSlider = GameObject.FindWithTag ("volume_game").GetComponent<Slider> ();

			muteToggle.isOn = PlayerPrefs.GetInt ("mute") == 1 ? true : false;
			mSlider.value = PlayerPrefs.GetInt ("master");
			sfxSlider.value = PlayerPrefs.GetInt ("sfx");
			menuSlider.value = PlayerPrefs.GetInt ("menu");
			gameSlider.value = PlayerPrefs.GetInt ("game");
		}
	}

	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene ().buildIndex < 2) {
			isMute = muteToggle.isOn;
			if (isMute) {
				for (int i = 0; i < sounds.Length; i++) {
					sounds [i].mute = true;
				}
			}
			if (!isMute) {
				for (int i = 0; i < sounds.Length; i++) {
					sounds [i].mute = false;
				}
			}

			//Master Volume
			masterVolume = (int)mSlider.value;
			switch (masterVolume) {
			case 0:
				AudioListener.volume = 0.0f;
				break;
			case 1:
				AudioListener.volume = 0.25f;
				break;
			case 2:
				AudioListener.volume = 0.5f;
				break;
			case 3:
				AudioListener.volume = 0.75f;
				break;
			default:
			case 4:
				AudioListener.volume = 1.0f;
				break;
			}

			//SFX Volume
			sfxVolume = (int)sfxSlider.value;
			switch (sfxVolume) {
			case 0:
				laser.volume = 0.0f;
				exp.volume = 0.0f;
				hover.volume = 0.0f;
				release.volume = 0.0f;
				powerUpPickup.volume = 0.0f;
				powerUpWearDown.volume = 0.0f;
				rapid_announce.volume = 0.0f;
				crossfire_announce.volume = 0.0f;
				heal_announce.volume = 0.0f;
				shield_announce.volume = 0.0f;
				countdownSfx.volume = 0.0f;
				countdownSequence.volume = 0.0f;
				break;
			case 1:
				laser.volume = 0.25f;
				exp.volume = 0.25f;
				hover.volume = 0.25f;
				release.volume = 0.25f;
				powerUpPickup.volume = 0.25f;
				powerUpWearDown.volume = 0.25f;
				rapid_announce.volume = 0.25f;
				crossfire_announce.volume = 0.25f;
				heal_announce.volume = 0.25f;
				shield_announce.volume = 0.25f;
				countdownSfx.volume = 0.25f;
				countdownSequence.volume = 0.25f;
				break;
			case 2:
				laser.volume = 0.5f;
				exp.volume = 0.5f;
				hover.volume = 0.5f;
				release.volume = 0.5f;
				powerUpPickup.volume = 0.5f;
				powerUpWearDown.volume = 0.5f;
				rapid_announce.volume = 0.5f;
				crossfire_announce.volume = 05f;
				heal_announce.volume = 0.5f;
				shield_announce.volume = 0.5f;
				countdownSfx.volume = 0.5f;
				countdownSequence.volume = 0.5f;
				break;
			case 3:
				laser.volume = 0.75f;
				exp.volume = 0.75f;
				hover.volume = 0.75f;
				release.volume = 0.75f;
				powerUpPickup.volume = 0.75f;
				powerUpWearDown.volume = 0.75f;
				rapid_announce.volume = 0.75f;
				crossfire_announce.volume = 0.75f;
				heal_announce.volume = 0.75f;
				shield_announce.volume = 0.75f;
				countdownSfx.volume = 0.75f;
				countdownSequence.volume = 0.75f;
				break;
			default:
			case 4:
				laser.volume = 1.0f;
				exp.volume = 1.0f;
				hover.volume = 1.0f;
				release.volume = 1.0f;
				powerUpPickup.volume = 1.0f;
				powerUpWearDown.volume = 1.0f;
				rapid_announce.volume = 1.0f;
				crossfire_announce.volume = 1.0f;
				heal_announce.volume = 1.0f;
				shield_announce.volume = 1.0f;
				countdownSfx.volume = 1.0f;
				countdownSequence.volume = 1.0f;
				break;
			}

			//Menu Music Volume
			menuVolume = (int)menuSlider.value;
			switch (menuVolume) {
			case 0:
				menuMusic.volume = 0.0f;
				break;
			case 1:
				menuMusic.volume = 0.25f;
				break;
			case 2:
				menuMusic.volume = 0.5f;
				break;
			case 3:
				menuMusic.volume = 0.75f;
				break;
			default:
			case 4:
				menuMusic.volume = 1.0f;
				break;
			}

			//Game Music Volume
			gameVolume = (int)gameSlider.value;
			switch (gameVolume) {
			case 0:
				bgMusic.volume = 0.0f;
				break;
			case 1:
				bgMusic.volume = 0.25f;
				break;
			case 2:
				bgMusic.volume = 0.5f;
				break;
			case 3:
				bgMusic.volume = 0.75f;
				break;
			default:
			case 4:
				bgMusic.volume = 1.0f;
				break;
			}
			
			PlayerPrefs.SetInt ("mute", isMute ? 1 : 0);
			PlayerPrefs.SetInt ("master", masterVolume);
			PlayerPrefs.SetInt ("sfx", sfxVolume);
			PlayerPrefs.SetInt ("menu", menuVolume);
			PlayerPrefs.SetInt ("game", gameVolume);
		}
	}

	public void Fire(){
		laser.Play ();
	}

	public void Explosion(){
		exp.Play ();
	}

	public void OnHover() {
		hover.Play();
	}

	public void OnRelease(){
		release.Play ();
	}

	public void PickUp() {
		powerUpPickup.Play();
	}

	public void WearDown() {
		powerUpWearDown.Play();
	}

	public void AnnounceRapidshot() {
		rapid_announce.Play();
	}

	public void AnnounceCrossfire() {
		crossfire_announce.Play ();
	}

	public void AnnounceHeal() {
		heal_announce.Play ();
	}

	public void AnnounceShield() {
		shield_announce.Play ();
	}

	public void Mute(){
		isMute = !isMute ? true : false;
	}

	public void PlayCountdownSequence() {
		countdownSequence.Play();
	}

	public void PlayCountdownSfx() {
		countdownSfx.Play();
	}

	void checkMusic(){
		switch (SceneManager.GetActiveScene ().buildIndex) {
		default:
		case 0:
			if (bgMusic.isPlaying) {
				bgMusic.Stop ();
			}
			if (!menuMusic.isPlaying) {
				menuMusic.Play ();
			}
			break;
		case 1:
			if (menuMusic.isPlaying)
				menuMusic.Stop ();
			if (!bgMusic.isPlaying) {
                bgMusic.Play();
				// StartCoroutine (PlayMusicAfterCountdown());
			} else return;
			break;
		case 2:
			if (bgMusic.isPlaying)
				bgMusic.Stop ();
			if (!menuMusic.isPlaying)
				menuMusic.Play ();
			else
				return;
			break;
		}
	}

    /*
    public IEnumerator PlayMusicAfterCountdown() {
		while (!ui.countdownFinished) {
			yield return null;
		}
		bgMusic.Play ();
	}*/
}
