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


    public Button EasyButton;
    public Button MediumButton;
    public Button HardButton;


    public TextMeshProUGUI high_scores;// a highscore-okat megjelenítő elem

    // Start is called before the first frame update
    void Start()
    {
        // a beolvasndó fájl
        jsonFile = File.ReadAllText(Application.dataPath + "/Resources/story.json");
        // a beolvasott fájl adatait eltároló változó
        data = JsonUtility.FromJson<Missions>(jsonFile);
        // a higscore-ok megjelenítése
        high_scores.text = "1. " + data.highscores[0] + " points\n\n2. " + data.highscores[1] + " points\n\n3. " + data.highscores[2] + " points";

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

    public void Easy()
    {
        Image buttonImageEasy = EasyButton.GetComponent<Image>();
        buttonImageEasy.color = Color.red;

        Image buttonImageMedium = MediumButton.GetComponent<Image>();
        buttonImageMedium.color = Color.white;

        Image buttonImageHard = HardButton.GetComponent<Image>();
        buttonImageHard.color = Color.white;

        data.choosed_difficulty = "easy";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/Resources/story.json", json);
    }
    public void Medium()
    {
        Image buttonImageEasy = EasyButton.GetComponent<Image>();
        buttonImageEasy.color = Color.white;

        Image buttonImageMedium = MediumButton.GetComponent<Image>();
        buttonImageMedium.color = Color.red;

        Image buttonImageHard = HardButton.GetComponent<Image>();
        buttonImageHard.color = Color.white;

        data.choosed_difficulty = "medium";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/Resources/story.json", json);
    }
    public void Hard()
    {
        Image buttonImageEasy = EasyButton.GetComponent<Image>();
        buttonImageEasy.color = Color.white;

        Image buttonImageMedium = MediumButton.GetComponent<Image>();
        buttonImageMedium.color = Color.white;

        Image buttonImageHard = HardButton.GetComponent<Image>();
        buttonImageHard.color = Color.red;

        data.choosed_difficulty = "hard";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/Resources/story.json", json);
    }
}
