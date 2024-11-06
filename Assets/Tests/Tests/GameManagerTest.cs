using NUnit.Framework; 
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class GameManagerTest : MonoBehaviour
{
    private GameObject gameManagerGO; // A GameManager GameObject
    private GameManagerTest gameManager; // A GameManager komponens
    private GameObject playButton; // A játék indító gomb
    private GameObject menuButton; // A menü gomb
    private GameObject playerPlane; // A játékos repülőgépe
    private GameObject enemySpawner; // Az ellenség generáló
    private GameObject gameOverGO; // A játék vége GameObject
    private GameObject scoreUITextGO; // A pontszám UI GameObject
    private GameObject destroyedUITextGO; // A megsemmisített ellenség UI GameObject
    private GameObject timerCounterGO; // Az időzítő számláló GameObject
    private GameObject gameTitleGO; // A játék címe GameObject
    private GameObject pauseButton; // A szünet gomb
    private GameObject powerUpSpawner; // A power-up generáló


    //

    

    

    //Vereség pop-up
    public GameObject GameOverGO;

    //Időt számláló UI
    public GameObject TimerCounterGO;

    //Cím kiirás
    public GameObject GameTitleGO;

    //Megállító gomb
    public GameObject PauseButton;

    //Játék állapotok tipus
    public enum GameManagerState{

        //Kezdő menü
        Opening,

        //Játék
        Gameplay,

        //Játék vége
        GameOver,
    }

    //Játék állapota
    GameManagerState GMState;
    //Első frame update előtt van meghívva
    void Start()
    {
        GMState = GameManagerState.Opening;
    }



    [SetUp]
    public void Setup()
    {
        // A teszthez szükséges GameObject-ek inicializálása
        gameManagerGO = new GameObject();
        gameManager = gameManagerGO.AddComponent<GameManagerTest>();

        playButton = new GameObject("PlayButton");
        menuButton = new GameObject("MenuButton");
        playerPlane = new GameObject("PlayerPlane");
        enemySpawner = new GameObject("EnemySpawner");
        gameOverGO = new GameObject("GameOverGO");
        scoreUITextGO = new GameObject("ScoreUITextGO");
        destroyedUITextGO = new GameObject("DestroyedUITextGO");
        timerCounterGO = new GameObject("TimerCounterGO");
        gameTitleGO = new GameObject("GameTitleGO");
        pauseButton = new GameObject("PauseButton");
        powerUpSpawner = new GameObject("PowerUpSpawner");

        // Szükséges komponensek beállítása a GameObject-ekhez
        scoreUITextGO.AddComponent<GameScoreTest>();
        destroyedUITextGO.AddComponent<DestroyedEnemyTest>();
        playerPlane.AddComponent<PlayerControlTest>();
        enemySpawner.AddComponent<EnemySpawnerTest>();
        powerUpSpawner.AddComponent<PowerUpSpawnerTest>();
        timerCounterGO.AddComponent<TimeCounterTest>();

        // GameObject-ek hozzárendelése a GameManagerhez
        gameManager.playButton = playButton;
        gameManager.menuButton = menuButton;
        gameManager.playerPlane = playerPlane;
        gameManager.enemySpawner = enemySpawner;
        gameManager.GameOverGO = gameOverGO;
        gameManager.scoreUITextGO = scoreUITextGO;
        gameManager.destroyedUITextGO = destroyedUITextGO;
        gameManager.TimerCounterGO = timerCounterGO;
        gameManager.GameTitleGO = gameTitleGO;
        gameManager.PauseButton = pauseButton;
        gameManager.powerUpSpawner = powerUpSpawner;
    }

     //A játék állapotának megváltoztatása
    void UpdateGameManagerState(){

        switch(GMState)
        {
            //Kezdeti állapot beállítása
            case GameManagerState.Opening:

                playButton.SetActive(true);

                menuButton.SetActive(true);

                GameTitleGO.SetActive(true);

                GameOverGO.SetActive(false);

                PauseButton.SetActive(false);

                break;

            //Játékmenet beállítása
            case GameManagerState.Gameplay:

                scoreUITextGO.GetComponent<GameScoreTest>().Score = 0;
                destroyedUITextGO.GetComponent<DestroyedEnemyTest>().Kills = 0;

                playButton.SetActive(false);

                menuButton.SetActive(false);

                GameTitleGO.SetActive(false);
                PauseButton.SetActive(true);

                playerPlane.GetComponent<PlayerControlTest>().Init();

                enemySpawner.GetComponent<EnemySpawnerTest>().ScheduleEnemySpawner();

                powerUpSpawner.GetComponent<PowerUpSpawnerTest>().SchedulePowerUpSpawner();

                TimerCounterGO.GetComponent<TimeCounterTest>().StartTimeCounter();
    
                break;
            //Játékvége beállítása
            case GameManagerState.GameOver:

                TimerCounterGO.GetComponent<TimeCounterTest>().StopTimeCounter();

                enemySpawner.GetComponent<EnemySpawnerTest>().UnScheduleEnemySpawner();

                powerUpSpawner.GetComponent<PowerUpSpawnerTest>().UnSchedulePowerUpSpawner();

                GameOverGO.SetActive(true);

                PauseButton.SetActive(false);

                Invoke("ChangeToOpeningState",8f);
                
                break;
        }
    }

    //Játék állapot beállítása adott állapotra
    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    //Játék állapotának beállítása játékmenetre
    public void StartGamePlay(){
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
    }

    [Test]
    public void GameManager_InitialStateIsOpening()
    {
        gameManager.Start();
        Assert.AreEqual(GameManagerTest.GameManagerState.Opening, gameManager.GMState);
    }

    [UnityTest]
    public IEnumerator GameManager_StartGamePlay_SetsStateToGameplay()
    {
        gameManager.StartGamePlay();
        yield return null;
        Assert.AreEqual(GameManagerTest.GameManagerState.Gameplay, gameManager.GMState);
    }

    [UnityTest]
    public IEnumerator GameManager_GameplayState_InitializesCorrectly()
    {
        gameManager.SetGameManagerState(GameManagerTest.GameManagerState.Gameplay);
        yield return null;

        Assert.IsFalse(playButton.activeSelf);
        Assert.IsFalse(menuButton.activeSelf);
        Assert.IsFalse(gameTitleGO.activeSelf);
        Assert.IsTrue(pauseButton.activeSelf);
        Assert.AreEqual(0, scoreUITextGO.GetComponent<GameScoreTest>().Score);
        Assert.AreEqual(0, destroyedUITextGO.GetComponent<DestroyedEnemyTest>().Kills);
    }

    [UnityTest]
    public IEnumerator GameManager_GameOverState_ShowsGameOverAndStopsSpawners()
    {
        gameManager.SetGameManagerState(GameManagerTest.GameManagerState.GameOver);
        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(gameOverGO.activeSelf);
        Assert.IsFalse(pauseButton.activeSelf);
        Assert.IsFalse(enemySpawner.GetComponent<EnemySpawnerTest>().IsInvoking("SpawnEnemy"));
        Assert.IsFalse(powerUpSpawner.GetComponent<PowerUpSpawnerTest>().IsInvoking("SpawnPowerUp"));
    }

    [UnityTest]
    public IEnumerator GameManager_OpeningState_ShowsMenuButtons()
    {
        gameManager.SetGameManagerState(GameManagerTest.GameManagerState.Opening);
        yield return null;

        Assert.IsTrue(playButton.activeSelf);
        Assert.IsTrue(menuButton.activeSelf);
        Assert.IsTrue(gameTitleGO.activeSelf);
        Assert.IsFalse(gameOverGO.activeSelf);
        Assert.IsFalse(pauseButton.activeSelf);
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítsuk meg a létrehozott GameObject-eket
        Object.Destroy(gameManagerGO);
        Object.Destroy(playButton);
        Object.Destroy(menuButton);
        Object.Destroy(playerPlane);
        Object.Destroy(enemySpawner);
        Object.Destroy(gameOverGO);
        Object.Destroy(scoreUITextGO);
        Object.Destroy(destroyedUITextGO);
        Object.Destroy(timerCounterGO);
        Object.Destroy(gameTitleGO);
        Object.Destroy(pauseButton);
        Object.Destroy(powerUpSpawner);
    }
}
