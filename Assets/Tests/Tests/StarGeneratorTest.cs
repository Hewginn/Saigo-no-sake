using NUnit.Framework;
using UnityEngine;

public class StarGeneratorTest
{
    private GameObject starGeneratorGO;  // A StarGenerator GameObject
    private StarGeneratorTest starGenerator;  // A StarGenerator komponens
    private GameObject starPrefab;        // A csillag prefab

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy GameObject-et a StarGenerator számára és hozzáadjuk a komponenst
        starGeneratorGO = new GameObject("StarGenerator");
        starGenerator = starGeneratorGO.AddComponent<StarGenerator>();

        // Létrehozunk egy prefab-t a csillagokhoz SpriteRenderer és Star komponenssel
        starPrefab = new GameObject("StarPrefab");
        var spriteRenderer = starPrefab.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);
        starPrefab.AddComponent<Star>();

        // Beállítjuk a StarGO hivatkozást a StarGenerator-ban
        starGenerator.StarGO = starPrefab;
        starGenerator.MaxStars = 5; // A generálandó csillagok maximális számának beállítása

        // Beállítunk egy kamerát a viewport számításokhoz
        Camera.main = new GameObject().AddComponent<Camera>();
        Camera.main.transform.position = new Vector3(0, 0, -10); // A kamera pozíciójának beállítása
    }

    [Test]
    public void Start_CreatesCorrectNumberOfStars()
    {
        // Meghívjuk a Start metódust a csillagok generálásához
        starGenerator.Start();

        // Megszámoljuk a StarGenerator alatti gyermekek számát
        Assert.AreEqual(5, starGeneratorGO.transform.childCount, "A létrehozott csillagok száma egyenlő kell legyen a MaxStars értékével.");
    }

    [Test]
    public void Start_SetsStarPositionsWithinViewport()
    {
        // Meghívjuk a Start metódust a csillagok generálásához
        starGenerator.Start();

        // Ellenőrizzük, hogy minden csillag pozíciója a kamera viewportján belül van-e
        foreach (Transform child in starGeneratorGO.transform)
        {
            Vector2 position = child.position;
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            Assert.GreaterOrEqual(position.x, min.x, "A csillag x pozíciójának a viewporton belül kell lennie.");
            Assert.LessOrEqual(position.x, max.x, "A csillag x pozíciójának a viewporton belül kell lennie.");
            Assert.GreaterOrEqual(position.y, min.y, "A csillag y pozíciójának a viewporton belül kell lennie.");
            Assert.LessOrEqual(position.y, max.y, "A csillag y pozíciójának a viewporton belül kell lennie.");
        }
    }

    [Test]
    public void Start_AssignsCorrectStarColors()
    {
        // Meghívjuk a Start metódust a csillagok generálásához
        starGenerator.Start();

        // Lekérjük az index alapján hozzárendelt színeket
        Color[] expectedColors = {
            new Color(0.5f, 0.5f, 1f),
            new Color(0f, 1f, 1f),
            new Color(1f, 1f, 0f),
            new Color(1f, 0f, 0f),
        };

        // Ellenőrizzük, hogy minden csillag színe helyesen van-e beállítva az index alapján
        int i = 0;
        foreach (Transform child in starGeneratorGO.transform)
        {
            Color assignedColor = child.GetComponent<SpriteRenderer>().color;
            Color expectedColor = expectedColors[i % expectedColors.Length];

            Assert.AreEqual(expectedColor, assignedColor, $"A {i} indexű csillagnak a megfelelő színnel kell rendelkeznie.");
            i++;
        }
    }

    [Test]
    public void Start_SetsStarSpeedCorrectly()
    {
        // Meghívjuk a Start metódust a csillagok generálásához
        starGenerator.Start();

        // Ellenőrizzük, hogy minden csillag sebessége helyesen van-e beállítva
        foreach (Transform child in starGeneratorGO.transform)
        {
            float speed = child.GetComponent<Star>().speed;

            Assert.Greater(speed, 0.5f, "A csillag sebessége nagyobb kell legyen, mint 0.5f.");
            Assert.Less(speed, 1.5f, "A csillag sebessége kisebb kell legyen, mint 1.5f.");
        }
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítsuk meg a létrehozott GameObject-eket
        Object.Destroy(starGeneratorGO);
        Object.Destroy(starPrefab);
    }
}
