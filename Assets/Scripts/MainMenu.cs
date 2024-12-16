using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    string jsonFile;// a json fájl elérési útvonala
    Missions data; //a történetet, és más adatokat tartalmazó osztály

    void Start()
    {
        // a beolvasndó fájl
        jsonFile = File.ReadAllText(Application.dataPath + "/StreamingAssets/story.json");
        // a beolvasott fájl adatait eltároló változó
        data = JsonUtility.FromJson<Missions>(jsonFile);
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
    /*
    public void ScoreToZero()
    {
        // nullára állítani a játékos pontszámát, amikor visszamegy a főmenübe
        data.score = 0;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/Resources/story.json", json);
    }*/
}

