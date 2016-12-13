using UnityEngine;
using System.Collections;

/// <summary>
/// Handles player movement and controls
/// </summary>
public class PlayerController : MonoBehaviour {

	public float speed;

	public float fireRate;
	public GameObject shot;
	public Transform shotSpawn;
	public Transform[] CrossShots;
	private float nextFire;

	Camera cam;
	OptionsController op;
	HealthBarManager hb;
	Transform turret;
	Transform shield;

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

	bool enteredViewport = false;

    /// <summary>
    /// Gather several objects necessary for a clean Start()
    /// </summary>
	void Awake(){
		turret = GameObject.Find ("Controller").GetComponent<Transform> ();
		hb = GameObject.FindWithTag ("HealthBar").GetComponent<HealthBarManager> ();
		shield = GameObject.FindWithTag ("shield").GetComponent<Transform> ();
	}

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		distanceZ = Mathf.Abs (cam.transform.position.z + transform.position.z);
		op = GameObject.FindWithTag ("OptionsController").GetComponent<OptionsController>();
		shield.gameObject.SetActive (false);

		///<description>
		/// The constraints determine the limits for the starfield.
		/// They are later used to wrap the map.
		///</description>
		leftConstraint = cam.ScreenToWorldPoint (new Vector3 (0.0f, 0.0f, distanceZ)).x;
		rightConstraint = cam.ScreenToWorldPoint (new Vector3 (Screen.width, 0.0f, distanceZ)).x;
		bottomConstraint = cam.ScreenToWorldPoint (new Vector3 (0.0f, 0.0f, distanceZ)).y;
		topConstraint = cam.ScreenToWorldPoint (new Vector3 (0.0f, Screen.height, distanceZ)).y;
	}

	/// <summary>
	/// Handles Inputs and executes the respective actions.
	/// </summary>
	void FixedUpdate (){

		/// <description>>
		/// Fires a projectile in the direction of the mouse.
		/// </description>
		if (Input.GetButton("Fire1") && Time.time > nextFire) {
			PlayerFire ();
		}

		float moveH = Input.GetAxis ("Horizontal");
		float moveV = Input.GetAxis ("Vertical");
		
		Vector3 movement = new Vector3 (moveH, moveV, 0.0f);
		transform.position += movement * speed * Time.deltaTime;
		//rb.velocity = new Vector2 (Mathf.Lerp(rb.position.x, moveH * speed, 0.8f), Mathf.Lerp(rb.position.y, moveV * speed, 0.8f));

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		turret.rotation = Quaternion.LookRotation (Vector3.forward, mousePos - transform.position);

		if (enteredViewport) {
			//This is a very poor implementation of wrapping the map, and needs improvement...
			if (transform.position.x < leftConstraint - buffer) {
				transform.position = new Vector3 (rightConstraint + buffer, transform.position.y, transform.position.z);
			}
			if (transform.position.x > rightConstraint + buffer) {
				transform.position = new Vector3 (leftConstraint - buffer, transform.position.y, transform.position.z);
			}
			if (transform.position.y < bottomConstraint - buffer) {
				transform.position = new Vector3 (transform.position.x, topConstraint + buffer, transform.position.z);
			}
			if (transform.position.y > topConstraint + buffer) {
				transform.position = new Vector3 (transform.position.x, bottomConstraint - buffer, transform.position.z);
			}
		}
	}

    /// <summary>
    /// [WIP] possibly needed for the intro sequence
    /// </summary>
	void OnBecameVisible() {
		enteredViewport = true;
	}

    /// <summary>
    /// Execute the firing of a projectile in the direction of the mouse cursor
    /// </summary>
	void PlayerFire() {
		nextFire = Time.time + fireRate;
        // if the CrossShot PowerUp is picked up, the ship fires 4 projectiles in all directions
		if (crossShotPickedUp) {
			for (int i = 0; i < CrossShots.Length; i++) {
				Instantiate (shot, CrossShots [i].position, CrossShots [i].rotation);
			}
			crossShotTicks--;
			if (crossShotTicks == 0) {
				crossShotPickedUp = false;
				crossShotTicks = 15;
				op.WearDown ();
			}
		} else if(!autoAttackActive) {
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		}
		op.Fire();
	}

    /// <summary>
    /// PowerUp AutoAttack rapidly and consecutively fires 30 projectiles in the direction of the mouse cursor.
    /// Does not stack up the PowerUp spawn counter.
    /// </summary>
	IEnumerator AutoAttack() {
		autoAttackActive = true;
		Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		op.Fire();
		for (int i = 0; i < autoAttackTicks - 1; i++) {
			yield return new WaitForSeconds (0.2f);
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			op.Fire();
		}
		autoAttackActive = false;
		op.WearDown ();
	}

    /// <summary>
    /// PowerUp Shield raises an indestructible shield for 30 seconds.
    /// </summary>
	IEnumerator Shield() {
		shield.gameObject.SetActive(true);
		for (int i = 0; i < shieldTicks - 1; i++) {
			yield return new WaitForSeconds (1.0f);
		}
		shield.gameObject.SetActive(false);
		op.WearDown ();
	}

    /// <summary>
    /// Activate the AutoAttack PowerUp
    /// </summary>
    public void ActivateAutoAttack() {
		StartCoroutine ("AutoAttack");
	}

    /// <summary>
    /// Heal 1 HP of the player with a Heal PowerUp
    /// </summary>
	public void Heal() {
		hb.IncreaseHealth ();
	}

    /// <summary>
    /// Activate the CrossShot PowerUp
    /// </summary>
	public void CrossShot() {
		crossShotPickedUp = true;
	}

    /// <summary>
    /// Activate the shield PowerUp
    /// </summary>
	public void ActivateShield() {
		StartCoroutine ("Shield");
	}

}
