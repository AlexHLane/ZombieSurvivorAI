using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_ReturnToPrev : BT_Task
{

    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        sAI.SetMvmtTree(MOVEMENT_TREE.DEFAULT);
        sAI.GetHiveRef().MoveSurvivorBack(sAI);
        //sAI.GetSurvivor().MoveTo(sAI.GetCurrDefensePoint().transform.position);

        return TASK_RETURN_STATUS.SUCCEED;
    }

}
