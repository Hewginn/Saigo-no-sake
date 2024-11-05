using NUnit.Framework;
using UnityEngine;

public class PowerUpTest
{
    private GameObject powerUpGO;  // A PowerUp GameObject
    private PowerUp powerUp;        // A PowerUp komponens
    private GameObject playerGO;    // A játékos GameObject

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy GameObject-et a PowerUp számára és hozzáadjuk a komponenst
        powerUpGO = new GameObject();
        powerUp = powerUpGO.AddComponent<PowerUp>();

        // Létrehozunk egy mock játékos GameObject-et
        playerGO = new GameObject("PlayerShip");
        playerGO.tag = "PlayerShipTag"; // A címke beállítása az ütközés kiváltásához

        // Beállítunk egy kamerát a viewport számításokhoz
        Camera.main = new GameObject().AddComponent<Camera>();
        Camera.main.transform.position = new Vector3(0, 0, -10); // A kamera pozíciójának beállítása
    }

    [Test]
    public void Start_SetsInitialSpeed()
    {
        // Ellenőrizzük, hogy a kezdeti sebesség helyesen van-e beállítva
        Assert.AreEqual(2f, powerUp.GetType().GetField("speed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(powerUp), "A kezdeti sebességnek 2f-nek kell lennie.");
    }

    [Test]
    public void Update_MovesPowerUpDown()
    {
        // Elmentjük a kezdeti pozíciót
        Vector2 initialPosition = powerUpGO.transform.position;

        // Meghívjuk az Update metódust a power-up mozgatásához
        powerUp.Update();

        // Ellenőrizzük, hogy a power-up lefelé mozdult-e
        Assert.Less(powerUpGO.transform.position.y, initialPosition.y, "A PowerUp-nak lefelé kell mozognia az Update hívása után.");
    }

    [Test]
    public void MoveForward_DestroysPowerUpWhenOutOfBounds()
    {
        // Beállítjuk a power-up pozícióját a képernyő alján kívül
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        powerUpGO.transform.position = new Vector2(0, min.y - 1f); // A minimum alá pozicionáljuk

        // Meghívjuk a MoveForward-t az eltávolítás ellenőrzéséhez
        powerUp.GetType().GetMethod("MoveForward", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(powerUp, null);

        // Ellenőrizzük, hogy a power-up el lett-e távolítva
        Assert.IsTrue(powerUpGO == null || powerUpGO.transform == null, "A PowerUp-nak el kell tűnnie, amikor a határon kívülre kerül.");
    }

    [Test]
    public void OnTriggerEnter2D_DestroysPowerUpOnPlayerCollision()
    {
        // Hozzáadunk egy ütköződetektáló komponenst a power-up-hoz és a játékoshoz
        var powerUpCollider = powerUpGO.AddComponent<BoxCollider2D>();
        var playerCollider = playerGO.AddComponent<BoxCollider2D>();
        
        // Biztosítjuk, hogy az ütközők trigger-ként legyenek beállítva
        powerUpCollider.isTrigger = true;
        playerCollider.isTrigger = true;

        // Beállítjuk a power-up pozícióját, hogy ütközzön a játékossal
        powerUpGO.transform.position = playerGO.transform.position;

        // Meghívjuk az OnTriggerEnter2D metódust
        powerUp.OnTriggerEnter2D(playerCollider);

        // Ellenőrizzük, hogy a power-up el lett-e távolítva
        Assert.IsTrue(powerUpGO == null || powerUpGO.transform == null, "A PowerUp-nak el kell tűnnie, amikor ütközik a játékossal.");
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítsuk meg a létrehozott GameObject-eket
        Object.Destroy(powerUpGO);
        Object.Destroy(playerGO);
    }
}
