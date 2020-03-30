using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WAVE_TYPE
{
    ZOMBUNNY,
    ZOMBEAR
}

[System.Serializable]
public class EnemyWaveData
{
    public EnemySubWaveData[] subWaves;
    public float timeBetweenSubWaves;
}

[System.Serializable]
public class EnemySubWaveData
{
    public Transform spawnPoint;
    public int ZomBunniesPerWave;
    public int ZomBearsPerWave;
    public int HellephantPerWave;
    public int ClownPerWave;
    public int SheepPerWave;
}
