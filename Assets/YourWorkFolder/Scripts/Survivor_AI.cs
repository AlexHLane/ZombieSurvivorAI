using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Moving
{
    MOVING_ON_ASTAR = 0,
    NOT_MOVING_ON_ASTAR
}

public class Survivor_AI : MonoBehaviour
{
    Survivor survivor;

    //set in the start so that 
    HiveOfSurvivors hiveRef;

    //top level
    BT_Task btBrain;
    BT_Task mvmtBrain;

    //generic trees to be filled in by specific trees
    BT_Task mvmtTree;
    BT_Task gunFireTree;
    BT_Task purchasingTree;

    //specific trees that fit into the above 3 categories
    BT_Task findEnemyBrain;
    BT_Task chooseGun;

    //astartree top level
    BT_Task aStarTree;
    BT_Task runThroughAStar;
    BT_Task aStarFinished;

    //defaulttree top level
    BT_Task defaultMvmtTree;
    BT_Task defenseMvmtTree;

    BT_Task runThroughPurchases;


    //sAI field
    Blackboard blackboard;
    float enemyHeightOffset = 0.0f;
    [SerializeField]
    DefenseNode currDefensePoint;

    /* for showing off AStar */
    //[SerializeField]
    public PathNode start;
    //[SerializeField]
    public PathNode end;

    public Moving mvmtTracker;


    void Start()
    {
        survivor = GetComponent<Survivor>();
        blackboard = new Blackboard();
        hiveRef = GameObject.FindObjectOfType<HiveOfSurvivors>();

        btBrain = new BT_Sequence();
        mvmtBrain = new BT_Sequence();
        mvmtTree = new BT_Sequence();

        this.CreateAStarBT();
        this.CreateDefaultMvmtTree();
        this.CreateMultiGunBT();
        this.SetMvmtTree(MOVEMENT_TREE.DEFAULT);

        mvmtBrain.AddTask(mvmtTree);

        blackboard.startNode = start;
        blackboard.goalNode = end;

        mvmtTracker = Moving.NOT_MOVING_ON_ASTAR;

    }

    void Update()
    {
        if(survivor.debugmode == DEBUGMODE.DISABLED)
        {
            btBrain.Run(this);
            mvmtBrain.Run(this);
            
        }
    }

    public Weapon GetCurrWeapon()
    {
        return survivor.currentWeapon;
    }


    public Survivor GetSurvivor()
    {
        return survivor;
    }

    public Blackboard GetBlackboard()
    {
        return blackboard;
    }

    public float GetEnemyHeightOffset()
    {
        return enemyHeightOffset;
    }

    public DefenseNode GetCurrDefensePoint()
    {
        return currDefensePoint;
    }

    public void SetDefensePoint(DefenseNode n)
    {
        currDefensePoint = n;
    }

    void CreateMultiGunBT()
    {        

        findEnemyBrain = new BT_Selector();
        chooseGun = new BT_Selector();

        //*
        purchasingTree = new BT_Selector();
        runThroughPurchases = new BT_Sequence();

        btBrain.AddTask(purchasingTree);
        purchasingTree.AddTask(new Task_CheckIfInBreakTime());
        purchasingTree.AddTask(runThroughPurchases);
        runThroughPurchases.AddTask(new Task_BuyGate());
        runThroughPurchases.AddTask(new Task_BuySurvivor());
        runThroughPurchases.AddTask(new Task_BuyTower());
        runThroughPurchases.AddTask(new Task_BuyShotgunAmmo());
        //runThroughPurchases.AddTask(new Task_BuyRifleAmmo());
        runThroughPurchases.AddTask(new Task_BuySniperAmmo());
        runThroughPurchases.AddTask(new Task_BuyRifleAmmo());
        //runThroughPurchases.AddTask(new Sequence_MoveToDefense(hiveRef));
        //*/

        btBrain.AddTask(findEnemyBrain);
        btBrain.AddTask(new Task_GetDistFromEnemy());
        btBrain.AddTask(chooseGun);
        btBrain.AddTask(new Task_FireGun());
        //btBrain.AddTask(new Task_ThrowGrenade());
        //btBrain.AddTask(new Task_FleeBasic());


        //btBrain.AddTask(new Task_Charge());
        //btBrain.AddTask(new Task_FleeBunnyHord());

        //findEnemyBrain.AddTask(new Task_FindClosestWorstEnemy());
        findEnemyBrain.AddTask(new Task_FindClosestBear());
        findEnemyBrain.AddTask(new Task_FindHellephant());
        findEnemyBrain.AddTask(new Task_FindClosestEnemy());

        chooseGun.AddTask(new Task_SwitchToGrenade());
        chooseGun.AddTask(new Task_SwitchToShotgun());
        chooseGun.AddTask(new Task_SwitchToSniper());
        chooseGun.AddTask(new Task_SwitchToRifle());
        chooseGun.AddTask(new Task_SwitchToPistol());



    }

    void CreateDefaultMvmtTree()
    {
        mvmtTree = new BT_Sequence();
        //mvmtBrain = mvmtTree;
    }

    void CreateDefenseMvmtTree()
    {
        defenseMvmtTree = new Sequence_MoveToDefense(hiveRef);
    }

    void CreateAStarBT()
    {
        aStarTree = new BT_Sequence();

        //btBrain.AddTask(aStarTree);

        runThroughAStar = new BT_Sequence();
        aStarFinished = new BT_Selector();

        aStarTree.AddTask(new Task_AStar());
        aStarTree.AddTask(runThroughAStar);

        runThroughAStar.AddTask(new Task_CheckNodeList());
        runThroughAStar.AddTask(new Task_MoveToLocation());
        runThroughAStar.AddTask(aStarFinished);

        aStarFinished.AddTask(new Task_CheckNodeList());
        aStarFinished.AddTask(new Task_ReturnToPrev());
        //aStarFinished.AddTask(new Task_SwitchTree());

        mvmtTree = aStarTree;
    }
   
    void CreateLootAndWaveTree()
    {
        defaultMvmtTree = new BT_Sequence();
    }


    public void SetMvmtTree(MOVEMENT_TREE type)
    {
        mvmtBrain.ClearTasks();

        switch (type)
        {
            case MOVEMENT_TREE.DEFAULT:
                mvmtTree = defaultMvmtTree;
                mvmtTracker = Moving.NOT_MOVING_ON_ASTAR;
                mvmtBrain.AddTask(mvmtTree);
                break;
            case MOVEMENT_TREE.DEFENSE:
                mvmtTree = defenseMvmtTree;
                mvmtTracker = Moving.NOT_MOVING_ON_ASTAR;
                mvmtBrain.AddTask(mvmtTree);
                break;
            case MOVEMENT_TREE.ASTAR:
                mvmtTree = aStarTree;
                mvmtTracker = Moving.MOVING_ON_ASTAR;
                mvmtBrain.AddTask(mvmtTree);
                break;
            case MOVEMENT_TREE.AGGRESSIVE:
                break;
            case MOVEMENT_TREE.SEARCHING:
                break;
        }
    }

    public void SetMvmtTree(BT_Task tree)
    {
        mvmtTree = tree;
    }

    public HiveOfSurvivors GetHiveRef()
    {
        return hiveRef;
    }



}

/* Wills crap 
 *     Task brain;
    BlackBoard blackboard;

    public float enemyHeightOffset = 0.5f;
    public float enemySearchRadius = 50;
    public float fireBallSearchRadius = 50;

    Task BT_Fire;

    // Start is called before the first frame update
    void Start()
    {
        survivor = GetComponent<Survivor>();

        blackboard = new BlackBoard();

        BT_Fire = new Sequence_DefaultFire();

        brain = BT_Fire;

    }

    // Update is called once per frame
    void Update()
    {
        if (survivor.debugmode == DEBUGMODE.DISABLED)
        {
            brain.Run(this);
        }
    }

    public BlackBoard GetBlackBoard()
    {
        return blackboard;
    }

    public Survivor GetSurvivor()
    {
        return survivor;
    }

    public void SetBehaviorTree(Task t)
    {
        brain = t;
    }
*/