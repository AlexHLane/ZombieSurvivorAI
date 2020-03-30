using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MOVE_TO_DEFENSE
{ 
    HAS_MOVED = 0,
    HAS_NOT_MOVED
}


public class Blackboard 
{

    public Enemy enemyTarget;
    public Enemy worstEnemyTarget;
    public Enemy closestBear;
    public Enemy []bunnyList;

    public float distFromTarget;
    public Vector3 moveToPosition;

    public List<PathNode> nodesForAStar = new List<PathNode>();
    public PathNode goalNode;
    public PathNode startNode;
    public PathNode currNodeToGetTo;

    public int WaveNumber = 0;
    public MOVE_TO_DEFENSE MovedToSpot = MOVE_TO_DEFENSE.HAS_NOT_MOVED;

    public Tower ne = null;
    public Tower nw = null;
    public Tower se = null;
    public Tower sw = null;

}
