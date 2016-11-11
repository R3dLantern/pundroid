using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

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

	public int powerUpType = 0;

	float distanceZ;

	Camera cam;
	Rigidbody2D rb;
	PlayerController pc;
	OptionsController op;

	public GameObject pickUpAnimation;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		distanceZ = Mathf.Abs (cam.transform.position.z + transform.position.z);
		rb = GetComponent<Rigidbody2D> ();
		op = GameObject.FindWithTag ("OptionsController").GetComponent<OptionsController> ();
		if (op == null) {
			Debug.Log ("OptionsController nicht gefunden!");	
		}
		pc = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		if (pc == null) {
			Debug.Log ("PlayerController nicht gefunden!");
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

		if (other.tag == "projectile") {
			Destroy (other);
		} else if (other.tag == "Player") {
			op.PickUp ();
			CheckPowerUp ();
			GameObject pickUp = (GameObject) Instantiate (pickUpAnimation, transform.position, Quaternion.Euler (0, 0, 0));
			Destroy (pickUp, 0.14f);
			Destroy (gameObject);
		} else {
			return;
		}
	}

	void CheckPowerUp() {
		switch (powerUpType) {
		case 0:
			pc.ActivateAutoAttack ();
			op.AnnounceRapidshot ();
			break;
		case 1:
			pc.Heal ();
			op.AnnounceHeal ();
			break;
		case 2:
			pc.CrossShot ();
			op.AnnounceCrossfire ();
			break;
		case 3:
			pc.ActivateShield ();
			op.AnnounceShield ();
			break;
		}
	}
}
