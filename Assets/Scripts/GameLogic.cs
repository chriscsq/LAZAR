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


    public float WIN_DELAY_SECONDS = 3.0f; // Min delay between last enemy destruction and "winning" the game.
    public GameObject winMessage;
    public WaveInfo[] waves;

    private float timer;
    private float nextWaveTime;
    private int waveIndex;
    [SerializeField]
    SettingScreenBehavior settingScreenBehavior;
    private int waveSize;


    private GameState winState;

    private float winAnnounceTime;


    // Start is called before the first frame update
    void Start()
    {
        // Sort the waves by startTime
        waves = waves.OrderBy(o => o.startTime).ToArray();

        timer = 0.0f;
        waveIndex = 0;
        nextWaveTime = waves[0].startTime;
        winState = GameState.IN_PROGRESS;

        winMessage.SetActive(false);
        handleDifficulty();

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

        TestIfGameWon();

        if(winState == GameState.WON && Time.time >= winAnnounceTime) winMessage.SetActive(true);
    }

    public bool TestIfGameWon() {
        if (winState == GameState.WON) return true;
        if (winState == GameState.LOST) return false;

        bool gameWon = false;
        // If there are no more waves left...
        if (waveIndex >= waves.Length) {
            // ... and the last wave has been completely spawned ...
            if (waves.LastOrDefault().spawnPoint.DoneSpawningEnemies()) {
                // ... and no enemies can be found in the scene ...
                if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) {
                    // ... then the game has been won!
                    gameWon = true;
                    winState = GameState.WON;
                    winAnnounceTime = Time.time + WIN_DELAY_SECONDS;
                }
            }
        }
        return gameWon;
    }

    public void handleDifficulty() {
        string difficulty = settingScreenBehavior.GetCurrentDifficulty();
        Debug.Log(difficulty);
        switch (difficulty) {
            case "Current: Easy":
                waveSize = 3;
                break;
            case "Current: Medium":
                waveSize = 5;
                break;
            case "Current: Hard":
                waveSize = 7;
                break;
        }
    }
}
