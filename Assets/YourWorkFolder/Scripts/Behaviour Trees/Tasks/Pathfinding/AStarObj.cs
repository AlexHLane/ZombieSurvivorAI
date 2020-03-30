using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarObj : MonoBehaviour
{


    float FVal;
    GameObject[] pathNodeArr;
    List<PathNode> pathNodeList;

    List<PathNode> open;
    List<PathNode> closed;

    List<PathNode> finalPath;

    void Start()
    {
        pathNodeArr = GameObject.FindGameObjectsWithTag("PNode");
        pathNodeList = new List<PathNode>();


        foreach (GameObject go in pathNodeArr)
        {
            PathNode n = go.GetComponent<PathNode>();
            pathNodeList.Add(n);
        }

        open = new List<PathNode>();
        closed = new List<PathNode>();
        finalPath = new List<PathNode>();
    }

    /*public AStarObj()
    {
        pathNodeArr = GameObject.FindGameObjectsWithTag("PNode");
        pathNodeList = new List<PathNode>();


        foreach (GameObject go in pathNodeArr)
        {
            PathNode n = go.GetComponent<PathNode>();
            pathNodeList.Add(n);
        }

        open = new List<PathNode>();
        closed = new List<PathNode>();
        finalPath = new List<PathNode>();
    }*/

    
    public void RunThroughPath(Survivor_AI sAI, PathNode start, PathNode end)
    {
        NullOutParentsAndDist();
        CalculatePath(sAI, start, end);
        sAI.GetBlackboard().nodesForAStar = finalPath;
    }

    void NullOutParentsAndDist()
    {
        foreach(PathNode n in pathNodeList)
        {
            n.SetParent(null);
            n.SetFVal(0);
        }
    }

    float CalculateFVal(Vector3 currPos, Vector3 startPos, Vector3 goalPos)
    {
        float G = CalculateDistFromStart(startPos, currPos);
        float H = CalculateDistFromGoal(currPos, goalPos);

        FVal = G + H; //then add in weights whenever that gets added in
        return FVal;
    }

    float CalculateDistFromStart(Vector3 startPos, Vector3 currPos)
    {
        return (startPos - currPos).magnitude;
    }

    float CalculateDistFromGoal(Vector3 currPos, Vector3 goalPos)
    {
        return (goalPos - currPos).magnitude;
    }

    void CalculatePath(Survivor_AI sAI, PathNode start, PathNode end)
    {
        //clean out stack & lists
        open.Clear();
        closed.Clear();
        finalPath.Clear();

        //start off with closest node
        PathNode currNode = start;
        closed.Add(currNode);
        //currNode.SetFVal(CalculateFVal(currNode.transform.position, start.transform.position, end.transform.position));
        CalculateNeighbors(currNode, start, end);
        
        //for each node until
        while (open.Count > 0) {

            if (open.Count > 0)
            {
                currNode = open[0];

                closed.Add(currNode);
                CalculateNeighbors(currNode, start, end);
                open.Remove(currNode);

            }

            if(currNode == end)
            {
                finalPath = OrderingNodesForFollowing(currNode);
                return;
            }  
        }

        if(open.Count == 0)
        {
            print("Path not found");
        }
    }

    void CalculateNeighbors(PathNode currNode, PathNode start, PathNode end)
    {
        PathNode[] neighborList = currNode.GetNeighbors();
        foreach (PathNode n in neighborList)
        {
            if (!closed.Contains(n))
            {
                if (!open.Contains(n))
                {
                    n.SetFVal(CalculateFVal(n.transform.position, start.transform.position, end.transform.position));
                    n.SetParent(currNode);
                    InsertIntoOpen(n);
                }
            }
        }
    }

    /* Inserts into the open list after calculations */
    void InsertIntoOpen(PathNode pn)
    {
        int counter = 0;
        while(counter < open.Count)
        {
            PathNode n = open[counter];
            counter++;
            //pn is bigger than n
            if (pn.GetFVal() > n.GetFVal())
            {
                open.Insert(counter, pn);
                return;
            }

        }
        open.Insert(counter, pn);
    }

    /* Inserts into the list in the correct order for traversing the nodes */
    public List<PathNode> OrderingNodesForFollowing(PathNode node)
    {
        List<PathNode> correctPath = new List<PathNode>();
        while(node.GetParent() != null)
        {
            correctPath.Insert(0, node);
            node = node.GetParent();
        }

        correctPath.Insert(0, node);
        return correctPath;
    }


    /* DEPRECATED
    PathNode GetSmallestFValPathNodeArr(PathNode[] neighborList, PathNode curr, PathNode start, PathNode end)
    {
        PathNode nextNode = new PathNode();

        float minF = Mathf.Infinity;
        //
        foreach (PathNode neighbor in neighborList)
        {
            FVal = CalculateFVal(curr.transform.position, start.transform.position, end.transform.position);
            if (FVal < minF)
            {
                nextNode = neighbor;
                minF = FVal;
            }
        }

        return nextNode;
    }

    PathNode GetSmallestFValPathNodeList(List<PathNode> neighborList, PathNode curr, PathNode start, PathNode end)
    {
        PathNode nextNode = new PathNode();

        float minF = Mathf.Infinity;
        //
        foreach (PathNode neighbor in neighborList)
        {
            FVal = CalculateFVal(curr.transform.position, start.transform.position, end.transform.position);
            if (FVal < minF)
            {
                
                nextNode = neighbor;
                minF = FVal;
            }
        }

        return nextNode;
    }
    */

}
