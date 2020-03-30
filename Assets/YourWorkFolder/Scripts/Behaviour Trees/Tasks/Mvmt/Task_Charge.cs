using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Charge : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        Blackboard bb = sAI.GetBlackboard();

        if(bb.enemyTarget != null)
        {

            if(bb.enemyTarget.getEnemyType() == EnemyType.HELLEPHANT || bb.enemyTarget.getEnemyType() == EnemyType.ZOMBEAR)
            {
                
                Vector3 dir = bb.enemyTarget.transform.position - sAI.transform.position;
                float distFromCrystal = (Vector3.zero - sAI.transform.position).magnitude;
                float dist = dir.magnitude;
                if (dist > 5.0f && distFromCrystal < 50.0f)
                {
                    UnityEngine.AI.NavMeshAgent agent = sAI.GetComponent<UnityEngine.AI.NavMeshAgent>();

                    agent.destination = bb.enemyTarget.transform.position;

                    output = TASK_RETURN_STATUS.SUCCEED;
                }
            }


        }


        return output;
    }

}
