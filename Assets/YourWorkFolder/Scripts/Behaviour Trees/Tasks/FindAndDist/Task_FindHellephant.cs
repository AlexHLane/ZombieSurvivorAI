using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_FindHellephant : BT_Task
{

    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        Enemy e = FindClosestHellephantTarget(sAI);
        if (e != null)
        {
            if (sAI.GetBlackboard().distFromTarget < 20.0f)
            {
                sAI.GetBlackboard().enemyTarget = e;
                output = TASK_RETURN_STATUS.SUCCEED;
            }
        }

        return output;
    }

    Enemy FindClosestHellephantTarget(Survivor_AI sAI)
    {
        Enemy target = null;

        float closestSoFar = Mathf.Infinity;
        //int worst = 0;

        List<Enemy> allEnemies = GameManager.getHellephantList();

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
