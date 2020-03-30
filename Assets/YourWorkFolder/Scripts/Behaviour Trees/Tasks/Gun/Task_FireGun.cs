using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_FireGun : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        Enemy target = sAI.GetBlackboard().enemyTarget;


        if (target != null)
        {
            sAI.GetSurvivor().Fire(target.transform.position + (sAI.GetEnemyHeightOffset() * Vector3.up));
            output = TASK_RETURN_STATUS.SUCCEED;
        }



        return output;
    }
}
