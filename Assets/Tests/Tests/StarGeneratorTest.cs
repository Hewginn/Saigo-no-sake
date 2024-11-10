using NUnit.Framework;
using UnityEngine;

public class StarGeneratorTest : MonoBehaviour
{
    private GameObject starGeneratorGO;  // A StarGenerator GameObject
    private StarGeneratorTest starGenerator;  // A StarGenerator komponens
    private GameObject starPrefab;        // A csillag prefab

    public GameObject StarGO;
    public int MaxStars;

    Color[] starColors = {
        new Color(0.5f, 0.5f, 1f),
        new Color(0f, 1f, 1f),
        new Color(1f, 1f, 0f),
        new Color(1f, 0f, 0f),
    };

    [SetUp]
    public void SetupTest()
    {
        // Létrehozunk egy GameObject-et a StarGenerator számára és hozzáadjuk a komponenst
        starGeneratorGO = new GameObject("StarGenerator");
        starGenerator = starGeneratorGO.AddComponent<StarGeneratorTest>();  // Megváltoztattuk StarGenerator-ra

        // Létrehozunk egy prefab-t a csillagokhoz SpriteRenderer és Star komponenssel
        starPrefab = new GameObject("StarPrefab");
        var spriteRenderer = starPrefab.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);
        starPrefab.AddComponent<StarTest>();

        // Beállítjuk a generálandó csillagok maximális számát
        starGenerator.MaxStars = 5;

        // Beállítunk egy kamerát a viewport számításokhoz
        GameObject cameraGO = new GameObject("MainCamera");
        cameraGO.tag = "MainCamera";
        cameraGO.AddComponent<Camera>();
        cameraGO.transform.position = new Vector3(0, 0, -10); // A kamera pozíciójának beállítása
    }

    void Start()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        for (int i = 0; i < MaxStars; ++i)
        {
            GameObject star = Instantiate(StarGO);
            star.GetComponent<SpriteRenderer>().color = starColors[i % starColors.Length];
            star.transform.position = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
            star.GetComponent<StarTest>().speed = -(1f * Random.value + 0.5f);
            star.transform.parent = transform;
        }
    }

    [Test]
    public void Start_CreatesCorrectNumberOfStars1()
    {
        // Meghívjuk a Start metódust a csillagok generálásához
        starGenerator.Start();  // Meghívjuk a valódi Start metódust

        // Megszámoljuk a StarGenerator alatti gyermekek számát
        Assert.AreEqual(5, starGeneratorGO.transform.childCount, "A létrehozott csillagok száma egyenlő kell legyen a MaxStars értékével.");
    }
}
