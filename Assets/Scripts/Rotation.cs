using UnityEngine;
using System.Collections;


/// <summary>
/// Rotation Script made to rotate the lower sprite of the player ship.
/// </summary>
public class Rotation : MonoBehaviour {

	public float rotationSpeed = 100f; //to tweak the value
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, rotationSpeed * -Time.deltaTime);
	}
}
