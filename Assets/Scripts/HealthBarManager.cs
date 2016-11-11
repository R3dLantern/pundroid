using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarManager : MonoBehaviour {

	public GameObject[] healthblocks;
	int playerHealth = 2;

	public void IncreaseHealth() {
		if (playerHealth < 2) {
			healthblocks [++playerHealth].SetActive (true);
		}
	}

	public void DecreaseHealth() {
		healthblocks [playerHealth--].SetActive (false);
	}

	public int GetPlayerHealth() {
		return playerHealth;
	}


}
