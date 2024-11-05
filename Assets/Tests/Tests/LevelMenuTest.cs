using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenuTest
{
    private GameObject levelMenuGO; // A szintmenü GameObject
    private LevelMenuTest levelMenu; // A LevelMenu komponens
    private Button[] buttons; // A szintválasztó gombok

    [SetUp]
    public void Setup()
    {
        // A szintmenü GameObject létrehozása és a komponens hozzáadása
        levelMenuGO = new GameObject();
        levelMenu = levelMenuGO.AddComponent<LevelMenuTest>();

        // Gombok létrehozása a teszteléshez
        buttons = new Button[5]; // Például 5 gomb
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = new GameObject("Button" + i).AddComponent<Button>();
            buttons[i].interactable = false; // Kezdetben nem interaktívan inicializálva
        }
        levelMenu.buttons = buttons; // Gombok hozzárendelése a LevelMenu-hoz

        // Kezdeti PlayerPrefs érték beállítása
        PlayerPrefs.SetInt("UnlockedLevel", 3); // Három szint feloldása
    }

    [Test]
    public void Awake_UnlocksCorrectLevels()
    {
        // Az Awake metódus hívása a szintek feloldásához
        levelMenu.Awake();

        // Ellenőrizzük, hogy az első három gomb interaktív
        Assert.IsTrue(buttons[0].interactable);
        Assert.IsTrue(buttons[1].interactable);
        Assert.IsTrue(buttons[2].interactable);

        // Ellenőrizzük, hogy a maradék gombok nem interaktívak
        Assert.IsFalse(buttons[3].interactable);
        Assert.IsFalse(buttons[4].interactable);
    }

    [Test]
    public void OpenLevel_LoadsCorrectScene()
    {
        // Feltételezve, hogy a szín neve "Level 1"
        levelMenu.OpenLevel(1);

        // Ellenőrizzük, hogy a jelenet helyesen lett betöltve
        Assert.AreEqual("Level 1", SceneManager.GetActiveScene().name);
    }

    [TearDown]
    public void TearDown()
    {
        // A létrehozott GameObject eltávolítása
        Object.Destroy(levelMenuGO);
        PlayerPrefs.DeleteAll(); // A PlayerPrefs törlése a tesztek után
    }
}
