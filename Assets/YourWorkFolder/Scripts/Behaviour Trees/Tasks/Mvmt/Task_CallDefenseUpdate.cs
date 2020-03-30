using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_CallDefenseUpdate : BT_Task
{
    HiveOfSurvivors hivemind;

    public Task_CallDefenseUpdate(HiveOfSurvivors hvm)
    {
        hivemind = hvm;
    }

    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        hivemind.UpdateDefensePoints();

        base.Run(sAI);

        return TASK_RETURN_STATUS.SUCCEED;
    }
}
