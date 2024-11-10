using NUnit.Framework;
using UnityEngine;

public class PlayerBulletTest : MonoBehaviour
{
    // Lövedék sebessége
    private float speed;

    // Első frame update előtt van meghívva
    void Start()
    {
        // Sebesség inicializálása
        speed = 8f;
    }

    private GameObject bulletGO;               // A lövedék GameObject
    private PlayerBulletTest playerBullet;         // A PlayerBullet komponens

    // Minden frame során megvan hívva
    void Update()
    {
        // a lövedék jelenlegi helyzete
        Vector2 position = transform.position;
        // a lövedék új helyének meghatározása
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);
        // a lövedék új helyének beállítása
        transform.position = position;

        // ez a játék jobb felső sarka
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        // ha a töltény elhagyja a játékteret, akkor semmisüljön meg
        if (transform.position.y > max.y)
        {
            Destroy(gameObject);
        }
    }

    //Ütközési esemény kezelő
    void OnTriggerEnter2D(Collider2D col){

        //Lövedék elpusztítása, ha ellenfélnek ütközik
        if(col.tag == "EnemyShipTag"){
            Destroy(gameObject);
        }
    }

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy GameObject-et a lövedék számára és hozzáadjuk a PlayerBullet komponenst
        bulletGO = new GameObject();
        playerBullet = bulletGO.AddComponent<PlayerBulletTest>();

        // Létrehozunk egy új GameObject-et és hozzárendeljük a Camera komponenst
        GameObject cameraGO = new GameObject("TestCamera");
        cameraGO.AddComponent<Camera>();  // Hozzáadjuk a Camera komponenst
        cameraGO.transform.position = new Vector3(0, 0, -10);  // Beállítjuk a pozícióját
    }

    

    [Test]
    public void Start_SetsInitialSpeed()
    {
        // Meghívjuk a Start-ot
        playerBullet.Start();

        // Ellenőrizzük, hogy a sebesség helyesen van inicializálva
        Assert.AreEqual(8f, playerBullet.GetType().GetField("speed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(playerBullet));
    }

    [Test]
    public void Update_MovesBulletUpwards()
    {
        // Meghívjuk a Start-ot a sebesség inicializálásához
        playerBullet.Start();

        // Elmentjük a kezdeti pozíciót
        Vector2 initialPosition = bulletGO.transform.position;

        // Szimulálunk egy framerate frissítést
        float deltaTime = 0.1f; // Szimuláljuk a 0.1 másodpercet
        playerBullet.Update();

        // Ellenőrizzük, hogy a lövedék pozíciója helyesen változott
        Assert.AreNotEqual(initialPosition, bulletGO.transform.position);
        float speedValue = (float)playerBullet.GetType().GetField("speed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(playerBullet);
        Assert.AreEqual(initialPosition.y + (speedValue * deltaTime), bulletGO.transform.position.y);
    }

    [Test]
public void Update_DestroysBulletWhenGoesOffScreen()
{
    // Meghívjuk a Start-ot a sebesség inicializálásához
    playerBullet.Start();

    // A lövedék a képernyő tetejére mozgatása
    Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    bulletGO.transform.position = new Vector2(0, max.y + 1); // Pozíció a képernyő fölött

    // Meghívjuk az Update-ot
    playerBullet.Update();

    // Ellenőrizzük, hogy a lövedék megsemmisült-e
    Assert.IsNull(bulletGO);
}


    [Test]
    public void OnTriggerEnter2D_DestroyBulletOnEnemyCollision()
    {
        // Létrehozunk egy mock ellenség GameObject-et
        GameObject enemyGO = new GameObject();
        enemyGO.tag = "EnemyShipTag"; // Beállítjuk az ütközéshez szükséges taget

        // Hozzáadunk egy Collider2D-t az ellenséghez (szükséges az trigger eseményekhez)
        enemyGO.AddComponent<BoxCollider2D>().isTrigger = true;

        // Meghívjuk a Start-ot a lövedék inicializálásához
        playerBullet.Start();

        // Meghívjuk az OnTriggerEnter2D-t
        playerBullet.OnTriggerEnter2D(enemyGO.GetComponent<Collider2D>());

        // Ellenőrizzük, hogy a lövedék megsemmisült-e
        Assert.IsNull(bulletGO);
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítsuk meg a létrehozott GameObject-eket
        if (bulletGO != null)
            Object.Destroy(bulletGO);
    }
}
