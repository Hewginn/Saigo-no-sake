using NUnit.Framework;             // NUnit tesztelési keretrendszer
using UnityEngine;                // Unity alapvető funkciók
using UnityEngine.TestTools;      // Unity tesztelési eszközök
using System.Collections;         // Gyűjteményekhez szükséges

public class EnemySpreadGunTest
{
    private GameObject enemyGO;          // Az ellenség GameObject
    private EnemySpreadGun enemySpreadGun; // Az EnemySpreadGun komponens

    [SetUp]
    public void SetUp()
    {
        // Létrehozunk egy GameObject-et az ellenség számára és hozzáadjuk az EnemySpreadGun komponenst
        enemyGO = new GameObject();
        enemySpreadGun = enemyGO.AddComponent<EnemySpreadGun>();
        enemySpreadGun.EnemyBulletGO = new GameObject("EnemyBullet");
        enemySpreadGun.EnemyBulletGO.AddComponent<EnemyBullet>();
    }

    [UnityTest]
    public IEnumerator EnemySpreadGun_FiresBulletsTowardsPlayerWithSpread()
    {
        // Létrehozunk egy játékos GameObject-et
        GameObject playerGO = new GameObject("PlayerGO");
        playerGO.transform.position = Vector2.zero; // Játékos pozíciójának beállítása

        enemyGO.transform.position = new Vector2(0, 5); // Az ellenség pozíciója
        yield return new WaitForSeconds(1.1f); // Várakozás a lövésekhez

        EnemyBullet[] bullets = Object.FindObjectsOfType<EnemyBullet>(); // Keresd meg az összes lövedéket
        Assert.AreEqual(3, bullets.Length); // Ellenőrizzük, hogy három lövedék van

        // Ellenőrizzük, hogy a lövedékek különböző irányokba repülnek
        Vector2 direction1 = (Vector2)(bullets[0].transform.position - enemyGO.transform.position).normalized;
        Vector2 direction2 = (Vector2)(bullets[1].transform.position - enemyGO.transform.position).normalized;
        Vector2 direction3 = (Vector2)(bullets[2].transform.position - enemyGO.transform.position).normalized;

        Assert.AreNotEqual(direction1, direction2); // Ellenőrizzük az irányok eltérését
        Assert.AreNotEqual(direction1, direction3);
        Assert.AreNotEqual(direction2, direction3);
    }
}
