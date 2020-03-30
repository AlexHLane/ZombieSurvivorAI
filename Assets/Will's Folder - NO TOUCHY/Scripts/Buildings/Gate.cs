using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum GATE_POSITION
{
    NORTH,
    SOUTH,
    EAST,
    WEST
}

public class Gate : Entity
{
    public GATE_POSITION position;

    protected float health;
    protected bool isAlive;
    NavMeshObstacle navObstacle;
    MeshRenderer mesh;
    Collider gateCollider;

    Slider healthBar;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        health = 100;
        isAlive = true;

        navObstacle = GetComponent<NavMeshObstacle>();
        mesh = GetComponent<MeshRenderer>();
        gateCollider = GetComponent<Collider>();
        healthBar = GetComponentInChildren<Slider>();

        if (healthBar != null)
        {
            healthBar.value = health;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

    }


    public float GetHealth()
    {
        return health;
    }

    public override void OnHit(Enemy e)
    {
        float damage = e.getMeleeDamage();

        Damage(damage);
    }

    public override void OnHit(Projectile p)
    {
        Damage(p.damage);
    }

    void Damage(float damage)
    {
        health -= damage;
        healthBar.value = health;

        if (health <= 0)
        {
            isAlive = false;
            navObstacle.enabled = false;
            mesh.enabled = false;
            gateCollider.enabled = false;
            healthBar.gameObject.SetActive(false);
        }
    }

    public bool Alive()
    {
        return isAlive;
    }

    public void Repair()
    {
        health = 100;
        healthBar.value = health;

        isAlive = true;
        navObstacle.enabled = true;
        mesh.enabled = true;
        gateCollider.enabled = true;
        healthBar.gameObject.SetActive(true);
    }

}
