using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_GetDistFromEnemy : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        Blackboard bb = sAI.GetBlackboard();

        bb.distFromTarget = GetDistFromTarget(bb.enemyTarget, sAI);
        output = TASK_RETURN_STATUS.SUCCEED;

        return output;
    }



    float GetDistFromTarget(Enemy e, Survivor_AI sAI)
    {
        float dist;

        Vector3 dtt = e.gameObject.transform.position - sAI.gameObject.transform.position;
        dist = dtt.magnitude;

        return dist;
    }
}
