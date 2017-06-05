using UnityEngine;
using System.Collections;

/// <summary>
/// The Asteroid Script handles the behaviour of all Asteroid prefabs.
/// </summary>
public class Asteroid: Spawnable {

	public int scoreValue;

	public GameObject explosion;
	public GameObject playerExplosion;
	public Asteroid[] smallerAsteroid;
	public PowerUp[] powerUps;

    GameController gameController;

	// Use this for initialization
	public override void IndividualStartConfiguration () {
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		if (gameController == null) {
			Debug.Log ("GameController nicht gefunden!");	
		}
	}

    /// <summary>
    /// Handling the collision event with other objects
    /// </summary>
    /// <param name="other">The object colliding with this object</param>
	public override void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            // Ignore collision with other Asteroids and PowerUps
            default:
            case "Enemy":
            case "powerUp":
                return;
            // Increase player score when hit by a projectile
            case "projectile":
                Destroy(other.gameObject);
                gameController.AddScore(scoreValue);
                break;
            case "Player":
                if (!player.IsOnHitCooldown())
                {
                    if (player.GetCurrentHealth() == 0)
                    {
                        GameObject exp_clone = (GameObject)Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                        Destroy(other.gameObject);
                        Destroy(exp_clone, 0.5f);
                        player.TakeDamage();
                        gameController.GameOver();
                    }
                    else { player.TakeDamage(); }
                }
                else { return; }
                break;
        }
		if (explosion != null)
        {
			GameObject exp_clone = Instantiate(explosion, transform.position, transform.rotation);
			Destroy(exp_clone, 0.5f);
		}
		Destroy(gameObject);
		soundController.Explosion();
		--gameController.hazardCount;

        // if no PowerUp is active, up the counter for the next spawn
		if (!player.autoAttackActive && !player.crossShotPickedUp) { gameController.pUpCount++; }

        // spawn a random PowerUp object after 15 destroyed asteroids
		if (gameController.pUpCount == 15)
        {
			Instantiate(powerUps[Mathf.FloorToInt(Random.Range (0, powerUps.Length - 1))], transform.position, transform.rotation);
			gameController.pUpCount = 0;
		}

        // Bigger asteroids spawn smaller asteroids
        if(smallerAsteroid.Length > 0)
        {
			for (int i = 0; i < 2; i++)
            {
				Instantiate(smallerAsteroid[Mathf.FloorToInt(Random.Range(0, smallerAsteroid.Length))], transform.position, transform.rotation);
				gameController.hazardCount++;
			}
		}
	}
}
