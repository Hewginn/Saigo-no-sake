using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

//Pontszámláló
public class GameScore : MonoBehaviour
{
    //Pontszámláló UI szöveg
    TextMeshProUGUI scoreTextUI;

    string jsonFile;// a json fájl elérési útvonala
    Missions data; //a történetet, és más adatokat tartalmazó osztály

    //Pontok
    int score;

    //Pontok setter, getter
    public int Score
    {

        get
        {
            return this.score;
        }
        set
        {
            this.score = value;
            UpdateScoreTextUI();
        }

    }

    //Első frame update előtt van meghívva
    void Start()
    {
        // a beolvasndó fájl
        jsonFile = File.ReadAllText(Application.dataPath + "/Resources/story.json");
        // a beolvasott fájl adatait eltároló változó
        data = JsonUtility.FromJson<Missions>(jsonFile);
        score = data.score;

        scoreTextUI = GetComponent<TextMeshProUGUI>();
        UpdateScoreTextUI();
    }

    //Pontszámláló UI átírása
    void UpdateScoreTextUI()
    {
        string scoreStr = string.Format("{0:000000}", score);
        scoreTextUI.text = scoreStr;
    }

}
