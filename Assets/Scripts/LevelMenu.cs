using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

//Szint választó menü
public class LevelMenu : MonoBehaviour
{
    //A pályák száma
    public Button[] buttons;

    string jsonFile;// a json fájl elérési útvonala

    Missions data; //a történetet tárolja a json fájlból
    /*
        private void Awake()
        {
            //Az első pálya feloldása
            int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
            // a még nem feloldott pályák száma
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = false;
            }
            // a feloldott pályák száma
            for (int i = 0; i < unlockedLevel; i++)
            {
                buttons[i].interactable = true;
            }*/


    void Start()
    {
        // a beolvasott fájl útvonala
        jsonFile = File.ReadAllText(Application.dataPath + "/StreamingAssets/story.json");
        // a beolvasott fájl adatait eltároló változó
        data = JsonUtility.FromJson<Missions>(jsonFile);
        // a ha a küldetés fel van oldva, akkor kiválasztható legyen a főmenüben, ha meg nem, akkor meg nem
        for (int i = 0; i < data.unlock_level.Length; i++)

            if (data.unlock_level[i] == true)
            {
                buttons[i].interactable = true;
            }
            else
            {
                buttons[i].interactable = false;
            }
    }

    public void UnlockedLevel()
    {

    }

    //A pályák betöltése
    public void OpenLevel(int levelId)
    {
        // nullára állítani a játékos pontszámát, amikor elkezdi az új küldetést a menüből
        string LevelName = "Level " + levelId;
        SceneManager.LoadScene(LevelName);
        data.score = 0;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/story.json", json);
    }
}
