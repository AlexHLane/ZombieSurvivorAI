using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_FindClosestBear : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        Enemy e = FindClosestBearTarget(sAI);
        if(e != null)
        {
            if (sAI.GetBlackboard().distFromTarget < 40.0f && (sAI.GetSurvivor().GetWeapon(WEAPON_TYPE.SNIPER).getAmmo() > 0 || sAI.GetSurvivor().GetWeapon(WEAPON_TYPE.ASSAULT).getAmmo() > 0))
            {
                sAI.GetBlackboard().enemyTarget = e;
                output = TASK_RETURN_STATUS.SUCCEED;
            }
        }

        return output;
    }

    Enemy FindClosestBearTarget(Survivor_AI sAI)
    {
        Enemy target = null;

        float closestSoFar = Mathf.Infinity;
        //int worst = 0;

        List<Enemy> allEnemies = GameManager.getZomBearList();

        foreach (Enemy e in allEnemies)
        {
            if (e.getState() != EnemyState.DEAD)
            {
                float dist = (e.transform.position - sAI.transform.position).magnitude;
                if (dist < closestSoFar)
                {                    
                    target = e;
                    closestSoFar = dist;
                    sAI.GetBlackboard().distFromTarget = closestSoFar;
                }
            }
        }


        return target;
    }
}
