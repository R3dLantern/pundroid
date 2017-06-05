using UnityEngine;

/// <summary>
/// Manages the player health and the UI health bar
/// </summary>
public class HealthbarController : MonoBehaviour
{
	public GameObject[] healthblocks;
	int playerHealth = 2;

    /// <summary>
    /// Increase the player's health if it's not full
    /// </summary>
	public void IncreaseHealth() { if (playerHealth < 2) { healthblocks [++playerHealth].SetActive(true); } }

    /// <summary>
    /// Decrease the player's health by one point
    /// </summary>
	public void DecreaseHealth() { healthblocks[playerHealth--].SetActive(false); }

    /// <summary>
    /// Get the player's current health
    /// </summary>
    /// <returns>Integer representing the Player's healthpoints</returns>
	public int GetPlayerHealth() { return playerHealth; }
}
