using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class MainMenu : MonoBehaviour
{
    //TextMeshProUGUI high_scores;
    string jsonFile;// a json fájl elérési útvonala
    Missions data; //a történetet, és más adatokat tartalmazó osztály


    public TextMeshProUGUI high_scores;// a highscore-okat megjelenítő elem


    void Start()
    {

        // a beolvasndó fájl
        jsonFile = File.ReadAllText(Application.dataPath + "/Resources/story.json");
        // a beolvasott fájl adatait eltároló változó
        data = JsonUtility.FromJson<Missions>(jsonFile);
        // a higscore-ok megjelenítése
        high_scores.text = "1. " + data.highscores[0] + " pont\n\n2. " + data.highscores[1] + "pont\n\n3. " + data.highscores[2] + " pont";
        data.score = 0;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/Resources/story.json", json);
    }

    //az első küldetés betöltése
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    //az második küldetés betöltése
    public void Mission2()
    {
        SceneManager.LoadSceneAsync(2);
    }
    //az harmadikküldetés betöltése
    public void Mission3()
    {
        SceneManager.LoadSceneAsync(3);
    }
    //vissza a fő menübe
    public void BacktoTheMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    //a játék bezárása
    public void QuitGame()
    {
        Application.Quit();
    }
}

