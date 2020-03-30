using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence_MoveToDefense : BT_Sequence
{

    BT_Task select;
    BT_Task runThru;
    public Sequence_MoveToDefense(HiveOfSurvivors hivemind)
    {
        select = new BT_Selector();
        select.AddTask(new Task_WaitOnCurrEnemies());

        runThru = new BT_Sequence();
        runThru.AddTask(new Task_CallDefenseUpdate(hivemind));
        runThru.AddTask(new Task_MoveToDefensePoint());

        select.AddTask(runThru);

        children.Add(select);
        //children.Add(runThru);
        //children.Add(new Task_CallDefenseUpdate(hivemind));
        //children.Add(new Task_MoveToDefensePoint());
    }

    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        base.Run(sAI);
        return TASK_RETURN_STATUS.SUCCEED;
    }
}
