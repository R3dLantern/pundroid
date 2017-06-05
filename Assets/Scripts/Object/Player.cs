﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Handles player movement and controls
/// </summary>
public class Player : MonoBehaviour
{
    SoundController soundController;
    HealthbarController healthbarController;

    Camera cam;
    Transform turret;
    Transform shield;

    public float speed;

	public float fireRate;
	public GameObject shot;
	public Transform shotSpawn;
	public Transform[] CrossShots;
	private float nextFire;

	public float leftConstraint = 0.0f;
	public float rightConstraint = 0.0f;
	public float topConstraint = 0.0f;
	public float bottomConstraint = 0.0f;
	float distanceZ;
	public float buffer = 0.1f;

	public bool autoAttackActive = false;
	public bool crossShotPickedUp = false;
	public int autoAttackTicks = 30;
	public int crossShotTicks = 15;
	public int shieldTicks = 10;

    bool hasHitCooldown = false;

	bool enteredViewport = false;

    /// <summary>
    /// Gather several objects necessary for a clean Start()
    /// </summary>
	void Awake()
    {
		turret = GameObject.Find("Controller").GetComponent<Transform>();
		healthbarController = GameObject.FindWithTag("HealthBar").GetComponent<HealthbarController>();
		shield = GameObject.FindWithTag("shield").GetComponent<Transform>();
	}

	// Use this for initialization
	void Start()
    {
		cam = Camera.main;
		distanceZ = Mathf.Abs(cam.transform.position.z + transform.position.z);
		soundController = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
		shield.gameObject.SetActive(false);

		///<description>
		/// The constraints determine the limits for the starfield.
		/// They are later used to wrap the map.
		///</description>
		leftConstraint = cam.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, distanceZ)).x;
		rightConstraint = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, distanceZ)).x;
		bottomConstraint = cam.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, distanceZ)).y;
		topConstraint = cam.ScreenToWorldPoint(new Vector3(0.0f, Screen.height, distanceZ)).y;
	}

	/// <summary>
	/// Handles Inputs and executes the respective actions.
	/// </summary>
	void FixedUpdate()
    {
		/// <description>>
		/// Fires a projectile in the direction of the mouse.
		/// </description>
		if (Input.GetButton("Fire1") && Time.time > nextFire) { PlayerFire(); }

		float moveH = Input.GetAxis("Horizontal");
		float moveV = Input.GetAxis("Vertical");
		
		Vector3 movement = new Vector3(moveH, moveV, 0.0f);
		transform.position += movement * speed * Time.deltaTime;
		//rb.velocity = new Vector2 (Mathf.Lerp(rb.position.x, moveH * speed, 0.8f), Mathf.Lerp(rb.position.y, moveV * speed, 0.8f));

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		turret.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

		if (enteredViewport)
        {
			//TODO: This is a very poor implementation of wrapping the map, and needs improvement...
			if (transform.position.x < leftConstraint - buffer) { transform.position = new Vector3(rightConstraint + buffer, transform.position.y, transform.position.z); }
			if (transform.position.x > rightConstraint + buffer) { transform.position = new Vector3(leftConstraint - buffer, transform.position.y, transform.position.z); }
			if (transform.position.y < bottomConstraint - buffer) { transform.position = new Vector3(transform.position.x, topConstraint + buffer, transform.position.z); }
			if (transform.position.y > topConstraint + buffer) { transform.position = new Vector3(transform.position.x, bottomConstraint - buffer, transform.position.z); }
		}
	}

    /// <summary>
    /// [WIP] possibly needed for the intro sequence
    /// </summary>
	void OnBecameVisible() { enteredViewport = true; }

    /// <summary>
    /// Execute the firing of a projectile in the direction of the mouse cursor
    /// </summary>
	void PlayerFire()
    {
		nextFire = Time.time + fireRate;
        // if the CrossShot PowerUp is picked up, the ship fires 4 projectiles in all directions
		if (crossShotPickedUp)
        {
			for (int i = 0; i < CrossShots.Length; i++) { Instantiate (shot, CrossShots[i].position, CrossShots[i].rotation); }
			crossShotTicks--;
			if (crossShotTicks == 0)
            {
				crossShotPickedUp = false;
				crossShotTicks = 15;
				soundController.WearDown();
			}
		}
        else if(!autoAttackActive) { Instantiate (shot, shotSpawn.position, shotSpawn.rotation); }
		soundController.Fire();
	}

    /// <summary>
    /// PowerUp AutoAttack rapidly and consecutively fires 30 projectiles in the direction of the mouse cursor.
    /// Does not stack up the PowerUp spawn counter.
    /// </summary>
	IEnumerator AutoAttack()
    {
		autoAttackActive = true;
		Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		soundController.Fire();
		for (int i = 0; i < autoAttackTicks - 1; i++)
        {
			yield return new WaitForSeconds(0.2f);
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			soundController.Fire();
		}
		autoAttackActive = false;
		soundController.WearDown();
	}

    /// <summary>
    /// PowerUp Shield raises an indestructible shield for 30 seconds.
    /// </summary>
	IEnumerator Shield()
    {
		shield.gameObject.SetActive(true);
		for (int i = 0; i < shieldTicks - 1; i++) { yield return new WaitForSeconds (1.0f); }
		shield.gameObject.SetActive(false);
		soundController.WearDown();
	}

    /// <summary>
    /// Activate the AutoAttack PowerUp
    /// </summary>
    public void ActivateAutoAttack() { StartCoroutine ("AutoAttack"); }

    /// <summary>
    /// Activate the CrossShot PowerUp
    /// </summary>
	public void ActivateCrossShot() { crossShotPickedUp = true; }

    /// <summary>
    /// Activate the shield PowerUp
    /// </summary>
	public void ActivateShield() { StartCoroutine ("Shield"); }

    /// <summary>
    /// Get the player's current health
    /// </summary>
    /// <returns>Integer representing health points</returns>
    public int GetCurrentHealth() { return healthbarController.GetPlayerHealth(); }

    /// <summary>
    /// Restore 1 HP
    /// </summary>
	public void Heal() { healthbarController.IncreaseHealth(); }

    /// <summary>
    /// Take 1 HP of damage
    /// </summary>
    public void TakeDamage() { healthbarController.DecreaseHealth(); }

    /// <summary>
    /// Checks whether the Player has recently taken damage and is invincible while
    /// on cooldown
    /// </summary>
    /// <returns>the current state regarding hit cooldown</returns>
    public bool IsOnHitCooldown() { return hasHitCooldown; }
}