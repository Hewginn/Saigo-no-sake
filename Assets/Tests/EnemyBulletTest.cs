using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class EnemyBulletTest
{
    private GameObject bulletGO;
    private EnemyBullet enemyBullet;

    [SetUp]
    public void SetUp()
    {
        // Létrehozunk egy új GameObject-et és hozzáadjuk az EnemyBullet komponenst
        bulletGO = new GameObject();
        enemyBullet = bulletGO.AddComponent<EnemyBullet>();

        // Az Awake metódus hívásának szimulálása
        enemyBullet.Awake();
    }

    [UnityTest]
    public IEnumerator Bullet_MovesInSetDirection()
    {
        // Beállítunk egy irányt és az Update frame alapú hívást szimuláljuk
        enemyBullet.SetDirection(Vector2.up);

        Vector3 initialPosition = bulletGO.transform.position;

        // Egy frame-et várunk, hogy a lövedék mozogjon
        yield return new WaitForSeconds(0.1f);

        // Ellenőrizzük, hogy a lövedék pozíciója megváltozott-e az irányban
        Assert.Greater(bulletGO.transform.position.y, initialPosition.y, "A lövedék nem mozgott felfelé!");
    }

    [UnityTest]
    public IEnumerator Bullet_DestroyedOutsideScreenBounds()
    {
        // Beállítunk egy irányt felfelé és az Update frame alapú hívást szimuláljuk
        enemyBullet.SetDirection(Vector2.up);

        // A lövedék pozícióját a képernyőn kívülre állítjuk
        bulletGO.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1.2f));

        // Egy frame-et várunk, hogy a lövedék elérje a határokat és megsemmisüljön
        yield return null;

        // Ellenőrizzük, hogy a GameObject megsemmisült-e
        Assert.IsTrue(bulletGO == null, "A lövedék nem lett megsemmisítve a képernyő határain kívül!");
    }

    [UnityTest]
    public IEnumerator Bullet_DestroyedOnPlayerCollision()
    {
        // Létrehozunk egy új GameObject-et, amely a játékos képviselője, és beállítjuk a megfelelő tag-et
        GameObject player = new GameObject();
        player.tag = "PlayerShipTag";
        player.AddComponent<BoxCollider2D>();

        // Ütközéshez szükséges collider hozzáadása a lövedékhez
        bulletGO.AddComponent<BoxCollider2D>();

        // Beállítjuk a lövedéket úgy, hogy ütközzön a játékossal
        bulletGO.transform.position = player.transform.position;

        // Egy frame-et várunk, hogy az ütközés megtörténjen és a lövedék megsemmisüljön
        yield return null;

        // Ellenőrizzük, hogy a lövedék megsemmisült-e
        Assert.IsTrue(bulletGO == null, "A lövedék nem lett megsemmisítve a játékossal való ütközéskor!");

        // Tisztítás
        Object.Destroy(player);
    }

    [TearDown]
    public void TearDown()
    {
        // Teszt utáni tisztítás, ha a lövedék még létezne
        if (bulletGO != null)
        {
            Object.Destroy(bulletGO);
        }
    }
}
