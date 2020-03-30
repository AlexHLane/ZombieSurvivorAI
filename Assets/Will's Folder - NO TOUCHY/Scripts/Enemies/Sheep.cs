using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Sheep : Enemy {

	public float explosionRadius;
	//public LayerMask targetMask;

	public float explosiveDamage;

	// Use this for initialization
	public override void Start () {
		base.Start ();
        
		eType = EnemyType.CLOWN;
		state = EnemyState.SPAWNING;
        
		Invoke ("StartMoving", 0.5f);
        
	}

	public override void Update () 
	{
		base.Update();

		
		if (state == EnemyState.NORMAL) {
			if (target == null) {
				agent.ResetPath();
				SheepFindTarget();
			} else {
				if (agent.isActiveAndEnabled)
				{
					if (canAttack)
					{
						if (isTargetInRange (attackRange))
						{
							Explode();
						}
					}
				}
			}
		}
	}

	void SheepFindTarget()
	{
		NavMeshPath newPath = null;

		if (agent.isOnNavMesh)
		{
			if (findPathToReachCrystal(out newPath))
			{
				target = crystalTarget;
			}
			else
			{
				findPathToClosestGate(gateTarget, out newPath);
			}
		}

		agent.path = newPath;

		Invoke("SheepFindTarget", searchSpeed);
	}
	

	void Explode()
	{
		Dead();

		Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, targetMask);

		foreach (Collider c in cols)
		{
			Entity e = c.GetComponent<Entity>();
			if (e != null)
			{
				e.OnHit(explosiveDamage, e.transform.position);
			}

		}

	}

	public override EnemyType getEnemyType ()
	{
		return EnemyType.CLOWN;
	}

	protected override void Dead ()
	{
		base.Dead ();
	}
}
