using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_SwitchToShotgun : BT_Task
{

    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;
        Blackboard bb = sAI.GetBlackboard();

        if (bb.distFromTarget < 10.0f)
        {
            if (sAI.GetSurvivor().GetWeapon(WEAPON_TYPE.SHOTGUN).getAmmo() > 0)
            {
                sAI.GetSurvivor().SwitchWeapons(WEAPON_TYPE.SHOTGUN);
                output = TASK_RETURN_STATUS.SUCCEED;
            }
        }

        return output;
    }

}
