using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Clown : Enemy {


    [SerializeField]
    private bool shieldUp;

	// Use this for initialization
	public override void Start () {
		base.Start ();
        
		eType = EnemyType.CLOWN;
		state = EnemyState.SPAWNING;

        shieldUp = true;

		Invoke ("StartMoving", 0.5f);

        Debug.Log(shieldUp);
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


    public bool isShieldUp()
    {
        return shieldUp;
    }

	
    public void ShieldDestroyed(ClownShield cs)
    {
        shieldUp = false;
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
