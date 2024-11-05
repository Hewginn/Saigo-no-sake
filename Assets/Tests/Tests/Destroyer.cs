using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class Destroyer : MonoBehaviour // MonoBehaviour öröklés hozzáadva
{
    public void DestroyGameObject()
    {
        // A GameObject megsemmisítése
        Destroy(gameObject);
    }
}

public class DestroyerTest
{
    private GameObject destroyerGO;
    private Destroyer destroyer;

    [SetUp]
    public void SetUp()
    {
        // Létrehozunk egy GameObject-et, amelyhez hozzáadjuk a Destroyer komponenst
        destroyerGO = new GameObject();
        destroyer = destroyerGO.AddComponent<Destroyer>(); // Destroyer hozzáadása a GameObject-hez
    }

    [UnityTest]
    public IEnumerator DestroyGameObject_DestroysObject()
    {
        // Meghívjuk a DestroyGameObject metódust
        destroyer.DestroyGameObject();

        // Egy frame-et várunk, hogy a Destroy művelet végrehajtódjon
        yield return null;

        // Ellenőrizzük, hogy az objektum megsemmisült-e
        Assert.IsTrue(destroyerGO == null, "A GameObject nem lett megsemmisítve.");
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítás a teszt után, ha a GameObject még létezik
        if (destroyerGO != null)
        {
            Object.Destroy(destroyerGO);
        }
    }
}
