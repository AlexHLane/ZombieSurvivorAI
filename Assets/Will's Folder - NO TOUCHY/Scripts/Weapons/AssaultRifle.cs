using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Weapon {

	Ray shootRay = new Ray();
	RaycastHit shootHit;
	int shootableMask;
	public Material lineMaterial;


	public override void Start()
	{
		base.Start ();

	}

	public override void EnableWeapon ()
	{
		base.EnableWeapon ();
		weaponComponents.gunLine.material = lineMaterial;
	}


	public override bool Fire (Vector3 fireLocation)
	{
		bool output = false;

		if (canFire && GameManager.GetAmmo(WEAPON_TYPE.ASSAULT) > 0)
		{
			weaponComponents.gunLine.material = lineMaterial;
			Shoot (fireLocation);
			output = true;
		}

		return output;
	}


	public override void DisableEffects ()
	{
		weaponComponents.gunLine.enabled = false;
		weaponComponents.gunLight.enabled = false;
	}


	void Shoot (Vector3 targetLocation)
	{
		GameManager.UseAmmo(WEAPON_TYPE.ASSAULT);

		timer = 0f;
		canFire = false;

		weaponComponents.gunAudio.Play ();
		weaponComponents.gunLight.enabled = true;
		weaponComponents.gunParticles.Stop ();
		weaponComponents.gunParticles.Play ();
		weaponComponents.gunLine.enabled = true;
		weaponComponents.gunLine.SetPosition (0, weaponComponents.barrel.transform.position);

		shootRay.origin = weaponComponents.barrel.transform.position;
		shootRay.direction = (targetLocation + Vector3.up * 0.5f) - weaponComponents.barrel.transform.position;

		
		if(Physics.Raycast (shootRay, out shootHit, range, shootMask))
		{
			Entity enemy = shootHit.collider.GetComponent <Entity> ();
			if(enemy != null)
			{
				enemy.OnHit(this, shootHit.point);
			}
			weaponComponents.gunLine.SetPosition (1, shootHit.point);
		}
		else
		{
			weaponComponents.gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}
}
