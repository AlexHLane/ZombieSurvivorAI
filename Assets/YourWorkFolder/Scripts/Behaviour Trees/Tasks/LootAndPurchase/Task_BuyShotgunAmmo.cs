using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_BuyShotgunAmmo : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        if(sAI.GetSurvivor().GetWeapon(WEAPON_TYPE.SHOTGUN).getAmmo() < 10)
        {
            //while (GameManager.GetCurrentGold() >= 25)
            //{
            GameManager.PurchaseAmmo(WEAPON_TYPE.SHOTGUN);
            GameManager.PurchaseAmmo(WEAPON_TYPE.SHOTGUN);
            //}
        }
        return TASK_RETURN_STATUS.SUCCEED;
    }
}
