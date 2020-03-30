﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_WaitOnCurrEnemies : BT_Task
{

    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        if(GameManager.getAllEnemies().Count > 4)
        {
            output = TASK_RETURN_STATUS.SUCCEED;
        }

        return output;
    }
}
