﻿using UnityEngine;

/// <summary>
/// Handles PowerUp behavior and ingame effects
/// </summary>
public class PowerUp : Spawnable
{
	public int powerUpType = 0;

	public GameObject pickUpAnimation;

	// Use this for initialization
	public override void IndividualStartConfiguration() { return; }

	public override void OnTriggerEnter2D(Collider2D other)
    {
		if (other.tag == "projectile") { Destroy (other); }
        else if (other.tag == "Player")
        {
			soundController.PickUp();
			CheckPowerUp();
			GameObject pickUp = Instantiate (pickUpAnimation, transform.position, Quaternion.Euler (0, 0, 0));
			Destroy(pickUp, 0.14f);
			Destroy(gameObject);
		} else { return; }
	}

	void CheckPowerUp()
    {
		switch (powerUpType)
        {
		    case 0:
			    playerController.ActivateAutoAttack();
			    soundController.AnnounceRapidshot();
			    break;
		    case 1:
			    playerController.Heal();
			    soundController.AnnounceHeal();
			    break;
		    case 2:
			    playerController.CrossShot();
			    soundController.AnnounceCrossfire();
			    break;
		    case 3:
			    playerController.ActivateShield();
			    soundController.AnnounceShield();
			    break;
		}
	}
}
