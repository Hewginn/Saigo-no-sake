using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class PauseMenuTest : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    private GameObject pauseMenuGO; // A PauseMenu GameObject
   

    // Update is called once per frame
    //vissza a menübe gomb


     public void Pause()
    {
         // a pause menü megjelenítése
        pauseMenu.SetActive(true);
        // a játék megállítása
        Time.timeScale=0;

    }
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        //a folyamatok folytatása
        Time.timeScale=1;
    }
   


         //játék folytatása
    public void Resume()
    {
        pauseMenu.SetActive(false); 
        //a játék folytatása
        Time.timeScale=1;
    }

    [SetUp]
    public void Setup()
    {
        // Létrehozunk GameObject-et a PauseMenu számára és hozzáadjuk a komponenst
        pauseMenuGO = new GameObject();
        pauseMenu = pauseMenuGO.AddComponent<PauseMenuTest>();

        // Létrehozzuk a pause menü UI GameObject-ját és inaktívra állítjuk
        GameObject menuUI = new GameObject();
        pauseMenuGO.SetActive(false);
        pauseMenuGO.transform.SetParent(menuUI.transform);
        pauseMenu.pauseMenu = pauseMenuGO;

        // Inicializáljuk a Time.timeScale-t
        Time.timeScale = 1;
    }

    [Test]
    public void Pause_DisplaysPauseMenuAndPausesGame()
    {
        // Meghívjuk a Pause metódust
        pauseMenu.Pause();

        // Ellenőrizzük, hogy a pause menü aktív-e
        Assert.IsTrue(pauseMenuGO.activeSelf);
        // Ellenőrizzük, hogy az idő skálája 0-ra van állítva
        Assert.AreEqual(0, Time.timeScale);
    }

   

    [Test]
    public void Resume_HidesPauseMenuAndResumesGame()
    {
        // Először meghívjuk a Pause-t, hogy a menü aktív legyen
        pauseMenu.Pause();

        // Meghívjuk a Resume metódust
        pauseMenu.Resume();

        // Ellenőrizzük, hogy a pause menü inaktív-e
        Assert.IsFalse(pauseMenuGO.activeSelf);
        // Ellenőrizzük, hogy az idő skálája vissza van állítva 1-re
        Assert.AreEqual(1, Time.timeScale);
    }

    [Test]
    public void Home_LoadsMainMenuSceneAndResetsTimeScale()
    {
        // Meghívjuk a Home metódust
        pauseMenu.Home();

        // Ellenőrizzük, hogy a jelenlegi aktív jelenet a Főmenü jelenet
        Assert.AreEqual("MainMenu", SceneManager.GetActiveScene().name);
        // Ellenőrizzük, hogy az idő skálája vissza van állítva 1-re
        Assert.AreEqual(1, Time.timeScale);
    }

    [Test]
    public void Restart_RestartsCurrentSceneAndResetsTimeScale()
    {
        // Meghívjuk a Restart metódust
        pauseMenu.Restart();

        // Ellenőrizzük, hogy a jelenlegi aktív jelenet ugyanaz, mint korábban
        Assert.AreEqual(SceneManager.GetActiveScene().buildIndex, SceneManager.GetActiveScene().buildIndex);
        // Ellenőrizzük, hogy az idő skálája vissza van állítva 1-re
        Assert.AreEqual(1, Time.timeScale);
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítsuk meg a létrehozott GameObject-et
        Object.Destroy(pauseMenuGO);
    }
}
