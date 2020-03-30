using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {

	RaycastHit shootHit;

	public float spreadH;
	public float spreadV;
	public int shotPerShell;
	public Material lineMaterial;
	public GameObject LineRenderPrefab;


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

		if (canFire && GameManager.GetAmmo(WEAPON_TYPE.SHOTGUN) > 0)
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
		GameManager.UseAmmo(WEAPON_TYPE.SHOTGUN);

		timer = 0f;
		canFire = false;

		weaponComponents.gunAudio.Play ();
		weaponComponents.gunLight.enabled = true;
		weaponComponents.gunParticles.Stop ();
		weaponComponents.gunParticles.Play ();

		//gunLine.enabled = true;
		//gunLine.SetPosition (0, transform.position);


		Ray[] rays = CreateShotgunSpread ();

		
		for (int i = 0; i < rays.Length; i++)
		{

			RaycastHit[] hits = Physics.RaycastAll(rays[i].origin, rays[i].direction, range, shootMask);

			Vector3 farthestHitPoint = Vector3.zero;

			if (hits.Length > 0)
			{
				List<RaycastHit> sortedHits = SortHitListByDistance(hits);


				foreach (RaycastHit rHit in sortedHits)
				{
					Entity enemy = rHit.collider.GetComponent<Entity>();

					farthestHitPoint = rHit.point;
					if (enemy != null)
					{
						enemy.OnHit(this, shootHit.point);
						break;
					}
					else
					{
						break;
					}
				}

			}

			GameObject obj = Instantiate(LineRenderPrefab, weaponComponents.barrel.transform.position, Quaternion.identity);
			LineRenderer line = obj.GetComponent<LineRenderer>();
			line.enabled = true;
			line.SetPosition(0, rays[i].origin);

			if (hits.Length > 0)
			{
				line.SetPosition(1, farthestHitPoint);
			}
			else
			{
				line.SetPosition(1, rays[i].origin + rays[i].direction * range);
			}

		}
	}


	Ray[] CreateShotgunSpread()
	{
		Ray[] shotgunRays = new Ray[shotPerShell];

		for (int i = 0; i < shotgunRays.Length; i++)
		{
			Ray r = new Ray ();
			r.origin = weaponComponents.barrel.transform.position;

			Vector3 v = weaponComponents.barrel.transform.forward;

			v = Quaternion.AngleAxis (Random.Range(-spreadH, spreadH), Vector3.up) * v;
			v = Quaternion.AngleAxis (Random.Range(-spreadV, spreadV), weaponComponents.barrel.transform.right) * v;

			v.Normalize ();

			r.direction = v;

			shotgunRays [i] = r;
		}

		return shotgunRays;
	}

	List<RaycastHit> SortHitListByDistance(RaycastHit[] hits)
	{
		List<RaycastHit> output = new List<RaycastHit>();

		foreach (RaycastHit hit in hits)
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
