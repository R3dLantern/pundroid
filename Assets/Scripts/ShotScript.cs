using UnityEngine;
using System.Collections;

public class ShotScript : MonoBehaviour {

	public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0, speed, 0);
	}

	void OnBecameInvisible(){
		Destroy (gameObject);
	}
}
