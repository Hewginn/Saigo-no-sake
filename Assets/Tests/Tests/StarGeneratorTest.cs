using NUnit.Framework;
using UnityEngine;

public class StarGeneratorTest : MonoBehaviour
{
    private GameObject starGeneratorGO;  // A StarGenerator GameObject
    private StarGeneratorTest starGenerator;  // A StarGenerator komponens
    private GameObject starPrefab;        // A csillag prefab

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
        Camera.main = new GameObject().AddComponent<Camera>();
        Camera.main.transform.position = new Vector3(0, 0, -10); // A kamera pozíciójának beállítása
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
