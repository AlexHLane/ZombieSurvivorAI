using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TASK_RETURN_STATUS
{
    SUCCEED,
    FAILED
}

public abstract class BT_Task
{
    protected List<BT_Task> children;


    public BT_Task()
    {
        children = new List<BT_Task>();
    }

    public void AddTask(BT_Task t)
    {
        children.Add(t);
    }

    public void RemoveTask(BT_Task t)
    {
        children.Remove(t);
    }

    public void ClearTasks()
    {
        children.Clear();
    }

    public virtual TASK_RETURN_STATUS Run(Survivor_AI sAI)
    {
        return TASK_RETURN_STATUS.FAILED;
    }


}
