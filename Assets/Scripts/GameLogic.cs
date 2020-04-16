using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class GameLogic : MonoBehaviour
{
    
    private enum GameState{
        IN_PROGRESS, WON, LOST
    } 

    [System.Serializable]
    public struct WaveInfo {
        public float startTime;
        public float spawnPeriod;
        public SpawnController spawnPoint;
        public SubwaveInfo[] subwaves;
    }

    public WaveInfo[] waves;

    private float timer;
    private float nextWaveTime;
    private int waveIndex;

    private GameState winState;

    // Start is called before the first frame update
    void Start()
    {
        // Sort the waves by startTime
        waves = waves.OrderBy(o => o.startTime).ToArray();

        timer = 0.0f;
        waveIndex = 0;
        nextWaveTime = waves[0].startTime;
        winState = GameState.IN_PROGRESS;
    }

    // Update is called once per frame
    void Update()
    {
        // If Pausing is ever implemented, can move most of the below into a block of if(!paused){...}
        timer += Time.deltaTime;
        if (timer > nextWaveTime && waveIndex < waves.Length && winState == GameState.IN_PROGRESS) {
            WaveInfo nextWave = waves[waveIndex];
            SpawnController sc = nextWave.spawnPoint;

            sc.StartWave(nextWave.spawnPeriod, nextWave.subwaves);
            
            waveIndex++;
            if (waveIndex < waves.Length) {
                nextWaveTime = waves[waveIndex].startTime;
            }
        }
    }
}
