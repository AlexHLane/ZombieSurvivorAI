using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TOWER_POSITION
{
	NULL,
	NORTHEAST,
	NORTHWEST,
	SOUTHEAST,
	SOUTHWEST
}



[System.Serializable]
public struct TowerData
{
	public TOWER_POSITION pos;
	public BulletTower bulletTower;
}

public class GameManager : MonoBehaviour {

	private static GameManager instance;

	private List<Enemy> activeEnemies;
	private List<Enemy> activeZomBunnies;
	private List<Enemy> activeZomBears;
	private List<Enemy> activeHellephants;
	private List<Enemy> activeClowns;
	private List<Enemy> activeSheep;

	private List<Loot> activeLoot;

	[SerializeField]
	private Base[] bases;

	private List<Survivor> survivors;
    
	public int startingAssaultAmmo;
	public int startingShotgunAmmo;
	public int startingSniperAmmo;
	public int startingGrenades;

	[SerializeField]
	private int totalEnemies;

	[SerializeField]
	private int gold;

	[SerializeField]
	private int gateRepairCost;

	[SerializeField]
	private int assualtAmmoCost;
	[SerializeField]
	private int assualtAmmoPerPurchase;
	[SerializeField]
	private int shotgunAmmoCost;
	[SerializeField]
	private int shotgunAmmoPerPurchase;
	[SerializeField]
	private int sniperAmmoCost;
	[SerializeField]
	private int sniperAmmoPerPurchase;
	[SerializeField]
	private int survivorCost;

	[SerializeField]
	private int bulletTowerCost;

	public Text goldText;
    public Text waveText;
	public Text breakTimerText;

	public Text assaultAmmoText;
	public Text shotgunAmmoText;
	public Text sniperAmmoText;
	public Text grenadeAmmoText;

	private SpawnController spawnController;

	// Ammo Storage
	int assaultAmmo;
	int shotgunAmmo;
	int sniperAmmo;
	int grenadeAmmo;

	public TowerData[] towerDatas;

	public GameObject survivorPrefab;
	public Transform survivorSpawnLocation;

	bool inHellMode;
	HellSpawner hellSpawnerControl;

	void Awake()
	{
		if (instance == null) {
			instance = this;
			instance.Setup ();
		} else {
			DestroyImmediate (gameObject);
		}
	}

	void Setup()
	{
		instance.activeEnemies = new List<Enemy> ();
		instance.activeZomBunnies = new List<Enemy> ();
		instance.activeZomBears = new List<Enemy> ();
		instance.activeHellephants = new List<Enemy> ();
		instance.activeClowns = new List<Enemy>();
		instance.activeSheep = new List<Enemy>();
		instance.activeLoot = new List<Loot>();

		spawnController = GetComponentInChildren<SpawnController>();
		hellSpawnerControl = GetComponentInChildren<HellSpawner>();

		Survivor[] survivors = GameObject.FindObjectsOfType<Survivor> ();
		foreach (Survivor s in survivors)
		{
			AddSurvivor (s);
		}

		assaultAmmo = startingAssaultAmmo;
		shotgunAmmo = startingShotgunAmmo;
		sniperAmmo = startingSniperAmmo;
		grenadeAmmo = startingGrenades;
	}

	// Use this for initialization
	void Start () {
		
		activeEnemies = new List<Enemy> ();
		activeZomBunnies = new List<Enemy> ();
        activeZomBears = new List<Enemy>();
		activeHellephants = new List<Enemy> ();
		activeClowns = new List<Enemy>();
		activeSheep = new List<Enemy>();
		activeLoot = new List<Loot>();
		if (survivors == null) {
			survivors = new List<Survivor> ();
		}

		Invoke("StartWaves", 5.0f);
    }

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.K))
		{
			Debug.Log(GameManager.isBreakInProgress());
		}


	}

	void LateUpdate()
	{
		totalEnemies = activeEnemies.Count;

	}


    public static float GetBreakTimeRemaining()
    {
		return instance.spawnController.GetTimeLeftInBreak();
    }

	public static bool InBreakTime()
	{
		bool output = false;

		if (instance.inHellMode == false)
		{
			if (instance.spawnController.GetTimeLeftInBreak() > 0)
			{
				output = true;
			}
		}
		else
		{
			if (instance.hellSpawnerControl.GetTimeLeftInBreak() > 0)
			{
				output = true;
			}

		}

		return output;
	}

	public static void AllWavesComplete(SpawnController s)
	{
		if (s.AreAllWavesComplete() && instance.inHellMode == false)
		{
			instance.inHellMode = true;
			instance.spawnController.enabled = false;
			instance.hellSpawnerControl.UnleashHell();
		}
	}

	public void StartWaves()
	{
		spawnController.StartNewWave();
	}
	
	public static bool IsWaveInProgress()
	{
		return instance.spawnController.isWaveInProgress();
	}

	public static int GetCurrentWaveNumber()
	{
		return instance.spawnController.GetWaveCount();
	}

    #region Crystal Base Code
	public static Base GetBase(CRYSTAL_COLOR color)
	{
		Base output = null;

		switch (color)
		{
			case CRYSTAL_COLOR.BLUE:
				output = instance.bases[0];
				break;
			case CRYSTAL_COLOR.GREEN:
				output = instance.bases[1];
				break;
			case CRYSTAL_COLOR.RED:
				output = instance.bases[2];
				break;
		}

		return output;
	}

	public static Gate GetGate(GATE_POSITION gPos)
	{
		Gate output = null;

		foreach(Gate g in instance.bases[0].gates)
		{
			if(g.position == gPos)
			{
				output = g;
			}

		}

		return output;
	}

    #endregion

    #region SurvivorCode
    public static void AddSurvivor(Survivor s)
	{
		if (instance.survivors == null) {
			instance.survivors = new List<Survivor> ();
		}

		instance.survivors.Add (s);
	}

	public static void RemoveSurvivor(Survivor s)
	{
		instance.survivors.Remove (s);
		Destroy(s.gameObject);
	}

	public static List<Survivor> getSurvivorList()
	{
		return instance.survivors;
	}
	#endregion

	#region ZombieCode
	// Zombie Code

	public static void AddEnemy(Enemy e)
	{
		EnemyType t = e.getEnemyType ();

		switch (t)
		{
		case EnemyType.ZOMBUNNY:
			instance.activeZomBunnies.Add (e);
			break;
		case EnemyType.ZOMBEAR:
			instance.activeZomBears.Add (e);
			break;
		case EnemyType.HELLEPHANT:
			instance.activeHellephants.Add (e);
			break;
		case EnemyType.CLOWN:
			instance.activeClowns.Add(e);
			break;
		case EnemyType.SHEEP:
			instance.activeSheep.Add(e);
			break;

		}


		instance.activeEnemies.Add (e);
	}

	public static void RemoveEnemy(Enemy e)
	{
		EnemyType type = e.getEnemyType ();

		switch (type) {
			case EnemyType.ZOMBUNNY:
				instance.activeZomBunnies.Remove (e);
				break;
			case EnemyType.ZOMBEAR:
				instance.activeZomBears.Remove (e);
				break;
			case EnemyType.HELLEPHANT:
				instance.activeHellephants.Remove (e);
				break;
			case EnemyType.CLOWN:
				instance.activeClowns.Remove(e);
				break;
			case EnemyType.SHEEP:
				instance.activeSheep.Remove(e);
				break;
			default:
				break;
		}

		instance.gold += e.pointValue;
		//instance.goldText.text = "Gold: " + instance.gold;

		instance.activeEnemies.Remove (e);
        
	}

	public static List<Enemy> getAllEnemies()
	{
		return instance.activeEnemies;
	}

	public static List<Enemy> getZomBunnyList()
	{
		return instance.activeZomBunnies;
	}
	
	public static List<Enemy> getZomBearList()
	{
		return instance.activeZomBears;
	}

	public static List<Enemy> getHellephantList()
	{
		return instance.activeHellephants;
	}

	public static List<Enemy> getClownList()
	{
		return instance.activeClowns;
	}

	public static List<Enemy> getSheepList()
	{
		return instance.activeSheep;
	}

	#endregion

	#region Weapon Code
	public static int GetAmmo(WEAPON_TYPE wType)
	{
		int output = 0;

		switch(wType)
		{
			case WEAPON_TYPE.ASSAULT:
				output = instance.assaultAmmo;
				break;
			case WEAPON_TYPE.SHOTGUN:
				output = instance.shotgunAmmo;
				break;
			case WEAPON_TYPE.SNIPER:
				output = instance.sniperAmmo;
				break;
			case WEAPON_TYPE.GRENADE_LAUNCHER:
				output = instance.grenadeAmmo;
				break;
			default:
				break;
		}
		return output;
	}

	public static void UseAmmo(WEAPON_TYPE wType)
	{
		switch (wType)
		{
			case WEAPON_TYPE.ASSAULT:
				instance.assaultAmmo -= 1;
				break;
			case WEAPON_TYPE.SHOTGUN:
				instance.shotgunAmmo -= 1;
				break;
			case WEAPON_TYPE.SNIPER:
				instance.sniperAmmo -= 1;
				break;
			case WEAPON_TYPE.GRENADE_LAUNCHER:
				instance.grenadeAmmo -= 1;
				break;
			default:
				break;
		}
	}

	private void AddAmmo(WEAPON_TYPE wEAPON_TYPE, int amount)
	{
		switch (wEAPON_TYPE)
		{
			case WEAPON_TYPE.ASSAULT:
				instance.assaultAmmo += amount;
				break;
			case WEAPON_TYPE.SHOTGUN:
				instance.shotgunAmmo += amount;
				break;
			case WEAPON_TYPE.SNIPER:
				instance.sniperAmmo += amount;
				break;
			case WEAPON_TYPE.GRENADE_LAUNCHER:
				instance.grenadeAmmo += amount;
				break;
			default:
				break;
		}
	}
    #endregion

    #region GoldCode
	public static int GetCurrentGold()
	{
		return instance.gold;
	}

	private bool CanAfford(int value)
	{
		bool output = false;
		if(value <= gold)
		{
			output = true;
		}
		return output;
	}

	public static void RepairGate(GATE_POSITION pos)
	{
		if (GameManager.isBreakInProgress())
		{
			if (instance.CanAfford(instance.gateRepairCost))
			{
				Base b = instance.bases[0];

				Gate targetGate = null;

				foreach (Gate g in b.gates)
				{
					if (g.position == pos)
					{
						targetGate = g;
					}
				}

				if (targetGate != null)
				{
					targetGate.Repair();
					instance.gold = (int)Mathf.Clamp(instance.gold - instance.gateRepairCost, 0, Mathf.Infinity);
				}
			}
		}
	
	}


	private void PurchaseAmmo(ref int ammo, int cost, int ammoPerPurchase)
	{
		if(CanAfford(cost))
		{
			gold = (int)Mathf.Clamp(gold - cost, 0, Mathf.Infinity);

			ammo += ammoPerPurchase;
		}
	}

	public static void PurchaseAmmo(WEAPON_TYPE wType)
	{
		if (GameManager.isBreakInProgress())
		{
			switch (wType)
			{
				case WEAPON_TYPE.ASSAULT:
					instance.PurchaseAmmo(ref instance.assaultAmmo, instance.assualtAmmoCost, instance.assualtAmmoPerPurchase);
					break;
				case WEAPON_TYPE.SHOTGUN:
					instance.PurchaseAmmo(ref instance.shotgunAmmo, instance.shotgunAmmoCost, instance.shotgunAmmoPerPurchase);
					break;
				case WEAPON_TYPE.SNIPER:
					instance.PurchaseAmmo(ref instance.sniperAmmo, instance.sniperAmmoCost, instance.sniperAmmoPerPurchase);
					break;
				default:
					break;
			}
		}
	}

	public static void PurchaseTower(TOWER_TYPE type, TOWER_POSITION pos)
	{
		if (GameManager.isBreakInProgress())
		{
			switch (pos)
			{
				case TOWER_POSITION.NORTHEAST:
					instance.BuyTower(instance.bulletTowerCost, instance.FindTowerByPosition(pos), type);
					break;
				case TOWER_POSITION.NORTHWEST:
					instance.BuyTower(instance.bulletTowerCost, instance.FindTowerByPosition(pos), type);
					break;
				case TOWER_POSITION.SOUTHEAST:
					instance.BuyTower(instance.bulletTowerCost, instance.FindTowerByPosition(pos), type);
					break;
				case TOWER_POSITION.SOUTHWEST:
					instance.BuyTower(instance.bulletTowerCost, instance.FindTowerByPosition(pos), type);
					break;
				default:
					break;
			}

		}
	}

	private TowerData FindTowerByPosition(TOWER_POSITION pos)
	{
		TowerData output;
		output.bulletTower = null;
		output.pos = TOWER_POSITION.NULL;

		foreach(TowerData data in towerDatas)
		{
			if(data.pos == pos)
			{
				output = data;
				break;
			}
		}


		return output;
	}

	private void BuyTower(int cost, TowerData data, TOWER_TYPE type)
	{
		if (GameManager.isBreakInProgress())
		{
			if (CanAfford(cost))
			{
				gold = (int)Mathf.Clamp(gold - cost, 0, Mathf.Infinity);

				DeActivateTower(data.pos);
				ActivateTower(data.pos, type);
			}
		}
	}

	public static void PurchaseSurvivor()
	{
		if (GameManager.isBreakInProgress())
		{
			if (instance.CanAfford(instance.survivorCost))
			{
				instance.gold = (int)Mathf.Clamp(instance.gold - instance.survivorCost, 0, Mathf.Infinity);
				instance.SpawnSuvivor();
			}
		}
	}

	void SpawnSuvivor()
	{
		GameObject obj = Instantiate(survivorPrefab, survivorSpawnLocation.position, Quaternion.identity);
		Survivor s = obj.GetComponent<Survivor>();
		AddSurvivor(s);
	}

    #endregion

    #region Tower Functions
	private void ActivateTower(TOWER_POSITION pos, TOWER_TYPE type)
	{
		TowerData targetTower;
		targetTower.pos = TOWER_POSITION.NULL;
		targetTower.bulletTower = null;

		foreach (TowerData data in towerDatas)
		{
			if (pos == data.pos)
			{
				targetTower = data;
			}
		}

		if (targetTower.pos != TOWER_POSITION.NULL)
		{
			if (type == TOWER_TYPE.BULLET_TOWER)
			{
				targetTower.bulletTower.gameObject.SetActive(true);
			}
		}
	}

	private	void DeActivateTower(TOWER_POSITION pos)
	{
		TowerData targetTower;
		targetTower.pos = TOWER_POSITION.NULL;
		targetTower.bulletTower = null;

		foreach(TowerData data in towerDatas)
		{
			if(pos == data.pos)
			{
				targetTower = data;
			}
		}

		if(targetTower.pos != TOWER_POSITION.NULL)
		{
			targetTower.bulletTower.gameObject.SetActive(false);
		}
	}

	private TowerData GetActiveTower(TOWER_POSITION pos)
	{
		TowerData targetTower;
		targetTower.pos = TOWER_POSITION.NULL;
		targetTower.bulletTower = null;

		foreach (TowerData data in towerDatas)
		{
			if (pos == data.pos)
			{
				targetTower = data;
			}
		}

		return targetTower;
	}


	#endregion

	#region GOLD COST GETTERS

	public static int GetGateRepairCost()
	{
		return instance.gateRepairCost;
	}

	public static int GetAssaultAmmoCost()
	{
		return instance.assualtAmmoCost;
	}

	public static int GetAssaultAmmoPerPurchase()
	{
		return instance.assualtAmmoPerPurchase;
	}

	public static int GetShotgunAmmoCost()
	{
		return instance.shotgunAmmoCost;
	}
	public static int GetShotgunAmmoPerPurchase()
	{
		return instance.shotgunAmmoPerPurchase;
	}
	public static int GetSniperAmmoCost()
	{
		return instance.sniperAmmoCost;
	}
	public static int GetSniperAmmoPerPurchase()
	{
		return instance.sniperAmmoPerPurchase;
	}
	public static int GetBulletTowerCost()
	{
		return instance.bulletTowerCost;
	}

	public static int GetSurvivorCost()
	{
		return instance.survivorCost;
	}

	#endregion

	#region Loot Functions
	public static void PickupLoot(Loot loot)
	{
		switch (loot.GetLootType())
		{
			case LOOT_TYPE.GOLD:
				instance.gold += loot.GetAmount();
				break;
			case LOOT_TYPE.ASSAULT:
				instance.AddAmmo(WEAPON_TYPE.ASSAULT, loot.GetAmount());
				break;
			case LOOT_TYPE.SHOTGUN:
				instance.AddAmmo(WEAPON_TYPE.SHOTGUN, loot.GetAmount());
				break;
			case LOOT_TYPE.SNIPER:
				instance.AddAmmo(WEAPON_TYPE.SNIPER, loot.GetAmount());
				break;
			case LOOT_TYPE.GRENADES:
				instance.AddAmmo(WEAPON_TYPE.GRENADE_LAUNCHER, loot.GetAmount());
				break;
		}

		bool removed = instance.activeLoot.Remove(loot);

		if(removed == false)
		{
			Debug.Log("LOOT NOT FOUND IN TABLE");
		}
	}

	public static void AddLootToList(Loot loot)
	{
		instance.activeLoot.Add(loot);
	}

	public static List<Loot> GetLootList()
	{
		return instance.activeLoot;
	}

    #endregion

    public static bool isBreakInProgress()
	{
		bool output = false;

		if(instance.spawnController.isBreakInProgress() || instance.hellSpawnerControl.isBreakInProgress())
		{
			output = true;
		}
		return output;
	}


    private void OnGUI()
	{
		if (goldText != null)
		{
			goldText.text = "Gold: " + instance.gold;
		}
		if (inHellMode == false)
		{
			if (waveText != null)
			{
				waveText.text = "Wave: " + spawnController.GetWaveCount();
			}
			if (breakTimerText != null)
			{

				if (spawnController.isBreakInProgress())
				{
					breakTimerText.text = "Break: " + spawnController.GetTimeLeftInBreak();
				}
				else
				{
					breakTimerText.text = "";
				}
			}
		}
		else
		{
			if (waveText != null)
			{
				waveText.text = "HELL!!!";
			}
			if (breakTimerText != null)
			{

				if (hellSpawnerControl.GetTimeLeftInBreak() > 0)
				{
					breakTimerText.text = "Break: " + hellSpawnerControl.GetTimeLeftInBreak();
				}
				else
				{
					breakTimerText.text = "";
				}
			}
		}


		AmmoUI_Update();

	}

	void AmmoUI_Update()
	{
		if (assaultAmmoText != null)
		{
			assaultAmmoText.text = assaultAmmo.ToString();
		}
		if (shotgunAmmoText != null)
		{
			shotgunAmmoText.text = shotgunAmmo.ToString();
		}
		if (sniperAmmoText != null)
		{
			sniperAmmoText.text = sniperAmmo.ToString();
		}
		if (grenadeAmmoText != null)
		{
			grenadeAmmoText.text = grenadeAmmo.ToString();
		}
	}

}
