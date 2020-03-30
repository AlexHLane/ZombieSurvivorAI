using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Weapon
{
    public GameObject grenadePrefab;
    public float shootAngle;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override bool Fire(Vector3 point)
    {
        bool output = false;

        if (canFire && GameManager.GetAmmo(WEAPON_TYPE.GRENADE_LAUNCHER) > 0)
        {
            LaunchGrenade(point);
            GameManager.UseAmmo(WEAPON_TYPE.GRENADE_LAUNCHER);
            output = true;
            canFire = false;
            timer = 0.0f;
        }
        return output;
    }

    void LaunchGrenade(Vector3 targetLoc)
    {
        float dist = Vector3.Distance(targetLoc, transform.position);

        if (dist < range)
        {
            GameObject ball = Instantiate(grenadePrefab, weaponComponents.barrel.position, Quaternion.identity);
            Rigidbody body = ball.GetComponent<Rigidbody>();
            body.velocity = BallisticVel(targetLoc, shootAngle);
        }

    }

    Vector3 BallisticVel(Vector3 target, float angle)
    {
        Vector3 dir = target - transform.position;  // get target direction
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

}
