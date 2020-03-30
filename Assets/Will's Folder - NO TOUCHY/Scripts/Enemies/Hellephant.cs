using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellephant : Enemy {



	// Use this for initialization
	public override void Start () {
		base.Start ();

		eType = EnemyType.HELLEPHANT;
		state = EnemyState.SPAWNING;

		Invoke ("StartMoving", 0.5f);

	}

	void FixedUpdate () 
	{
		base.Update();

		if (state == EnemyState.NORMAL)
		{
			if (target == null)
			{
				agent.ResetPath();
				findClosestTarget();
			}
			else
			{
				if (agent.isActiveAndEnabled)
				{

					if (canAttack)
					{
						if (isTargetInRange(attackRange))
						{
							//anim.SetTrigger ("Attack");
							canAttack = false;

							target.OnHit(this);

							Invoke("Cooldown", attackSpeed);
						}
					}
				}
			}
		}
	}

	public override EnemyType getEnemyType ()
	{
		return EnemyType.HELLEPHANT;
	}


	protected override void Dead ()
	{
		GameManager.RemoveEnemy (this);
		base.Dead ();
	}
}
