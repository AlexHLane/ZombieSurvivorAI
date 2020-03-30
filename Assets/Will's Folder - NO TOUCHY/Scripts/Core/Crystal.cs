using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum CRYSTAL_COLOR
{
    BLUE,
    RED,
    GREEN
}

public class Crystal : Entity
{
    public CRYSTAL_COLOR Color;

    float health;

    Slider healthBar;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        health = 100;

        healthBar = GetComponentInChildren<Slider>();

        if (healthBar != null)
        {
            healthBar.value = 100;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

    }

    public override void OnHit(Enemy e)
    {
        Damage(e.getMeleeDamage());
    }

    public override void OnHit(Projectile p)
    {
        Damage(p.damage);
    }

    void Damage(float damage)
    {
        health -= damage;
        healthBar.value = damage;

        if(health <= 0)
        {
            // Temporary WILL REMOVE LATER
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
