using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellSpawner : MonoBehaviour
{

    public GameObject zomBunnyPrefab;
    public GameObject zombBearPrefab;
    public GameObject hellephantPrefab;
    public GameObject clownPrefab;
    public GameObject sheepPrefab;

    EnemySpawnPoint[] spawnPoints;
    

    float difficultyHealthMultiplier = 1.0f;
    float difficultySpawnModifier_Bunnies;
    float difficultySpawnModifier_Specials;

    public float breakTimeFrequency;
    public float breakTimeLength;

    private float waveTimeCounter;
    private float breakTimeCounter;
    private bool inBreakTime;

    public float spawnBunnyFrequency;
    public float spawnSpecialFrequency;

    public float chanceOfBear;
    public float chanceOfHellephant;
    public float chanceOfClown;
    

    private GameManager gameMan;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GameObject.FindObjectsOfType<EnemySpawnPoint>();

        float totalSum = 0;

        totalSum += chanceOfBear + chanceOfClown + chanceOfHellephant;

        chanceOfBear = (chanceOfBear / totalSum) * 100;
        chanceOfHellephant = (chanceOfClown / totalSum) * 100;
        chanceOfClown = (chanceOfClown / totalSum) * 100;

        chanceOfHellephant += chanceOfBear;
        chanceOfClown += chanceOfHellephant;

        difficultySpawnModifier_Bunnies = 1.0f;
        difficultySpawnModifier_Specials = 1.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (inBreakTime)
        {
            breakTimeCounter += Time.deltaTime;

            if(breakTimeCounter >= breakTimeLength)
            {
                inBreakTime = false;
                breakTimeCounter = 0;
            }

        }
        else
        {
            waveTimeCounter += Time.deltaTime;

            if (waveTimeCounter >= breakTimeFrequency)
            {
                inBreakTime = true;
                difficultyHealthMultiplier += 0.5f;
                difficultySpawnModifier_Bunnies += 1;
                difficultySpawnModifier_Specials += 0.5f;

                waveTimeCounter = 0;
            }
        }


    }

    public bool isBreakInProgress()
    {
        return inBreakTime;
    }

    public float GetTimeLeftInBreak()
    {
        return breakTimeLength - breakTimeCounter;
    }

    public void UnleashHell()
    {
        inBreakTime = true;

        Invoke("SpawnBunnies", breakTimeLength);
        Invoke("SpawnSpecials", breakTimeLength);
    }

    public void SpawnBunnies()
    {
        for (int i = 0; i < (int)difficultySpawnModifier_Bunnies; i++)
        {
            Vector3 pos = RandomSpawnPoint();

            // Spawn Bunnies
            GameObject obj = Instantiate(zomBunnyPrefab, pos, Quaternion.identity);
            Enemy e = obj.GetComponent<Enemy>();
            e.AddHealthModifier(difficultyHealthMultiplier);
            GameManager.AddEnemy(e);

            // Give them a crystal to attack
            Base b = GameManager.GetBase(CRYSTAL_COLOR.BLUE);
            e.SetBaseTarget(b);

        }

        Invoke("SpawnBunnies", spawnBunnyFrequency);

    }

    public void SpawnSpecials()
    {
        for (int i = 0; i < (int)difficultySpawnModifier_Specials; i++)
        {
            Vector3 pos = RandomSpawnPoint();

            GameObject randomEnemy = RandomSpecial();

            // Spawn Bunnies
            GameObject obj = Instantiate(randomEnemy, pos, Quaternion.identity);
            Enemy e = obj.GetComponent<Enemy>();
            e.AddHealthModifier(difficultyHealthMultiplier);
            GameManager.AddEnemy(e);

            // Give them a crystal to attack
            Base b = GameManager.GetBase(CRYSTAL_COLOR.BLUE);
            e.SetBaseTarget(b);
        }

        Invoke("SpawnSpecials", spawnSpecialFrequency);
    }

    public GameObject RandomSpecial()
    {
        GameObject output = null;

        float randomNum = Random.Range(0, 100);

        if(randomNum < chanceOfBear)
        {
            output = zombBearPrefab;
        }
        else if(randomNum > chanceOfBear && randomNum < chanceOfHellephant)
        {
            output = hellephantPrefab;
        }
        else if(randomNum > chanceOfHellephant)
        {
            output = clownPrefab;
        }

        return output;
    }


    public Vector3 RandomSpawnPoint()
    {
        int randomPos = Random.Range(0, spawnPoints.Length);

        EnemySpawnPoint eSpawnPoint = spawnPoints[randomPos];

        return eSpawnPoint.transform.position;
    }

}
