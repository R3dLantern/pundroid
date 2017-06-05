using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles ingame options, such as Volume and SoundFX-management.
/// </summary>
public class SoundController : MonoBehaviour
{
	public static SoundController instance = null;

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

	UIController uiController;

    /// <summary>
    /// Check for singleton status and assign UI elements and Controllers based on scene
    /// </summary>
	void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
		DontDestroyOnLoad (gameObject);
		sounds = GetComponents<AudioSource>();
		if (SceneManager.GetActiveScene ().buildIndex < 2)
        {
			muteToggle = GameObject.FindWithTag("volume_mute").GetComponent<Toggle>();
			mSlider = GameObject.FindWithTag("volume_master").GetComponent<Slider>();
			sfxSlider = GameObject.FindWithTag("volume_sfx").GetComponent<Slider>();
			menuSlider = GameObject.FindWithTag("volume_menu").GetComponent<Slider>();
			gameSlider = GameObject.FindWithTag("volume_game").GetComponent<Slider>();
		}
		if (SceneManager.GetActiveScene ().buildIndex == 1)
        {
			uiController = GameObject.FindWithTag("SoundController").GetComponent<UIController>();
		}
	}

    /// <summary>
    /// Assign sounds to array and play scene-based background music, also apply sound settings
    /// from PlayerPrefs
    /// </summary>
	void Start()
    {
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

		checkMusic();
		if (SceneManager.GetActiveScene ().buildIndex < 2)
        {
			muteToggle.isOn = PlayerPrefs.GetInt("mute") == 1 ? true : false;
			mSlider.value = PlayerPrefs.GetInt("master");
			sfxSlider.value = PlayerPrefs.GetInt("sfx");
			menuSlider.value = PlayerPrefs.GetInt("menu");
			gameSlider.value = PlayerPrefs.GetInt("game");
		}

        SceneManager.activeSceneChanged += OnSceneChange;
	}

	void OnSceneChange(Scene last, Scene now)
    {
        checkMusic();
		if (SceneManager.GetActiveScene().buildIndex < 2)
        {
			muteToggle = GameObject.FindWithTag("volume_mute").GetComponent<Toggle>();
			mSlider = GameObject.FindWithTag("volume_master").GetComponent<Slider>();
			sfxSlider = GameObject.FindWithTag("volume_sfx").GetComponent<Slider>();
			menuSlider = GameObject.FindWithTag("volume_menu").GetComponent<Slider>();
			gameSlider = GameObject.FindWithTag("volume_game").GetComponent<Slider>();

			muteToggle.isOn = PlayerPrefs.GetInt("mute") == 1 ? true : false;
			mSlider.value = PlayerPrefs.GetInt("master");
			sfxSlider.value = PlayerPrefs.GetInt("sfx");
			menuSlider.value = PlayerPrefs.GetInt("menu");
			gameSlider.value = PlayerPrefs.GetInt("game");
		}
	}

	// Update is called once per frame
	void Update()
    {
		if (SceneManager.GetActiveScene().buildIndex < 2)
        {
            // Check mute button
			isMute = muteToggle.isOn;
			if (isMute)
            { for (int i = 0; i < sounds.Length; i++) { sounds [i].mute = true; } }
			else { for (int i = 0; i < sounds.Length; i++) { sounds[i].mute = false; } }

			//Master Volume
			masterVolume = (int)mSlider.value;
            AudioListener.volume = masterVolume * 0.25f;

			//SFX Volume
			sfxVolume = (int)sfxSlider.value;
            for(int j = 2; j < sounds.Length; j++) { sounds[j].volume = sfxVolume * 0.25f; }

			//Menu Music Volume
			menuVolume = (int)menuSlider.value;
            menuMusic.volume = menuVolume * 0.25f;

			//Game Music Volume
			gameVolume = (int)gameSlider.value;
            bgMusic.volume = gameVolume * 0.25f;
			
			PlayerPrefs.SetInt("mute", isMute ? 1 : 0);
			PlayerPrefs.SetInt("master", masterVolume);
			PlayerPrefs.SetInt("sfx", sfxVolume);
			PlayerPrefs.SetInt("menu", menuVolume);
			PlayerPrefs.SetInt("game", gameVolume);
		}
	}

	public void Fire() { laser.Play(); }

	public void Explosion() { exp.Play(); }

    public void OnHover() { hover.Play(); }

	public void OnRelease() { release.Play(); }

	public void PickUp() { powerUpPickup.Play(); }

	public void WearDown() { powerUpWearDown.Play(); }

	public void AnnounceRapidshot() { rapid_announce.Play(); }

	public void AnnounceCrossfire() { crossfire_announce.Play(); }

	public void AnnounceHeal() { heal_announce.Play(); }

	public void AnnounceShield() { shield_announce.Play(); }

	public void Mute() { isMute = !isMute ? true : false; }

	public void PlayCountdownSequence() { countdownSequence.Play(); }

	public void PlayCountdownSfx() { countdownSfx.Play(); }

	void checkMusic()
    {
		switch (SceneManager.GetActiveScene().buildIndex)
        {
		    default:
		    case 0:
			    if (bgMusic.isPlaying) { bgMusic.Stop(); }
			    if (!menuMusic.isPlaying) { menuMusic.Play(); }
			    break;
		    case 1:
                if (menuMusic.isPlaying) { menuMusic.Stop(); }
                if (!bgMusic.isPlaying)
                {
                    bgMusic.Play();
                    // StartCoroutine (PlayMusicAfterCountdown());
                }
                else { return; }
			    break;
		    case 2:
                if (bgMusic.isPlaying) { bgMusic.Stop(); }
                if (!menuMusic.isPlaying) { menuMusic.Play(); }
                else { return; }
                break;
		}
	}

    /*
    public IEnumerator PlayMusicAfterCountdown()
    {
		while (!uiController.countdownFinished) { yield return null; }
		bgMusic.Play ();
	}*/
}
