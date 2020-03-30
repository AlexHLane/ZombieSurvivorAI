using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Selector : BT_Task
{

    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;
        
        foreach(BT_Task child in children)
        {
            if(child.Run(sAI) == TASK_RETURN_STATUS.SUCCEED)
            {
                output = TASK_RETURN_STATUS.SUCCEED;
                break;
            }
        }      
        
        return output;
    }

}
