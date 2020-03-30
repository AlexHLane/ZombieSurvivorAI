using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MOVEMENT_TREE
{ 
    DEFAULT = 0,
    DEFENSE,
    ASTAR,
    AGGRESSIVE,
    SEARCHING
}


public class HiveOfSurvivors : MonoBehaviour
{
    Survivor_AI[] currSurvivors;
    public DefenseWall[] defenses;
    int currWave;
    bool AreMovingToDFPoints;

    DefenseNode[] wave3Nodes;
    DefenseNode[] wave4Nodes;
    DefenseNode[] wave5AndUpNodes;

    PathNode[] pathNodeList;

    public bool SearchingForLoot;


    void Start()
    {
        currSurvivors = GameObject.FindObjectsOfType<Survivor_AI>();
        pathNodeList = GameObject.FindObjectsOfType<PathNode>(); //FindGameObjectsWithTag("PNode");

        AreMovingToDFPoints = false;
        CalculateOutDFLists();
        SearchingForLoot = false;

        Invoke("UpdateDefensePoints", 1.0f);
        Invoke("SearchForLoot", 10.0f);
    }


    void Update()
    {
        
    }

    void ResetMvmtTree()
    {
        foreach(Survivor_AI sai in currSurvivors)
        {
            sai.SetMvmtTree(MOVEMENT_TREE.DEFAULT);
        }
    }

    public void UpdateSurvivorList()
    {
        currSurvivors = GameObject.FindObjectsOfType<Survivor_AI>();
    }
    

    /* FOR SHOWING OFF ASTAR IN GAME IF TIME *****************************************/
    void SearchForLoot()
    {
        if (SearchingForLoot == false) // && GameManager.GetCurrentWaveNumber() < 4)
        {
            List<Loot> lootlist = GameManager.GetLootList();
            if (lootlist.Count > 0)
            {
                this.UpdateSurvivorList();

                foreach (Loot loot in lootlist)
                {
                    //if(loot.GetLootType() != LOOT_TYPE.GRENADES || loot.GetLootType() != LOOT_TYPE.NULL)
                    //{
                    Survivor_AI moving = GetClosestSurvivor(loot.transform.position);
                    PathNode[] nodes = GetClosestPathNode(moving.transform.position, loot.transform.position);
                    if (moving != null)
                    {
                        if (moving.mvmtTracker != Moving.MOVING_ON_ASTAR)
                        {
                            moving.start = nodes[0];
                            moving.end = nodes[1];
                            moving.GetBlackboard().startNode = nodes[0];
                            moving.GetBlackboard().goalNode = nodes[1];
                            moving.SetMvmtTree(MOVEMENT_TREE.ASTAR);
                            SearchingForLoot = true;
                            moving.mvmtTracker = Moving.MOVING_ON_ASTAR;
                            break;
                        }
                    }
                    //}
                }
            }
            else
            {
                Invoke("SearchForLoot", 15.0f);
            }

        }



    }

    Survivor_AI GetClosestSurvivor(Vector3 lootspawn)
    {
        Survivor_AI toMove = null;

        float minDist = 1000000;

        if (currSurvivors.Length > 0)
            toMove = currSurvivors[0];

        foreach(Survivor_AI sAI in currSurvivors)
        {
            float d = (lootspawn - sAI.transform.position).magnitude;
            if(d < minDist && sAI.mvmtTracker != Moving.MOVING_ON_ASTAR)
            {
                toMove = sAI;
                minDist = d;
            }
        }

        return toMove;
    }

    PathNode[] GetClosestPathNode(Vector3 sailocation, Vector3 lootloc)
    {
        PathNode[] node = new PathNode[2];

        float minDist = 1000000;
        float minDistLoot = 1000000;

        foreach (PathNode currNode in pathNodeList)
        {
            float d = (sailocation - currNode.transform.position).magnitude;
            if (d < minDist)
            {
                node[0] = currNode;
                minDist = d;
            }

            float d2 = (lootloc - currNode.transform.position).magnitude;
            if(d2 < minDistLoot)
            {
                node[1] = currNode;
                minDistLoot = d2;
            }
        }

        return node;
    }

    public void MoveSurvivorBack(Survivor_AI sAI)
    {
        sAI.GetSurvivor().MoveTo(sAI.GetCurrDefensePoint().transform.position);
        sAI.mvmtTracker = Moving.NOT_MOVING_ON_ASTAR;
        SearchingForLoot = false;
        Invoke("SearchForLoot", 20.0f);
    }


    /* DEFENSE ***************************************************************************/
    void ClearDefensePoints()
    {
        foreach(Survivor_AI sAI in currSurvivors)
        {
            sAI.SetDefensePoint(null);
        }

        
        foreach(DefenseWall wall in defenses)
        {
            
            foreach(DefenseNode node in wall.defenseNodes)
            {
                node.SetOccupiedStatus(OCCUPIED.NOT_OCCUPIED);
            }
        }
    }


    DefenseWall GetDefenseWall(DIRECTION dir)
    {
        DefenseWall wallNodes = null;

        foreach(DefenseWall df in defenses)
        {
            if(df.direction == dir)
            {
                wallNodes = df;
            }
        }

        return wallNodes;
    }

    void SetUpDefensePosition(DefenseNode[] dwall) //DefenseWall dwall)
    {


        //bool allthewaythrough = false;
        int nodeCounter = 0;
        int dwallLength = dwall.Length;

        for (int k = 0; k < currSurvivors.Length; k++)
        {
            DefenseNode target = null;
 
            for(int i = 0; i < dwallLength; i++)
            {
                DefenseNode currNode = dwall[i];

                if(currNode.GetOccupiedStatus() == OCCUPIED.NOT_OCCUPIED)
                {
                    target = currNode;
                    currSurvivors[k].SetDefensePoint(currNode);
                    if(currSurvivors[k].mvmtTracker == Moving.NOT_MOVING_ON_ASTAR)
                        currSurvivors[k].GetSurvivor().MoveTo(currNode.transform.position);
                    currNode.SetOccupiedStatus(OCCUPIED.SINGLE);
                    nodeCounter++;
                    //set mvmt tree ?
                    //allthewaythrough = false;
                    break;
                }
                else if(currNode.GetOccupiedStatus() == OCCUPIED.SINGLE && nodeCounter >= dwallLength)
                {
                    target = currNode;
                    currSurvivors[k].SetDefensePoint(currNode);
                    if (currSurvivors[k].mvmtTracker == Moving.NOT_MOVING_ON_ASTAR)
                        currSurvivors[k].GetSurvivor().MoveTo(currNode.transform.position);
                    currNode.SetOccupiedStatus(OCCUPIED.DOUBLE);

                    //again set tree? i don't think so...
                    //allthewaythrough = false;
                    break;
                }
                /*else if(allthewaythrough == false && i < dwallLength-1)
                {
                    continue;
                }

                allthewaythrough = true;*/
            }

            //set tree if target == null
            if(target == null)
            {
                //tree thing here to go out and about, too many hens in the house
            }
        }
    }



    public void UpdateDefensePoints()
    {
        if (AreMovingToDFPoints == false && GameManager.GetBreakTimeRemaining() < 10.0f && GameManager.InBreakTime())
        {
            this.UpdateSurvivorList();
            this.ClearDefensePoints();
            currWave = GameManager.GetCurrentWaveNumber() + 1;

            if (currWave == 1)
             {
                 //north
                 DefenseWall pos = GetDefenseWall(DIRECTION.NORTH);
                 SetUpDefensePosition(pos.defenseNodes);
             }
            if (currWave == 2)
            {
                //south
                DefenseWall pos = GetDefenseWall(DIRECTION.SOUTH);
                SetUpDefensePosition(pos.defenseNodes);
            }
            else if (currWave == 3)
            {
                //north, south
                //DefenseWall pos1 = GetDefenseWall(DIRECTION.NORTH);
                //DefenseWall pos2 = GetDefenseWall(DIRECTION.SOUTH);

                

                SetUpDefensePosition(wave3Nodes);
                //SetUpDefensePosition(pos2);

            }
            else if (currWave == 4)
            {
                //north, south, east, west
                //DefenseWall pos1 = GetDefenseWall(DIRECTION.NORTH);
                //DefenseWall pos2 = GetDefenseWall(DIRECTION.SOUTH);
                //DefenseWall pos3 = GetDefenseWall(DIRECTION.WEST);
                //DefenseWall pos4 = GetDefenseWall(DIRECTION.EAST);
                //
                //SetUpDefensePosition(pos1);
                //SetUpDefensePosition(pos2);
                //SetUpDefensePosition(pos3);
                SetUpDefensePosition(wave3Nodes);
            }
            else if(currWave == 5)
            {
                SetUpDefensePosition(wave4Nodes);
            }
            else if (currWave > 5)
            {
                SetUpDefensePosition(wave5AndUpNodes);

            }
            /*else if (currWave > 4)
            {
                //north, south, east, west
                DefenseWall pos1 = GetDefenseWall(DIRECTION.NORTH);
                DefenseWall pos2 = GetDefenseWall(DIRECTION.SOUTH);
                DefenseWall pos3 = GetDefenseWall(DIRECTION.WEST);
                DefenseWall pos4 = GetDefenseWall(DIRECTION.EAST);

                SetUpDefensePosition(pos1);
                SetUpDefensePosition(pos2);
                SetUpDefensePosition(pos3);
                SetUpDefensePosition(pos4);
            }*/

            AreMovingToDFPoints = true;
        }
        else if(GameManager.InBreakTime() == false)
        {
            AreMovingToDFPoints = false;
        }

        Invoke("UpdateDefensePoints", 4.0f);

    }


    void CalculateOutDFLists()
    {
        // wave 3
        DefenseWall pos1 = GetDefenseWall(DIRECTION.NORTH);
        DefenseWall pos2 = GetDefenseWall(DIRECTION.SOUTH);

        wave3Nodes = new DefenseNode[8];
        int counter = 0;
        foreach(DefenseNode n in pos1.defenseNodes)
        {
            wave3Nodes[counter] = n;
            counter++;
        }
        foreach(DefenseNode n in pos2.defenseNodes)
        {
            wave3Nodes[counter] = n;
            counter++;
        }

        counter = 0;

        /* wave 4 */

        pos1 = GetDefenseWall(DIRECTION.WEST);
        pos2 = GetDefenseWall(DIRECTION.EAST);
        wave4Nodes = new DefenseNode[8];
        counter = 0;
        foreach (DefenseNode n in pos1.defenseNodes)
        {
            wave4Nodes[counter] = n;
            counter++;
        }
        foreach (DefenseNode n in pos2.defenseNodes)
        {
            wave4Nodes[counter] = n;
            counter++;
        }

        counter = 0;

        //wave 5+

        pos1 = GetDefenseWall(DIRECTION.NORTH);
        pos2 = GetDefenseWall(DIRECTION.SOUTH);
        DefenseWall pos3 = GetDefenseWall(DIRECTION.WEST);
        DefenseWall pos4 = GetDefenseWall(DIRECTION.EAST);

        wave5AndUpNodes = new DefenseNode[16];

        foreach (DefenseNode n in pos1.defenseNodes)
        {
            wave5AndUpNodes[counter] = n;
            counter++;
        }
        foreach (DefenseNode n in pos2.defenseNodes)
        {
            wave5AndUpNodes[counter] = n;
            counter++;
        }
        foreach (DefenseNode n in pos3.defenseNodes)
        {
            wave5AndUpNodes[counter] = n;
            counter++;
        }
        foreach (DefenseNode n in pos4.defenseNodes)
        {
            wave5AndUpNodes[counter] = n;
            counter++;
        }
    }




}
