using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public abstract class Entity : MonoBehaviour {


	// Use this for initialization
	public virtual void Start () {

	}
	
	// Update is called once per frame
	public virtual void Update () {
		
	}

	public virtual void OnHit (Weapon w, Vector3 hitPoint)
	{
	}

	public virtual void OnHit (Enemy e)
	{
		
	}

	public virtual void OnHit(Tower t)
	{

	}

    public virtual void OnHit(Projectile p)
    {

    }

	public virtual void OnHit(float damage, Vector3 hitPoint)
	{

	}

	protected virtual void Dead ()
	{
	}
}
