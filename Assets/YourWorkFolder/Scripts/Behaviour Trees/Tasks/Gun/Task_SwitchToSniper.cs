using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_SwitchToSniper : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
      
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;
        Blackboard bb = sAI.GetBlackboard();
        if (sAI.mvmtTracker == Moving.MOVING_ON_ASTAR)
            return output;

        if (bb.distFromTarget > 25.0f && bb.distFromTarget < 37.0f && bb.enemyTarget.getEnemyType() == EnemyType.ZOMBEAR)
        {
            if (sAI.GetSurvivor().GetWeapon(WEAPON_TYPE.SNIPER).getAmmo() > 0) //&& sAI.GetBlackboard().enemyTarget.getEnemyType() != EnemyType.ZOMBUNNY)
            {
                sAI.GetSurvivor().SwitchWeapons(WEAPON_TYPE.SNIPER);
                output = TASK_RETURN_STATUS.SUCCEED;
            }
        }

        return output;
    }
}
