using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Sequence : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.SUCCEED;

        foreach(BT_Task child in children)
        {
            if(child.Run(sAI) == TASK_RETURN_STATUS.FAILED)
            {
                output = TASK_RETURN_STATUS.FAILED;
                break;
            }
        }

        return output;
    }

}
