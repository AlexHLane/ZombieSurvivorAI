using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_IncrementWaveCount : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        sAI.GetBlackboard().WaveNumber++;
        return TASK_RETURN_STATUS.SUCCEED;
    }
}
