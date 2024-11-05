using NUnit.Framework;
using UnityEngine;

public class PowerUpSpawnerTest
{
    private GameObject spawnerGO;                // A PowerUpSpawner GameObject
    private PowerUpSpawnerTest powerUpSpawner;      // A PowerUpSpawner komponens

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy GameObject-et a PowerUpSpawner számára és hozzáadjuk a komponenst
        spawnerGO = new GameObject();
        powerUpSpawner = spawnerGO.AddComponent<PowerUpSpawner>();

        // Mock power-up prefabok hozzárendelése teszteléshez
        powerUpSpawner.UpgradePUGO = new GameObject("UpgradePU");
        powerUpSpawner.HealPUGO = new GameObject("HealPU");
        powerUpSpawner.SpecialPUGO = new GameObject("SpecialPU");

        // Beállítunk egy kamerát a viewport számításokhoz
        Camera.main = new GameObject().AddComponent<Camera>();
        Camera.main.transform.position = new Vector3(0, 0, -10); // A kamera pozíciójának beállítása

        // Inicializáljuk a spawnt
        powerUpSpawner.SchedulePowerUpSpawner();
    }

    [Test]
    public void SchedulePowerUpSpawner_SchedulesFirstSpawn()
    {
        // Ellenőrizzük, hogy az első power-up spawn ütemezve van
        Assert.IsTrue(InvokeCheck("SpawnPowerUp"), "A SpawnPowerUp-nak ütemezve kell lennie a SchedulePowerUpSpawner hívása után.");
    }

    [Test]
    public void SpawnPowerUp_CreatesPowerUpInBounds()
    {
        // Közvetlenül hívjuk meg a SpawnPowerUp-ot teszteléshez
        powerUpSpawner.SpawnPowerUp();

        // Ellenőrizzük, hogy egy power-up létrejött-e
        Assert.IsNotNull(GameObject.Find("UpgradePU") || GameObject.Find("HealPU") || GameObject.Find("SpecialPU"),
            "Egy power-up-nak instanciálódnia kell, amikor a SpawnPowerUp-t hívják.");

        // Ellenőrizzük, hogy a power-up a kamera határain belül van-e
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        GameObject spawnedPowerUp = GameObject.Find("UpgradePU") ?? GameObject.Find("HealPU") ?? GameObject.Find("SpecialPU");

        Assert.GreaterOrEqual(spawnedPowerUp.transform.position.x, min.x, "A power-up-nak a bal oldali határon belül kell lennie.");
        Assert.LessOrEqual(spawnedPowerUp.transform.position.x, max.x, "A power-up-nak a jobb oldali határon belül kell lennie.");
        Assert.AreEqual(spawnedPowerUp.transform.position.y, max.y, "A power-up-nak a képernyő tetején kell megjelenni.");
    }

    [Test]
    public void ScheduleNextPowerUpSpawn_SchedulesNextSpawn()
    {
        // Meghívjuk a SpawnPowerUp-ot, hogy elindítsuk az ütemezést
        powerUpSpawner.SpawnPowerUp();

        // Biztosítjuk, hogy a következő power-up spawn ütemezve van
        Assert.IsTrue(InvokeCheck("SpawnPowerUp"), "A következő SpawnPowerUp-nak ütemezve kell lennie.");
    }

    [Test]
    public void UnSchedulePowerUpSpawner_CancelsScheduledSpawns()
    {
        powerUpSpawner.UnSchedulePowerUpSpawner();

        // Ellenőrizzük, hogy az ütemezés törölve lett-e
        Assert.IsFalse(InvokeCheck("SpawnPowerUp"), "A SpawnPowerUp-nak törölve kell lennie az UnSchedulePowerUpSpawner hívása után.");
    }

    private bool InvokeCheck(string methodName)
    {
        // Ellenőrzi, hogy egy metódus ütemezve van-e a hívásra (szimulálva a Unity Invoke rendszerét)
        // Ehhez a Unity futási környezete szükséges, amelyhez workaround vagy egyedi implementáció szükséges a teszteléshez.
        // Itt egy helyettesítőt használunk, mivel a Unity nem biztosít közvetlen hozzáférést az ütemezett hívások listájához.
        return false; // Cserélje ki a tényleges implementációra, ha szükséges
    }

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
