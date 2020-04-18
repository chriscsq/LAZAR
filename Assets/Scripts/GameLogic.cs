using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class GameLogic : MonoBehaviour
{
    
    private enum GameState{
        UNSTARTED, PAUSED, IN_PROGRESS, WON, LOST
    } 



    [System.Serializable]
    public struct WaveInfo {
        public float startTime;
        public float spawnPeriod;
        public SpawnController spawnPoint;
        public SubwaveInfo[] subwaves;
    }

    [System.Serializable]
    public struct MedDif {
        public float startTime;
        public float spawnPeriod;
        public SpawnController spawnPoint;
        public SubwaveInfo[] subwaves;
    }

    [System.Serializable]
    public struct HardDif {
        public float startTime;
        public float spawnPeriod;
        public SpawnController spawnPoint;
        public SubwaveInfo[] subwaves;
    }


    public float WIN_DELAY_SECONDS = 3.0f; // Min delay between last enemy destruction and "winning" the game.
    public GameObject winMessage;
    public GameObject loseMessage;
    public WaveInfo[] waves;
    public MedDif[] medDifWaves;
    public HardDif[] hardDifWaves;

    private float timer;
    private float nextWaveTime;
    private int waveIndex;
    private float medNextWaveTime;
    private int medWaveIndex;
    private float hardNextWaveTime;
    private int hardWaveIndex;
    bool triggerE, triggerM, triggerH = false;
    private GameDifficulty difficulty;
    public GameDifficulty debugDifficulty;


    [HideInInspector]
    public GameObject settingsScreen;
    //private int waveSize;


    private GameState winState;
    //private GameState lastUnpausedState;

    private float winAnnounceTime;


    // Start is called before the first frame update
    void Start()
    {
        // Sort the waves by startTime
        waves = waves.OrderBy(o => o.startTime).ToArray();
        medDifWaves = medDifWaves.OrderBy(o => o.startTime).ToArray();
        hardDifWaves = hardDifWaves.OrderBy(o => o.startTime).ToArray();

        timer = 0.0f;
        waveIndex = medWaveIndex = hardWaveIndex = 0;
        nextWaveTime = waves[0].startTime;
        medNextWaveTime = medDifWaves[0].startTime;
        hardNextWaveTime = hardDifWaves[0].startTime;
        
        difficulty = GameSettings.Difficulty;

        if (GameSettings.Difficulty == GameDifficulty.UNSET && Application.isEditor)
        {
            difficulty = debugDifficulty;
        }


        switch (difficulty)
        {
            case GameDifficulty.EASY:
                triggerE = true;
                Debug.Log("Using difficulty EASY");
                break;
            case GameDifficulty.MEDIUM:
                triggerM = true;
                Debug.Log("Using difficulty MEDIUM");
                break;
            case GameDifficulty.HARD:
                triggerH = true;
                Debug.Log("Using difficulty HARD");
                break;
            default:
                triggerE = true;
                Debug.Log("Using difficulty DEFAULT/EASY");
                break;
        }

        winState = GameState.UNSTARTED;
        //lastUnpausedState = GameState.IN_PROGRESS;


        winMessage.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(winState == GameState.IN_PROGRESS) {
            if (triggerE)
                startEasyWave();
            else if (triggerM)
                startMedWave();
            else if (triggerH)
                startHardWave();
            
            TestIfGameWon();
            
            //lastUnpausedState = winState;
            
            if(winState == GameState.WON && Time.time >= winAnnounceTime) winMessage.SetActive(true);
        }
    }

    

    public void TestIfGameWon() {


        if(GameObject.FindGameObjectsWithTag("EnemyGoal").Length <= 0) {
            winState = GameState.LOST;
            loseMessage.SetActive(true);
        }

        // If there are no more waves left...
        if (waveIndex >= waves.Length || medWaveIndex >= medDifWaves.Length || hardWaveIndex >= hardDifWaves.Length) {
            // ... and the last wave has been completely spawned ...
            if (waves.LastOrDefault().spawnPoint.DoneSpawningEnemies() || medDifWaves.LastOrDefault().spawnPoint.DoneSpawningEnemies() ||
                hardDifWaves.LastOrDefault().spawnPoint.DoneSpawningEnemies()) {
                // ... and no enemies can be found in the scene ...
                if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) {
                    // ... then the game has been won!
                    winState = GameState.WON;
                    winAnnounceTime = Time.time + WIN_DELAY_SECONDS;
                }
            }
        }
    }

    public void startEasyWave() {
        // If Pausing is ever implemented, can move most of the below into a block of if(!paused){...}
        timer += Time.deltaTime;
        if (timer > nextWaveTime && waveIndex < waves.Length && winState == GameState.IN_PROGRESS)
        {
            WaveInfo nextWave = waves[waveIndex];
            SpawnController sc = nextWave.spawnPoint;

            sc.StartWave(nextWave.spawnPeriod, nextWave.subwaves);

            waveIndex++;
            if (waveIndex < waves.Length)
            {
                nextWaveTime = waves[waveIndex].startTime;
            }
        }
    }

    private void startMedWave() {
        // If Pausing is ever implemented, can move most of the below into a block of if(!paused){...}
        timer += Time.deltaTime;
        if (timer > medNextWaveTime && medWaveIndex < medDifWaves.Length && winState == GameState.IN_PROGRESS)
        {
            MedDif nextWave = medDifWaves[medWaveIndex];
            SpawnController sc = nextWave.spawnPoint;

            sc.StartWave(nextWave.spawnPeriod, nextWave.subwaves);

            medWaveIndex++;
            if (medWaveIndex < medDifWaves.Length)
            {
                medNextWaveTime = medDifWaves[medWaveIndex].startTime;
            }
        }
    }
    private void startHardWave() {
        // If Pausing is ever implemented, can move most of the below into a block of if(!paused){...}
        timer += Time.deltaTime;
        if (timer > hardNextWaveTime && hardWaveIndex < hardDifWaves.Length && winState == GameState.IN_PROGRESS)
        {
            HardDif nextWave = hardDifWaves[hardWaveIndex];
            SpawnController sc = nextWave.spawnPoint;

            sc.StartWave(nextWave.spawnPeriod, nextWave.subwaves);

            hardWaveIndex++;
            if (hardWaveIndex < hardDifWaves.Length)
            {
                hardNextWaveTime = hardDifWaves[hardWaveIndex].startTime;
            }
        }
    }

    public void Pause() {
        /*lastUnpausedState = winState;
        winState = GameState.PAUSED;*/
        Debug.Log("Implement pausing some time in the future.");
    }

    public void Unpause() {
        //winState = lastUnpausedState;
        Debug.Log("Implement unpausing at some time in the future.");
    }

    public void StartGame() {
        winState = GameState.IN_PROGRESS;
    }
}
