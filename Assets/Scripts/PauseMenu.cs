using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;// Pause menu 

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
