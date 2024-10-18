using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame(){
        SceneManager.LoadSceneAsync(1);
    }
    public void BacktoTheMenu(){
        SceneManager.LoadSceneAsync(0);
    }
    //a játék bezárása
    public void QuitGame(){
        Application.Quit();
    }

}
