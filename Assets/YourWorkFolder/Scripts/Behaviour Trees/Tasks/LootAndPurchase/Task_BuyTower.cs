using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_BuyTower : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        Blackboard bb = sAI.GetBlackboard();
        if(GameManager.GetCurrentWaveNumber() > 4 && GameManager.GetCurrentGold() > 150)
        {
            if (bb.ne == null)
            {
                bb.ne = new Tower();
                GameManager.PurchaseTower(TOWER_TYPE.BULLET_TOWER, TOWER_POSITION.NORTHEAST);
            }
            else if (bb.nw == null)
            {
                bb.nw = new Tower();
                GameManager.PurchaseTower(TOWER_TYPE.BULLET_TOWER, TOWER_POSITION.NORTHWEST);
            }
            else if (bb.se == null)
            {
                bb.se = new Tower();
                GameManager.PurchaseTower(TOWER_TYPE.BULLET_TOWER, TOWER_POSITION.NORTHEAST);
            }
            else
            {
                GameManager.PurchaseTower(TOWER_TYPE.BULLET_TOWER, TOWER_POSITION.NORTHEAST);
                bb.sw = new Tower();
            }

        }
        return TASK_RETURN_STATUS.SUCCEED;
    }
}
