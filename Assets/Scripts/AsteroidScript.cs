using UnityEngine;
using System.Collections;

/// <summary>
/// The Asteroid Script handles the behaviour of all Asteroid prefabs.
/// </summary>
public class AsteroidScript: MonoBehaviour {

	public float minTorque = -500f;
	public float maxTorque = 50f;
	public float minForce = 100f;
	public float maxForce = 400f;

	public float leftConstraint = 0.0f;
	public float rightConstraint = 0.0f;
	public float topConstraint = 0.0f;
	public float bottomConstraint = 0.0f;
	public float buffer = 0.1f;

	public float speed;
	public int scoreValue;

	public GameObject explosion;
	public GameObject playerExplosion;
	public GameObject[] smallerAsteroid;
	public GameObject[] powerUps;

	float distanceZ;

	Camera cam;
	Rigidbody2D rb;
	GameController gc;
	PlayerController pc;
	OptionsController op;
	HealthBarManager hb;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		distanceZ = Mathf.Abs (cam.transform.position.z + transform.position.z);
		rb = GetComponent<Rigidbody2D> ();
		gc = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		if (gc == null) {
			Debug.Log ("GameController nicht gefunden!");	
		}
		pc = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		if (pc == null) {
			Debug.Log ("PlayerController nicht gefunden!");	
		}
		op = GameObject.FindWithTag ("OptionsController").GetComponent<OptionsController> ();
		if (op == null) {
			Debug.Log ("OptionsController nicht gefunden!");	
		}
		hb = GameObject.FindWithTag ("HealthBar").GetComponent<HealthBarManager> ();
		if (hb == null) {
			Debug.Log ("HealthBarManager nicht gefunden!");	
		}
		
		leftConstraint = cam.ScreenToWorldPoint (new Vector3 (0.0f, 0.0f, distanceZ)).x;
		rightConstraint = cam.ScreenToWorldPoint (new Vector3 (Screen.width, 0.0f, distanceZ)).x;
		bottomConstraint = cam.ScreenToWorldPoint (new Vector3 (0.0f, 0.0f, distanceZ)).y;
		topConstraint = cam.ScreenToWorldPoint (new Vector3 (0.0f, Screen.height, distanceZ)).y;
		
		float magnitude = Random.Range (minForce, maxForce);
		float torque = Random.Range (minTorque, maxForce);
		float x = Random.Range (-1f, 1f);
		float y = Random.Range (-1f, 1f);

		rb.AddForce (magnitude * new Vector2 (x, y));
		rb.AddTorque (torque);

	}
	void FixedUpdate () {

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

	void OnTriggerEnter2D(Collider2D other){

		if (other.tag == "Enemy" || other.tag == "powerUp")
			return; 

		if (other.tag == "projectile") {
			Destroy (other.gameObject);
			gc.AddScore (scoreValue);
		}

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
		if (!pc.autoAttackActive && !pc.crossShotPickedUp) {
			gc.pUpCount++;
		}
		if (gc.pUpCount == 15) {
			Instantiate (powerUps [Random.Range (0, powerUps.Length - 1)], transform.position, transform.rotation);
			gc.pUpCount = 0;
		}
		if (smallerAsteroid.Length == 0)
			return; //The smallest asteroid will not spawn any new ones.
		else
		{
			for (int i = 0; i < 2; i++) {
				Instantiate (smallerAsteroid[Random.Range(0, smallerAsteroid.Length-1)], transform.position, transform.rotation);
				gc.hazardCount++;
			}
		}
	}
}
