using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //az első küldetés betöltése
    public void PlayGame(){
        SceneManager.LoadSceneAsync(1);
    }
     //az második küldetés betöltése
    public void Mission2(){
        SceneManager.LoadSceneAsync(2);
    }
     //az harmadikküldetés betöltése
    public void Mission3(){
        SceneManager.LoadSceneAsync(3);
    }
    //vissza a fő menübe
    public void BacktoTheMenu(){
        SceneManager.LoadSceneAsync(0);
    }
    //a játék bezárása
    public void QuitGame(){
        Application.Quit();
    }

}