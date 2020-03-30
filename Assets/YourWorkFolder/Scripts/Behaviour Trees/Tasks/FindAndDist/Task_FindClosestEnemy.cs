using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_FindClosestEnemy : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        Enemy closest = FindClosestTarget(sAI);

        if (closest != null)
        {
            sAI.GetBlackboard().enemyTarget = closest;
            float dist = (closest.transform.position - sAI.transform.position).magnitude;
            sAI.GetBlackboard().distFromTarget = dist;
            output = TASK_RETURN_STATUS.SUCCEED;
        }


        return output;
    }



    Enemy FindClosestTarget(Survivor_AI sAI)
    {
        Enemy target = null;

        float closestSoFar = Mathf.Infinity;
        List<Enemy> allEnemies = GameManager.getAllEnemies();

        foreach (Enemy e in allEnemies)
        {
            if (e.getState() != EnemyState.DEAD)
            {
                float dist = (e.transform.position - sAI.transform.position).magnitude;
                if (dist < closestSoFar && dist < sAI.GetCurrWeapon().getRange())
                {
                    target = e;
                    closestSoFar = dist;
                }
            }
        }


        return target;
    }
}
