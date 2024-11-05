using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Fő menü
public class MainMenu : MonoBehaviour
{
    //Játék megkezdése
    public void PlayGame(){
        SceneManager.LoadSceneAsync(1);
    }

    //Menübe visszaugrás
    public void BacktoTheMenu(){
        SceneManager.LoadSceneAsync(0);
    }

    //A játék bezárása
    public void QuitGame(){
        Application.Quit();
    }

}
