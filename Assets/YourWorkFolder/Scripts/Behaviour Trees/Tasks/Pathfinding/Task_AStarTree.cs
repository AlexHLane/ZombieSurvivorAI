using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_AStarTree : BT_Task
{
    public BT_Task aStarTree;
    BT_Task runThroughAStar;
    BT_Task aStarFinished;

    public Task_AStarTree()
    {
        aStarTree = new BT_Sequence();
        runThroughAStar = new BT_Sequence();
        aStarFinished = new BT_Selector();

        aStarTree.AddTask(new Task_AStar());
        aStarTree.AddTask(runThroughAStar);

        runThroughAStar.AddTask(new Task_CheckNodeList());
        runThroughAStar.AddTask(new Task_MoveToLocation());
        runThroughAStar.AddTask(aStarFinished);

        aStarFinished.AddTask(new Task_CheckNodeList());
        aStarFinished.AddTask(new Task_SwitchTree());
    }

    

    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        return base.Run(sAI);

    }
}
