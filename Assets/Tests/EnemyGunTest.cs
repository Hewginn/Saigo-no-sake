using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyGunTest
{
    private GameObject enemyGO;
    private EnemyGun enemyGun;
    private GameObject playerGO;

    [SetUp]
    public void SetUp()
    {
        // Létrehozunk egy ellenséges objektumot és hozzáadjuk az EnemyGun komponenst
        enemyGO = new GameObject();
        enemyGun = enemyGO.AddComponent<EnemyGun>();

        // Létrehozunk egy játékos objektumot, amit célpontként használunk
        playerGO = new GameObject("PlayerGO");

        // Létrehozunk egy lövedék prefab objektumot
        enemyGun.EnemyBulletGO = new GameObject();
        enemyGun.EnemyBulletGO.AddComponent<EnemyBullet>(); // Hozzáadjuk az EnemyBullet komponenst a prefabhoz
    }

    [UnityTest]
    public IEnumerator EnemyGun_ShootsAfterOneSecond()
    {
        // Inicializáljuk az ellenséges lövést
        enemyGun.Start();

        // 1 másodperc várakozás a lövés elindulásához
        yield return new WaitForSeconds(1.1f);

        // Ellenőrizzük, hogy van-e egy új lövedék a jelenetben
        EnemyBullet bullet = GameObject.FindObjectOfType<EnemyBullet>();
        Assert.IsNotNull(bullet, "A lövedék nem lett létrehozva 1 másodperc elteltével.");
    }

    [UnityTest]
    public IEnumerator EnemyGun_ZigZagShootsAdditionalBulletAfterThreeSeconds()
    {
        // Beállítjuk a cikázó ellenséget
        enemyGun.isEnemyZigZag = true;

        // Inicializáljuk az ellenséges lövést
        enemyGun.Start();

        // 3 másodperc várakozás a második lövés elindulásához
        yield return new WaitForSeconds(3.1f);

        // Ellenőrizzük, hogy van legalább két lövedék a jelenetben
        EnemyBullet[] bullets = GameObject.FindObjectsOfType<EnemyBullet>();
        Assert.IsTrue(bullets.Length >= 2, "A cikázó ellenség nem lőtt másodjára 3 másodperc után.");
    }

    [UnityTest]
    public IEnumerator EnemyGun_FiresTowardPlayer()
    {
        // Beállítjuk a játékos helyzetét az ellenség felett
        playerGO.transform.position = new Vector3(0, 5, 0);

        // Inicializáljuk az ellenséges lövést
        enemyGun.Start();

        // 1 másodperc várakozás a lövés elindulásához
        yield return new WaitForSeconds(1.1f);

        // Ellenőrizzük a lövedék irányát
        EnemyBullet bullet = GameObject.FindObjectOfType<EnemyBullet>();
        Assert.IsNotNull(bullet, "A lövedék nem lett létrehozva a játékos felé irányítva.");

        Vector2 expectedDirection = (playerGO.transform.position - bullet.transform.position).normalized;
        Vector2 actualDirection = bullet.GetComponent<Rigidbody2D>().velocity.normalized;

        Assert.AreEqual(expectedDirection.x, actualDirection.x, 0.1f, "A lövedék nem a játékos felé irányult.");
        Assert.AreEqual(expectedDirection.y, actualDirection.y, 0.1f, "A lövedék nem a játékos felé irányult.");
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítás a teszt után, ha az objektum még létezik
        if (enemyGO != null) Object.Destroy(enemyGO);
        if (playerGO != null) Object.Destroy(playerGO);
        if (enemyGun.EnemyBulletGO != null) Object.Destroy(enemyGun.EnemyBulletGO);
    }
}
