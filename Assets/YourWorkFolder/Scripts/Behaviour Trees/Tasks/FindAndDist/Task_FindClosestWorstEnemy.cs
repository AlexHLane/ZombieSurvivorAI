using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_FindClosestWorstEnemy : BT_Task
{

    /* DO NOT USE UNLESS FOR SURE THAT THE ENEMY ORDERING IS EASIEST TO HARDEST IN ENUM*/

    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        Enemy target = FindClosestWorstTarget(sAI);

        if(target != null)
        {
            sAI.GetBlackboard().enemyTarget = target;
            output = TASK_RETURN_STATUS.SUCCEED;
        }


        return output;
    }



    Enemy FindClosestWorstTarget(Survivor_AI sAI)
    {
        Enemy target = null;

        float closestSoFar = Mathf.Infinity;
        int worst = 0;

        List<Enemy> allEnemies = GameManager.getAllEnemies();

        foreach (Enemy e in allEnemies)
        {
            if (e.getState() != EnemyState.DEAD)
            {
                float dist = (e.transform.position - sAI.transform.position).magnitude;
                if (dist < closestSoFar && dist < sAI.GetCurrWeapon().getRange())
                {
                    if ((int)e.getEnemyType() > worst)
                    {
                        target = e;
                        closestSoFar = dist;
                    }
                    else if(e.getEnemyType() == EnemyType.ZOMBEAR)
                    {
                        target = e;
                        return target;
                    }
                }
            }
        }


        return target;
    }

}
