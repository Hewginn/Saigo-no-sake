using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;


public class DifficultyColor : MonoBehaviour
{
    string jsonFile;// a json fájl elérési útvonala
    Missions data; //a történetet, és más adatokat tartalmazó osztály


    public Button EasyButton;//a legkönnyebb szint gombja
    public Button MediumButton;// a közepes szint gombja
    public Button HardButton;// a nehéz szint gobja


    public TextMeshProUGUI high_scores;// a highscore-okat megjelenítő elem

    // Start is called before the first frame update
    void Start()
    {
        // a beolvasndó fájl
        jsonFile = File.ReadAllText(Application.dataPath + "/StreamingAssets/story.json");
        // a beolvasott fájl adatait eltároló változó
        data = JsonUtility.FromJson<Missions>(jsonFile);
        // a higscore-ok megjelenítése
        high_scores.text = "1. " + data.highscores[0] + " points\n\n2. " + data.highscores[1] + " points\n\n3. " + data.highscores[2] + " points";

        //annak függvényében hogy melyik szint van bveállítva a json fájlba, azt a gombot színezze be pirosra
        if (data.choosed_difficulty == "easy")
        {
            Image buttonImageEasy = EasyButton.GetComponent<Image>();
            buttonImageEasy.color = Color.red;
        }
        else if (data.choosed_difficulty == "medium")
        {
            Image buttonImageMedium = MediumButton.GetComponent<Image>();
            buttonImageMedium.color = Color.red;
        }
        else if (data.choosed_difficulty == "hard")
        {
            Image buttonImageHard = HardButton.GetComponent<Image>();
            buttonImageHard.color = Color.red;
        }
    }

    //a könnyű nehézségű gomb kiválasztása (ez piros színű, a többi fehér)
    public void Easy()
    {
        Image buttonImageEasy = EasyButton.GetComponent<Image>();
        buttonImageEasy.color = Color.red;

        Image buttonImageMedium = MediumButton.GetComponent<Image>();
        buttonImageMedium.color = Color.white;

        Image buttonImageHard = HardButton.GetComponent<Image>();
        buttonImageHard.color = Color.white;

        //a kiválasztott szint elmentése a json fájlba
        data.choosed_difficulty = "easy";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/story.json", json);
    }
    //a közepes nehézségű gomb kiválasztása (ez piros színű, a többi fehér)
    public void Medium()
    {
        Image buttonImageEasy = EasyButton.GetComponent<Image>();
        buttonImageEasy.color = Color.white;

        Image buttonImageMedium = MediumButton.GetComponent<Image>();
        buttonImageMedium.color = Color.red;

        Image buttonImageHard = HardButton.GetComponent<Image>();
        buttonImageHard.color = Color.white;

        //a kiválasztott szint elmentése a json fájlba
        data.choosed_difficulty = "medium";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/story.json", json);
    }
    //a nehéz nehézségű gomb kiválasztása (ez piros színű, a többi fehér)
    public void Hard()
    {
        Image buttonImageEasy = EasyButton.GetComponent<Image>();
        buttonImageEasy.color = Color.white;

        Image buttonImageMedium = MediumButton.GetComponent<Image>();
        buttonImageMedium.color = Color.white;

        Image buttonImageHard = HardButton.GetComponent<Image>();
        buttonImageHard.color = Color.red;

        //a kiválasztott szint elmentése a json fájlba
        data.choosed_difficulty = "hard";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/story.json", json);
    }
}
