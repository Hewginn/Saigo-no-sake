using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    // Start is called before the first frame update

    public void Pause()
    {
        // a pause menü megjelenítése
        pauseMenu.SetActive(true);
        // a játék megállítása
        Time.timeScale = 0;

    }

    // Update is called once per frame
    //vissza a menübe gomb
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        //a folyamatok folytatása
        Time.timeScale = 1;
    }
    //játék folytatása
    public void Resume()
    {
        pauseMenu.SetActive(false);
        //a játék folytatása
        Time.timeScale = 1;
    }
    //a pálya újra kezdése
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //a folyamatok folytatása
        Time.timeScale = 1;
    }
}
