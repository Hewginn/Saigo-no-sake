using NUnit.Framework;             // NUnit tesztelési keretrendszer
using UnityEngine;                // Unity alapvető funkciók
using TMPro;                     // TextMeshProUGUI használata

public class PlayerControlTest
{
    private GameObject playerGO;             // A játékos GameObject
    private PlayerControl playerControl;     // A PlayerControl komponens

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy GameObject-et a játékos számára és hozzáadjuk a PlayerControl komponenst
        playerGO = new GameObject();
        playerControl = playerGO.AddComponent<PlayerControl>();

        // Beállítunk egy kamerát a viewport számításokhoz
        Camera.main = new GameObject().AddComponent<Camera>();
        Camera.main.transform.position = new Vector3(0, 0, -10); // A kamera pozíciójának beállítása

        // Inicializáljuk a TextMeshProUGUI komponenseket
        playerControl.LivesUIText = new GameObject().AddComponent<TextMeshProUGUI>();
        playerControl.SpecialsUIText = new GameObject().AddComponent<TextMeshProUGUI>();
        playerControl.scoreUITextGO = new GameObject(); // Placeholder a pontszám UI számára
        playerControl.scoreUITextGO.AddComponent<GameScore>();

        // Hozzárendelünk dummy GameObject-eket más referenciákhoz
        playerControl.GameManagerGO = new GameObject();
        playerControl.PlayerBulletGO = new GameObject();
        playerControl.bulletPosition01 = new GameObject();
        playerControl.bulletPosition02 = new GameObject();
        playerControl.bulletPosition03 = new GameObject();
        playerControl.bulletPosition04 = new GameObject();
        playerControl.bulletPosition05 = new GameObject();
        playerControl.bulletPosition06 = new GameObject();
        playerControl.specialPosition = new GameObject();
        playerControl.ExpolsionGO = new GameObject();
        playerControl.SpecialGO = new GameObject();
        
        // Meghívjuk az Init-et a kezdeti állapotok beállításához
        playerControl.Init();
    }

    [Test]
    public void Init_SetsInitialValues()
    {
        // Ellenőrizzük, hogy a kezdeti életek helyesen vannak beállítva
        Assert.AreEqual(3, playerControl.LivesUIText.text);
        Assert.AreEqual(0, playerControl.GetType().GetField("upgradeLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(playerControl));
        Assert.AreEqual(0, playerControl.GetType().GetField("specials", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(playerControl));
        Assert.AreEqual("X 0", playerControl.SpecialsUIText.text);
    }

    [Test]
    public void Move_UpdatesPlayerPosition()
    {
        // Beállítjuk a sebességet és szimuláljuk a mozgáshoz szükséges bemenetet
        playerControl.speed = 5f;

        // Mozgás jobbra
        Input.SetKey("right");
        playerControl.Update();

        // Ellenőrizzük, hogy a játékos jobbra mozdult
        Assert.Greater(playerGO.transform.position.x, 0);

        // Mozgás balra
        Input.SetKey("left");
        playerControl.Update();

        // Ellenőrizzük, hogy a játékos balra mozdult
        Assert.Less(playerGO.transform.position.x, 0);
    }

    [Test]
    public void Shoot_FiresBullets()
    {
        playerControl.lastShoot = 0f; // Visszaállítjuk az utolsó lövés idejét
        playerControl.Update(); // Meghívjuk az Update-ot a bemenet ellenőrzésére

        // Szimuláljuk a space billentyű lenyomását a lövéshez
        Input.SetKeyDown("space");
        playerControl.Update();

        // Ellenőrizzük, hogy a lövedékek instanciálva lettek-e (mock vagy megfelelő módszer ellenőrzése)
        // Valós esetben ellenőriznünk kell, hogy a lövedék prefab helyesen lett-e instanciálva.
        // Ehhez szükséges lehet a PlayerControl osztály módosítása a lövedékek számának vagy hasonló kitételének kitettségéhez.
    }

    [Test]
    public void OnTriggerEnter2D_EnemyCollision_DecreasesLives()
    {
        // Szimulálunk egy ütközést egy ellenséggel
        GameObject enemyGO = new GameObject { tag = "EnemyShipTag" };
        enemyGO.AddComponent<BoxCollider2D>().isTrigger = true;

        playerControl.OnTriggerEnter2D(enemyGO.GetComponent<Collider2D>());

        // Ellenőrizzük, hogy az életek csökkentek-e
        Assert.AreEqual(2, playerControl.GetType().GetField("lives", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(playerControl));
    }

    [Test]
    public void OnTriggerEnter2D_HealPU_IncreasesLives()
    {
        // Szimuláljuk egy egészségügyi power-up felvételét
        GameObject healPU = new GameObject { tag = "HealPU" };
        healPU.AddComponent<BoxCollider2D>().isTrigger = true;

        playerControl.OnTriggerEnter2D(healPU.GetComponent<Collider2D>());

        // Ellenőrizzük, hogy az életek helyesen nőttek-e
        Assert.AreEqual(3, playerControl.GetType().GetField("lives", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(playerControl));
    }

    [Test]
    public void OnTriggerEnter2D_UpgradePU_IncreasesUpgradeLevel()
    {
        // Szimuláljuk egy upgrade power-up felvételét
        GameObject upgradePU = new GameObject { tag = "UpgradePU" };
        upgradePU.AddComponent<BoxCollider2D>().isTrigger = true;

        playerControl.OnTriggerEnter2D(upgradePU.GetComponent<Collider2D>());

        // Ellenőrizzük, hogy az upgrade szint növekedett-e
        Assert.AreEqual(1, playerControl.GetType().GetField("upgradeLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(playerControl));
    }
}
