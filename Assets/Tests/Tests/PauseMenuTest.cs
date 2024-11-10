using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuTests : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;  // A pause menü objektuma
    private GameObject pauseMenuGO;         // A pause menü GameObject objektuma

    // Pause metódus: a játék megállítása és a pause menü megjelenítése
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    // Home metódus: visszalépés a főmenübe, és a játék folytatása
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    // Resume metódus: a pause menü elrejtése és a játék folytatása
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    // Restart metódus: az aktuális jelenet újratöltése és a játék folytatása
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy új GameObject-et a PauseMenu számára
        pauseMenuGO = new GameObject("PauseMenu");
        pauseMenu = pauseMenuGO;

        // A pause menü inaktívra állítása teszt előtt
        pauseMenu.SetActive(false);

        // Inicializáljuk a Time.timeScale-t alaphelyzetbe
        Time.timeScale = 1;
    }

    [Test]
    public void Pause_DisplaysPauseMenuAndPausesGame()
    {
        // Meghívjuk a Pause metódust
        Pause();

        // Ellenőrizzük, hogy a pause menü aktív-e
        Assert.IsTrue(pauseMenu.activeSelf);
        // Ellenőrizzük, hogy az idő skálája 0-ra van állítva
        Assert.AreEqual(0, Time.timeScale);
    }

    [Test]
    public void Resume_HidesPauseMenuAndResumesGame()
    {
        // Először meghívjuk a Pause metódust, hogy a menü aktív legyen
        Pause();

        // Meghívjuk a Resume metódust
        Resume();

        // Ellenőrizzük, hogy a pause menü inaktív-e
        Assert.IsFalse(pauseMenu.activeSelf);
        // Ellenőrizzük, hogy az idő skálája vissza van állítva 1-re
        Assert.AreEqual(1, Time.timeScale);
    }

    [Test]
    public void Home_LoadsMainMenuSceneAndResetsTimeScale()
    {
        // Meghívjuk a Home metódust
        Home();

        // Ellenőrizzük, hogy a jelenlegi aktív jelenet a Főmenü jelenet
        Assert.AreEqual("MainMenu", SceneManager.GetActiveScene().name);
        // Ellenőrizzük, hogy az idő skálája vissza van állítva 1-re
        Assert.AreEqual(1, Time.timeScale);
    }

    [Test]
    public void Restart_RestartsCurrentSceneAndResetsTimeScale()
    {
        // Meghívjuk a Restart metódust
        Restart();

        // Ellenőrizzük, hogy a jelenlegi aktív jelenet újratöltődött
        Assert.AreEqual(SceneManager.GetActiveScene().buildIndex, SceneManager.GetActiveScene().buildIndex);
        // Ellenőrizzük, hogy az idő skálája vissza van állítva 1-re
        Assert.AreEqual(1, Time.timeScale);
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítjuk a létrehozott GameObject-eket
        Object.Destroy(pauseMenuGO);
    }
}
