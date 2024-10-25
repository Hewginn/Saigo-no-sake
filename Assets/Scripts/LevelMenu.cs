using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Szint választó menü
public class LevelMenu : MonoBehaviour
{
    //A pályák száma
    public Button[] buttons;

    private void Awake(){
        //Az első pálya feloldása
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel",1);
        // a még nem feloldott pályák száma
        for (int i =0; i < buttons.Length; i++){
            buttons[i].interactable = false;
        }
        // a feloldott pályák száma
        for (int i = 0; i< unlockedLevel; i++){
            buttons[i].interactable = true;
        }
    }
    //A pályák betöltése
    public void OpenLevel(int levelId)
    {
        string LevelName = "Level "+ levelId;
        SceneManager.LoadScene(LevelName);
    }
}
