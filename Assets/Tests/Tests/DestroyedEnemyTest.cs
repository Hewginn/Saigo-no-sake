using NUnit.Framework;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class DestroyedEnemyTest : MonoBehaviour
{
    private GameObject enemyGameObject; // A DestroyedEnemy GameObject
    private DestroyedEnemyTest destroyedEnemy; // A DestroyedEnemy komponens
    private GameObject GameManagerGO; // Mock GameManager GameObject
    private GameObject playerGO; // Mock Player GameObject

    // Játékos
    public GameObject Player;

    // Kiiktatást számláló UI szöveg
    TextMeshProUGUI killsTextUI;
    
    int kills; // Kiiktatásokat számláló
    public int Kills
    {
        get
        {
            return this.kills;
        }
        set
        {
            this.kills = value;
            UpdateScoreTextUI();
        }
    }

    void UpdateScoreTextUI()
    {
        // UI frissítés
        string killsStr = string.Format("{0:0}", kills);
        killsTextUI.text = killsStr;

        // Küldetés teljesítése elért kiiktatással
        if (killsStr == "100")
        {
            // Játék megszakítása
            GameManagerGO.GetComponent<GameManagerTest>().SetGameManagerState(GameManagerTest.GameManagerState.GameOver);
            Player.SetActive(false);

            // Új szint feloldása
            UnlockNewLevel();
        }
    }

    // Új szint feloldása és feloldott szintek elmentése
    void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy GameObject-et a DestroyedEnemy-hez és hozzáadjuk a komponenst
        enemyGameObject = new GameObject();
        destroyedEnemy = enemyGameObject.AddComponent<DestroyedEnemyTest>();

        // Létrehozunk egy mock GameManager GameObject-et
        GameManagerGO = new GameObject();
        GameManagerGO.AddComponent<GameManagerTest>();

        // Létrehozunk egy mock Player GameObject-et
        playerGO = new GameObject();

        // Hozzárendeljük a GameManager és Player hivatkozásokat
        destroyedEnemy.GameManagerGO = GameManagerGO;
        destroyedEnemy.Player = playerGO;

        // Inicializáljuk a kills szöveg UI-t
        var textMeshProUGUI = enemyGameObject.AddComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = "0"; // Kezdeti szöveg a kills számlálóhoz
    }

    [Test]
    public void Kills_Grow_IfSet()
    {
        // Előkészítés
        destroyedEnemy.Kills = 0;

        // Művelet
        destroyedEnemy.Kills = 5;

        // Ellenőrzés
        Assert.AreEqual(5, destroyedEnemy.Kills);
    }

    [Test]
    public void Kills_UIUpdate()
    {
        // Előkészítés
        var textMeshProUGUI = enemyGameObject.GetComponent<TextMeshProUGUI>();

        // Művelet
        destroyedEnemy.Kills = 10;

        // Ellenőrzés
        Assert.AreEqual("10", textMeshProUGUI.text);
    }

    [Test]
    public void Kills_Reach_100_GameOver_State()
    {
        // Előkészítés
        destroyedEnemy.Kills = 99;

        // Művelet
        destroyedEnemy.Kills = 100;

        // Ellenőrzés
        Assert.IsTrue(GameManagerGO.GetComponent<GameManagerTest>().GameOverGO.activeSelf, "A GameOver állapotnak aktívnak kell lennie.");
        Assert.IsFalse(playerGO.activeSelf, "A játékosnak inaktívnak kell lennie.");
    }

    [TearDown]
    public void TearDown()
    {
        // A létrehozott GameObject eltávolítása
        Object.Destroy(enemyGameObject);
        Object.Destroy(GameManagerGO);
        Object.Destroy(playerGO);
    }
}
