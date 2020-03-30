using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_ThrowGrenade : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        WEAPON_TYPE prevType = sAI.GetSurvivor().GetCurrentWeapon().type;
        sAI.GetSurvivor().SwitchWeapons(WEAPON_TYPE.GRENADE_LAUNCHER);
        if(sAI.GetSurvivor().GetCurrentWeapon().getAmmo() > 0)
        {
            Enemy target = sAI.GetBlackboard().enemyTarget;
            sAI.GetSurvivor().Fire(target.transform.position + (sAI.GetEnemyHeightOffset() * Vector3.up));
            sAI.GetSurvivor().SwitchWeapons(prevType);
            output = TASK_RETURN_STATUS.SUCCEED;
        }
        return output;
    }
}
