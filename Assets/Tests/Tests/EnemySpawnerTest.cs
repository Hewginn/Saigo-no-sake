using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemySpawnerTest : MonoBehaviour
{
    //Első ellenség fajta
    public GameObject EnemyGO;
    private GameObject spawnerGO;
    private EnemySpawnerTest enemySpawner; // A megfelelő osztályra állítjuk

    //Ellenség létrehozási gyakoriság (alap: 5mp)
    float maxSpawnRateInSeconds;

    [SetUp]
    public void SetUp()
    {
        // Létrehozunk egy spawner GameObject-et, és hozzáadjuk az EnemySpawner komponenst
        spawnerGO = new GameObject();
        enemySpawner = spawnerGO.AddComponent<EnemySpawnerTest>();

        // Egy ellenség prefab létrehozása a teszthez
        enemySpawner.EnemyGO = new GameObject("Enemy1");
    }

    [UnityTest]
    public IEnumerator EnemySpawner_SpawnEnemiesAtScheduledRate()
    {
        // Ellenségek létrehozásának megkezdése
        enemySpawner.ScheduleEnemySpawner();

        // Rövid várakozás az első ellenségek megjelenéséhez
        yield return new WaitForSeconds(enemySpawner.maxSpawnRateInSeconds + 0.1f);

        // Ellenőrizzük, hogy az ellenség is megjelenik-e a jelenetben
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Assert.AreEqual(1, enemies.Length, "Az ellenség nem lett létrehozva az ütemezett időben.");
    }

    //Ellenségek létrehozásának megkezdése
    public void ScheduleEnemySpawner()
    {
        
        maxSpawnRateInSeconds = 5f;
        Invoke("SpawnEnemy", maxSpawnRateInSeconds);
        InvokeRepeating("IncreaseSpawnRate", 0f, 30f);

    }

    //Ellnségek létrehozásának megszakítása
    public void UnScheduleEnemySpawner()
    {
        CancelInvoke("SpawnEnemy");
        CancelInvoke("IncreaseSpawnRate");
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítás a teszt után
        if (spawnerGO != null) Object.Destroy(spawnerGO);
        if (enemySpawner.EnemyGO != null) Object.Destroy(enemySpawner.EnemyGO);
    }
}
