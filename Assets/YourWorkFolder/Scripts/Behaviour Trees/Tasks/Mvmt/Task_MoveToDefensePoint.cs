using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_MoveToDefensePoint : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        //TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;


        //if(sAI.transform.position != sAI.GetCurrDefensePoint().transform.position)
        //{
            sAI.GetSurvivor().MoveTo(sAI.GetCurrDefensePoint().transform.position);
       // }
        //else
        //{
            //output = TASK_RETURN_STATUS.SUCCEED;
        //}

        return TASK_RETURN_STATUS.SUCCEED;
    }
}
