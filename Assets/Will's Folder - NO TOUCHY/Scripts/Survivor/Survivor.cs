using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HealthPack")]

public enum DEBUGMODE
{
	DISABLED,
	ENABLED
}

public enum SURVIVORSTATE
{
	NORMAL,
	DEAD,
}

public enum SURVIVORNAME
{
	FRANCIS,
	LOUIS,
	ZOEY,
	BILL
}


public class Survivor : Entity {

	NavMeshAgent agent;
	Animator animator;

	public Transform barrel; 

	public SURVIVORNAME survivorName;
	SURVIVORSTATE state;

	public DEBUGMODE debugmode;
	public Slider healthSlider;

	private float currentHealth;

	public Weapon currentWeapon;
	Weapon[] weaponList;

	int groundMask;

	private float switchWeaponTimer;


	// Use this for initialization
	public override void Start()
	{
		base.Start();
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		groundMask = LayerMask.GetMask("Floor");

		state = SURVIVORSTATE.NORMAL;

		currentHealth = 100.0f;


		if (healthSlider != null)
		{
			healthSlider.value = currentHealth;
		}

		weaponList = GetComponents<Weapon>();


		foreach (Weapon w in weaponList)
		{
			w.enabled = true;
			w.DisableWeapon();
			if (w.type == WEAPON_TYPE.PISTOL)
			{
				currentWeapon = w;
				currentWeapon.EnableWeapon();
			}
		}
	}



	
	// Update is called once per frame
	public override void Update () {
		base.Start();

		UpdateAnimation ();

		if (debugmode == DEBUGMODE.ENABLED) {
			DebugMove ();
			DebugFire ();
			DebugSwitchWeapons ();
		}

		switchWeaponTimer -= Time.deltaTime;
	}

	public void MoveTo(Vector3 point)
	{
		if (state == SURVIVORSTATE.NORMAL && debugmode == DEBUGMODE.DISABLED) {
			agent.SetDestination (point);
		}

	}

	public void Fire(Vector3 point)
	{
		if (state == SURVIVORSTATE.NORMAL && debugmode == DEBUGMODE.DISABLED) {

			transform.LookAt (point);

			if (currentWeapon.Fire (point)) {
			}
		}
	}

    
	Weapon FindWeapon(WEAPON_TYPE type)
	{
		InitializeWeaponList();

		Weapon output = null;

		foreach (Weapon w in weaponList) {
			if (type == w.type) {
				output = w;
				break;
			}
		}

		return output;
	}

	public void SwitchWeapons(WEAPON_TYPE type)
	{
		InitializeWeaponList();

		if (state == SURVIVORSTATE.NORMAL)
		{
			if (switchWeaponTimer <= 0)
			{
				switchWeaponTimer = 0.25f;
				currentWeapon.DisableWeapon ();

				foreach (Weapon w in weaponList)
				{
					if (type == w.type)
					{
						currentWeapon = w;
						currentWeapon.EnableWeapon ();
						break;
					}
				}
			}
		}
	}

	public Weapon[] GetWeaponList()
	{
		InitializeWeaponList();

		return weaponList;
	}

	public Weapon GetWeapon(WEAPON_TYPE type)
	{
		InitializeWeaponList();
		Weapon output = null;
		foreach (Weapon w in weaponList)
		{
			if (w.type == type)
			{
				output = w;
				break;
			}
		}

		return output;
	}

	public Weapon GetCurrentWeapon()
	{
		return currentWeapon;
	}


	private void InitializeWeaponList()
	{
		if (weaponList == null)
		{
			weaponList = GetComponents<Weapon>();

			foreach (Weapon w in weaponList)
			{
				w.enabled = true;
				w.DisableWeapon();
				if (w.type == WEAPON_TYPE.PISTOL)
				{
					currentWeapon = w;
					currentWeapon.EnableWeapon();
				}
			}
		}
	}

	protected override void Dead ()
	{
		state = SURVIVORSTATE.DEAD;
		agent.isStopped = true;
		GameManager.RemoveSurvivor(this);
	}

	public SURVIVORSTATE GetSurvivorState()
	{
		return state;
	}

	public override void OnHit (Weapon w, Vector3 hitPoint)
	{
        TakeDamage(w.getDamage());
	}

	public override void OnHit(Enemy e)
	{
        TakeDamage(e.getMeleeDamage());
	}

    public override void OnHit(Projectile p)
    {
        TakeDamage(p.damage);
    }

    void TakeDamage(float d)
    {
		if (d >= 0)
		{
			currentHealth -= d;

			currentHealth = Mathf.Clamp(currentHealth, 0, 100);

			if (healthSlider != null)
			{
				healthSlider.value = currentHealth;
			}
		}

        if (currentHealth <= 0)
        {
			Dead ();
        }
    }


    void UpdateAnimation()
	{
		if (agent.velocity.sqrMagnitude > 0.1f)
		{
			animator.SetBool ("IsWalking", true);
		}
		else
		{
			animator.SetBool ("IsWalking", false);
		}

	}

	public void OnGUI()
	{
	}

	public float GetHealth()
	{
		return currentHealth;
	}

	/*
	public void AddAmmo(AmmoPickup pickup)
	{
		switch (pickup.getAmmoType ()) 
		{
		case AMMOTYPE.ASSAULT:
			getWeapon (WEAPON_TYPE.ASSAULT).AddAmmo (pickup as AssaultAmmo);
			break;
		case AMMOTYPE.SHOTGUN:
			getWeapon (WEAPON_TYPE.SHOTGUN).AddAmmo (pickup as ShotgunAmmo);
			break;
		case AMMOTYPE.GRENADE:
			getWeapon (WEAPON_TYPE.GRENADE).AddAmmo (pickup as GrenadeAmmo);
			break;
		default:
			break;
		}
	}
	*/

	/*
	public void AddHealth(HealthPack pack)
	{
		AddHealth (pack.getHealthValue());
	}
	*/

	
	
	



	#region DEBUG_CODE

	void DebugMove()
	{
		if (Input.GetButtonDown ("Fire1")) {

			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit groundHit;

			if (Physics.Raycast (camRay, out groundHit, 1000.0f, groundMask)) {
				if (state == SURVIVORSTATE.NORMAL) {
					agent.SetDestination (groundHit.point);
				}
			}
		}
	}

	void DebugFire()
	{
		if (Input.GetButton ("Fire2")) {

			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit groundHit;

			Debug.DrawRay(camRay.origin, camRay.direction * 1000.0f, Color.red);


			if (Physics.Raycast (camRay, out groundHit, 1000.0f, groundMask)) {
				transform.LookAt (groundHit.point);
				if (currentWeapon.Fire (groundHit.point)) {
					//animator.SetTrigger ("Shoot");
				}
			}
		}

	}

	void DebugSwitchWeapons()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			SwitchWeapons (WEAPON_TYPE.PISTOL);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			SwitchWeapons (WEAPON_TYPE.ASSAULT);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			SwitchWeapons (WEAPON_TYPE.SNIPER);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			SwitchWeapons (WEAPON_TYPE.SHOTGUN);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			SwitchWeapons(WEAPON_TYPE.GRENADE_LAUNCHER);
		}
	}

	#endregion
}
