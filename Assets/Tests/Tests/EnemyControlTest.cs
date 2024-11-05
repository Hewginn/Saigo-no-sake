using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class EnemyControlTest : MonoBehaviour
{
    private GameObject enemyGO;
    private EnemyControlTest enemyControl; // Itt a megfelelő osztályt használjuk

    private GameObject scoreUITextGO;
    private GameScoreTest gameScore; // Megfelelő típust használd

     //Logikai változó: Játékos pusztította-e el az ellenséget
    bool isDestroyedByPlayer = false;
    private GameObject killsUITextGO;
    private DestroyedEnemyTest destroyedEnemy; // Megfelelő típust használd

    //Robbanás
    public GameObject ExpolsionGO;

     //Sebesség
    float speed;

    //Első frame update előtt van meghívva
    void Start()
    {
        //Objektum létrehozásakor szükséges változók inicializálása
        speed = 2f;
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
        killsUITextGO = GameObject.FindGameObjectWithTag("DestroyedEnemies");
    }

    [SetUp]
    public void SetUp()
    {
        // Létrehozunk egy GameObject-et, amelyhez hozzáadjuk az EnemyControl komponenst
        enemyGO = new GameObject();
        enemyControl = enemyGO.AddComponent<EnemyControlTest>();

        // Beállítjuk a robbanás prefab-jét, amit inicializáláshoz használ
        enemyControl.ExpolsionGO = new GameObject("Explosion");

        // Létrehozzuk a pontszám és kiiktatási számlálókat, és hozzáadjuk a szükséges komponenseket
        scoreUITextGO = new GameObject();
        scoreUITextGO.tag = "ScoreTextTag";
        gameScore = scoreUITextGO.AddComponent<GameScoreTest>();

        killsUITextGO = new GameObject();
        killsUITextGO.tag = "DestroyedEnemies";
        destroyedEnemy = killsUITextGO.AddComponent<DestroyedEnemyTest>();

        // Az enemyControl komponenst összekapcsoljuk a pontszám és kiiktatás számlálóval
        enemyControl.Start();
    }

    [UnityTest]
    public IEnumerator Enemy_MovesDownward()
    {
        // Rögzítjük az ellenség kezdeti pozícióját
        Vector3 initialPosition = enemyGO.transform.position;

        // Egy frame-et várunk, hogy a mozgás végbemenjen
        yield return new WaitForSeconds(0.1f);

        // Ellenőrizzük, hogy az ellenség lefelé mozdult-e
        Assert.Less(enemyGO.transform.position.y, initialPosition.y, "Az ellenség nem mozdult lefelé.");
    }

    [UnityTest]
    public IEnumerator Enemy_DestroyedOutsideScreenBounds()
    {
        // Az ellenséget a képernyő alá helyezzük, hogy a következő frame-ben törlődjön
        enemyGO.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, -0.1f));

        // Egy frame-et várunk, hogy a törlés végbemenjen
        yield return null;

        // Ellenőrizzük, hogy az ellenség megsemmisült-e
        Assert.IsTrue(enemyGO == null, "Az ellenség nem lett megsemmisítve a képernyő határán kívül.");
    }

    [UnityTest]
    public IEnumerator Enemy_DestroyedOnPlayerCollision()
    {
        // Létrehozunk egy új játékos lövedéket a szükséges `PlayerBulletTag` tag-gel
        GameObject playerBullet = new GameObject();
        playerBullet.tag = "PlayerBulletTag";
        playerBullet.AddComponent<BoxCollider2D>();

        // Ütközéshez szükséges collider hozzáadása az ellenséghez
        enemyGO.AddComponent<BoxCollider2D>();

        // Az ellenség pozícióját a játékos lövedék pozíciójára állítjuk
        enemyGO.transform.position = playerBullet.transform.position;

        // Egy frame-et várunk az ütközés és megsemmisítés szimulálásához
        yield return null;

        // Ellenőrizzük, hogy az ellenség megsemmisült-e az ütközés után
        Assert.IsTrue(enemyGO == null, "Az ellenség nem lett megsemmisítve a játékossal való ütközéskor.");

        // Tisztítás
        Object.Destroy(playerBullet);
    }

    [UnityTest]
    public IEnumerator ScoreAndKills_IncreasedOnPlayerDestruction()
    {
        // Előzetes értékek mentése
        int initialScore = gameScore.Score;
        int initialKills = destroyedEnemy.Kills;

        // Szimuláljuk az ellenség megsemmisítését a játékos által
        enemyControl.isDestroyedByPlayer = true;

        // Egy frame-et várunk, hogy a megsemmisítés során lefusson a `OnDestroy` metódus
        Object.Destroy(enemyGO);
        yield return null;

        // Ellenőrizzük, hogy a pontszám és a kiiktatás számláló növekedett
        Assert.AreEqual(initialScore + 100, gameScore.Score, "A pontszám nem növekedett megfelelően.");
        Assert.AreEqual(initialKills + 1, destroyedEnemy.Kills, "A kiiktatási számláló nem növekedett megfelelően.");
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítás a teszt után, ha az objektum még létezne
        if (enemyGO != null) Object.Destroy(enemyGO);
        if (scoreUITextGO != null) Object.Destroy(scoreUITextGO);
        if (killsUITextGO != null) Object.Destroy(killsUITextGO);
    }
}
