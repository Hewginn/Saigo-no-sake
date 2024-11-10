using NUnit.Framework;
using UnityEngine;

public class PlayerSpecialTest : MonoBehaviour
{
    // Kezdő pozíció
    Vector2 startPosition;
    private GameObject specialBulletGO;        // A speciális golyó GameObject
    private PlayerSpecialTest playerSpecial;   // A PlayerSpecial komponens

    // Lövedék sebessége
    float speed;

    // Különleges lövedék robbanása
    public GameObject specialExplosion;

    // Billentyűzet szimuláció
    private bool keyFPressed = false;

    // Minden frame során megvan hívva
    void Update()
    {
        // A lövedék jelenlegi helyzete
        Vector2 position = transform.position;
        // A lövedék új helyének meghatározása
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);
        // A lövedék új helyének beállítása
        transform.position = position;

        // Ez a játék jobb felső sarka
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        // Ha a töltény elhagyja a játékteret vagy ha a játékos megnyomja az 'F' gombot, akkor semmisüljön meg
        if (transform.position.y > max.y || startPosition.y + 5f < transform.position.y || keyFPressed)
        {
            Destroy(gameObject);
        }
    }

    // Simulált bemenet a teszteléshez (F billentyű lenyomásának szimulálása)
    public void SimulateKeyPress(KeyCode key)
    {
        if (key == KeyCode.F)
        {
            keyFPressed = true;
        }
    }

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy GameObject-et a speciális golyó számára és hozzáadjuk a PlayerSpecial komponenst
        specialBulletGO = new GameObject();
        playerSpecial = specialBulletGO.AddComponent<PlayerSpecialTest>();

        // Létrehozunk egy új kamerát, mivel nem lehet hozzárendelni Camera.main-hez
        GameObject cameraGO = new GameObject("MainCamera");
        cameraGO.tag = "MainCamera";
        Camera camera = cameraGO.AddComponent<Camera>();
        camera.transform.position = new Vector3(0, 0, -10); // A kamera pozíciójának beállítása

        // Hozzárendelünk egy speciális robbanás prefabot a teszteléshez
        playerSpecial.specialExplosion = new GameObject(); // A robbanás prefab helyettesítője
        playerSpecial.specialExplosion.AddComponent<SpriteRenderer>(); // SpriteRenderer hozzáadása a teszteléshez
    }

    [Test]
    public void Start_SetsInitialValues()
    {
        // Ellenőrizzük, hogy a kezdő pozíció helyesen van-e beállítva
        Assert.AreEqual(specialBulletGO.transform.position, playerSpecial.startPosition);
    }

    [Test]
    public void Update_MovesSpecialBullet()
    {
        // Szimuláljuk egy frame frissítést
        playerSpecial.Update();

        // Ellenőrizzük, hogy a pozíció megváltozott-e
        Assert.Greater(specialBulletGO.transform.position.y, playerSpecial.startPosition.y);
    }

    [Test]
    public void Update_DestroyBulletOffScreen()
    {
        // A golyó pozícióját beállítjuk, hogy éppen a képernyő teteje fölött legyen
        specialBulletGO.transform.position = new Vector2(0, 10);
        playerSpecial.Update(); // Meghívjuk az Update-ot a mozgás szimulálásához

        // Ellenőrizzük, hogy a golyónak el kell tűnnie, amikor a képernyőről kifelé megy
        Assert.IsNull(specialBulletGO);
    }

    [Test]
    public void Update_DestroyBulletOnKeyPress()
    {
        // Szimuláljuk az 'F' billentyű lenyomását
        playerSpecial.SimulateKeyPress(KeyCode.F);
        playerSpecial.Update(); // Meghívjuk az Update-ot a bemenet ellenőrzésére

        // Ellenőrizzük, hogy a golyónak el kell tűnnie
        Assert.IsNull(specialBulletGO);
    }

    // Ütközés kezelő
    void OnTriggerEnter2D(Collider2D col)
    {
        // Ha ellenfélnek ütközik semmisüljön meg
        if (col.tag == "EnemyShipTag")
        {
            Destroy(gameObject);
        }
    }

    [Test]
    public void OnTriggerEnter2D_DestroyOnEnemyCollision()
    {
        // Létrehozunk egy ellenség GameObject-et az ütközéshez
        GameObject enemyGO = new GameObject { tag = "EnemyShipTag" };
        enemyGO.AddComponent<BoxCollider2D>().isTrigger = true;

        // Szimuláljuk az ütközést
        playerSpecial.OnTriggerEnter2D(enemyGO.GetComponent<Collider2D>());

        // Ellenőrizzük, hogy a golyónak el kell tűnnie
        Assert.IsNull(specialBulletGO);
    }

    // Törlödés esetén robbanjon fel
    private void OnDestroy()
    {
        GameObject explosion = Instantiate(specialExplosion);
        explosion.transform.position = transform.position;
    }

    [Test]
    public void OnDestroy_InstantiateExplosion()
    {
        // Előkészítjük a robbanás létrehozásának rögzítését
        GameObject explosionPrefab = playerSpecial.specialExplosion;

        // Meghívjuk az OnDestroy-t, hogy elindítsuk a robbanást
        playerSpecial.OnDestroy();

        // Ellenőrizzük, hogy a robbanás a megfelelő helyen lett-e létrehozva
        Assert.AreEqual(specialBulletGO.transform.position, explosionPrefab.transform.position);
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítsuk meg a létrehozott GameObject-eket
        if (specialBulletGO != null)
            Object.Destroy(specialBulletGO);
    }
}
