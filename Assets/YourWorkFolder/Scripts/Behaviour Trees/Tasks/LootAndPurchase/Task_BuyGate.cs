using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_BuyGate : BT_Task
{
    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        Gate north = GameManager.GetGate(GATE_POSITION.NORTH);
        Gate east = GameManager.GetGate(GATE_POSITION.EAST);
        Gate south = GameManager.GetGate(GATE_POSITION.SOUTH);
        Gate west = GameManager.GetGate(GATE_POSITION.WEST);

        
        if(north.GetHealth() <= 0)
        {
            GameManager.RepairGate(GATE_POSITION.NORTH);
        }

        if (east.GetHealth() <= 0)
        {
            GameManager.RepairGate(GATE_POSITION.EAST);
        }

        if (south.GetHealth() <= 0)
        {
            GameManager.RepairGate(GATE_POSITION.SOUTH);
        }

        if (west.GetHealth() <= 0)
        {
            GameManager.RepairGate(GATE_POSITION.WEST);
        }


        return TASK_RETURN_STATUS.SUCCEED;
    }
}
