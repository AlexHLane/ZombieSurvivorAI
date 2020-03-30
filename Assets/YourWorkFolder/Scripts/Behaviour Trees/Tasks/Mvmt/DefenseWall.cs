using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DIRECTION
{
    NORTH = 0,
    SOUTH,
    EAST,
    WEST,
    NULL
}

//can have up to double occupation after single is all filled out

public class DefenseWall : MonoBehaviour
{

    public DefenseNode [] defenseNodes;

    public DIRECTION direction;


    private void Start()
    {
        defenseNodes = GetComponentsInChildren<DefenseNode>();
    }




}
