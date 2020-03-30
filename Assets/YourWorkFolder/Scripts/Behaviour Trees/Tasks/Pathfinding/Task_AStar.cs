using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_AStar : BT_Task
{

    public override TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        //run A*
        TASK_RETURN_STATUS output = TASK_RETURN_STATUS.FAILED;

        Blackboard bb = sAI.GetBlackboard();


        if (bb.nodesForAStar.Count <= 0)
        {
            GameObject go = GameObject.FindGameObjectWithTag("AStar");
            AStarObj aStarObj = go.GetComponent<AStarObj>();
            //AStarObj aStarObj = GameObject.FindGameObjectWithTag("AStar").GetComponent<AStarObj>(); //new AStarObj();
            aStarObj.RunThroughPath(sAI, bb.startNode, bb.goalNode);
            if (bb.nodesForAStar.Count > 0)
            {
                output = TASK_RETURN_STATUS.SUCCEED;
                bb.currNodeToGetTo = bb.nodesForAStar[0];
                bb.moveToPosition = bb.currNodeToGetTo.transform.position;
            }
        }
        else
        {
            output = TASK_RETURN_STATUS.SUCCEED;
        }

        return output;
    }





}
