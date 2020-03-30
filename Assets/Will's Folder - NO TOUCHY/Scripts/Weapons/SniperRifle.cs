using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : Weapon {

	Ray shootRay = new Ray();

	public Material lineMaterial;


	public override void Start()
	{
		base.Start ();

	}

	public override bool Fire (Vector3 fireLocation)
	{
		bool output = false;

		if (canFire && GameManager.GetAmmo(WEAPON_TYPE.SNIPER) > 0)
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
		GameManager.UseAmmo(WEAPON_TYPE.SNIPER);

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

		RaycastHit[] shootHits;

		shootHits = Physics.RaycastAll (shootRay, range, shootMask);

		Vector3 farthestHitPoint = Vector3.zero;


		if(shootHits.Length > 0)
		{
			List<RaycastHit> sortedHits = SortHitListByDistance(shootHits);

			
			for (int i = 0; i < sortedHits.Count; i++)
			{
				RaycastHit hit = sortedHits[i];
				farthestHitPoint = hit.point;

				Entity enemy = hit.collider.GetComponent<Entity>();
				ClownShield clownShield = hit.collider.GetComponent<ClownShield>();

				if (enemy != null)
				{
					enemy.OnHit(this, hit.point);

					if(clownShield != null)
					{
						break;
					}

				}
				else
				{
					break;
				}
			}
		}

		if (shootHits.Length > 0)
		{
			weaponComponents.gunLine.SetPosition(1, farthestHitPoint);
		}
		else
		{
			weaponComponents.gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
		}
	}

	List<RaycastHit> SortHitListByDistance(RaycastHit[] hits)
	{
		List<RaycastHit> output = new List<RaycastHit>();

		foreach(RaycastHit hit in hits)
		{
			output.Add(hit);
		}

		output.Sort(SortByDistance);

		return output;

	}

	static int SortByDistance(RaycastHit hit1, RaycastHit hit2)
	{
		return hit1.distance.CompareTo(hit2.distance);
	}

}
