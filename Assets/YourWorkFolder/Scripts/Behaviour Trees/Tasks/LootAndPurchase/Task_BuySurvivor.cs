using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_BuySurvivor : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        
        if(GameManager.GetCurrentWaveNumber() > 2)
        {
            if (GameManager.GetCurrentGold() > 200 && GameManager.getSurvivorList().Count < 12)
            {
                GameManager.PurchaseSurvivor();
                GameManager.PurchaseSurvivor();

            }
            else if(GameManager.GetCurrentGold() > 100)
            {
                GameManager.PurchaseSurvivor();

            }
        }
        else
        {
            GameManager.PurchaseSurvivor();
        }
        return TASK_RETURN_STATUS.SUCCEED;
    }
}
