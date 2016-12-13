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

    GameController gc;
	HealthBarManager hb;

	// Use this for initialization
	public override void IndividualStartConfiguration () {
		gc = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		if (gc == null) {
			Debug.Log ("GameController nicht gefunden!");	
		}
		hb = GameObject.FindWithTag ("HealthBar").GetComponent<HealthBarManager> ();
		if (hb == null) {
			Debug.Log ("HealthBarManager nicht gefunden!");	
		}
	}

    /// <summary>
    /// Handling the collision event with other objects
    /// </summary>
    /// <param name="other">The object colliding with this object</param>
	public override void OnTriggerEnter2D(Collider2D other){

        // PowerUps and other asteroids are ignored
        if (other.tag == "Enemy" || other.tag == "powerUp")
        {
            return;
        }
        // Projectiles are destroyed on collision
		if (other.tag == "projectile") {
			Destroy (other.gameObject);
			gc.AddScore (scoreValue);
		}

        // Player collision handling
		if (other.tag == "Player") {
			if (!gc.playerIsHit) {
				if (hb.GetPlayerHealth() == 0) {
					GameObject exp_clone = (GameObject) Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
					Destroy (other.gameObject);
					Destroy (exp_clone, 0.5f);
					gc.PlayerHit ();
					gc.GameOver ();
				} else
					gc.PlayerHit ();
			} else
				return;
		}

		if (explosion != null) {
			GameObject exp_clone = (GameObject) Instantiate (explosion, transform.position, transform.rotation);
			Destroy (exp_clone, 0.5f);
		}
		Destroy (gameObject);
		op.Explosion ();
		gc.hazardCount--;

        // if no PowerUp is active, up the counter for the next spawn
		if (!pc.autoAttackActive && !pc.crossShotPickedUp) {
			gc.pUpCount++;
		}

        // spawn a random PowerUp object after 15 destroyed asteroids
		if (gc.pUpCount == 15) {
			Instantiate (powerUps [Random.Range (0, powerUps.Length - 1)], transform.position, transform.rotation);
			gc.pUpCount = 0;
		}

        // Bigger asteroids spawn smaller asteroids
        if(smallerAsteroid.Length > 0) {
			for (int i = 0; i < 2; i++) {
				Instantiate (smallerAsteroid[Random.Range(0, smallerAsteroid.Length-1)], transform.position, transform.rotation);
				gc.hazardCount++;
			}
		}
	}
}
