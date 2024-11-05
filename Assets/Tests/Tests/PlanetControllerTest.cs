using NUnit.Framework;
using UnityEngine;

public class PlanetControllerTest : MonoBehaviour
{
    private GameObject planetControllerGO; // A PlanetController GameObject
    private PlanetControllerTest planetController; // A PlanetController komponens

    private GameObject planet1; // Az első bolygó GameObject
    private GameObject planet2; // A második bolygó GameObject
    private GameObject planet3; // A harmadik bolygó GameObject


    

    [SetUp]
    public void Setup()
    {
        // Létrehozunk GameObject-eket a bolygók számára
        planet1 = new GameObject();
        planet1.AddComponent<PlanetTest>();
        
        planet2 = new GameObject();
        planet2.AddComponent<PlanetTest>();
        
        planet3 = new GameObject();
        planet3.AddComponent<PlanetTest>();

        // Létrehozzuk a GameObject-et a PlanetController számára és hozzáadjuk a komponenst
        planetControllerGO = new GameObject();
        planetController = planetControllerGO.AddComponent<PlanetControllerTest>();
        
        // A bolygók hozzárendelése a PlanetController-hez
        planetController.PlanetTest = new GameObject[] { planet1, planet2, planet3 };

        // A Queue inicializálása, hogy biztosítsuk, hogy minden bolygó elérhető
        planetController.Start();
    }

    [Test]
    public void Start_InitializesAvailablePlanetsQueue()
    {
        // Ellenőrizzük, hogy a sor a bolygókkal van inicializálva
        Assert.AreEqual(3, planetController.availablePlanets.Count);
    }

    [Test]
    public void MovePlanetDown_DequeuesPlanetAndMovesIt()
    {
        // Meghívjuk a MovePlanetDown metódust
        planetController.MovePlanetDown();

        // Ellenőrizzük, hogy egy bolygó ki lett dequeuelve és mozgásra van állítva
        Assert.AreEqual(2, planetController.availablePlanets.Count);
        Assert.IsTrue(planet1.GetComponent<PlanetTest>().isMoving);
    }

    [Test]
    public void EnqueuePlanets_ResetsPositionOfMovedPlanets()
    {
        // A bolygót a küszöb alá állítjuk
        planet1.transform.position = new Vector3(0, -1, 0);
        planet1.GetComponent<PlanetTest>().isMoving = false;

        // Meghívjuk az EnqueuePlanets-t
        planetController.EnqueuePlanets();

        // Ellenőrizzük, hogy a bolygó pozíciója vissza lett állítva
        Assert.AreEqual(Vector3.zero, planet1.transform.position); // Feltételezve, hogy a ResetPosition az origóra állítja
        Assert.AreEqual(1, planetController.availablePlanets.Count);
    }

    [Test]
    public void MovePlanetDown_OnlyMovesAvailablePlanets()
    {
        // A bolygót a küszöb alá állítjuk
        planet1.transform.position = new Vector3(0, -1, 0);
        planet1.GetComponent<PlanetTest>().isMoving = true; // Mozgásra állítjuk

        // Meghívjuk a MovePlanetDown-t
        planetController.MovePlanetDown();

        // Ellenőrizzük, hogy a planet1 nem lett dequeuelve, mivel már mozgásban van
        Assert.AreEqual(3, planetController.availablePlanets.Count);
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítsuk meg a létrehozott GameObject-eket
        Object.Destroy(planetControllerGO);
        Object.Destroy(planet1);
        Object.Destroy(planet2);
        Object.Destroy(planet3);
    }
}
