using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_FleeBasic : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        Blackboard bb = sAI.GetBlackboard();
        if (bb.enemyTarget != null)
        {

            Vector3 dir = sAI.transform.position - bb.enemyTarget.gameObject.transform.position;

            float dist = dir.magnitude;
            if (dist < 5.0f) {

                //dir.Normalize();

                dir += sAI.transform.position;
                UnityEngine.AI.NavMeshAgent agent = sAI.GetComponent<UnityEngine.AI.NavMeshAgent>();
                agent.destination = dir;

                //sAI.transform.position += dir;

                output = TASK_RETURN_STATUS.SUCCEED;
            }
        }
        return output;
    }

}
