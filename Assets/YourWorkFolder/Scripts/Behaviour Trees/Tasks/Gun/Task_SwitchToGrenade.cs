using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_SwitchToGrenade : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        if(sAI.GetSurvivor().GetWeapon(WEAPON_TYPE.GRENADE_LAUNCHER).getAmmo() > 0)
        {
            if ((sAI.transform.position - sAI.GetBlackboard().enemyTarget.transform.position).magnitude <= 10)
            {
                sAI.GetSurvivor().SwitchWeapons(WEAPON_TYPE.GRENADE_LAUNCHER);
                output = TASK_RETURN_STATUS.SUCCEED;
            }
        }

        return output;
    }
}
