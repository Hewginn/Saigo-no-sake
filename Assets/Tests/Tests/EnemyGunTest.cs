using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyGunTest : MonoBehaviour
{
    private GameObject enemyGO;
    private EnemyGunTest enemyGun;
    private GameObject playerGO;

    //Cikázó ellnfél
    public bool isEnemyZigZag;

    //Első frame update előtt van meghívva
    void Start()
    {

        //Lövés 1 másodperc elteltével
        Invoke("FireEnemyBullet", 1f);

        //Ha az ellenfél cikázik 3 másodperc múlva is löjjön egyet
        if (isEnemyZigZag)
        {
            Invoke("FireEnemyBullet", 3f);
        }
    }

    [SetUp]
    public void SetUp()
    {
        // Létrehozunk egy ellenséges objektumot és hozzáadjuk az EnemyGun komponenst
        enemyGO = new GameObject();
        enemyGun = enemyGO.AddComponent<EnemyGunTest>();

        // Létrehozunk egy játékos objektumot, amit célpontként használunk
        playerGO = new GameObject("PlayerGO");
    }

    [UnityTest]
    public IEnumerator EnemyGun_ShootsAfterOneSecond()
    {
        // Inicializáljuk az ellenséges lövést
        enemyGun.Start();

        // 1 másodperc várakozás a lövés elindulásához
        yield return new WaitForSeconds(1.1f);

        // Ellenőrizzük, hogy van-e egy új lövedék a jelenetben
        EnemyBulletTest bullet = GameObject.FindObjectOfType<EnemyBulletTest>();
        Assert.IsNotNull(bullet, "A lövedék nem lett létrehozva 1 másodperc elteltével.");
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
        EnemyBulletTest bullet = GameObject.FindObjectOfType<EnemyBulletTest>();
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
    }
}
