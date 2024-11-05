using NUnit.Framework; 
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class GameManagerTest
{
    private GameObject gameManagerGO; // A GameManager GameObject
    private GameManager gameManager; // A GameManager komponens
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

    [SetUp]
    public void Setup()
    {
        // A teszthez szükséges GameObject-ek inicializálása
        gameManagerGO = new GameObject();
        gameManager = gameManagerGO.AddComponent<GameManager>();

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
        scoreUITextGO.AddComponent<GameScore>();
        destroyedUITextGO.AddComponent<DestroyedEnemy>();
        playerPlane.AddComponent<PlayerControl>();
        enemySpawner.AddComponent<EnemySpawner>();
        powerUpSpawner.AddComponent<PowerUpSpawner>();
        timerCounterGO.AddComponent<TimeCounter>();

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

    [Test]
    public void GameManager_InitialStateIsOpening()
    {
        gameManager.Start();
        Assert.AreEqual(GameManager.GameManagerState.Opening, gameManager.GMState);
    }

    [UnityTest]
    public IEnumerator GameManager_StartGamePlay_SetsStateToGameplay()
    {
        gameManager.StartGamePlay();
        yield return null;
        Assert.AreEqual(GameManager.GameManagerState.Gameplay, gameManager.GMState);
    }

    [UnityTest]
    public IEnumerator GameManager_GameplayState_InitializesCorrectly()
    {
        gameManager.SetGameManagerState(GameManager.GameManagerState.Gameplay);
        yield return null;

        Assert.IsFalse(playButton.activeSelf);
        Assert.IsFalse(menuButton.activeSelf);
        Assert.IsFalse(gameTitleGO.activeSelf);
        Assert.IsTrue(pauseButton.activeSelf);
        Assert.AreEqual(0, scoreUITextGO.GetComponent<GameScore>().Score);
        Assert.AreEqual(0, destroyedUITextGO.GetComponent<DestroyedEnemy>().Kills);
    }

    [UnityTest]
    public IEnumerator GameManager_GameOverState_ShowsGameOverAndStopsSpawners()
    {
        gameManager.SetGameManagerState(GameManager.GameManagerState.GameOver);
        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(gameOverGO.activeSelf);
        Assert.IsFalse(pauseButton.activeSelf);
        Assert.IsFalse(enemySpawner.GetComponent<EnemySpawner>().IsInvoking("SpawnEnemy"));
        Assert.IsFalse(powerUpSpawner.GetComponent<PowerUpSpawner>().IsInvoking("SpawnPowerUp"));
    }

    [UnityTest]
    public IEnumerator GameManager_OpeningState_ShowsMenuButtons()
    {
        gameManager.SetGameManagerState(GameManager.GameManagerState.Opening);
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
