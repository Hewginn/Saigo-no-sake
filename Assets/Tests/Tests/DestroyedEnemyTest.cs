using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DestroyedEnemyTest
{
    private GameObject gameManagerGO;
    private GameObject player;
    private GameObject destroyedEnemyGO;
    private DestroyedEnemyTest destroyedEnemy;
    private GameManagerTest gameManager;

    [SetUp]
    public void SetUp()
    {
        // Létrehozza a GameManager, Player és DestroyedEnemy játékobjektumokat
        gameManagerGO = new GameObject();
        gameManager = gameManagerGO.AddComponent<GameManager>();
        
        player = new GameObject();
        player.AddComponent<PlayerControl>(); // Player vezérlés szimulálása

        destroyedEnemyGO = new GameObject();
        destroyedEnemy = destroyedEnemyGO.AddComponent<DestroyedEnemy>();

        // TextMeshProUGUI szöveg elem hozzáadása a kiiktatások számlálásához
        var killsText = destroyedEnemyGO.AddComponent<TextMeshProUGUI>();
        
        // Referenciák beállítása
        destroyedEnemy.GameManagerGO = gameManagerGO;
        destroyedEnemy.Player = player;

        // GameOverGO objektum hozzáadása a GameManager-hez
        gameManager.GameOverGO = new GameObject();
        gameManager.GameOverGO.SetActive(false); // Alapértelmezésben inaktív

        // DestroyedEnemy UI elem beállítása
        destroyedEnemyGO.GetComponent<TextMeshProUGUI>().text = "0";
    }

    [Test]
    public void Kills_SetKillsTo100_TriggersGameOver()
    {
        // Kiiktatások számlálójának beállítása 100-ra
        destroyedEnemy.Kills = 100;

        // Ellenőrizzük, hogy a GameOverGO aktívvá vált-e
        Assert.IsTrue(gameManager.GameOverGO.activeSelf, "GameOverGO nem aktív!");

        // Ellenőrizzük, hogy a játékos objektum inaktív-e
        Assert.IsFalse(destroyedEnemy.Player.activeSelf, "A játékos objektum nem lett inaktív!");
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítás, az összes létrehozott objektum megsemmisítése a tesztek után
        Object.Destroy(gameManagerGO);
        Object.Destroy(player);
        Object.Destroy(destroyedEnemyGO);
    }
}
