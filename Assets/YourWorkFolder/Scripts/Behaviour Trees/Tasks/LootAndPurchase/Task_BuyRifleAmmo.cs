using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_BuyRifleAmmo : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {

        while (GameManager.GetCurrentGold() >= 10) //40)
        {
            GameManager.PurchaseAmmo(WEAPON_TYPE.ASSAULT);
        }
        
        return TASK_RETURN_STATUS.SUCCEED;
    }
}
