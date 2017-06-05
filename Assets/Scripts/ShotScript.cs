using UnityEngine;

public class ShotScript : MonoBehaviour
{
	public float speed;
	
	// Update is called once per frame
	void Update () { transform.Translate (0, speed, 0); }

	void OnBecameInvisible(){ Destroy (gameObject); }
}
