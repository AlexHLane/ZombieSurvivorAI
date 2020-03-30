using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownShield : Entity
{
    public float health;
    public float damagePerHit = 10.0f;

    Clown clown;

    public override void Start()
    {
        base.Start();

        clown = GetComponentInParent<Clown>();

    }

    public override void OnHit(float damage, Vector3 hitPoint)
    {
        health -= damagePerHit;

        if(health <= 0)
        {
            gameObject.SetActive(false);

            clown.ShieldDestroyed(this);
        }
    }

    public override void OnHit(Weapon w, Vector3 hitPoint)
    {
        health -= damagePerHit;

        if (health <= 0)
        {
            gameObject.SetActive(false);

            clown.ShieldDestroyed(this);
        }

    }

}
