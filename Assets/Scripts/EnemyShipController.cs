using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// [WIP] Controller class for enemy ships
/// </summary>
public class EnemyShipController : Spawnable {
    // TODO: implement evasion logic and pathfinding AI

    public int scoreValue;

    public GameObject explosion;
    public GameObject playerExplosion;
    public PowerUp[] powerUps;

	public float fireRate;
	public GameObject shot;
	public Transform shotSpawn;

	float nextFire;

	Camera cam;
	Transform turret;
    GameController gc;
    HealthBarManager hb;

	float distanceZ;

	void Awake(){
		turret = GameObject.Find ("Controller").GetComponent<Transform> ();
	}

    public override void IndividualStartConfiguration()
    {
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        if (gc == null)
            Debug.LogError("GameController not found");
        hb = GameObject.FindWithTag("HealthBar").GetComponent<HealthBarManager>();
        if (hb == null)
            Debug.LogError("HealthBarManager not found");
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

    void Fire()
	{
		nextFire = Time.time + fireRate;
	}

    void HandleDestruction(Collider2D other, int scoreValue = 0)
    {
        if (other.tag == "Player")
        {
            if (!gc.playerIsHit)
            {
                if (hb.GetPlayerHealth() == 0)
                {
                    DestroyImmediate(other.gameObject);
                    GameObject exp_clone = (GameObject)Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                    Destroy(exp_clone, 0.5f);
                    gc.PlayerHit();
                    gc.GameOver();
                }
                else
                {
                    gc.PlayerHit();
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            DestroyImmediate(other.gameObject);
            gc.AddScore(scoreValue);
        }
        if(explosion != null)
        {
            GameObject exp_clone2 = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
        }
        DestroyImmediate(this.gameObject);
        op.Explosion();
        gc.hazardCount--;

        if(!pc.autoAttackActive && !pc.crossShotPickedUp)
        {
            gc.pUpCount++;
        }
        if(gc.pUpCount == 15)
        {
            Instantiate(powerUps[UnityEngine.Random.Range(0, powerUps.Length - 1)], transform.position, transform.rotation);
            gc.pUpCount = 0;
        }
    }


}
