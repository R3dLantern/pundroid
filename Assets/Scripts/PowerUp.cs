using UnityEngine;
using System.Collections;

/// <summary>
/// Handles PowerUp behavior and ingame effects
/// </summary>
public class PowerUp : Spawnable {

	public int powerUpType = 0;

	public GameObject pickUpAnimation;

	// Use this for initialization
	public override void IndividualStartConfiguration ()
    {
        return;
	}

	public override void OnTriggerEnter2D(Collider2D other)
    {
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
