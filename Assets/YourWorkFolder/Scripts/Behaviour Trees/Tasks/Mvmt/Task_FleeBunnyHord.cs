using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_FleeBunnyHord : BT_Task
{

    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        Blackboard bb = sAI.GetBlackboard();

        if (GameManager.getZomBunnyList().Count > 5)
        {
            
            if ((FindClosestBunnyTarget(sAI).transform.position - sAI.transform.position).magnitude < 8.0f)
            {
                //sAI.gameObject
                Vector3 normalizedDir = FleeHord(sAI);
                UnityEngine.AI.NavMeshAgent agent = sAI.GetComponent<UnityEngine.AI.NavMeshAgent>();

                agent.destination = normalizedDir + sAI.transform.position;
                output = TASK_RETURN_STATUS.SUCCEED;
            }
        }

        return output;
    }


    public Vector3 FleeHord(Survivor_AI sAI)
    {
        Vector3 newDir = Vector3.zero;
        Vector3 sAIPos = sAI.transform.position;
        
        
        List<Enemy> bunnyList = GameManager.getZomBunnyList();

        int counter = 0;
        foreach(Enemy e in bunnyList)
        {
            Vector3 diff = sAIPos - e.gameObject.transform.position;
            newDir += diff;
            counter++;
        }

        newDir /= counter;

        newDir = newDir.normalized;

        return newDir;
    }

    Enemy FindClosestBunnyTarget(Survivor_AI sAI)
    {
        Enemy target = null;

        float closestSoFar = Mathf.Infinity;
        int worst = 0;

        List<Enemy> allEnemies = GameManager.getZomBunnyList();

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
