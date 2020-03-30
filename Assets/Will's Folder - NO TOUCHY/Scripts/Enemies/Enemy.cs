using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public enum EnemyState
{
	DEAD,
	SPAWNING,
	NORMAL
}

public enum EnemyType
{
	ZOMBUNNY,
    ZOMBEAR,
	HELLEPHANT,
	CLOWN,
	SHEEP,
	NULL

}

public class Enemy : Entity {

	public Entity target;

	protected NavMeshAgent agent;
	public EnemyState state;

	protected CapsuleCollider capCollider;
	protected SphereCollider sphereCollider;

	protected bool canAttack;
	[SerializeField]
	protected float attackSpeed;
	[SerializeField]
	protected float attackRange;
	[SerializeField]
	protected float searchSpeed = 2.0f;
	[SerializeField]
	protected float aggroRange = 1.0f;
	[SerializeField]
	protected LayerMask targetMask;

	protected EnemyType eType;

	// Enemy Health Stuff
	public float startingHealth = 100;
	[SerializeField]
	private float currentHealth;
	public float sinkSpeed = 2.5f;
	[SerializeField]
	private float meleeDamage = 10.0f;
	public AudioClip deathClip;

	protected Animator anim;
	AudioSource enemyAudio;
	ParticleSystem hitParticles;
	bool isSinking;

    public int pointValue;

	protected Base baseTarget;
	protected Crystal crystalTarget;

	protected Gate gateTarget;


	// Use this for initialization
	public override void Start () {
		base.Start ();

		anim = GetComponent <Animator> ();
		enemyAudio = GetComponent <AudioSource> ();
		hitParticles = GetComponentInChildren <ParticleSystem> ();

		currentHealth = startingHealth;
        
		agent = GetComponent<NavMeshAgent> ();
		state = EnemyState.SPAWNING;
        
		canAttack = true;
		capCollider = GetComponent<CapsuleCollider> ();
		sphereCollider = GetComponent<SphereCollider>();

		gateTarget = findClosestGate();
	}
	public override void Update ()
	{
		base.Update();

		if(isSinking)
		{
			transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
		}
	}

	public EnemyState getState()
	{
		return state;
	}
	public virtual EnemyType getEnemyType()
	{
		return eType;
	}

	public float getHealth()
	{
		return currentHealth;
	}

	public float getMeleeDamage()
	{
		return meleeDamage;
	}

	public override void OnHit (Weapon w, Vector3 hitPoint)
	{
		TakeDamage (w.getDamage(), hitPoint);

		if (currentHealth <= 0)
		{
			Dead ();
		}
	}

	public override void OnHit(float damage, Vector3 hitPoint)
	{
		TakeDamage(damage, hitPoint);

		if (currentHealth <= 0)
		{
			Dead();
		}
	}

	public void SetBaseTarget(Base b)
	{
		baseTarget = b;
		crystalTarget = b.crystal;
	}

	public void SetGateTarget(Gate g)
	{
		gateTarget = g;
	}


	public virtual void findClosestTarget()
	{
		target = null;
		NavMeshPath newPath = null;

		if (agent.isOnNavMesh)
		{
			if(gateTarget.Alive())
			{
				if(findPathToClosestGate(gateTarget, out newPath))
				{
					target = gateTarget;
				}
			}
			else
			{
				if(findPathToReachCrystal(out newPath))
				{
					target = crystalTarget;
				}
			}

			Survivor s = findClosestSurvivor();
			float distToSurvivor = Vector3.Distance(s.transform.position, gameObject.transform.position);
			NavMeshPath pathToSurvivor = null;

			if(distToSurvivor < aggroRange)
			{
				float difInHeight = s.transform.position.y - transform.position.y;

				if (difInHeight < 0.1f)
				{
					if (findPathToSurvivor(s, out pathToSurvivor))
					{
						newPath = pathToSurvivor;
						target = s;
					}
				}
			}

			agent.path = newPath;
		}

		

		Invoke("findClosestTarget", searchSpeed);
	}


	protected bool isTargetInRange(float attackRange)
	{
		bool output = false;

		if (Vector3.Distance(target.transform.position, transform.position) < attackRange) {
			output = true;
		}
		return output;
	}

	protected bool findPathToReachCrystal(out NavMeshPath path)
	{
		bool output = false;
		path = null;

		int mask = 1 << NavMesh.GetAreaFromName("Walkable");
		NavMeshPath p = new NavMeshPath();

		if(NavMesh.CalculatePath(transform.position, crystalTarget.transform.position, mask, p))
		{
			output = true;
			path = p;
		}

		return output;
	}

	protected bool findPathToSurvivor(Survivor s, out NavMeshPath path)
	{
		bool output = false;
		path = null;

		int mask = 1 << NavMesh.GetAreaFromName("Walkable");
		NavMeshPath p = new NavMeshPath();

		if (NavMesh.CalculatePath(transform.position, s.transform.position, mask, p))
		{
			output = true;
			path = p;
		}

		return output;
	}

	protected bool findPathToClosestGate(Gate g, out NavMeshPath path)
	{
		bool output = false;
		path = null;

		int mask = 1 << NavMesh.GetAreaFromName("Walkable");
		NavMeshPath p = new NavMeshPath();
		if (NavMesh.CalculatePath(transform.position, g.transform.position, mask, p))
		{
			output = true;
			path = p;
		}
		return output;


	}

	protected Gate findClosestGate()
	{
		Gate closest = null;
		float closestDistance = Mathf.Infinity;

        if (baseTarget != null)
        {
            for (int i = 0; i < baseTarget.gates.Length; i++)
            {
                Gate temp = baseTarget.gates[i];
                float dist = Vector3.Distance(temp.transform.position, transform.position);

                if (dist < closestDistance)
                {
                    //if (temp.Alive())
                    //{
                    closestDistance = dist;
                    closest = temp;
                    //}
                }
            }
        }

		return closest;
	}

	protected Survivor findClosestSurvivor()
	{
		Survivor closest = null;
		float closestDistance = Mathf.Infinity;
		GameManager.getSurvivorList();

		for (int i = 0; i < GameManager.getSurvivorList().Count; i++)
		{
			Survivor temp = GameManager.getSurvivorList()[i];
			float dist = Vector3.Distance(temp.transform.position, transform.position);

			if (dist < closestDistance)
			{
				if (temp.GetSurvivorState() == SURVIVORSTATE.NORMAL)
				{
					closestDistance = dist;
					closest = temp;
				}
			}
		}

		return closest;
	}

	protected Entity findTargetInRange()
	{
		Entity output = null;

		float distToCrystal = Vector3.Distance(transform.position, crystalTarget.transform.position);
		Gate g = findClosestGate();
		float distToClosestGate = Vector3.Distance(g.transform.position, transform.position);
		Survivor s = findClosestSurvivor();
		float distToClosestSurvivor = Vector3.Distance(s.transform.position, transform.position);

		if (distToCrystal <= attackRange)
		{
			output = crystalTarget;
		}
		else if (distToClosestGate <= attackRange)
		{
			output = g;
		}
		else if (distToClosestSurvivor <= attackRange)
		{
			output = s;
		}



		return output;
	}

	protected void Cooldown()
	{
		canAttack = true;
	}

	protected override void Dead()
	{
		base.Dead ();
		if (state != EnemyState.DEAD)
		{

			capCollider.enabled = false;
			sphereCollider.enabled = false;

			//Destroy (gameObject);
			GameManager.RemoveEnemy(this);
			state = EnemyState.DEAD;

			DeathAnim();

			Invoke("CleanUp", 3.0f);
		}
	}

	private void CleanUp()
	{
		Destroy (gameObject);
	}


	private void TakeDamage (float amount, Vector3 hitPoint)
	{
		if(state == EnemyState.DEAD)
			return;

		enemyAudio.Play ();

		currentHealth -= amount;

		hitParticles.transform.position = hitPoint;
		hitParticles.Play();

		if(currentHealth <= 0)
		{
			Dead();
		}
	}

	void DeathAnim()
	{
		anim.SetTrigger ("Dead");

		enemyAudio.clip = deathClip;
		enemyAudio.Play ();
	}

	protected void StartSinking ()
	{
		GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
		GetComponent <Rigidbody> ().isKinematic = true;
		isSinking = true;
	}

    protected void StartMoving()
    {
        state = EnemyState.NORMAL;
        anim.SetBool("IsWalking", true);
    }

	public void AddHealthModifier(float healthModifier)
	{
		float newHealth = startingHealth * healthModifier;

		startingHealth = newHealth;
	}

}
