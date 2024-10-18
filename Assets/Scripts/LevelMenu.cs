using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    // a pályák száma
    public Button[] buttons;

    private void Awake(){
        // az első pálya feloldása
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
    //a pályák betöltése
    public void OpenLevel(int levelId)
    {
        string LevelName = "Level "+ levelId;
        SceneManager.LoadScene(LevelName);
    }
}
