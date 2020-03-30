using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_CheckIfInBreakTime : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        /* UNLIKE NAME SUGGESTS, it will only return success if the wave is still running) */
        if(GameManager.InBreakTime() == false)
        {
            output = TASK_RETURN_STATUS.SUCCEED;
        }
        //else
        //{
            //sAI.GetBlackboard().WaveNumber++;
        //}

        return output;
    }
}
