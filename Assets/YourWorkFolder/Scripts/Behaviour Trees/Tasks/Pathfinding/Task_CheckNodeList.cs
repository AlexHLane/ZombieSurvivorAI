using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_CheckNodeList : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;
        Blackboard bb = sAI.GetBlackboard();
        if(bb.nodesForAStar.Count > 0)
        {
            output = TASK_RETURN_STATUS.SUCCEED;
            
        }
        //for breakpointing
        else
        {
            output = TASK_RETURN_STATUS.FAILED;
        }
        return output;
    }
}
