using NUnit.Framework;             // NUnit tesztelési keretrendszer
using UnityEngine;                // Unity alapvető funkciók
using UnityEngine.TestTools;      // Unity tesztelési eszközök
using System.Collections;         // Gyűjteményekhez szükséges

public class EnemyBulletTest : MonoBehaviour
{
    private GameObject bulletGO;         // A lövedék GameObject
    private EnemyBulletTest enemyBullet;     // Az EnemyBullet komponens

    [SetUp]
    public void SetUp()
    {
        // Létrehozunk egy új GameObject-et a lövedék számára és hozzáadjuk az EnemyBullet komponenst
        bulletGO = new GameObject();
        enemyBullet = bulletGO.AddComponent<EnemyBulletTest>();
        enemyBullet.direction = Vector2.up; // Beállítjuk az irányt felfelé
    }

    [UnityTest]
    public IEnumerator Bullet_MovesInSetDirection()
    {
        Vector3 initialPosition = bulletGO.transform.position; // A kezdeti pozíció mentése

        // Rövid várakozás, hogy a lövedék mozogni tudjon
        yield return new WaitForSeconds(0.1f);

        // Ellenőrizzük, hogy a lövedék pozíciója megváltozott-e a várt irányban
        Assert.Greater(bulletGO.transform.position.y, initialPosition.y, "A lövedék nem mozgott felfelé!");
    }

    [UnityTest]
    public IEnumerator Bullet_DestroyedOutsideScreenBounds()
    {
        // A lövedék pozícióját a képernyő határain kívülre állítjuk
        bulletGO.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1.2f));

        // Várunk egy frame-et, hogy a lövedék láthatóságát ellenőrizhesse
        yield return null;

        // Ellenőrizzük, hogy a GameObject megsemmisült-e
        Assert.IsNull(bulletGO, "A lövedék nem lett megsemmisítve, amikor a képernyő határain kívülre került!");
    }

    [UnityTest]
    public IEnumerator Bullet_DestroyedOnPlayerCollision()
    {
        // Létrehozunk egy új GameObject-et, amely a játékost képviseli, és beállítjuk a megfelelő tag-et
        GameObject player = new GameObject();
        player.tag = "PlayerShipTag";
        player.AddComponent<BoxCollider2D>(); // Collider hozzáadása a játékoshoz

        // Collider hozzáadása a lövedékhez
        bulletGO.AddComponent<BoxCollider2D>();

        // A lövedéket beállítjuk, hogy ütközzön a játékossal
        bulletGO.transform.position = player.transform.position;

        // Várunk egy frame-et, hogy az ütközés megtörténjen
        yield return null;

        // Ellenőrizzük, hogy a lövedék megsemmisült-e
        Assert.IsNull(bulletGO, "A lövedék nem lett megsemmisítve a játékossal való ütközéskor!");

        // Tisztítás
        Object.Destroy(player);
    }

    [TearDown]
    public void TearDown()
    {
        // Teszt utáni tisztítás
        if (bulletGO != null)
        {
            Object.Destroy(bulletGO);
        }
    }
}
