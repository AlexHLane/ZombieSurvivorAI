using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class ZomBunny : Enemy {



	// Use this for initialization
	public override void Start () {
		base.Start ();
        
		eType = EnemyType.ZOMBUNNY;
		state = EnemyState.SPAWNING;
        
		Invoke ("StartMoving", 0.5f);
        
	}

	public override void Update () 
	{
		base.Update();

		
		if (state == EnemyState.NORMAL) {
			if (target == null) {
				agent.ResetPath();
				findClosestTarget ();
			} else {
				if (agent.isActiveAndEnabled)
				{

					if (canAttack)
					{
						if (isTargetInRange (attackRange))
						{
							//anim.SetTrigger ("Attack");
							canAttack = false;

							target.OnHit (this);

							Invoke ("Cooldown", attackSpeed);
						}
					}
				}
			}
		}
	}

	


	public override EnemyType getEnemyType ()
	{
		return EnemyType.ZOMBUNNY;
	}

	protected override void Dead ()
	{
		base.Dead ();
	}
}
