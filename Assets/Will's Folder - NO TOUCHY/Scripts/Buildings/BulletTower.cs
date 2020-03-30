using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTower : Tower
{
    public float fireSpeed;
    private float fireCounter;

    public float fireRange;
    public float damagePerShot;

    public LayerMask shootMask;

    public Transform barrel1;
    public Transform barrel2;
    public Transform gunBase;

    public LineRenderer gunLine1;
    public LineRenderer gunLine2;

    private float displayTime;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        fireCounter = fireSpeed;
        displayTime = 0.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fireCounter -= Time.fixedDeltaTime;
        displayTime -= Time.fixedDeltaTime;

        if(fireCounter <= 0)
        {
            fireCounter = fireSpeed;

            Enemy target = FindClosestTarget();

            if (target != null)
            {
                Shoot(target.transform.position);
                displayTime = 0.1f;

                gunBase.transform.LookAt(new Vector3(target.transform.position.x,
                    transform.position.y,
                    target.transform.position.z));

            }
        }

        if(displayTime <= 0)
        {
            DisableEffects();
        }
        
    }

    public void DisableEffects()
    {
        gunLine1.enabled = false;
        gunLine2.enabled = false;
    }

    void Shoot (Vector3 targetLocation)
	{
        Ray shootRay1 = new Ray();
        Ray shootRay2 = new Ray();

        RaycastHit shootHit;

		gunLine1.enabled = true;
		gunLine1.SetPosition (0, barrel1.transform.position);

        gunLine2.enabled = true;
        gunLine2.SetPosition(0, barrel2.transform.position);

        shootRay1.origin = barrel1.transform.position;
		shootRay1.direction = (targetLocation + Vector3.up * 0.5f) - barrel1.transform.position;

        shootRay2.origin = barrel2.transform.position;
        shootRay2.direction = (targetLocation + Vector3.up * 0.5f) - barrel2.transform.position;

        if (Physics.Raycast (shootRay1, out shootHit, fireRange, shootMask))
		{
			Enemy enemy = shootHit.collider.GetComponent <Enemy> ();
			if(enemy != null)
			{
                enemy.OnHit(damagePerShot, shootHit.point);
			}
			gunLine1.SetPosition (1, shootHit.point);
            gunLine2.SetPosition(1, shootHit.point);
        }
		else
		{
			gunLine1.SetPosition (1, shootRay1.origin + shootRay1.direction * fireRange);
            gunLine2.SetPosition(1, shootRay2.origin + shootRay2.direction * fireRange);
        }
	}


    Enemy FindClosestTarget()
    {
        Enemy output = null;
        float closestSoFar = Mathf.Infinity;
        List<Enemy> enemies = GameManager.getAllEnemies();

        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy e = enemies[i];

            if (e.getState() != EnemyState.DEAD)
            {
                float dist = (e.transform.position - gunBase.transform.position).magnitude;
                if (dist < closestSoFar && dist < fireRange)
                {
                    output = e;
                    closestSoFar = dist;
                }
            }
        }

        return output;
    }

}
