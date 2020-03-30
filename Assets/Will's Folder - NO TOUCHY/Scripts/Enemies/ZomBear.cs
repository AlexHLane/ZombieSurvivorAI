using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZomBear : Enemy {

	public GameObject FireballPrefab;
    public Transform fireLocation;

    //public Transform myTarget;  // drag the target here\
    public float shootAngle = 30;  // elevation angle

    // Use this for initialization
    public override void Start () {
        base.Start();

        eType = EnemyType.ZOMBEAR;
        state = EnemyState.SPAWNING;

        Invoke("StartMoving", 0.5f);

    }


    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (state == EnemyState.NORMAL)
        {
            if (target == null)
            {
				BearFindClosestTarget();
            }
            else
            {
                if (agent.isActiveAndEnabled)
                {
					if (isTargetInRange(attackRange) && isTargetVisible(target))
                    {
                        if (canAttack)
                        {
                            transform.LookAt(target.transform.position);

                            ThrowFireBall();

                            canAttack = false;

                            Invoke("Cooldown", attackSpeed);
                        }

                        agent.SetDestination(transform.position);
                    }
                }
            }
        }
        
    }

	void BearFindClosestTarget()
	{
		target = null;
		NavMeshPath newPath = null;

		if (agent.isOnNavMesh)
		{
			if (gateTarget.Alive())
			{
				if (findPathToClosestGate(gateTarget, out newPath))
				{
					target = gateTarget;
				}
			}
			else
			{
				if (findPathToReachCrystal(out newPath))
				{
					target = crystalTarget;
				}
			}

			Survivor s = findClosestSurvivor();
			float distToSurvivor = Vector3.Distance(s.transform.position, gameObject.transform.position);
			NavMeshPath pathToSurvivor = null;

			if (distToSurvivor < aggroRange)
			{
				//float difInHeight = s.transform.position.y - transform.position.y;

				//if (difInHeight < 0.1f)
				//{
				if (findPathToSurvivor(s, out pathToSurvivor))
				{
					newPath = pathToSurvivor;
					target = s;
				}
				//}
			}

			agent.path = newPath;
		}



		Invoke("BearFindClosestTarget", searchSpeed);
	}

	bool isTargetVisible(Entity target)
	{
		bool output = false;

		if (target != null)
		{
			
			RaycastHit hit;
			Vector3 dir = ( (target.transform.position + Vector3.up * 0.5f) - fireLocation.position);
			dir.Normalize ();

			int layerMask = LayerMask.NameToLayer ("Enemy");
			layerMask = 1 << layerMask;
			layerMask = ~layerMask;

			if (Physics.Raycast (fireLocation.position, dir, out hit, 1000.0f, layerMask))
			{
				if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Player"))
				{
                    output = true;
				}
			}
		}
		return output;
	}

    void ThrowFireBall()
    {
        GameObject ball = Instantiate(FireballPrefab, fireLocation.position, Quaternion.identity);
        Rigidbody body = ball.GetComponent<Rigidbody>();
        body.velocity = BallisticVel(target.transform, shootAngle);

    }

    Vector3 BallisticVel(Transform target, float angle)
    {
        Vector3 dir = target.position - fireLocation.position;  // get target direction
        float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        float a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences

        // calculate the velocity magnitude
        float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    }

	public override EnemyType getEnemyType ()
	{
		return EnemyType.ZOMBEAR;
	}
}
