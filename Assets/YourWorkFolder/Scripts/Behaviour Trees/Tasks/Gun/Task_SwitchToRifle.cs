using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_SwitchToRifle : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {

        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;
        Blackboard bb = sAI.GetBlackboard();

        if (sAI.mvmtTracker == Moving.MOVING_ON_ASTAR)
            return output;

        if (bb.distFromTarget < 35.0f && bb.distFromTarget > 25.0f)
        {
            if (sAI.GetSurvivor().GetWeapon(WEAPON_TYPE.ASSAULT).getAmmo() > 0)
            {
                sAI.GetSurvivor().SwitchWeapons(WEAPON_TYPE.ASSAULT);
                output = TASK_RETURN_STATUS.SUCCEED;
            }
        }

        return output;
    }

}
