using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LOOT_TYPE
{
    NULL,
    GOLD,
    GRENADES,
    ASSAULT,
    SNIPER,
    SHOTGUN
    
}

[System.Serializable]
public struct LOOT_DATA
{
    public GameObject prefab;
    [Range(0, 100)]
    public float lootChance;
    public int amount;
}

public class LootSpawner : MonoBehaviour
{
    public LOOT_DATA[] lootObjects;

    public float LootSpawnCheckFrequency;

    public float ChanceForDrop;

    public GameObject[] lootSpawnLocations;

    // Start is called before the first frame update
    void Start()
    {

        float totalSum = 0;

        for(int i = 0; i < lootObjects.Length; i++)
        {
            totalSum += lootObjects[i].lootChance;
        }

        lootObjects[0].lootChance = (lootObjects[0].lootChance / totalSum) * 100;

        for (int i = 1; i < lootObjects.Length; i++)
        {
            lootObjects[i].lootChance = (lootObjects[i].lootChance / totalSum) * 100;
            lootObjects[i].lootChance += lootObjects[i-1].lootChance;
        }


        Invoke("SpawnLoot", LootSpawnCheckFrequency);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnLoot()
    {
        LOOT_DATA data;
        data.lootChance = 0;
        data.prefab = null;
        data.amount = 0;

        bool drop = false;

        float lootCheckRoll = Random.Range(0.0f, 100.0f);

        if(lootCheckRoll < ChanceForDrop)
        {
            float lootTypeRoll = Random.Range(0.0f, 100.0f);


            for(int i = 0; i < lootObjects.Length; i++)
            {
                if (i == 0)
                {
                    if (lootTypeRoll <= lootObjects[i].lootChance)
                    {
                        drop = true;
                        data = lootObjects[i];
                    }
                }
                else
                {
                    if (lootTypeRoll > lootObjects[i-1].lootChance && lootTypeRoll <= lootObjects[i].lootChance)
                    {
                        drop = true;
                        data = lootObjects[i];
                    }
                }

            }

        }

        // Spawn new loot if dropped
        if(drop == true)
        {
            int randomSpawn = Random.Range(0, lootSpawnLocations.Length - 1);

            Vector3 pos = lootSpawnLocations[randomSpawn].gameObject.transform.position;

            GameObject obj = Instantiate(data.prefab,
                pos,
                Quaternion.identity);

            Loot loot = obj.GetComponent<Loot>();

            loot.SetAmount(data.amount, this);

        }

        Invoke("SpawnLoot", LootSpawnCheckFrequency);
    }
}
