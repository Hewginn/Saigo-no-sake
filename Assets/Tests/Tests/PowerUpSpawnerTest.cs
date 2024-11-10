using NUnit.Framework;
using UnityEngine;

public class PowerUpSpawnerTest : MonoBehaviour
{
    private GameObject spawnerGO;                // A PowerUpSpawner GameObject
    private PowerUpSpawnerTest powerUpSpawner;       // A PowerUpSpawner komponens

    // Fejlesztés
    public GameObject UpgradePUGO;

    // Gyógyítás
    public GameObject HealPUGO;

    // Különleges lövedék
    public GameObject SpecialPUGO;

    // Max létrehozási gyakoriság
    float maxSpawnRateInSeconds;

     //Következő tárgy ütemezése
    void ScheduleNextPowerUpSpawn()
    {
        //Létrehozási idő mp-ben
        float spawnInSeconds;

        //értéke legalább 1mp
        if (maxSpawnRateInSeconds > 1f)
        {   
            spawnInSeconds = Random.Range(1f, maxSpawnRateInSeconds);
        }
        else{
            spawnInSeconds = 1f;
        }

        //Létrehozás rekurzív meghívása x mp-en belül
        Invoke("SpawnPowerUp", spawnInSeconds);
    }

    //Első létreghozás ütemezése
    public void SchedulePowerUpSpawner()
    {
        //Max létrehozási gyakoriság értékadás
        maxSpawnRateInSeconds = 5f;

        //Létrehozás meghívás x mp-en belül
        Invoke("SpawnPowerUp", maxSpawnRateInSeconds);

    }

    //Létrehozás befejezése
    public void UnSchedulePowerUpSpawner()
    {
        CancelInvoke("SpawnPowerUp");
    }


    // SpawnPowerUp metódus itt marad

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy GameObject-et a PowerUpSpawner számára és hozzáadjuk a komponenst
        spawnerGO = new GameObject();
        powerUpSpawner = spawnerGO.AddComponent<PowerUpSpawnerTest>();

        // Mock power-up prefabok hozzárendelése teszteléshez
        powerUpSpawner.UpgradePUGO = new GameObject("UpgradePU");
        powerUpSpawner.HealPUGO = new GameObject("HealPU");
        powerUpSpawner.SpecialPUGO = new GameObject("SpecialPU");

        // Létrehozunk egy kamerát és beállítjuk a MainCamera tag-et
        GameObject cameraObject = new GameObject("Main Camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        cameraObject.tag = "MainCamera"; // Beállítjuk a kamerát MainCamera tag-re

        // Inicializáljuk a spawnt
        powerUpSpawner.SchedulePowerUpSpawner();
    }

    // További tesztmetódusok itt maradnak, pl. SchedulePowerUpSpawner_SchedulesFirstSpawn

    [TearDown]
    public void TearDown()
    {
        // Tisztítsuk meg a létrehozott GameObject-eket
        Object.Destroy(spawnerGO);
        Object.Destroy(GameObject.Find("UpgradePU"));
        Object.Destroy(GameObject.Find("HealPU"));
        Object.Destroy(GameObject.Find("SpecialPU"));
    }
}
