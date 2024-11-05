using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuTest
{
    private GameObject mainMenuGO; // A főmenü GameObject
    private MainMenu mainMenu; // A MainMenu komponens

    [SetUp]
    public void Setup()
    {
        // A főmenü GameObject létrehozása és a komponens hozzáadása
        mainMenuGO = new GameObject();
        mainMenu = mainMenuGO.AddComponent<MainMenu>();
        
        // A jelenetek nevének beállítása
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    [Test]
    public void PlayGame_LoadsGameScene()
    {
        // A PlayGame metódus meghívása
        mainMenu.PlayGame();

        // Ellenőrizzük, hogy a betöltött jelenet a játék jelenet (feltételezve, hogy az index 1 a játék jelenet)
        Assert.AreEqual(1, SceneManager.GetActiveScene().buildIndex);
    }

    [Test]
    public void BacktoTheMenu_LoadsMainMenuScene()
    {
        // A BacktoTheMenu metódus meghívása
        mainMenu.BacktoTheMenu();

        // Ellenőrizzük, hogy a betöltött jelenet a főmenü jelenet (feltételezve, hogy az index 0 a főmenü)
        Assert.AreEqual(0, SceneManager.GetActiveScene().buildIndex);
    }

    [Test]
    public void QuitGame_ApplicationQuits()
    {
        // A QuitGame metódus meghívása
        mainMenu.QuitGame();

        // Mivel az Application.Quit() ténylegesen nem lép ki az editorból, 
        // csak azt ellenőrizhetjük, hogy a metódus kivétel nélkül végrehajtódik
        Assert.DoesNotThrow(() => mainMenu.QuitGame());
    }

    [TearDown]
    public void TearDown()
    {
        // A létrehozott GameObject eltávolítása
        Object.Destroy(mainMenuGO);
    }
}
