using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_BuySniperAmmo : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        if (sAI.GetSurvivor().GetWeapon(WEAPON_TYPE.SNIPER).getAmmo() <= 12)
        {
            //while (GameManager.GetCurrentGold() > 50)
            if (GameManager.GetCurrentGold() > 160)
            {
                GameManager.PurchaseAmmo(WEAPON_TYPE.SNIPER);
                GameManager.PurchaseAmmo(WEAPON_TYPE.SNIPER);
                GameManager.PurchaseAmmo(WEAPON_TYPE.SNIPER);
            }
            else if (GameManager.GetCurrentGold() > 80)
            {
                GameManager.PurchaseAmmo(WEAPON_TYPE.SNIPER);
                GameManager.PurchaseAmmo(WEAPON_TYPE.SNIPER);
            }
            else if (GameManager.GetCurrentGold() > 40)
            {
                GameManager.PurchaseAmmo(WEAPON_TYPE.SNIPER);
            }
        }
        
        return TASK_RETURN_STATUS.SUCCEED;
    }
}
