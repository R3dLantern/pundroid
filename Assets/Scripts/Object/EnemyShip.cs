using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// [WIP] Controller class for enemy ships
/// </summary>
public class EnemyShip : Spawnable
{
    // TODO: implement evasion logic and pathfinding AI
    public int scoreValue;

    public GameObject explosion;
    public GameObject playerExplosion;
    public PowerUp[] powerUps;

	public float fireRate;
	public GameObject shot;
	public Transform shotSpawn;

	//float nextFire;

	Camera cam;
	//Transform turret;
    GameController gameController;

	float distanceZ;

	//void Awake() { turret = GameObject.Find("Controller").GetComponent<Transform>(); }

    public override void IndividualStartConfiguration()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        if (gameController == null) { Debug.LogError("GameController not found"); }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.tag)
        {
            case "enemy":
            case "powerUp":
                break;
            case "projectile":
                HandleDestruction(other, scoreValue);
                break;
            case "Player":
                HandleDestruction(other);
                break;
        }
        return;
    }

    //void Fire() { nextFire = Time.time + fireRate; }

    void HandleDestruction(Collider2D other, int scoreValue = 0)
    {
        if (other.tag == "Player")
        {
            if (!player.IsOnHitCooldown())
            {
                if (player.GetCurrentHealth() == 0)
                {
                    DestroyImmediate(other.gameObject);
                    GameObject exp_clone = Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                    Destroy(exp_clone, 0.5f);
                    player.TakeDamage();
                    gameController.GameOver();
                }
                else { player.TakeDamage(); }
            }
            else { return; }
        }
        else
        {
            DestroyImmediate(other.gameObject);
            gameController.AddScore(scoreValue);
        }
        if (explosion != null)
        {
            GameObject exp_clone = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
            Destroy(exp_clone, 0.5f);
        }
        DestroyImmediate(gameObject);
        soundController.Explosion();
        gameController.hazardCount--;

        if(!player.autoAttackActive && !player.crossShotPickedUp) { gameController.pUpCount++; }
        if(gameController.pUpCount == 15)
        {
            Instantiate(powerUps[UnityEngine.Random.Range(0, powerUps.Length - 1)], transform.position, transform.rotation);
            gameController.pUpCount = 0;
        }
    }
}
