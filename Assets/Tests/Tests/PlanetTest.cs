using NUnit.Framework;
using UnityEngine;

public class PlanetTest
{
    private GameObject planetGO;       // A bolygó GameObject
    private PlanetTest planet;             // A Planet komponens

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy GameObject-et a bolygó számára és hozzáadjuk a Planet komponenst
        planetGO = new GameObject();
        planet = planetGO.AddComponent<Planet>();
        
        // Alapértelmezett sebességet állítunk be a bolygónak
        planet.speed = 5f;

        // Beállítjuk a kamera pozícióját, hogy a min/max számítások érvényesek legyenek
        Camera.main = new GameObject().AddComponent<Camera>();
        Camera.main.transform.position = new Vector3(0, 0, -10); // A kamera érvényes pozíciójának beállítása
    }

    [Test]
    public void Awake_InitializesIsMovingAndBounds()
    {
        // Meghívjuk az Awake-ot
        planet.Awake();

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
        planet.Update();

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
