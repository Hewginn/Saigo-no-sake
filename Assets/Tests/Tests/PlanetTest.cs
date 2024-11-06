using NUnit.Framework;
using UnityEngine;

public class PlanetTest : MonoBehaviour
{

    
    Vector2 min;
    Vector2 max;

    //Gyógyítás
    public GameObject HealPUGO;
    public bool isMoving;
    public float speed;
    private GameObject planetGO;       // A bolygó GameObject

    //Különleges lövedék
    public GameObject SpecialPUGO;
    private PlanetTest planet;             // A Planet komponens

    

    // Update is called once per frame
    void Update()
    {
        if(!isMoving){
            return;
        }
        Vector2 position = transform.position;

         position = new Vector2 (position.x, position.y+ speed * Time.deltaTime);

        transform.position = position;

        if(transform.position.y <min.y){
            isMoving = false;
        }
    } 

    void SpawnPowerUp()
    {
        //Létrehozási határok
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //Egy véletelenszerű tárgy kiválasztására szám
        int powerUpSwitch = Random.Range(0, 3);

        //A tárgy
        GameObject aPowerUp = null;

        //A tárgy értékadása
        switch(powerUpSwitch){
            case 0:
                aPowerUp = (GameObject)Instantiate(UpgradePUGO);
                break;
            case 1:
                aPowerUp = (GameObject)Instantiate(HealPUGO);
                break;
            case 2:
                aPowerUp = (GameObject)Instantiate(SpecialPUGO);
                break;
        }

        //Létrehozás véletelnszerű helyen a határok közöttt
        aPowerUp.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        //Kövekező tárgy rekurzív ütemezése
        ScheduleNextPowerUpSpawn();
    }


     //Következő tárgy ütemezése
    void ScheduleNextPowerUpSpawn()
    {
        //Létrehozási idő mp-ben
        float spawnInSeconds;

        //értéke legalább 1mp
        if (maxSpawnRateInSeconds > 1f)
        {   
            spawnInSeconds = Random.Range(1f, maxSpawnRateInSeconds);
        }
        else{
            spawnInSeconds = 1f;
        }

        //Létrehozás rekurzív meghívása x mp-en belül
        Invoke("SpawnPowerUp", spawnInSeconds);
    }

    //Fejlesztés
    public GameObject UpgradePUGO;

    

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy GameObject-et a bolygó számára és hozzáadjuk a Planet komponenst
        planetGO = new GameObject();
        planet = planetGO.AddComponent<PlanetTest>();
        
        // Alapértelmezett sebességet állítunk be a bolygónak
        planet.speed = 5f;

        // Beállítjuk a kamera pozícióját, hogy a min/max számítások érvényesek legyenek
        Camera.main = new GameObject().AddComponent<Camera>();
        Camera.main.transform.position = new Vector3(0, 0, -10); // A kamera érvényes pozíciójának beállítása
    }
    public void ResetPosition(){
        transform.position = new Vector2(Random.Range(min.x, max.x), max.y);
    }

    void Awake(){
        isMoving = false;

        min = Camera.main.ViewportToWorldPoint (new Vector2(0,0));

        max = Camera.main.ViewportToWorldPoint (new Vector2(1,1));

        max.y = max.y + GetComponent<SpriteRenderer> ().sprite.bounds.extents.y;
        min.y = min.y - GetComponent<SpriteRenderer> ().sprite.bounds.extents.y;
    }

    [Test]
    public void Awake_InitializesIsMovingAndBounds()
    {

        // Ellenőrizzük, hogy az isMoving false-ra van inicializálva
        Assert.IsFalse(planet.isMoving);

        // Ellenőrizzük, hogy a min és max határok be vannak állítva
        Assert.IsNotNull(planet.min);
        Assert.IsNotNull(planet.max);
    }

    [Test]
    public void ResetPosition_SetsRandomPositionAtMaxY()
    {
        // Meghívjuk a ResetPosition-t
        planet.ResetPosition();

        // Ellenőrizzük, hogy a bolygó Y pozíciója a max Y pozíción van
        Assert.AreEqual(planet.transform.position.y, planet.max.y);
    }

    [Test]
    public void Update_MovesPlanetWhenIsMovingIsTrue()
    {
        // A bolygót mozgásra állítjuk
        planet.isMoving = true;
        Vector2 initialPosition = planet.transform.position;

        // Meghívjuk az Update-ot egyszer, a Time.deltaTime szimulálásával egy framerate
        float deltaTime = 0.1f; // Szimuláljunk 0.1 másodpercet
        //planet.Update();

        // Ellenőrizzük, hogy a bolygó pozíciója helyesen változott
        Assert.AreNotEqual(initialPosition, planet.transform.position);
        Assert.AreEqual(initialPosition.y + (planet.speed * deltaTime), planet.transform.position.y);
    }

    [Test]
    public void Update_DoesNotMoveWhenIsMovingIsFalse()
    {
        // A bolygót nem mozgásra állítjuk
        planet.isMoving = false;
        Vector2 initialPosition = planet.transform.position;

        // Meghívjuk az Update-ot
        planet.Update();

        // Ellenőrizzük, hogy a pozíció változatlan maradt
        Assert.AreEqual(initialPosition, planet.transform.position);
    }

    [Test]
    public void Update_StopsMovingWhenBelowMinY()
    {
        // A bolygó pozícióját a min.y alatt állítjuk be
        planet.min = new Vector2(0, 0); // Min beállítása (0,0) pozícióra
        planet.transform.position = new Vector2(0, -1); // A pozíció a min alatt

        // Meghívjuk az Update-ot
        planet.Update();

        // Ellenőrizzük, hogy az isMoving false-ra van állítva
        Assert.IsFalse(planet.isMoving);
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítsuk meg a létrehozott GameObject-eket
        Object.Destroy(planetGO);
    }
}
