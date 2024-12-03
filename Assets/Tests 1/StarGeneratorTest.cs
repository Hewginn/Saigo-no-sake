using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class StarGeneratorTest : StarGenMonoBehavTest
{
    [UnityTest]
    public IEnumerator Start_CreatesCorrectNumberOfStars()
    {
        // Arrange
        var stargen = subject.AddComponent<StarGenerator>();
        
        // Létrehozunk egy StarGO GameObject-et a teszt során
        var starPrefab = new GameObject("StarPrefab");
        starPrefab.AddComponent<SpriteRenderer>();
        starPrefab.AddComponent<Star>(); // A Star osztály elvárt
        stargen.StarGO = starPrefab; // Hozzárendeljük a StarGO-hoz

        // Beállítjuk a MaxStars értékét
        stargen.MaxStars = 3;

        // Act
        yield return new WaitForEndOfFrame();

        // Assert
        var generatedStars = subject.GetComponentsInChildren<Star>();
        Assert.AreEqual(3, generatedStars.Length, "The number of generated stars does not match MaxStars.");
    }

    [UnityTest]
    public IEnumerator Start_StarsAreCorrectlyConfigured()
    {
        // Arrange
        var stargen = subject.AddComponent<StarGenerator>();
        
        // StarGO GameObject dinamikus létrehozása
        var starPrefab = new GameObject("StarPrefab");
        starPrefab.AddComponent<SpriteRenderer>();
        starPrefab.AddComponent<Star>(); // A Star osztály elvárt
        stargen.StarGO = starPrefab;

        // Beállítjuk a MaxStars értékét
        stargen.MaxStars = 3;

        // Act
        yield return new WaitForEndOfFrame();

        // Assert
        var stars = subject.GetComponentsInChildren<Star>();
        foreach (var star in stars)
        {
            Assert.IsNotNull(star.GetComponent<SpriteRenderer>(), "Each star must have a SpriteRenderer component.");
            Assert.IsTrue(star.speed < 0, "Each star's speed must be negative.");
        }
    }
}
