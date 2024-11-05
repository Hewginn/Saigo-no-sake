using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemySpawnerTest : MonoBehaviour
{
    private GameObject spawnerGO;
    private EnemySpawnerTest enemySpawner; // A megfelelő osztályra állítjuk

    [SetUp]
    public void SetUp()
    {
        // Létrehozunk egy spawner GameObject-et, és hozzáadjuk az EnemySpawner komponenst
        spawnerGO = new GameObject();
        enemySpawner = spawnerGO.AddComponent<EnemySpawner>();

        // Három ellenségtípus prefab létrehozása a teszthez
        enemySpawner.EnemyGO = new GameObject("Enemy1");
        enemySpawner.EnemyGO2 = new GameObject("Enemy2");
        enemySpawner.EnemyGO3 = new GameObject("Enemy3");
    }

    [UnityTest]
    public IEnumerator EnemySpawner_SpawnEnemiesAtScheduledRate()
    {
        // Ellenségek létrehozásának megkezdése
        enemySpawner.ScheduleEnemySpawner();

        // Rövid várakozás az első ellenségek megjelenéséhez
        yield return new WaitForSeconds(enemySpawner.maxSpawnRateInSeconds + 0.1f);

        // Ellenőrizzük, hogy három típusú ellenség is megjelenik-e a jelenetben
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Assert.AreEqual(3, enemies.Length, "A három ellenségtípus nem lett létrehozva az ütemezett időben.");
    }

    [UnityTest]
    public IEnumerator EnemySpawner_IncreasesSpawnRateOverTime()
    {
        // Ellenségek létrehozásának megkezdése
        enemySpawner.ScheduleEnemySpawner();

        // Várakozás a létrehozási gyakoriság csökkenésére
        yield return new WaitForSeconds(60f);

        // Ellenőrizzük, hogy a maxSpawnRateInSeconds 1-re csökkent
        Assert.AreEqual(1f, enemySpawner.maxSpawnRateInSeconds, "A létrehozási gyakoriság nem csökkent 1 másodpercre 60 másodperc alatt.");
    }

    [UnityTest]
    public IEnumerator EnemySpawner_StopsSpawningOnUnschedule()
    {
        // Ellenségek létrehozásának megkezdése
        enemySpawner.ScheduleEnemySpawner();

        // Rövid várakozás, hogy az első ellenségek létrejöjjenek
        yield return new WaitForSeconds(enemySpawner.maxSpawnRateInSeconds + 0.1f);

        // Létrehozás ütemezésének leállítása
        enemySpawner.UnScheduleEnemySpawner();

        // Újabb várakozás annak ellenőrzésére, hogy nem jelenik meg több ellenség
        yield return new WaitForSeconds(2f);

        // Ellenőrizzük, hogy új ellenségek nem jelentek meg a leállítás után
        GameObject[] enemiesAfterStop = GameObject.FindGameObjectsWithTag("Enemy");
        Assert.AreEqual(3, enemiesAfterStop.Length, "Több ellenség jött létre az ütemezés leállítása után.");
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítás a teszt után
        if (spawnerGO != null) Object.Destroy(spawnerGO);
        if (enemySpawner.EnemyGO != null) Object.Destroy(enemySpawner.EnemyGO);
        if (enemySpawner.EnemyGO2 != null) Object.Destroy(enemySpawner.EnemyGO2);
        if (enemySpawner.EnemyGO3 != null) Object.Destroy(enemySpawner.EnemyGO3);
    }
}
