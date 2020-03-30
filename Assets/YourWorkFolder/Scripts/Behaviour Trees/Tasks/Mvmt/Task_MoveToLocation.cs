using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_MoveToLocation : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        Blackboard bb = sAI.GetBlackboard();
        float dist = (sAI.transform.position - bb.moveToPosition).magnitude;
        if(dist > 1.0f)
        {
            UnityEngine.AI.NavMeshAgent agent = sAI.GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.destination = bb.moveToPosition;

            output = TASK_RETURN_STATUS.FAILED;
        }
        else
        {
            bb.nodesForAStar.Remove(bb.currNodeToGetTo);
            if (bb.nodesForAStar.Count > 0)
            {
                bb.currNodeToGetTo = bb.nodesForAStar[0];
                bb.moveToPosition = bb.currNodeToGetTo.transform.position;
            }
            output = TASK_RETURN_STATUS.SUCCEED;
        }

        return output;
    }
}
