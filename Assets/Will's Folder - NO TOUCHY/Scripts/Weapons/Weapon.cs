using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WEAPON_TYPE
{
	NULL,
	PISTOL,
	ASSAULT,
	SNIPER,
	SHOTGUN,
	GRENADE_LAUNCHER
}

[System.Serializable]
public struct WeaponComponents
{
	public Transform barrel;
	public ParticleSystem gunParticles;
	public LineRenderer gunLine;
	public AudioSource gunAudio;
	public Light gunLight;
}

public abstract class Weapon : MonoBehaviour {

	public WEAPON_TYPE type;
	protected bool canFire;
	//[SerializeField]
	//protected int ammo;
	[SerializeField]
	protected float damage;
	[SerializeField]
	protected float range;
    [SerializeField]
    protected float attackSpeed;

	protected float timer;
	protected float effectsDisplayTime = 0.2f;

	public float weaponOffset = 0.5f;

	public bool active = false;

	public LayerMask shootMask;
	public WeaponComponents weaponComponents;



	public virtual void Start()
	{
		canFire = true;
	}

	public virtual void Update()
	{
		if (active)
		{
			timer += Time.deltaTime;

			if (timer >= attackSpeed && Time.timeScale != 0)
			{
				canFire = true;
			}

			if (timer >= attackSpeed * effectsDisplayTime)
			{
				DisableEffects ();
			}
		}
	}

	public abstract bool Fire (Vector3 point);

	protected void Cooldown()
	{
		canFire = true;
	}

	public virtual void EnableWeapon()
	{
		active = true;
	}

	public virtual void DisableWeapon()
	{
		active = false;
	}

	public int getAmmo()
	{
		return GameManager.GetAmmo(type);
	}

	public float getDamage()
	{
		return damage;
	}

	public float getRange()
	{
		return range;
	}

	public virtual void DisableEffects ()
	{
	}
}
