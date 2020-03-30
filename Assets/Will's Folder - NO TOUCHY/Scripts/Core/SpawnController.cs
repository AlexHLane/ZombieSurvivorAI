using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{ 
    //public Transform[] spawnAreas;

    public GameObject zomBunnyPrefab;
    public GameObject zombBearPrefab;
    public GameObject hellephantPrefab;
    public GameObject clownPrefab;
    public GameObject sheepPrefab;

    public EnemyWaveData[] Waves;
    private EnemyWaveData currentWave;
    private int waveCounter;

    //public float difficultyHealthMultiplier = 1.0f;

    public float breakTimeLength;
    private float breakTimeCounter;

    private bool waveInProgress;
    private bool inBreakBetweenWave;
    private int subWaveCounter;
    private int maxSubWave;

    bool allWavesComplete;

    private GameManager gameMan;
    // Start is called before the first frame update
    void Start()
    {
        if (Waves.Length > 0)
        {
            currentWave = Waves[0];
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if(inBreakBetweenWave)
        {
            breakTimeCounter -= Time.deltaTime;

            if(breakTimeCounter < 0)
            {
                inBreakBetweenWave = false;
                StartNewWave();
            }
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            CompletedAllWaves();
        }

    }

    public void StartNewWave()
    {
        if (allWavesComplete == false)
        {
            waveInProgress = true;
            waveCounter += 1;

            maxSubWave = currentWave.subWaves.Length;
            subWaveCounter = 0;

            if (subWaveCounter < maxSubWave)
            {
                SpawnWave(currentWave.subWaves[subWaveCounter]);
                subWaveCounter += 1;
                Invoke("SpawnNextSubWave", currentWave.timeBetweenSubWaves);
            }
        }
    }

    void SpawnNextSubWave()
    {
        if (subWaveCounter < maxSubWave)
        {
            subWaveCounter += 1;
            SpawnWave(currentWave.subWaves[subWaveCounter-1]);
            Invoke("SpawnNextSubWave", currentWave.timeBetweenSubWaves);
        }
        else
        {
            WaveComplete();
        }
    }

    void WaveComplete()
    {
        waveInProgress = false;

        if(waveCounter < Waves.Length)
        {
            currentWave = Waves[waveCounter];
            inBreakBetweenWave = true;
            breakTimeCounter = breakTimeLength;
        }
        else
        {
            CompletedAllWaves();
        }
    }

    void CompletedAllWaves()
    {
        allWavesComplete = true;
        GameManager.AllWavesComplete(this);
    }

    public bool AreAllWavesComplete()
    {
        return allWavesComplete;
    }

    void SpawnWave(EnemySubWaveData data)
    {
        for (int i = 0; i < data.ZomBunniesPerWave; i++)
        {
            SpawnZombie(data, zomBunnyPrefab);
        }
        for (int i = 0; i < data.ZomBearsPerWave; i++)
        {
            SpawnZombie(data, zombBearPrefab);
        }
        for (int i = 0; i < data.HellephantPerWave; i++)
        {
            SpawnZombie(data, hellephantPrefab);
        }
        for (int i = 0; i < data.ClownPerWave; i++)
        {
            SpawnZombie(data, clownPrefab);
        }
        for (int i = 0; i < data.SheepPerWave; i++)
        {
            SpawnZombie(data, sheepPrefab);
        }
    }

    void SpawnZombie(EnemySubWaveData data, GameObject enemyPrefab)
    {
        Vector3 pos = data.spawnPoint.position;

        // Spawn Bunnies
        GameObject obj = Instantiate(enemyPrefab, pos, Quaternion.identity);
        Enemy e = obj.GetComponent<Enemy>();
        GameManager.AddEnemy(e);

        // Give them a crystal to attack
        Base b = GameManager.GetBase(CRYSTAL_COLOR.BLUE);
        e.SetBaseTarget(b);
    }

    public bool isBreakInProgress()
    {
        return inBreakBetweenWave;
    }

    public float GetTimeLeftInBreak()
    {
        return breakTimeCounter;
    }

    public bool isWaveInProgress()
    {
        return waveInProgress;
    }
   
    public int GetWaveCount()
    {
        return waveCounter;
    }
}
