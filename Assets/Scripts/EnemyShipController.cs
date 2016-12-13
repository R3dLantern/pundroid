using UnityEngine;
using System.Collections;

/// <summary>
/// [WIP] Controller class for enemy ships
/// </summary>
public class EnemyShipController : MonoBehaviour {
    // TODO: implement evasion logic and pathfinding AI

	public float speed = 0.1f;

	public float fireRate;
	public GameObject shot;
	public Transform shotSpawn;

	float nextFire;

	Camera cam;
	Transform turret;

	public float leftConstraint = 0.0f;
	public float rightConstraint = 0.0f;
	public float topConstraint = 0.0f;
	public float bottomConstraint = 0.0f;
	public float buffer = 0.1f;

	float distanceZ;

	void Awake(){
		turret = GameObject.Find ("Controller").GetComponent<Transform> ();
	}

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		distanceZ = Mathf.Abs (cam.transform.position.z + transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float rndVal = Random.value - 0.5f * 7f * speed;
		Vector3 rnd = new Vector3 (
			rndVal,
			rndVal,
			0
		);

		transform.position = Vector3.Lerp(	
			transform.position,
			transform.position + rnd,
			Time.time
		);

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

	void Fire()
	{
		nextFire = Time.time + fireRate;
	}
}
